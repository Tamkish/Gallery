using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gallery.Data;
using Gallery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Gallery.Pages
{
    public class AddAlbumModel : PageModel
    {
        private ApplicationDbContext _context;
        private IConfiguration _configuration;

        public AddAlbumModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [BindProperty]
        public bool aPublic { get; set; }
        [BindProperty]
        public string aName { get; set; }

        public IActionResult OnGet()
        {
            string userId = "";
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("Identity/Account/Login");
            }
            else
            {
                userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value; // získáme id přihlášeného uživatele
            }

            aPublic = false;
            aName = "";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value; // získáme id přihlášeného uživatele

			if (_context.Albums.Where(a => a.OwnerId == userId && a.Name == aName).Count() > 0)
			{
                throw new Exception("name already exists");
			}

            Album newAlbum = new Album()
            {
                OwnerId = userId,
                Name = aName,
                Public = aPublic/*,
                Files = new List<StoredFile>()*/
            };

            _context.Albums.Add(newAlbum);


            await _context.SaveChangesAsync();
            return RedirectToPage("/ShowAlbum", new { aId = newAlbum.Name, uId = userId});

        }
    }
}
