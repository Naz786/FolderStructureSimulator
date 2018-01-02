using MiniWebProject.Models;
using MiniWebProject.Services;
using System;
using System.Net;
using System.Web.Mvc;
using static MiniWebProject.Models.MWPModels;

namespace MiniWebProject.Controllers
{
    public class BookmarkRESTController : Controller
    {
        BookmarkRepository repository = new BookmarkRepository();

        /**************************CREATE ITEMS********************************/
        public ActionResult CreateFolder(FolderViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Redirect("/Home/Index");
                }

                int parentId = repository.GetParentIdFromPath(model.parent);

                Bookmark folder = new Folder(model.name, parentId);

                if (parentId != 0)
                {
                    if (!repository.itemExistsInPath(folder))
                    {
                        Folder parentFolder = repository.GetParentFolder(parentId);

                        parentFolder.FolderBookmarks.Add(folder);
                        repository.saveChangesToItem(parentFolder);
                    }

                    return RedirectToAction("Index", "Home");
                }

                repository.AddItem(folder);

                return RedirectToAction("Index", "Home");

            }
            catch (Exception e)
            {
                return Redirect("/Home/Index");
            }
        }

        public ActionResult CreateLink(LinkViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Redirect("/Home/Index");
                }

                int parentId = repository.GetParentIdFromPath(model.parent);
                Bookmark linkCheck = new Link(model.name, parentId);
                Bookmark link = new Link(model.name, model.uri, parentId);

                if (parentId >= 0)
                {
                    if (!repository.itemExistsInPath(linkCheck))
                    {
                        Folder parentFolder = repository.GetParentFolder(parentId);

                        parentFolder.FolderBookmarks.Add(link);
                        repository.saveChangesToItem(parentFolder);
                    }
                    return RedirectToAction("Index", "Home"); 
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                return Redirect("/Home/Index");
            }
        }

        public ActionResult CreateLocation(LocationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Redirect("/Home/Index");
                }

                int parentId = repository.GetParentIdFromPath(model.parent);
                Bookmark locationCheck = new Location(model.name, parentId);
                Bookmark location = new Location(model.name,model.latitude, model.longitude, parentId);

                if (parentId >= 0)
                {
                    if (!repository.itemExistsInPath(locationCheck))
                    {
                        Folder parentFolder = repository.GetParentFolder(parentId);

                        parentFolder.FolderBookmarks.Add(location);
                        repository.saveChangesToItem(parentFolder);
                    }
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                return Redirect("/Home/Index");
            }
        }

        public ActionResult CreateTextFile(TextFileViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Redirect("/Home/Index");
                }

                int parentId = repository.GetParentIdFromPath(model.parent);
                Bookmark textFileCheck = new Location(model.name, parentId);
                Bookmark textFile = new TextFile(model.name, model.text, parentId);

                if (parentId >= 0)
                {
                    if (!repository.itemExistsInPath(textFileCheck))
                    {
                        Folder parentFolder = repository.GetParentFolder(parentId);

                        parentFolder.FolderBookmarks.Add(textFile);
                        repository.saveChangesToItem(parentFolder);
                    }
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Index", "Home");

            }
            catch (Exception e)
            {
                return Redirect("/Home/Index");
            }
        }
        /**************************END OF CREATE ITEMS********************************/


        /*******************************EDIT ITEMS************************************/
        [HttpPost]
        public JsonResult EditItem(int? Id = 0)
        {
            try
            {
                Bookmark bookmark = repository.GetBookmark(Id);

                
                if (bookmark is Link)
                {
                    return Json(new { redirectUrl = Url.Action("EditLink", "BookmarkREST", new { Id = Id }) });
                }
                if (bookmark is Location)
                {
                    return Json(new { redirectUrl = Url.Action("EditLocation", "BookmarkREST", new { Id = Id }) });
                }
                if (bookmark is TextFile)
                {
                    return Json(new { redirectUrl = Url.Action("EditTextFile", "BookmarkREST", new { Id = Id }) });
                }
                if (bookmark is Folder)
                {
                    return Json(new { redirectUrl = Url.Action("EditFolder", "BookmarkREST", new { Id = Id }) });
                }

                return Json(new { message = "can't edit item" });

            }
            catch (Exception e)
            {
                return Json( new { message = "can't edit item" });
            }

        }

        [HttpGet]
        public ActionResult EditLink(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = repository.GetLink(id); 

            if (link == null)
            {
                return HttpNotFound();
            }
            return View(new LinkViewModel { name = link.Name, uri = link.URI });
        }

        [HttpPost]
        public ActionResult EditLink(int? id, LinkViewModel model)
        {
            if (ModelState.IsValid)
            {
                LinkViewModel modified = model;
                Link link = repository.GetLink(id);
                link.Name = modified.name;
                link.URI = modified.uri;

                repository.SaveEdits(link);

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult EditLocation(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = repository.GetLocation(id);

            if (location == null)
            {
                return HttpNotFound();
            }
            return View(new LocationViewModel { name = location.Name, latitude = location.Latitude, longitude = location.Longitude });
        }

        [HttpPost]
        public ActionResult EditLocation(int? id, LocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                LocationViewModel modified = model;
                Location location = repository.GetLocation(id);
                location.Name = modified.name;
                location.Latitude= modified.latitude;
                location.Longitude = modified.longitude;

                repository.SaveEdits(location);

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult EditTextFile(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TextFile textFile = repository.GetTextFile(id);

            if (textFile == null)
            {
                return HttpNotFound();
            }
            return View(new TextFileViewModel { name = textFile.Name, text = textFile.Text });
        }

        [HttpPost]
        public ActionResult EditTextFile(int? id, TextFileViewModel model)
        {
            if (ModelState.IsValid)
            {
                TextFileViewModel modified = model;
                TextFile textFile = repository.GetTextFile(id);
                textFile.Name = modified.name;
                textFile.Text = modified.text;

                repository.SaveEdits(textFile);

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult EditFolder(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Folder folder = repository.GetFolder(id);

            if (folder == null)
            {
                return HttpNotFound();
            }
            return View(new FolderViewModel { name = folder.Name});
        }

        [HttpPost]
        public ActionResult EditFolder(int? id, FolderViewModel model)
        {
            if (ModelState.IsValid)
            {
                FolderViewModel modified = model;
                Folder folder = repository.GetFolder(id);
                folder.Name = modified.name;

                repository.SaveEdits(folder);

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        /****************************END OF EDIT ITEMS************************************/


    }
}
