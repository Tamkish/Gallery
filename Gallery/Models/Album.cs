using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Gallery.Models
{
    public class Album
    {
        [ForeignKey("OwnerId")] public IdentityUser Owner { get; set; }

        public string OwnerId { get; set; }
        public string Name { get; set; }
        public ICollection<StoredFile> Files { get; set; }

        [Required] public bool Public { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}