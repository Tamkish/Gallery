using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Gallery.Data;
using Gallery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gallery.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;


        public List<StoredFile> files;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var userId = "";
            if (!User.Identity.IsAuthenticated)
                return Redirect("Identity/Account/Login");
            userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()
                .Value; // získáme id přihlášeného uživatele
            //                                                                                                  
            /*files =_context.Files
                .OrderByDescending(f => f.UploadedAt)
                .Where(f =>
                    _context.Albums.Where(a =>
                        a.Files.Any(g => g.Id == f.Id) && a.Public)
                    .ToList().Count > 0)
                .Take(12).ToList();
            */
            files = new List<StoredFile>();
            var albums = _context.Albums.Where(a => a.Public);
            var filez = _context.Files.OrderByDescending(f => f.UploadedAt);

            foreach (var file in filez)
            {
                if (albums.Any(a => a.Files.Any(f => f.Id == file.Id)))
                {
                    _context.Entry(file).Collection(f => f.Thumbnails).Load();
                    files.Add(file);
                }

                if (files.Count >= 12) break;
            }


            return Page();
        }
    }
}