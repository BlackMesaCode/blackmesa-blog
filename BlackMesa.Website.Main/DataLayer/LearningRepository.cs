using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BlackMesa.Website.Main.Models.Learning;
using WebGrease.Css.Extensions;

namespace BlackMesa.Website.Main.DataLayer
{
    public class LearningRepository
    {

        private readonly BlackMesaDbContext _dbContext;

        public LearningRepository(BlackMesaDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void AddFolder(string name, string ownerId, string parentFolderId)
        {

            // todo check for valid parentFolderId

            Folder parentFolder = null;
            if (!String.IsNullOrEmpty(parentFolderId))
                parentFolder = _dbContext.Learning_Folders.Find(new Guid(parentFolderId));

            var owner = _dbContext.Users.Find(ownerId);
            var newFolder = new Folder
            {
                Name = name,
                OwnerId = ownerId,
                Owner = owner,
                ParentFolder = parentFolder,
                Level = parentFolder != null ? parentFolder.Level + 1 : 1,
            };

            if (parentFolder != null)
                parentFolder.SubFolders.Add(newFolder);

            _dbContext.Learning_Folders.Add(newFolder);
            _dbContext.SaveChanges();
        }



        public void EditFolder(string folderId, string newFolderName)
        {
            var folder = _dbContext.Learning_Folders.Find(new Guid(folderId));

            if (folder.ParentFolder == null)
                throw new Exception("Root folders name must not be changed.");

            folder.Name = newFolderName;
            _dbContext.SaveChanges();

        }


        public IEnumerable<Folder> GetFolders(string userId)
        {
            return _dbContext.Learning_Folders.Where(f => f.Owner.Id == userId && f.Level == 1).AsEnumerable();
        }

        public void GetAllSubfolders(string folderId, ref List<Folder> subfolders)
        {
            var folder = GetFolder(folderId);
            foreach (var subfolder in folder.SubFolders)
            {
                subfolders.Add(subfolder);
                GetAllSubfolders(subfolder.Id.ToString(), ref subfolders);
            }
        }

        public void GetFolderCount(Folder folder, ref int folderCount, bool includeSubfolders = true, bool countOnlySelected = false)
        {
            if (countOnlySelected)
            {
                folderCount += folder.SubFolders.Count(f => f.IsSelected);
            }
            else
            {
                folderCount += folder.SubFolders.Count;
            }
            

            if (includeSubfolders)
            {
                foreach (var subFolder in folder.SubFolders)
                {
                    GetFolderCount(subFolder, ref folderCount, includeSubfolders, countOnlySelected);
                }
            }
        }


        public void GetUnitCount<T>(Folder folder, ref int unitCount, bool includeSubfolders = true, bool countOnlySelected = false) where T : Unit
        {
            if (countOnlySelected)
            {
                unitCount += folder.LearningUnits.OfType<T>().Count(u => u.IsSelected);
            }
            else
            {
                unitCount += folder.LearningUnits.OfType<T>().Count();
            }

            if (includeSubfolders)
            {
                foreach (var subFolder in folder.SubFolders)
                {
                    GetUnitCount<T>(subFolder, ref unitCount, includeSubfolders, countOnlySelected);
                }
            }
        }


        public Folder GetRootFolder(string userId)
        {
            return _dbContext.Learning_Folders.SingleOrDefault(f => f.Owner.Id == userId && f.ParentFolder == null);;
        }

        public void CreateRootFolder(string name, string ownerId)
        {
            var owner = _dbContext.Users.Find(ownerId);
            var newFolder = new Folder
            {
                Name = name,
                OwnerId = ownerId,
                Owner = owner,
                ParentFolder = null,
                Level = 1,
            };

            _dbContext.Learning_Folders.Add(newFolder);
            _dbContext.SaveChanges();
        }


        public Folder GetFolder(string folderId)
        {
            return _dbContext.Learning_Folders.SingleOrDefault(f => f.Id == new Guid(folderId));
        }


        public void GetFolderPath(Folder folder, ref  Dictionary<string, string> path)
        {
            path.Add(folder.Name, folder.Id.ToString());
            if (folder.ParentFolder != null)
            {
                GetFolderPath(folder.ParentFolder, ref path);
            }
        }


        public void RemoveFolder(string folderId)
        {
            var folderToDelete = _dbContext.Learning_Folders.Find(new Guid(folderId));

            if (folderToDelete.ParentFolder == null)
                throw new Exception("Root folder must not be deleted.");

            for (var i = folderToDelete.LearningUnits.Count-1; i >= 0; i--)
            {
                RemoveUnit(folderToDelete.LearningUnits[i].Id.ToString());
            }


            for (var i = folderToDelete.SubFolders.Count - 1; i >= 0; i--)
            {
                RemoveFolder(folderToDelete.SubFolders[i].Id.ToString());
            }

            _dbContext.Learning_Folders.Remove(folderToDelete);
            _dbContext.SaveChanges();
        }

        public void MoveFolder(string folderId, string newParentFolderId)
        {
            var folder = _dbContext.Learning_Folders.Find(new Guid(folderId));

            if (folder.ParentFolder == null)
                throw new Exception("Root folder must not be moved.");

            var oldParentFolder = folder.ParentFolder;
            var newParentFolder = _dbContext.Learning_Folders.Find(new Guid(newParentFolderId));

            // Remove Folder from the subfolders List of the old parent folder if there is one existing
            oldParentFolder.SubFolders.Remove(folder);

            // Add Folder to the subfolders list of the new parent folder 
            newParentFolder.SubFolders.Add(folder);

            // Adjust its new parent folder
            folder.ParentFolder = newParentFolder;  // todo check if null is assigned if there is no new parentFolder

            // Adjust its level
            folder.Level = folder.ParentFolder != null ? folder.ParentFolder.Level + 1 : 1;  //todo add level calculation to property getter


            _dbContext.SaveChanges();
        }


        public void GetLearningUnitsIncludingSubfolders(string folderId, ref List<Unit> units)
        {
            var folder = GetFolder(folderId);
            units.AddRange(folder.LearningUnits);
            foreach (var subfolder in folder.SubFolders)
            {
                GetLearningUnitsIncludingSubfolders(subfolder.Id.ToString(), ref units);
            }
        }


        public Unit GetUnit(string unitId)
        {
            return _dbContext.Learning_Units.SingleOrDefault(f => f.Id == new Guid(unitId));
        }

        public IndexCard GetIndexCard(string cardId)
        {
            return _dbContext.Learning_IndexCards.SingleOrDefault(f => f.Id == new Guid(cardId));
        }


        public void AddIndexCard(string folderId, string ownerId, string question, string answer)
        {

            var owner = _dbContext.Users.Find(ownerId);
            var folder = _dbContext.Learning_Folders.Find(new Guid(folderId));

            var newStandardUnit = new IndexCard
            {
                FolderId = new Guid(folderId),
                OwnerId = ownerId,
                Owner = owner,
                FrontSide = question,
                BackSide = answer,
                DateCreated = DateTime.Now,
                DateEdited = DateTime.Now,
            };

            folder.LearningUnits.Add(newStandardUnit);
            _dbContext.SaveChanges();
        }

        public void EditIndexCard(string id, string newQuestion, string newAnswer, string newHint, 
            string newCodeSnipped, string newImageUrl)
        {
            var currentIndexCard = _dbContext.Learning_IndexCards.Find(new Guid(id));

            currentIndexCard.FrontSide = newQuestion;
            currentIndexCard.BackSide = newAnswer;
            currentIndexCard.Hint = newHint;
            currentIndexCard.CodeSnipped = newCodeSnipped;
            currentIndexCard.ImageUrl = newImageUrl;

            _dbContext.SaveChanges();
        }

        public void MoveUnit(string unitId, string newFolderId)
        {
            var unit = _dbContext.Learning_Units.Find(new Guid(unitId));
            var oldFolder = unit.Folder;
            var newFolder = _dbContext.Learning_Folders.Find(new Guid(newFolderId));

            unit.FolderId = new Guid(newFolderId);
            unit.Folder = newFolder;

            // Remove unit from the list of learning units from the old folder
            oldFolder.LearningUnits.Remove(unit);

            // Add unit to the list of learning units of the new folder
            newFolder.LearningUnits.Add(unit);

            _dbContext.SaveChanges();
        }


        public void ChangeOrder(string sourceFolderId, string indexCardIdToInsertAfter)
        {
            var folder = GetFolder(sourceFolderId);
            var unitToInsertAfter = GetUnit(indexCardIdToInsertAfter);
            var selectedIndexCards = folder.LearningUnits.OfType<IndexCard>().Where(u => u.IsSelected).ToList();

            foreach (var indexCard in selectedIndexCards)
            {
                RemoveUnit(indexCard.Id.ToString());
            }

            var newIndex = folder.LearningUnits.IndexOf(unitToInsertAfter) + 1;

            for (int i = 0; i < selectedIndexCards.Count; i++)
            {
                
            }

            folder.LearningUnits.InsertRange(newIndex, selectedIndexCards);

            _dbContext.SaveChanges();
        }



        public void RemoveUnit(string id)
        {
            var learningUnit = _dbContext.Learning_Units.Find(new Guid(id));
            _dbContext.Learning_Units.Remove(learningUnit);
            _dbContext.SaveChanges();
        }


        public List<QueryItem> GetQueries(string indexCardId)
        {
            return _dbContext.Learning_IndexCards.Find(new Guid(indexCardId)).Queries;
        }

        public void AddQuery(string unitId, Unit unit, DateTime questionTime, DateTime answerTime, QueryResult result)
        {
            var query = new QueryItem
            {
                UnitId = new Guid(unitId),
                Unit = unit,

                StartTime = questionTime,
                EndTime = answerTime,
                Result = result,
            };

            unit.Queries.Add(query);
            _dbContext.SaveChanges();  
        }


        public void SelectUnit(string unitId)
        {
            var unit = GetUnit(unitId);
            unit.IsSelected = true;

            //if (!unit.Folder.IsSelected && AllChildsSelected(unit.Folder))
            //    unit.Folder.IsSelected = true;

            _dbContext.SaveChanges();
        }

        public bool AllChildsSelected(Folder folder)
        {
            if (folder.SubFolders.Any(subFolder => !subFolder.IsSelected))
                return false;
            if (folder.LearningUnits.Any(unit => !unit.IsSelected))
                return false;

            return true;
        }

        public void DeSelectUnit(string unitId)
        {
            var unit = GetUnit(unitId);
            unit.IsSelected = false;

            if (unit.Folder.IsSelected)
                unit.Folder.IsSelected = false;

            _dbContext.SaveChanges();
        }

        public void SelectFolder(string folderId)
        {
            var folder = GetFolder(folderId);
            folder.IsSelected = true;
            foreach (var unit in folder.LearningUnits)
            {
                unit.IsSelected = true;
            }
            foreach (var subfolder in folder.SubFolders)
            {
                SelectFolder(subfolder.Id.ToString());
            }


            //if (folder.ParentFolder!= null && !folder.ParentFolder.IsSelected && AllChildsSelected(folder.ParentFolder))
            //    folder.ParentFolder.IsSelected = true;

            _dbContext.SaveChanges();
        }

        public void DeSelectFolder(string folderId)
        {
            var folder = GetFolder(folderId);
            folder.IsSelected = false;

            if (folder.ParentFolder != null)
            {
                if (folder.ParentFolder.IsSelected)
                    folder.ParentFolder.IsSelected = false;
            }

            foreach (var unit in folder.LearningUnits)
            {
                unit.IsSelected = false;
            }
            foreach (var subfolder in folder.SubFolders)
            {
                DeSelectFolder(subfolder.Id.ToString());
            }
            _dbContext.SaveChanges();
        }

    }
}