
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static MiniWebProject.Models.MWPModels;

namespace MiniWebProject.Models
{

    public class FolderViewModel
    {
        [Required]
        public string name { get; set; }
  
        public string parent { get; set; }
    }

    public class LinkViewModel
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string uri { get; set; }

        public string parent { get; set; }
    }

    public class LocationViewModel
    {
        [Required]
        public string name { get; set; }
        [Required]
        public double latitude { get; set; }
        [Required]
        public double longitude { get; set; }

        public string parent { get; set; }
    }

    public class TextFileViewModel
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string text { get; set; }

        public string parent { get; set; }
    }

    // Data Transfer Objects
    public class FolderDTO
    {
        public int id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }

        public string type { get; set; }
    }

    public class ItemDTO
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }

        public string type { get; set; }
    }

    public class FolderStructureDTO
    {
        public string Folder { get; set; }
        public List<FolderStructureDTO> Subfolders { get; set; }
    }

}