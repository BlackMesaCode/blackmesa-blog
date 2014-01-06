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
                InludeSubfolders = true,
                OrderType = OrderType.Ordered,
                QueryType = QueryType.Normal,
                SelectedFolderId = folder.Id.ToString(),
                SelectedFolderName = folder.Name,
            };

            return View(viewModel);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Start(SetupQueryViewModel viewModel)
        {
            var selectedCards = new List<Card>();
            var folder = _learningRepo.GetFolder(viewModel.SelectedFolderId);

            // Selection
            if (viewModel.InludeSubfolders)
                _learningRepo.GetCardsIncludingSubfolders(folder.Id.ToString(), ref selectedCards);
            else
                selectedCards.AddRange(_learningRepo.GetFolder(folder.Id.ToString()).Cards);


            // Ordering
            selectedCards.Reverse(); // We reverse order by default as we enumerate backwards

            if (viewModel.OrderType == OrderType.Reversed)
                selectedCards.Reverse();
            else if (viewModel.OrderType == OrderType.Shuffled)
                selectedCards.Shuffle();

            var firstCard = _learningRepo.GetCard(selectedCards.Last().Id.ToString());

            // ReverseSides if option has been chosen
            var frontSide = viewModel.ReverseSides ? firstCard.BackSide : firstCard.FrontSide;
            var backSide = viewModel.ReverseSides ? firstCard.FrontSide : firstCard.BackSide;

            var initialQueryViewModel = new QueryViewModel
            {
                FolderId = viewModel.SelectedFolderId,
                SelectedCards = selectedCards.Select(u => u.Id.ToString()).JoinStrings(","),
                RemainingCards = selectedCards.Select(u => u.Id.ToString()).JoinStrings(","),
                Position = selectedCards.Count-1,
                QueryType = viewModel.QueryType,

                StartTime = DateTime.Now,

                FrontSide = frontSide,
                BackSide = backSide,
                Hint = firstCard.Hint,
                CodeSnipped = firstCard.CodeSnipped,
                ImageUrl = firstCard.ImageUrl,
            };

            return View("Show", initialQueryViewModel);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Show(QueryViewModel resultViewModel)
        {
            ModelState.Clear();
            var currentTime = DateTime.Now;
            var remainingCards = resultViewModel.RemainingCards.Split(new[] {','}).ToList();

            // Parsing "old" query result

            var oldCardId = remainingCards[resultViewModel.Position];
            var oldCard = _learningRepo.GetCard(oldCardId);

            _learningRepo.AddQuery(oldCardId, oldCard, resultViewModel.StartTime, currentTime, resultViewModel.Result);


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
                    QueryType = resultViewModel.QueryType,
                    SelectedCards = resultViewModel.SelectedCards,
                    RemainingCards = remainingCards.JoinStrings(","),
                    Position = nextPosition,
                    StartTime = DateTime.Now,

                    FrontSide = frontSide,
                    BackSide = backSide,
                    Hint = nextCard.Hint,
                    CodeSnipped = nextCard.CodeSnipped,
                    ImageUrl = nextCard.ImageUrl,
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