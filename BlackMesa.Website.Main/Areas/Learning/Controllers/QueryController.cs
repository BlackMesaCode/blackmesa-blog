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

            var queryId = _learningRepo.AddQuery(User.Identity.GetUserId(), viewModel.QueryOnlyDueCards, viewModel.ReverseSides,
                viewModel.OrderType, viewModel.QueryType, cardsToQuery, cardsToQuery);

            return RedirectToAction("GetQueryItem",
                new
                {
                    queryId = queryId,
                    position = 0,
                    folderId = viewModel.FolderId
                });
        }


        private QueryItemViewModel GetQueryItemViewModel(string queryId, string folderId, int positionOffset = 0)
        {
            var query = _learningRepo.GetQuery(queryId);
            Card card = null;
            if (positionOffset == 0)
            {
                var unqueriedCards = query.CardsToQuery.Where(c => !c.QueryItems.Exists(i => i.QueryId.ToString() == queryId));
                if (unqueriedCards.Any()) // maybe this can be deleted
                {
                    card = unqueriedCards.First(); 
                }
                else if (query.QueryType == QueryType.Normal)
                {
                    var wrongCardsToRequery = query.CardsToQuery
                        .Where(
                            c =>
                                c.QueryItems.Single(i => i.QueryId.ToString() == queryId).Result ==
                                QueryResult.PartlyCorrect
                                || c.QueryItems.Single(i => i.QueryId.ToString() == queryId).Result == QueryResult.Wrong);
                    card = wrongCardsToRequery.First();
                }
            }
            else
            {
                var lastQueriedCardsOrderedByStartTime = query.CardsToQuery.Where(c => c.QueryItems.Exists(i => i.QueryId.ToString() == queryId))
                    .OrderBy(c => c.QueryItems.Single(i => i.QueryId.ToString() == queryId).StartTime);
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
                };
            }
            return null;
        }


        public ActionResult GetQueryItem(string queryId, string folderId, int positionOffset = 0)
        {
            // update position?
            var viewModel = GetQueryItemViewModel(queryId, folderId, positionOffset);
            if (viewModel == null)
                RedirectToAction("Completed", "Query", new {queryId = queryId, folderId = folderId});

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveQueryItem(QueryItemViewModel resultViewModel)
        {
            //ModelState.Clear();
            var currentTime = DateTime.Now;

            var query = _learningRepo.GetQuery(resultViewModel.QueryId);
            var queriedCard = _learningRepo.GetCard(resultViewModel.CardId);

            var queryItem = queriedCard.QueryItems.SingleOrDefault(i => i.QueryId.ToString() == resultViewModel.QueryId);
            if (queryItem != null)
            {
                _learningRepo.EditQueryItem(queryItem.Id.ToString(), resultViewModel.StartTime, currentTime, resultViewModel.Result);
            }
            else
            {
                _learningRepo.AddQueryItem(queriedCard.Id.ToString(), queriedCard, resultViewModel.QueryId, query,
                    resultViewModel.StartTime, currentTime, resultViewModel.Result);
            }
            

            return RedirectToAction("GetQueryItem",
                new
                {
                    queryId = resultViewModel.QueryId,
                    folderId = resultViewModel.FolderId
                });
        }

        public ActionResult Aborted(string queryId, string folderId)
        {
            _learningRepo.EditQuery(queryId, null, DateTime.Now, QueryStatus.Aborted);

            return RedirectToAction("Details", "Folder", new {id = folderId});
        }


        public ActionResult Completed(string queryId, string folderId)
        {
            _learningRepo.EditQuery(queryId, null, DateTime.Now, QueryStatus.Completed);
            var query = _learningRepo.GetQuery(queryId);

            var viewModel = new QueryCompletedViewModel
            {
                FolderId = folderId,
                TotalCount = query.CardsToQuery.Count,
                CorrectCount = 0,
                PartlyCorrectCount = 0,
                WrongCount = 0,
            };
            return View(viewModel);
        }

    }
}