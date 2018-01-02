using MiniWebProject.Services;
using System.Web.Http;
using System.Linq;

namespace MiniWebProject.Controllers
{
    [RoutePrefix("service")]
    public class BookmarkController : ApiController
    {
        public BookmarkRepository bookmarkRepository = new BookmarkRepository();
        
        [HttpGet]
        [Route("bookmarks")]
        public IHttpActionResult GetFileStructure()
        {
            var folderDTOs = bookmarkRepository.GetFolderDTOs(); 
            var textFileDTOs = bookmarkRepository.GetAllTextFiles();
            var locationDTOs = bookmarkRepository.GetAllLocations();
            var linkDTOs = bookmarkRepository.GetAllLinks();

            var allFolderAndFileDTOs = folderDTOs.Concat(textFileDTOs).Concat(locationDTOs).Concat(linkDTOs).ToList();

            return Ok(allFolderAndFileDTOs);
        }

        
        

    }
}
