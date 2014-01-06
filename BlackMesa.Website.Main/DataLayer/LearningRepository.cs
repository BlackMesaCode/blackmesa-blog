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


        public void GetCardCount(Folder folder, ref int cardCount, bool includeSubfolders = true, bool countOnlySelected = false)
        {
            if (countOnlySelected)
            {
                cardCount += folder.Cards.Count(u => u.IsSelected);
            }
            else
            {
                cardCount += folder.Cards.Count();
            }

            if (includeSubfolders)
            {
                foreach (var subFolder in folder.SubFolders)
                {
                    GetCardCount(subFolder, ref cardCount, includeSubfolders, countOnlySelected);
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

            for (var i = folderToDelete.Cards.Count-1; i >= 0; i--)
            {
                RemoveCard(folderToDelete.Cards[i].Id.ToString());
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


        public void GetCardsIncludingSubfolders(string folderId, ref List<Card> cards)
        {
            var folder = GetFolder(folderId);
            cards.AddRange(folder.Cards);
            foreach (var subfolder in folder.SubFolders)
            {
                GetCardsIncludingSubfolders(subfolder.Id.ToString(), ref cards);
            }
        }


        public Card GetCard(string cardId)
        {
            return _dbContext.Learning_Cards.SingleOrDefault(f => f.Id == new Guid(cardId));
        }


        public void AddCard(string folderId, string ownerId, string question, string answer)
        {

            var owner = _dbContext.Users.Find(ownerId);
            var folder = _dbContext.Learning_Folders.Find(new Guid(folderId));

            var newStandardCard = new Card
            {
                FolderId = new Guid(folderId),
                OwnerId = ownerId,
                Owner = owner,
                FrontSide = question,
                BackSide = answer,
                DateCreated = DateTime.Now,
                DateEdited = DateTime.Now,
            };

            folder.Cards.Add(newStandardCard);
            _dbContext.SaveChanges();
        }

        public void EditCard(string id, string newQuestion, string newAnswer, string newHint, 
            string newCodeSnipped, string newImageUrl)
        {
            var currentCard = _dbContext.Learning_Cards.Find(new Guid(id));

            currentCard.FrontSide = newQuestion;
            currentCard.BackSide = newAnswer;
            currentCard.Hint = newHint;
            currentCard.CodeSnipped = newCodeSnipped;
            currentCard.ImageUrl = newImageUrl;

            _dbContext.SaveChanges();
        }

        public void MoveCard(string cardId, string newFolderId)
        {
            var card = _dbContext.Learning_Cards.Find(new Guid(cardId));
            var oldFolder = card.Folder;
            var newFolder = _dbContext.Learning_Folders.Find(new Guid(newFolderId));

            card.FolderId = new Guid(newFolderId);
            card.Folder = newFolder;

            // Remove card from the list of learning cards from the old folder
            oldFolder.Cards.Remove(card);

            // Add card to the list of learning cards of the new folder
            newFolder.Cards.Add(card);

            _dbContext.SaveChanges();
        }


        public void ChangeOrder(string sourceFolderId, string cardIdToInsertAfter)
        {
            var folder = GetFolder(sourceFolderId);
            var cardToInsertAfter = GetCard(cardIdToInsertAfter);
            var selectedCards = folder.Cards.Where(u => u.IsSelected).ToList();

            foreach (var card in selectedCards)
            {
                RemoveCard(card.Id.ToString());
            }

            var newIndex = folder.Cards.IndexOf(cardToInsertAfter) + 1;

            for (int i = 0; i < selectedCards.Count; i++)
            {
                
            }

            folder.Cards.InsertRange(newIndex, selectedCards);

            _dbContext.SaveChanges();
        }



        public void RemoveCard(string id)
        {
            var card = _dbContext.Learning_Cards.Find(new Guid(id));
            _dbContext.Learning_Cards.Remove(card);
            _dbContext.SaveChanges();
        }


        public List<QueryItem> GetQueries(string cardId)
        {
            return _dbContext.Learning_Cards.Find(new Guid(cardId)).Queries;
        }

        public void AddQuery(string cardId, Card card, DateTime questionTime, DateTime answerTime, QueryResult result)
        {
            var query = new QueryItem
            {
                CardId = new Guid(cardId),
                Card = card,

                StartTime = questionTime,
                EndTime = answerTime,
                Result = result,
            };

            card.Queries.Add(query);
            _dbContext.SaveChanges();  
        }


        public void SelectCard(string cardId)
        {
            var card = GetCard(cardId);
            card.IsSelected = true;

            //if (!card.Folder.IsSelected && AllChildsSelected(card.Folder))
            //    card.Folder.IsSelected = true;

            _dbContext.SaveChanges();
        }

        public bool AllChildsSelected(Folder folder)
        {
            if (folder.SubFolders.Any(subFolder => !subFolder.IsSelected))
                return false;
            if (folder.Cards.Any(card => !card.IsSelected))
                return false;

            return true;
        }

        public void DeSelectCard(string cardId)
        {
            var card = GetCard(cardId);
            card.IsSelected = false;

            if (card.Folder.IsSelected)
                card.Folder.IsSelected = false;

            _dbContext.SaveChanges();
        }

        public void SelectFolder(string folderId)
        {
            var folder = GetFolder(folderId);
            folder.IsSelected = true;
            foreach (var card in folder.Cards)
            {
                card.IsSelected = true;
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

            foreach (var card in folder.Cards)
            {
                card.IsSelected = false;
            }
            foreach (var subfolder in folder.SubFolders)
            {
                DeSelectFolder(subfolder.Id.ToString());
            }
            _dbContext.SaveChanges();
        }

    }
}