using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gallery.Data;
using Gallery.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gallery.Pages
{
    public class ShowImageModel : PageModel
    {
        private IWebHostEnvironment _environment;
        private readonly ILogger<ShowImageModel> _logger;
        private ApplicationDbContext _context;

        public ShowImageModel(ILogger<ShowImageModel> logger, IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _environment = environment;
            _logger = logger;
            _context = context;
        }

        public string Error { get; set; }
        public StoredFile Photo { get; set; }
        public string photosrc { get; set; }
        public string PhotoId { get; set; }

        public byte[] bytes { get; set; }
        public string base64img { get; set; }
        private string userId { get; set; }
        public string HtmlComments { get; set; }
        public void OnGet()
        {
            Error = "no image";
        }

        public List<Comment> comments { get; set; }
        
        public string LoadComments(Comment cc)
		{
            string output = "";
            output += "<li>";
            output += " <div>";
            output += "  <span class=\"d-block\"><strong>"+cc.Author+"</strong></span>";
            output += "  <span>"+cc.Content+"</span>";
			if (cc.AuthorId == userId)
			{
                output += "  <form asp-page=\"EditComment\" method=\"post\" >";
                output += "   <input name=\"cAction\" type=\"hidden\" value=\"delete\" />";
                output += "   <input name=\"guid\" type=\"hidden\" value=\""+cc.guid+"\" />";
                output += "   <input name=\"content\" value=\"\" type=\"hidden\">";
                output += "   <button type=\"submit\" class=\"btn btn-danger\">Delete</button>";
                output += "  </form>";
			}
            
            output += "  <form asp-page=\"EditComment\" method=\"post\">";
            output += "   <input name=\"cAction\" type=\"hidden\" value=\"reply\" />";
            output += "   <input name=\"guid\" type=\"hidden\" value=\""+cc.guid+"\" />";
            output += "   <input name=\"content\" class=\"form-control form-control-sm\" type=\"text\" placeholder=\"Comment\">";
            output += "   <button type=\"submit\" class=\"btn btn-secondary mb-2\">Reply</button>";
            output += "  </form>";
            output += " </div>";


            if (cc.HasComments)
			{
				_context.Entry(cc).Collection(c => c.Children).Load();
                
                output += " <ul>";
                foreach (var ccc in cc.Children)
				{
                    output += LoadComments(ccc);
                }
                output += " </ul>";
            }
            output += "</li>";

            return output;
        }
        
        public async Task<IActionResult> OnPostAsync(string PhotoId)
		{
            Guid photoGuid = new Guid(PhotoId);
            Error = null;

            userId = "";
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("Identity/Account/Login");
            }
            else
            {
                userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value; // získáme id přihlášeného uživatele
            }

			if (photoGuid == null)
			{
                Error = "no id";
                return Page();
			}


            Photo =  _context.Files.Where(f => f.Id == photoGuid).FirstOrDefault();
			if (Photo == null)
			{
                Error = "couldnt find photo";
                return Page();
			}
            
            Album album = _context.Albums.Where(a => (a.OwnerId == userId || a.Public) && a.Files.Any(a => a.Id == photoGuid) ).FirstOrDefault();
			if (album == null)
			{
                Error = "no album";
                return Page();
			}

            //check if accesibkle

            photosrc = Photo.Id + ".";
            string extention = Photo.ContentType.Split("/").Last();
            if (extention == "jpeg") extention = "jpg";
            photosrc += extention; 

            bytes = System.IO.File.ReadAllBytes(Path.Combine("Uploads",photosrc));
            base64img = System.Convert.ToBase64String(bytes);

            _context.Entry(Photo).Collection(p => p.Comments).Load();
            comments = Photo.Comments.ToList();

            HtmlComments = "<ul>";
			foreach (var c in comments)
			{
                HtmlComments += LoadComments(c);
			}
            HtmlComments += "</ul>";
           

            

            return Page();

        }
    }
}
