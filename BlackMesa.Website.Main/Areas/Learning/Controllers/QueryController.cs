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

namespace BlackMesa.Website.Main.Areas.Learning.Controllers
{
    [Authorize]
    public class QueryController : BaseController
    {
        private readonly LearningRepository _learningRepo = new LearningRepository(new BlackMesaDbContext());

        public ActionResult Setup(string folderId)
        {
            var folder = _learningRepo.GetFolder(folderId);
            var selectedFolders = new List<Folder> { folder };

            var viewModel = new SetupQueryViewModel
            {
                InludeSubfolders = true,
                OrderType = OrderType.Ordered,
                QueryType = QueryType.Normal,
                SelectedFolders = selectedFolders,
            };

            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setup(SetupQueryViewModel viewModel)
        {
            

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Start(SetupQueryViewModel viewModel)
        {
            var selectedLearningUnits = new List<Unit>();

            // Selection

            foreach (var folder in viewModel.SelectedFolders)
            {
                if (viewModel.InludeSubfolders)
                    _learningRepo.GetLearningUnitsIncludingSubfolders(folder.Id.ToString(), ref selectedLearningUnits);
                else
                    selectedLearningUnits.AddRange(_learningRepo.GetFolder(folder.Id.ToString()).LearningUnits);
            }

            // Ordering
            selectedLearningUnits.Reverse(); // We reverse order by default as we enumerate backwards

            if (viewModel.OrderType == OrderType.Reversed)
                selectedLearningUnits.Reverse();
            else if (viewModel.OrderType == OrderType.Shuffled)
                selectedLearningUnits.Shuffle();

            var initialQueryResult = new QueryResultViewModel
            {
                SelectedLearningUnits = selectedLearningUnits.Select(u => u.Id.ToString()).ToList(),
                RemainingLearningUnits = selectedLearningUnits.Select(u => u.Id.ToString()).ToList(),
                Position = selectedLearningUnits.Count,
                QueryType = viewModel.QueryType,
                IsInitialQuery = true,
            };

            return RedirectToAction("Show", new { initialQueryResult });
        }


        [HttpPost]
        public ActionResult Show(QueryResultViewModel result)
        {
            var currentTime = DateTime.Now;

            if (!result.IsInitialQuery) // if we already have an answered index card
            {
                // Parsing "old" query result

                var oldUnitId = result.RemainingLearningUnits[result.Position];
                var oldIndexCard = _learningRepo.GetIndexCard(oldUnitId);

                var query = new Query
                {
                    Unit = oldIndexCard,
                    UnitId = new Guid(oldUnitId),
                    QuestionTime = result.QuestionTime,
                    AnswerTime = currentTime,
                    Result = result.Result,
                };

                oldIndexCard.Queries.Add(query);


                // Adjusting RemainingIndexCards

                if (result.QueryType == QueryType.Normal)
                {
                    if (result.Result == QueryResult.Correct)
                        result.RemainingLearningUnits.Remove(oldUnitId);
                }
            }

            // Decreasing position
            var nextPosition = result.Position - 1;


            // If there are remaining learning units
            if (result.RemainingLearningUnits.Count > 0)
            {
                // if query type is normal and we reached the end of a cycle
                if (result.QueryType == QueryType.Normal && nextPosition == -1 && result.RemainingLearningUnits.Count > 0) 
                    result.Position = result.RemainingLearningUnits.Count - 1;  // resetting the position for another cycle


                // Preparing new query viewmodel

                var nextUnitId = result.RemainingLearningUnits[nextPosition];
                var nextIndexCard = _learningRepo.GetIndexCard(nextUnitId);


                var viewModel = new QueryViewModel
                {
                    SelectedLearningUnits = result.SelectedLearningUnits,
                    RemainingLearningUnits = result.RemainingLearningUnits,
                    Position = nextPosition,
                    QuestionTime = DateTime.Now,

                    Question = nextIndexCard.Question,
                    Answer = nextIndexCard.Answer,
                    Hint = nextIndexCard.Hint,
                    CodeSnipped = nextIndexCard.CodeSnipped,
                    ImageUrl = nextIndexCard.ImageUrl,
                };

                if (result.IsInitialQuery)
                    viewModel.IsInitialQuery = false;

                return View(viewModel);

            }
            else // no more remaining learning units
                return RedirectToAction("Finish", new { result });

        }


        public ActionResult Finish(QueryResultViewModel result)
        {
            return View();
        }

    }
}