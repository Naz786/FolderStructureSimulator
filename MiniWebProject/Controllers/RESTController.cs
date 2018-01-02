using MiniWebProject.Context;
using MiniWebProject.Models;
using MiniWebProject.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using static MiniWebProject.Models.MWPModels;

namespace MiniWebProject.Controllers
{

    [RoutePrefix("service")]
    public class FolderController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        BookmarkRepository repository = new BookmarkRepository();

        [HttpGet]
        [Route("create")]
        public IHttpActionResult Create(string folder = null, int? parentId = 0)
        {
            try
            {
                Bookmark newFolder = new Folder(folder, parentId.Value);

                if (parentId != 0)
                {
                    if (!repository.itemExistsInPath(newFolder))
                    {
                        Folder parentFolder = repository.GetParentFolder(parentId.Value);
                        parentFolder.FolderBookmarks.Add(newFolder);
                        repository.saveChangesToItem(parentFolder);

                        return Ok("Successfully Create Folder.");
                    }       

                }

                repository.AddItem(newFolder);

                return Ok("Successfully Created New Root Folder.");
            }
            catch (Exception)
            {
                return BadRequest("Could not create folder.");
            }

        }

        [HttpGet]
        [Route("edit")]
        public IHttpActionResult Edit(string newName = null, int? itemId = 0)
        {
            try
            {
                repository.EditFolder(newName, itemId.Value);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("Could not delete folder.");
            }

        }

        [HttpGet]
        [Route("delete")]
        public IHttpActionResult Delete(int? folderId = 0)
        {
            try
            {
                repository.DeleteFolder(folderId.Value);             

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("Could not delete folder.");
            }

        }

        [HttpGet]
        [Route("structure")]
        public IHttpActionResult GetStructure(int? folderId = 0)
        {
            try
            {
                FolderStructureDTO folder = repository.GetStructure(folderId.Value);

                return Ok(folder);

            } catch (Exception e)
            {
                return BadRequest("Could not get folder structure.");
            }
        }

        
    }
}
