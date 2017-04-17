using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BlogClient.Core.Objects
{
    public class Post
    {
        [Required(ErrorMessage="Id:Field is Required")]
        public virtual int Id{ get; set; }

        [Required(ErrorMessage="Title:Field is Required")]
        [StringLength(500,ErrorMessage="Title length should not exceed 500 characters")]
        public virtual string Title{ get; set; }

        [Required(ErrorMessage = "ShortDescription: Field is required")]
        public virtual string ShortDescription{ get; set; }

        [Required(ErrorMessage = "Description: Field is required")]
        public virtual string Description{ get; set; }

        [Required(ErrorMessage = "Meta: Field is required")]
        [StringLength(1000, ErrorMessage = "Meta: UrlSlug should not exceed 50 characters")]
        public virtual string Meta{ get; set; }

        public virtual string UrlSlug{ get; set; }

        public virtual bool Published{ get; set; }

        [Required(ErrorMessage = "PostedOn: Field is required")]
        public virtual DateTime PostedOn{ get; set; }

        public virtual DateTime? Modified{ get; set; }

        public virtual Category Category { get; set; }

        public virtual IList<Tag> Tags { get; set; }
    }
}
