using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Gallery.Data;
using Gallery.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Gallery.Pages
{
    public class AlbumListModel : PageModel
    {
        private readonly ILogger<AlbumListModel> _logger;
        private readonly ApplicationDbContext _context;

        private IWebHostEnvironment _environment;
        //public List<string> Files { get; set; } = new List<string>();

        public AlbumListModel(ILogger<AlbumListModel> logger, IWebHostEnvironment environment,
            ApplicationDbContext context)
        {
            _environment = environment;
            _logger = logger;
            _context = context;
        }

        [TempData] public string SuccessMessage { get; set; }

        [TempData] public string ErrorMessage { get; set; }

        public List<Album> Albums { get; set; } = new List<Album>();

        public IActionResult OnGet([FromQuery(Name = "uId")] string uId /*album owner*/)
        {
            var userId = ""; //logged in user
            if (!User.Identity.IsAuthenticated)
                return Redirect("Identity/Account/Login");
            userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()
                .Value; // získáme id přihlášeného uživatele


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