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


        //public void EditFolder(Folder newFolder)
        //{
        //    var currentFolder = _dbContext.Learning_Folders.Find(newFolder.Id);

        //}


        public IEnumerable<Folder> GetFolders(string userId)
        {
            return _dbContext.Learning_Folders.Where(f => f.Owner.Id == userId && f.Level == 1).AsEnumerable();
        }


        public Folder GetFolder(string userId, string folderId)
        {
            // todo check for users rights e.g. if he is owner

            return _dbContext.Learning_Folders.SingleOrDefault(f => f.Owner.Id == userId && f.Id == new Guid(folderId));
        }

        public void RemoveFolder(string folderId)
        {
            // todo check for users rights e.g. if he is owner

            var folderToDelete = _dbContext.Learning_Folders.Find(new Guid(folderId));
            _dbContext.Learning_Folders.Remove(folderToDelete);
            _dbContext.SaveChanges();
        }

        public void ChangeFolderName(string folderId, string newName)
        {
            // todo check for users rights e.g. if he is owner

            var folder = _dbContext.Learning_Folders.Find(new Guid(folderId));
            folder.Name = newName;
            // EntityState.Modified ???
            _dbContext.SaveChanges();
        }

        public void ChangeParentFolder(string folderId, string newParentFolderId)
        {

            // todo check for users rights e.g. if he is owner

            var folder = _dbContext.Learning_Folders.Find(new Guid(folderId));
            var oldParentFolder = folder.ParentFolder;
            var newParentFolder = _dbContext.Learning_Folders.Find(new Guid(newParentFolderId));

            // Remove Folder from the subfolders List of the old parent folder
            oldParentFolder.SubFolders.Remove(folder);

            // Adjust its new parent folder
            folder.ParentFolder = newParentFolder;  // todo check if null is assigned if there is no new parentFolder

            // Adjust its level
            folder.Level = folder.ParentFolder != null ? folder.ParentFolder.Level + 1 : 1;  //todo add level calculation to property getter

            // Add Folder to the subfolders list of the new parent folder 
            newParentFolder.SubFolders.Add(folder);

            _dbContext.SaveChanges();
        }



    }
}