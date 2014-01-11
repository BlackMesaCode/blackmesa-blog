using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Query;
using BlackMesa.Website.Main.Controllers;
using BlackMesa.Website.Main.DataLayer;
using BlackMesa.Website.Main.Models.Learning;
using BlackMesa.Website.Main.Utilities;
using dotless.Core.Utils;
using Microsoft.AspNet.Identity;

namespace BlackMesa.Website.Main.Areas.Learning.Controllers
{
    [Authorize]
    public class QueryController : BaseController
    {
        private readonly LearningRepository _learningRepo = new LearningRepository(new BlackMesaDbContext());

        public ActionResult Setup(string folderId)
        {
            var folder = _learningRepo.GetFolder(folderId);

            var viewModel = new SetupQueryViewModel
            {
                FolderId = folder.Id.ToString(),
                QueryOnlyDueCards = true,
                ReverseSides = false,
                OrderType = OrderType.Ordered,
                QueryType = QueryType.Normal,
            };

            return View(viewModel);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setup(SetupQueryViewModel viewModel)
        {

            var folder = _learningRepo.GetFolder(viewModel.FolderId);

            // Selection

            var cardsToQuery = new List<Card>();
            _learningRepo.GetAllSelectedCardsInFolder(folder.Id.ToString(), ref cardsToQuery);

            if (viewModel.QueryOnlyDueCards)
            {
                
            }

            // Ordering
            if (viewModel.OrderType == OrderType.Shuffled)
                cardsToQuery.Shuffle();


            // Create Query

            var query = new Query
            {
                Id = Guid.NewGuid(),
                QueryOnlyDueCards = viewModel.QueryOnlyDueCards,
                ReverseSides = viewModel.ReverseSides,
                OrderType = viewModel.OrderType,
                QueryType = viewModel.QueryType,
                CardsToQuery = cardsToQuery,
                QueryStatus = QueryStatus.InProgress,
                StartTime = DateTime.Now,
            };

            // Save Query to User Session

            Session["Query"] = query;

            return RedirectToAction("GetQueryItem",
                new
                {
                    queryId = query.Id.ToString(),
                    position = 0,
                    folderId = viewModel.FolderId
                });
        }

        public ActionResult GetQueryItem(string queryId, string folderId, int positionOffset = 0)
        {
            var viewModel = GetQueryItemViewModel(queryId, folderId, positionOffset);
            if (viewModel == null)
                return RedirectToAction("Completed", "Query", new {queryId = queryId, folderId = folderId});

            return View(viewModel);
        }


        private QueryItemViewModel GetQueryItemViewModel(string queryId, string folderId, int positionOffset = 0)
        {
            var query = Session["Query"] as Query;
            Card card = null;
            int cardsLeft = 0;
            if (positionOffset == 0)
            {
                var dbCardsToQuery = query.CardsToQuery.Select(cardToQuery => _learningRepo.GetCard(cardToQuery.Id.ToString()));
                var unqueriedCards = dbCardsToQuery.Where(c => !c.QueryItems.Exists(i => i.QueryId == queryId)); ;

                if (query.QueryType == QueryType.Normal)
                    cardsLeft = dbCardsToQuery.Count() - dbCardsToQuery.Count(c => c.QueryItems.Exists(i => i.QueryId == queryId && i.Result == QueryResult.Correct));
                else if (query.QueryType == QueryType.SinglePass)
                    cardsLeft = unqueriedCards.Count();

                if (unqueriedCards.Any())
                {
                    card = unqueriedCards.First();
                }
                else if (query.QueryType == QueryType.Normal)
                {
                    var wrongCardsToRequery = dbCardsToQuery
                        .Where(
                            c =>
                                c.QueryItems.Last(i => i.QueryId == queryId).Result == QueryResult.PartlyCorrect
                                || c.QueryItems.Last(i => i.QueryId == queryId).Result == QueryResult.Wrong);
                    if (wrongCardsToRequery.Any())
                    {
                        card = wrongCardsToRequery.OrderBy(c => c.QueryItems.Last(i => i.QueryId == queryId).StartTime).First();
                    }
                }
            }
            else
            {
                var lastQueriedCardsOrderedByStartTime = query.CardsToQuery.Where(c => c.QueryItems.Exists(i => i.QueryId == queryId))
                    .OrderBy(c => c.QueryItems.Single(i => i.QueryId == queryId).StartTime);
                card = lastQueriedCardsOrderedByStartTime.ElementAt(lastQueriedCardsOrderedByStartTime.Count() - positionOffset + 1);

            }
            if (card != null)
            {
                return new QueryItemViewModel
                {
                    FolderId = folderId,
                    QueryId = queryId,
                    CardId = card.Id.ToString(),
                    FrontSide = (query.ReverseSides ? card.BackSide : card.FrontSide),
                    BackSide = (query.ReverseSides ? card.FrontSide : card.BackSide),
                    StartTime = DateTime.Now,
                    Result = QueryResult.Correct,
                    CardsLeft = cardsLeft,
                };
            }
            return null;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveQueryItem(QueryItemViewModel resultViewModel)
        {
            var currentTime = DateTime.Now;

            var query = Session["Query"] as Query;
            var queriedCard = _learningRepo.GetCard(resultViewModel.CardId);

            _learningRepo.AddQueryItem(queriedCard.Id.ToString(), queriedCard, resultViewModel.QueryId, query,
                resultViewModel.StartTime, currentTime, resultViewModel.Result);
            
            return RedirectToAction("GetQueryItem",
                new
                {
                    queryId = resultViewModel.QueryId,
                    folderId = resultViewModel.FolderId
                });
        }


        public ActionResult Aborted(string queryId, string folderId)
        {
            Session["Query"] = null;

            return RedirectToAction("Details", "Folder", new {id = folderId});
        }


        public ActionResult Completed(string queryId, string folderId)
        {
            var query = Session["Query"] as Query;

            var viewModel = new QueryCompletedViewModel
            {
                FolderId = folderId,
                Duration = DateTime.Now - query.StartTime,
                TotalCount = query.CardsToQuery.Count,
                CorrectCount = query.CardsToQuery.Count(c => c.QueryItems.Any(i => i.QueryId == queryId && i.Result == QueryResult.Correct)),
                PartlyCorrectCount = query.CardsToQuery.Count(c => c.QueryItems.Any(i => i.QueryId == queryId && i.Result == QueryResult.PartlyCorrect)),
                WrongCount = query.CardsToQuery.Count(c => c.QueryItems.Any(i => i.QueryId == queryId && i.Result == QueryResult.Wrong)),
            };

            Session["Query"] = null;

            return View(viewModel);
        }

    }
}