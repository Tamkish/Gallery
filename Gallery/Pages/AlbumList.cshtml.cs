using Gallery.Data;
using Gallery.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gallery.Pages
{
    public class AlbumListModel : PageModel
    {
        private IWebHostEnvironment _environment;
        private readonly ILogger<AlbumListModel> _logger;
        private ApplicationDbContext _context;

        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }

        public List<Album> Albums { get; set; } = new List<Album>();
        //public List<string> Files { get; set; } = new List<string>();

        public AlbumListModel(ILogger<AlbumListModel> logger, IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _environment = environment;
            _logger = logger;
            _context = context;
        }

        public IActionResult OnGet([FromQuery(Name = "uId")] string uId /*album owner*/)
        {
            string userId = "";//logged in user
			if (!User.Identity.IsAuthenticated)
            {
                return Redirect("Identity/Account/Login");
			}
			else
			{
                userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value; // získáme id přihlášeného uživatele
            }


            Albums = _context.Albums.Where(a => a.OwnerId == userId).ToList();

            /*
            var fullNames = Directory.GetFiles(Path.Combine(_environment.ContentRootPath, "Uploads")).ToList();
            foreach (var fn in fullNames)
            {
                Files.Add(Path.GetFileName(fn));
            }
            */
            return Page();
        }

        /*public IActionResult OnGetDownload(string filename)
        {
            var fullName = Path.Combine(_environment.ContentRootPath, "Uploads", filename);
            if (System.IO.File.Exists(fullName))
            {
                return PhysicalFile(fullName, MediaTypeNames.Application.Octet, filename);
            }
            else
            //return NotFound();
            {
                ErrorMessage = "There is no such file.";
                return RedirectToPage();
            }
        }*/
    }
}
