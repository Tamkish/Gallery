using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Gallery.Models
{
    public class Comment
    {
        [Key] public Guid guid { get; set; }

        [ForeignKey("AuthorId")] public IdentityUser Author { get; set; }

        [Required] public string AuthorId { get; set; }

        [Required] public string Content { get; set; }

        [Required] public DateTime Datetime { get; set; }

        [ForeignKey("ChildOfId")] public StoredFile ChildOf { get; set; }

        [Required] public Guid ChildOfId { get; set; }

        [Required] public bool IsChild { get; set; } //is it comment of another comment?

        public Comment parent { get; set; }

        [Required] public bool HasComments { get; set; }

        public ICollection<Comment> Children { get; set; }
    }
}