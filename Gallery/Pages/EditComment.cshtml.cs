using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gallery.Data;
using Gallery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gallery.Pages
{
    public class EditCommentModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditCommentModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            throw new Exception("this page shouldnt be viewed in GET");
        }

        public async Task<IActionResult> OnPostAsync(string cAction, string guid, string content)
        {
            var userId = "";
            if (!User.Identity.IsAuthenticated)
                return Redirect("Identity/Account/Login");
            userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()
                .Value; // získáme id přihlášeného uživatele

            Comment comment;
            StoredFile file;

            switch (cAction)
            {
                case "add":
                    file = _context.Files.Where(f => f.Id == new Guid(guid)).FirstOrDefault();
                    if (file == null) throw new Exception("nonexistent picture");
                    if (content == null || content == "") throw new Exception("comment cant be empty");
                    var newComment = new Comment
                    {
                        AuthorId = userId,
                        Content = content,
                        Datetime = DateTime.Now,
                        IsChild = false,
                        ChildOf = file,
                        HasComments = false
                    };
                    _context.Comments.Add(newComment);

                    break;

                case "reply":
                    comment = _context.Comments.Where(c => c.guid == new Guid(guid)).FirstOrDefault();
                    if (comment == null) throw new Exception("nonexistent comment");
                    if (content == null || content == "") throw new Exception("comment cant be empty");
                    file = comment.ChildOf;
                    if (file == null) throw new Exception("comment doesnt have parent");
                    //find file

                    var replyComment = new Comment
                    {
                        AuthorId = userId,
                        Content = content,
                        Datetime = DateTime.Now,
                        IsChild = true,
                        ChildOf = file,
                        HasComments = false
                    };
                    _context.Comments.Add(replyComment);
                    break;

                /*case "delete":
                    comment = _context.Comments.Where(c => c.guid == new Guid(guid)).FirstOrDefault();
                    if (comment == null)
                    {
                        throw new Exception("nonexistent comment");
                    }
					if (comment.AuthorId != userId)
					{
                        throw new Exception("unauthorized action (cant delete someone elses comment)");
					}
                    _context.Comments.Remove(comment);


                    break;
                */
            }

            await _context.SaveChangesAsync();
            return Page();
            throw new Exception("redirect back"); //todo;
        }
    }
}