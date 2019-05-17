using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mvcmusicstoredemo.Models
{
    public class Album
    {
        //唱片
        [ScaffoldColumn(false)]
        public int AlbumId { get; set; }
        //类型外键
        [DisplayName("Genre")]
        public int GenreId { get; set; }
        //艺术家外键
        [DisplayName("Artist")]
        public int ArtistId { get; set; }

        [Required(ErrorMessage="An Album Title is required")]
        [StringLength(160)]
        public string Title { get; set; }
        [Required(ErrorMessage ="Price is required")]
        [Range(0.01,100.00,ErrorMessage = "Price must be between 0.01 and 100.00")]
        public decimal Price { get; set; }
        [DisplayName("Album Art URL")]
        [StringLength(1024)]
        public string AlbumArtUrl { get; set; }
        //专辑 Album 的属性 Genre 和 Artist 设置为虚拟的 virtual ，这将会使 EF-Code First 使用延迟加载
        public virtual Genre Genre { get; set; }
        public virtual Artist Artist { get; set; }
        
    }
}