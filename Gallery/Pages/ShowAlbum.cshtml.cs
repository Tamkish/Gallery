using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gallery.Data;
using Gallery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gallery.Pages
{
    public class ShowAlbumModel : PageModel
    {
        private ApplicationDbContext _context;
        public ShowAlbumModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<StoredFile> Photos { get; set; }
		public Album Album { get; set; }


		public IActionResult OnGet([FromQuery(Name = "aId")] string aId /*album id (unique name)*/, [FromQuery(Name = "uId")] string uId /*user id*/)
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


            Album = _context.Albums.Where(a => a.Name == aId && a.OwnerId == uId).FirstOrDefault();

			if (Album == null)
			{
                Photos = null;
			}
			else
			{
                _context.Entry(Album).Collection(a => a.Files).Load();

				foreach (var photo in Album.Files)
				{
                    _context.Entry(photo).Collection(a => a.Thumbnails).Load();
                }


                if (Album.Files == null)
                {
                    Photos = new List<StoredFile>();
                }
                else
                {
                    Photos = Album.Files.ToList();

                }
			}



            return Page();
        }
    }
}
