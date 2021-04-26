using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery.Models
{
    public class ThumbnailBlob
    {
        /*
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public StoredFile ThumbnailOf { get; set; }
        public byte[] Blob { get; set; }
        */


        [ForeignKey("FileId")]
        public StoredFile File { get; set; }
        [Key]
        public Guid FileId { get; set; }
        [Key]
        public ThumbnailType Type { get; set; }
        public byte[] Blob { get; set; }
    }
}
