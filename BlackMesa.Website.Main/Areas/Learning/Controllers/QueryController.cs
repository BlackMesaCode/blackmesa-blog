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
            var selectedLearningUnits = new List<Unit>();
            var folder = _learningRepo.GetFolder(viewModel.SelectedFolderId);

            // Selection
            if (viewModel.InludeSubfolders)
                _learningRepo.GetLearningUnitsIncludingSubfolders(folder.Id.ToString(), ref selectedLearningUnits);
            else
                selectedLearningUnits.AddRange(_learningRepo.GetFolder(folder.Id.ToString()).LearningUnits);


            // Ordering
            selectedLearningUnits.Reverse(); // We reverse order by default as we enumerate backwards

            if (viewModel.OrderType == OrderType.Reversed)
                selectedLearningUnits.Reverse();
            else if (viewModel.OrderType == OrderType.Shuffled)
                selectedLearningUnits.Shuffle();

            var firstUnit = _learningRepo.GetIndexCard(selectedLearningUnits.Last().Id.ToString());

            var initialQueryViewModel = new QueryViewModel
            {
                FolderId = viewModel.SelectedFolderId,
                SelectedLearningUnits = selectedLearningUnits.Select(u => u.Id.ToString()).JoinStrings(","),
                RemainingLearningUnits = selectedLearningUnits.Select(u => u.Id.ToString()).JoinStrings(","),
                Position = selectedLearningUnits.Count-1,
                QueryType = viewModel.QueryType,

                QuestionTime = DateTime.Now,

                Question = firstUnit.Question,
                Answer = firstUnit.Answer,
                Hint = firstUnit.Hint,
                CodeSnipped = firstUnit.CodeSnipped,
                ImageUrl = firstUnit.ImageUrl,
            };

            return View("Show", initialQueryViewModel);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Show(QueryViewModel resultViewModel)
        {
            ModelState.Clear();
            var currentTime = DateTime.Now;
            var remainingLearningUnits = resultViewModel.RemainingLearningUnits.Split(new[] {','}).ToList();

            // Parsing "old" query result

            var oldUnitId = remainingLearningUnits[resultViewModel.Position];
            var oldIndexCard = _learningRepo.GetIndexCard(oldUnitId);

            _learningRepo.AddQuery(oldUnitId, oldIndexCard, resultViewModel.QuestionTime, currentTime, resultViewModel.Result);


            // Adjusting RemainingIndexCards

            if (resultViewModel.QueryType == QueryType.Normal)
            {
                if (resultViewModel.Result == QueryResult.Correct)
                    remainingLearningUnits.Remove(oldUnitId);
            }

            if (resultViewModel.QueryType == QueryType.SinglePass)
            {
                remainingLearningUnits.Remove(oldUnitId);
            }


            // Decreasing position
            var nextPosition = resultViewModel.Position - 1;


            // If there are remaining learning units
            if (remainingLearningUnits.Count > 0)
            {
                // if query type is normal and we reached the end of a cycle
                if (resultViewModel.QueryType == QueryType.Normal && nextPosition == -1 &&
                    remainingLearningUnits.Count > 0)
                    nextPosition = remainingLearningUnits.Count - 1; // resetting the position for another cycle


                // Preparing new query viewmodel

                var nextUnitId = remainingLearningUnits[nextPosition];
                var nextIndexCard = _learningRepo.GetIndexCard(nextUnitId);


                var viewModel = new QueryViewModel
                {
                    FolderId = resultViewModel.FolderId,
                    SelectedLearningUnits = resultViewModel.SelectedLearningUnits,
                    RemainingLearningUnits = remainingLearningUnits.JoinStrings(","),
                    Position = nextPosition,
                    QuestionTime = DateTime.Now,

                    Question = nextIndexCard.Question,
                    Answer = nextIndexCard.Answer,
                    Hint = nextIndexCard.Hint,
                    CodeSnipped = nextIndexCard.CodeSnipped,
                    ImageUrl = nextIndexCard.ImageUrl,
                };

                return View(viewModel);

            }
            else // no more remaining learning units
            {
                var selectedLearningUnits = resultViewModel.SelectedLearningUnits.Split(new[] { ',' }).ToList();
                var queries = new List<Query>();
                foreach (var unitId in selectedLearningUnits)
                {
                    queries.Add(_learningRepo.GetIndexCard(unitId).Queries.Last());
                }

                var viewModel = new QueryCompletedViewModel
                {
                    FolderId = resultViewModel.FolderId,
                    TotalCount = selectedLearningUnits.Count,
                    CorrectCount = queries.Count(q => q.Result == QueryResult.Correct),
                    PartlyCorrectCount = queries.Count(q => q.Result == QueryResult.PartlyCorrect),
                    WrongCount = queries.Count(q => q.Result == QueryResult.Wrong),
                };
                return View("Completed", viewModel);
            }

        }


    }
}