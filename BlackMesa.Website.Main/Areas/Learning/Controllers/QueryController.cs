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
        public ActionResult Start(SetupQueryViewModel viewModel)
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

            var initialQueryViewModel = GetQueryViewModel(queryId, 0, folder.Id.ToString());

            return View("Show", initialQueryViewModel);
        }


        private QueryViewModel GetQueryViewModel(string queryId, int positionOffset, string folderId)
        {
            var query = _learningRepo.GetQuery(queryId);
            var card = query.RemainingCards.ElementAt(query.Position + positionOffset);
            var queryViewModel = new QueryViewModel
            {
                FolderId = folderId,
                QueryId = queryId,
                FrontSide = (query.ReverseSides ? card.BackSide : card.FrontSide),
                BackSide = (query.ReverseSides ? card.FrontSide : card.BackSide),
                StartTime = DateTime.Now,
                Result = QueryResult.Correct,
            };
            return queryViewModel;
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Show(QueryViewModel resultViewModel)
        {
            var currentTime = DateTime.Now;

            var query = _learningRepo.GetQuery(resultViewModel.QueryId);
            var queriedCard = query.RemainingCards.ElementAt(query.Position);

            // Add QueryItem for the queriedCard

            // todo add existence check  add or update
            _learningRepo.AddQueryItem(queriedCard.Id.ToString(), queriedCard, resultViewModel.StartTime, 
                currentTime, resultViewModel.Result);

            // Update Query
            // remaining cards
            // position etc

            // Prepare viewModel for new card

            // todo call RedirectToAction GetCard(queryId, forward/backward)












            ModelState.Clear();
            var currentTime = DateTime.Now;
            var remainingCards = resultViewModel.RemainingCards.Split(new[] {','}).ToList();

            // Parsing "old" query result

            var oldCardId = remainingCards[resultViewModel.Position];
            var oldCard = _learningRepo.GetCard(oldCardId);

            _learningRepo.AddQueryItem(oldCardId, oldCard, resultViewModel.StartTime, currentTime, resultViewModel.Result);


            // Adjusting RemainingCards

            if (resultViewModel.QueryType == QueryType.Normal)
            {
                if (resultViewModel.Result == QueryResult.Correct)
                    remainingCards.Remove(oldCardId);
            }
            else if (resultViewModel.QueryType == QueryType.SinglePass)
            {
                remainingCards.Remove(oldCardId);
            }


            // Decreasing position
            var nextPosition = resultViewModel.Position - 1;


            // If there are remaining learning cards
            if (remainingCards.Count > 0)
            {
                // if query type is normal and we reached the end of a cycle
                if (resultViewModel.QueryType == QueryType.Normal && nextPosition == -1 &&
                    remainingCards.Count > 0)
                    nextPosition = remainingCards.Count - 1; // resetting the position for another cycle


                // Preparing new query viewmodel

                var nextCardId = remainingCards[nextPosition];
                var nextCard = _learningRepo.GetCard(nextCardId);


                // ReverseSides if option has been chosen
                var frontSide = resultViewModel.ReverseSides ? nextCard.BackSide : nextCard.FrontSide;
                var backSide = resultViewModel.ReverseSides ? nextCard.FrontSide : nextCard.BackSide;


                var viewModel = new QueryViewModel
                {
                    FolderId = resultViewModel.FolderId,
                    StartTime = DateTime.Now,

                    FrontSide = frontSide,
                    BackSide = backSide,
                };

                return View(viewModel);

            }
            else // no more remaining learning cards
            {
                var selectedCards = resultViewModel.SelectedCards.Split(new[] { ',' }).ToList();
                var queries = new List<QueryItem>();
                foreach (var cardId in selectedCards)
                {
                    queries.Add(_learningRepo.GetCard(cardId).QueryItems.Last());
                }

                var viewModel = new QueryCompletedViewModel
                {
                    FolderId = resultViewModel.FolderId,
                    TotalCount = selectedCards.Count,
                    CorrectCount = queries.Count(q => q.Result == QueryResult.Correct),
                    PartlyCorrectCount = queries.Count(q => q.Result == QueryResult.PartlyCorrect),
                    WrongCount = queries.Count(q => q.Result == QueryResult.Wrong),
                };
                return View("Completed", viewModel);
            }

        }


    }
}