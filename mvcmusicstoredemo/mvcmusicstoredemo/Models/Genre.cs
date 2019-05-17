using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvcmusicstoredemo.Models
{
    public class Genre
    {
        //类型
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Album> Albums { get; set; }
    }
}