using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MiniWebProject.Models
{
    public class MWPModels
    {

        public class Bookmark
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int ParentId { get; set; }

            public Bookmark(string name) : base()
            {
                Name = name;
            }

            public Bookmark(string name, int parent)
            {
                Name = name;
                ParentId = parent;
            }

            public Bookmark()
            {

            }
            
        }

        public class Folder : Bookmark
        {

            public List<Bookmark> FolderBookmarks { get; set; }

            public Folder()
            {
                FolderBookmarks = new List<Bookmark>();
            }

            public Folder(string name, int parent) : base(name, parent)
            {
                Name = name;
            } 
            
        }

        public class Link : Bookmark
        {
            public string URI { get; set; }

            public Link()
            {
            }

            public Link(string name, string uri, int parent) : base (name, parent)
            {
                URI = uri;
            }

            public Link(string name, int parent) : base(name, parent)
            {
            }
        }

        public class Location : Bookmark
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public Location()
            {
            }

            public Location(string name, double latitude, double longitude, int parent): base (name, parent)
            {
                Longitude = longitude;
                Latitude = latitude;
            }

            public Location(string name, int parent) : base(name, parent)
            {
            }
        }

        public class TextFile : Bookmark
        {
            public string Text { get; set; }

            public TextFile()
            {
            }

            public TextFile(string name, string text, int parent) : base (name, parent)
            {
                Text = text;
            }

            public TextFile(string name, int parent) : base(name, parent)
            {
            }
        }

    }


}
