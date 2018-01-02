using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiniWebProject.Models;
using static MiniWebProject.Models.MWPModels;
using System.Data.Entity;

namespace MiniWebProject.Context
{
    public class BookmarkDAO
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        internal List<ItemDTO> GetFolderDTOs()
        {
            var folderDTOs = db.Bookmarks.OfType<Folder>().Select(x => new ItemDTO() { id = x.Id.ToString(), parent = x.ParentId.ToString(), text = x.Name }).ToList();
            
            foreach(var item in folderDTOs)
            {
                if (item.parent.Equals("0"))
                {
                    item.parent = "#"; 
                }
            }

            return folderDTOs;
        }
        
        internal List<ItemDTO> GetAllTextFiles()
        {
            var textFileDTOs = db.Bookmarks.OfType<TextFile>().Select(x => new ItemDTO() { id = x.Id.ToString(), parent = x.ParentId.ToString(), text = x.Name, type = "file"}).ToList(); 
            return textFileDTOs;
        }

        internal List<ItemDTO> GetAllLocations()
        {
            var locationDTOs = db.Bookmarks.OfType<Location>().Select(x => new ItemDTO() { id = x.Id.ToString(), parent = x.ParentId.ToString(), text = x.Name, type = "file" }).ToList();
            return locationDTOs;
        }
        
        internal List<ItemDTO> GetAllLinks()
        {
            var linkDTOs = db.Bookmarks.OfType<Link>().Select(x => new ItemDTO() { id = x.Id.ToString(), parent = x.ParentId.ToString(), text = x.Name, type = "file" }).ToList();
            return linkDTOs;
        }

        // REST API

        public void AddItem(Bookmark item)
        {
            if (item != null)
            {
                db.Bookmarks.Add(item);
                db.SaveChanges();
            }

        }

        public int GetParentIdFromPath(string path)
        {
            int _parentId = 0;

            if (path != null)
            {
                var folders = GetFolderStructure();
                var splitPath = path.Split('|');

                foreach (var item in splitPath)
                {
                    _parentId = folders.Where(x => x.Name == item).SingleOrDefault().Id;
                    folders = folders.Where(x => x.Name == item).SingleOrDefault().FolderBookmarks.OfType<Folder>().ToList();
                }
                
                return _parentId;
            }

            return _parentId;
        }

        public List<Folder> GetFolders()
        {
            var folders = db.Bookmarks.OfType<Folder>().ToList();
            return folders;
        }

        public List<Folder> GetFolderStructure()
        {
            var folders = GetFolders();

            foreach (var folder in folders)
            {
                var bookmarks = db.Bookmarks.Where(x => x.ParentId == folder.Id).ToList();
                folder.FolderBookmarks = bookmarks;
            }

            return folders;
        }

        public bool itemExistsInPath(Bookmark newItem)
        {
           var exists = db.Bookmarks.Any(x => x.Name == newItem.Name && x.ParentId == newItem.ParentId);

           return exists;
        }

        public Folder GetParentFolder(int id)
        {
            Folder folder = db.Bookmarks.OfType<Folder>().Where(x => x.Id == id).Single();
            return folder;
        }


        public void saveChangesToItem(Bookmark item)
        {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteFolder(int itemId)
        {
            Bookmark itemToDelete = db.Bookmarks.Where(x => x.Id == itemId).SingleOrDefault();

            if (itemToDelete is Link || itemToDelete is Location || itemToDelete is TextFile) 
            {
                db.Bookmarks.Remove(itemToDelete);
                db.SaveChanges();
            }

            if (itemToDelete is Folder)
            {
                var allBookmarks = GetFolderStructure();

                var folder = allBookmarks.Where(x => x.Id == itemId).SingleOrDefault();
                var folderBookmarks = folder.FolderBookmarks;

                if (!(folderBookmarks.Count() > 0))
                {
                    db.Bookmarks.Remove(itemToDelete);
                    db.SaveChanges();
                } else if (folderBookmarks.Count() > 0)
                {
                    DestroyList(folderBookmarks);

                    db.Bookmarks.Remove(itemToDelete);
                    db.SaveChanges();
                } 
            }
            
        }

        public void DestroyList(List<Bookmark> folderBookmarks)
        {

            var listToDelete = new List<Bookmark>();

            foreach(var item in folderBookmarks)
            {
                if (item is Link || item is Location || item is TextFile)
                {
                    listToDelete.Add(item);
                } else if (item is Folder)
                {
                    listToDelete.Add(item);

                    var allBookmarks = GetFolderStructure();
                    var folder = allBookmarks.Where(x => x.Id == item.Id).SingleOrDefault();
                    var fBookmarks = folder.FolderBookmarks;

                    if (!(fBookmarks.Count() > 0))
                    {
                        listToDelete.Add(item);
                    }
                    else if (folderBookmarks.Count() > 0)
                    {
                        DestroyList(fBookmarks);
                    }
                }
            }

            foreach(var item in listToDelete)
            {
                db.Bookmarks.Remove(item);
                db.SaveChanges();
            }
        }


        public void EditFolder(string newName, int itemId)
        {
            var folder = db.Bookmarks.Where(x => x.Id == itemId).SingleOrDefault();
            folder.Name = newName;
            db.SaveChanges();
        }

        public FolderStructureDTO GetFolderStructureDTOs(int folderId)
        {
            var folder = GetFolders().Where(x => x.Id == folderId).SingleOrDefault();
            var folderDTOList = new List<FolderStructureDTO>();

            var bookmarks = db.Bookmarks.OfType<Folder>().Where(x => x.ParentId == folder.Id).ToList(); 

            foreach(var item in bookmarks)
            {
                FolderStructureDTO folderDto = new FolderStructureDTO();
                folderDto.Folder = item.Name;

                if (item.FolderBookmarks.Count() > 0)
                {
                    GetFolderStructureDTOs(item.Id);
                }

                folderDTOList.Add(folderDto);
            }

            FolderStructureDTO folderStructureDTO = new FolderStructureDTO();
            folderStructureDTO.Folder = folder.Name;
            folderStructureDTO.Subfolders = folderDTOList;
            

            return folderStructureDTO;
        }

        public FolderStructureDTO GetStructure(int folderId)
        {
            var Folder = GetFolderStructureDTOs(folderId);

            return Folder;
        }

        internal Bookmark GetBookmark(int? id)
        {
            var bookmark = db.Bookmarks.Where(x => x.Id == id).SingleOrDefault();
            return bookmark;
        }


        internal Link GetLink(int? id)
        {
            var link = db.Bookmarks.OfType<Link>().Where(x => x.Id == id).SingleOrDefault();
            return link;
        }

        internal Location GetLocation(int? id)
        {
            var location = db.Bookmarks.OfType<Location>().Where(x => x.Id == id).SingleOrDefault();
            return location;
        }

        internal TextFile GetTextFile(int? id)
        {
            var textFile = db.Bookmarks.OfType<TextFile>().Where(x => x.Id == id).SingleOrDefault();
            return textFile;
        }

        internal Folder GetFolder(int? id)
        {
            var folder = db.Bookmarks.OfType<Folder>().Where(x => x.Id == id).SingleOrDefault();
            return folder;
        }

        internal void SaveEdits(Bookmark item)
        {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}