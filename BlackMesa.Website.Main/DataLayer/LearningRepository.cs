using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BlackMesa.Website.Main.Areas.Learning.ViewModels.Query;
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




        // ================================ Folders ================================ //

        public string AddFolder(string name, string ownerId, string parentFolderId)
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

            //if (parentFolder != null)
            //    parentFolder.SubFolders.Add(newFolder);

            _dbContext.Learning_Folders.Add(newFolder);
            _dbContext.SaveChanges();

            return newFolder.Id.ToString();
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


        public void GetFolderPath(Folder folder, ref  List<Folder> path)
        {
            path.Add(folder);
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


        // ================================ Cards ================================ //


        public Card GetCard(string cardId)
        {
            return _dbContext.Learning_Cards.SingleOrDefault(f => f.Id == new Guid(cardId));
        }


        public void GetAllSelectedCardsInFolder(string folderId, ref List<Card> cards)
        {
            var folder = GetFolder(folderId);
            cards.AddRange(folder.Cards.Where(c => c.IsSelected).OrderBy(c => c.Position));
            foreach (var subfolder in folder.SubFolders.Where(f => f.IsSelected).OrderBy(f => f.Name))
            {
                GetAllSelectedCardsInFolder(subfolder.Id.ToString(), ref cards);
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


        public void AddCard(string folderId, string ownerId, string frontSide, string backSide)
        {

            var owner = _dbContext.Users.Find(ownerId);
            var folder = _dbContext.Learning_Folders.Find(new Guid(folderId));

            var newStandardCard = new Card
            {
                FolderId = new Guid(folderId),
                Folder = folder,
                OwnerId = ownerId,
                Owner = owner,
                IsSelected = false,
                Position = (folder.Cards != null ? folder.Cards.Count : 0),
                FrontSide = frontSide,
                BackSide = backSide,
                DateCreated = DateTime.Now,
                DateEdited = DateTime.Now,
            };

            _dbContext.Learning_Cards.Add(newStandardCard);
            //folder.Cards.Add(newStandardCard);
            _dbContext.SaveChanges();
        }


        public void EditCard(string id, string newFrontSide, string newBackSide)
        {
            var currentCard = _dbContext.Learning_Cards.Find(new Guid(id));

            currentCard.FrontSide = newFrontSide;
            currentCard.BackSide = newBackSide;

            _dbContext.SaveChanges();
        }

        public void MoveCard(string cardId, string newFolderId)
        {
            var card = _dbContext.Learning_Cards.Find(new Guid(cardId));
            var oldFolder = card.Folder;
            var newFolder = _dbContext.Learning_Folders.Find(new Guid(newFolderId));

            DecreasePositionOfSubsequentCards(oldFolder, card, 1);

            card.FolderId = new Guid(newFolderId);
            card.Folder = newFolder;
            card.Position = newFolder.Cards.Count;

            // Remove card from the list of learning cards from the old folder
            oldFolder.Cards.Remove(card);

            // Add card to the list of learning cards of the new folder
            newFolder.Cards.Add(card);

            _dbContext.SaveChanges();
        }


        private void DecreasePositionOfSubsequentCards(Folder folder, Card card, int offset)
        {
            var affectedCards = folder.Cards.Where(c => c.Position > card.Position);
            foreach (var affectedCard in affectedCards)
            {
                affectedCard.Position = affectedCard.Position - offset;
            }
        }


        public void ChangeCardOrder(string sourceFolderId, string insertAfterCardId)
        {
            var folder = GetFolder(sourceFolderId);
            var insertAfterCard = GetCard(insertAfterCardId);
            var selectedCards = folder.Cards.Where(u => u.IsSelected).OrderBy(c => c.Position).ToList();

            for (int i = 0; i < selectedCards.Count(); i++)
            {
                ChangeCardPosition(folder.Cards.Single(c => c.Position == (insertAfterCard.Position + i)), selectedCards.ElementAt(i));
            }

            _dbContext.SaveChanges();
        }

        private void ChangeCardPosition(Card insertAfterCard, Card card)
        {
            card.Position = -1;
            var cardsBefore = insertAfterCard.Folder.Cards.Where(c => c.Position <= insertAfterCard.Position && c.Position >= 0).OrderBy(c => c.Position).ToList();
            var cardsAfter = insertAfterCard.Folder.Cards.Where(c => c.Position > insertAfterCard.Position).OrderBy(c => c.Position).ToList();

            int b = 0;
            foreach (var cardBefore in cardsBefore)
            {
                insertAfterCard.Folder.Cards.Find(c => c.Id == cardBefore.Id).Position = b;
                b++;
            }

            int a = insertAfterCard.Position + 2;
            foreach (var cardAfter in cardsAfter)
            {
                insertAfterCard.Folder.Cards.Find(c => c.Id == cardAfter.Id).Position = a;
                a++;
            }
            card.Position = insertAfterCard.Position + 1;

            _dbContext.SaveChanges();
        }


        public void RemoveCard(string id)
        {
            var card = _dbContext.Learning_Cards.Find(new Guid(id));
            DecreasePositionOfSubsequentCards(card.Folder, card, 1);
            _dbContext.Learning_Cards.Remove(card);
            _dbContext.SaveChanges();
        }


        // ================================ Queries ================================ //


        public List<QueryItem> GetQueryItems(string cardId)
        {
            return _dbContext.Learning_Cards.Find(new Guid(cardId)).QueryItems;
        }

        public QueryItem GetQueryItem(string queryItemId)
        {
            return _dbContext.Learning_QueryItems.Find(new Guid(queryItemId));
        }

        public void AddQueryItem(string cardId, Card card, string queryId, Query query, DateTime startTime, DateTime endTime, QueryResult result)
        {
            var queryItem = new QueryItem
            {
                CardId = new Guid(cardId),
                Card = card,

                QueryId = new Guid(queryId),
                Query = query,

                StartTime = startTime,
                EndTime = endTime,
                Result = result,
            };

            card.QueryItems.Add(queryItem);
            _dbContext.SaveChanges();
        }

        public void EditQueryItem(string queryItemId, DateTime? startTime, DateTime? endTime, QueryResult result)
        {
            var queryItem = GetQueryItem(queryItemId);

            if (startTime.HasValue)
                queryItem.StartTime = startTime.Value;

            if (endTime.HasValue)
                queryItem.EndTime = endTime.Value;

            queryItem.Result = result;

            _dbContext.SaveChanges();
        }

        public Query GetQuery(string queryId)
        {
            return _dbContext.Learning_Queries.Find(new Guid(queryId));
        }

        public string AddQuery(string ownerId, bool queryOnlyDueCards, bool reverseSides, 
            OrderType orderType, QueryType queryType, List<Card> cardsToQuery)
        {
            var owner = _dbContext.Users.Find(ownerId);
            var currentTime = DateTime.Now;
            var query = new Query
            {
                OwnerId = ownerId,
                Owner = owner,
                QueryOnlyDueCards = queryOnlyDueCards,
                ReverseSides = reverseSides,
                OrderType = orderType,
                QueryType = queryType,
                CardsToQuery = cardsToQuery,
                StartTime = currentTime,
                EndTime = currentTime,
                QueryStatus = QueryStatus.InProgress,
            };

            _dbContext.Learning_Queries.Add(query);
            _dbContext.SaveChanges();

            return query.Id.ToString();
        }


        public void EditQuery(string queryId, DateTime? startTime, DateTime? endTime, QueryStatus queryStatus)
        {
            var query = GetQuery(queryId);
            if (startTime.HasValue)
                query.StartTime = startTime.Value;
            if(endTime.HasValue)
                query.EndTime = endTime.Value;
            query.QueryStatus = queryStatus;

            _dbContext.SaveChanges();
        }


        // ================================ Selections ================================ //


        public bool AllChildsSelected(Folder folder)
        {
            if (folder.SubFolders.Any(subFolder => !subFolder.IsSelected))
                return false;
            if (folder.Cards.Any(card => !card.IsSelected))
                return false;

            return true;
        }

        public void SelectCard(Card card)
        {
            card.IsSelected = true;

            //if (!card.Folder.IsSelected && AllChildsSelected(card.Folder))
            //    card.Folder.IsSelected = true;

            _dbContext.SaveChanges();
        }


        public void DeSelectCard(Card card)
        {
            card.IsSelected = false;

            var cardsFolderIsSelected = _dbContext.Learning_Folders.Where(f => f.Id == card.FolderId).Select(f => f.IsSelected).Single();
            if (cardsFolderIsSelected)
                card.Folder.IsSelected = false;

            _dbContext.SaveChanges();
        }


        public void SelectFolder(Folder folder)
        {
            folder.IsSelected = true;
            foreach (var card in folder.Cards)
            {
                card.IsSelected = true;
            }
            foreach (var subfolder in folder.SubFolders.Where(f => !f.IsSelected))
            {
                subfolder.IsSelected = true;
            }

            //if (folder.ParentFolder!= null && !folder.ParentFolder.IsSelected && AllChildsSelected(folder.ParentFolder))
            //    folder.ParentFolder.IsSelected = true;

            _dbContext.SaveChanges();
        }


        public void DeSelectFolder(Folder folder)
        {
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

            foreach (var subfolder in folder.SubFolders.Where(f => f.IsSelected))
            {
                subfolder.IsSelected = false;
            }

            _dbContext.SaveChanges();
        }



    }
}