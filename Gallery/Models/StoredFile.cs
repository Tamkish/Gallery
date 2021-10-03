using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Gallery.Models
{
    public class StoredFile
    {
        /*
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        [ForeignKey("UploaderId")]
        public IdentityUser Uploader { get; set; }
        [Required]
        public string UploaderId { get; set; }
        [Required]
        public DateTime Uploaded { get; set; }
        [Required]
        public string OriginalName { get; set; }
        [Required]
        public string ContentType { get; set; }
        public ThumbnailBlob Thumbnail { get; set; }
        public Guid? ThumbnailId { get; set; }
		public object UploadedAt { get; internal set; }
	    */

        [Key] public Guid Id { get; set; } // identifikátor souboru a název fyzického souboru

        [ForeignKey("UploaderId")] public IdentityUser Uploader { get; set; } // kdo soubor nahrál

        [Required] public string UploaderId { get; set; } // identifikátor uživatele, který soubor nahrál

        [Required] public DateTime UploadedAt { get; set; } // datum a čas nahrání souboru

        [Required] public string OriginalName { get; set; } // původní název souboru

        [Required] public string ContentType { get; set; } // druh obsahu v souboru (MIME type)

        public ICollection<ThumbnailBlob> Thumbnails { get; set; } // kolekce všech možných náhledů

        public ICollection<Comment> Comments { get; set; }


        public Album Album { get; set; }
        public string OwnerId { get; set; }
        public string Name { get; set; }
    }
}