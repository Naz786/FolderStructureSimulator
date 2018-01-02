using MiniWebProject.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using static MiniWebProject.Models.MWPModels;
using MiniWebProject.Models;
using System.Data.Entity;

namespace MiniWebProject.Services
{
    public class BookmarkRepository
    {
        // Tree user interface data methods
        BookmarkDAO dao = new BookmarkDAO();

        internal List<ItemDTO> GetFolderDTOs()
        {
            return dao.GetFolderDTOs();
        }

        internal List<ItemDTO> GetAllTextFiles()
        {
            return dao.GetAllTextFiles();
        }

        internal List<ItemDTO> GetAllLinks()
        {
            return dao.GetAllLinks();
        }

        internal List<ItemDTO> GetAllLocations()
        {
            return dao.GetAllLocations();
        }


        // Rest Service Methods
        public void AddItem(Bookmark item)
        {
            dao.AddItem(item);
        }

        public int GetParentIdFromPath(string path)
        {
            return dao.GetParentIdFromPath(path);
        }

        public List<Folder> GetFolders()
        {
            return dao.GetFolders();
        }

        public bool itemExistsInPath(Bookmark item)
        {
            return dao.itemExistsInPath(item);
        }

        public Folder GetParentFolder(int id)
        {
            return dao.GetParentFolder(id);
        }


        public void saveChangesToItem(Bookmark item)
        {
            dao.saveChangesToItem(item);
        }

        public void DeleteFolder(int id)
        {
            dao.DeleteFolder(id);
        }

        public void EditFolder(string newName, int itemId)
        {
            dao.EditFolder(newName, itemId);
        }

        public FolderStructureDTO GetStructure(int folderId)
        {
            return dao.GetStructure(folderId);
        }

        public Bookmark GetBookmark(int? id)
        {
            return dao.GetBookmark(id);
        }

        public Link GetLink(int? id)
        {
            return dao.GetLink(id);
        }

        public Location GetLocation(int? id)
        {
            return dao.GetLocation(id);
        }

        public TextFile GetTextFile(int? id)
        {
            return dao.GetTextFile(id);
        }

        public Folder GetFolder(int? id)
        {
            return dao.GetFolder(id);
        }

        public void SaveEdits(Bookmark item)
        {
            dao.SaveEdits(item);
        }
    }
}