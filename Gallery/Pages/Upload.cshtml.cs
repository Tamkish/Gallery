using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gallery.Data;
using Gallery.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace Gallery.Pages
{
    public class UploadModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public int _sameAspectRatioHeigth;
        public int _squareSize;

        public UploadModel(IWebHostEnvironment environment, ApplicationDbContext context, IConfiguration configuration)
        {
            _environment = environment;

            _context = context;

            _configuration = configuration;

            if (int.TryParse(_configuration["Thumbnails:SquareSize"], out _squareSize) == false)
                _squareSize = 64; // získej data z konfigurave nebo použij 64

            if (int.TryParse(_configuration["Thumbnails:SameAspectRatioHeigth"], out _sameAspectRatioHeigth) == false)
                _sameAspectRatioHeigth = 128;
        }

        [TempData] public string SuccessMessage { get; set; }

        [TempData] public string ErrorMessage { get; set; }

        public ICollection<IFormFile> Upload { get; set; }

        [BindProperty] public string UploadAlbumId { get; set; }

        public List<Album> Albums { get; set; }
        public string Name { get; set; }

        public IActionResult OnGet([FromQuery(Name = "aId")] string aId)
        {
            var userId = "";
            if (!User.Identity.IsAuthenticated)
                return Redirect("Identity/Account/Login");
            userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()
                .Value; // získáme id přihlášeného uživatele

            Albums = _context.Albums.Where(a => a.OwnerId == userId).ToList();

            Name = Albums.First().Name;
            if (aId != null) Name = aId;

            return Page();
        }

        /*       public async Task<IActionResult> OnPostAsync()
               {
                   int successfulProcessing = 0;
                   int failedProcessing = 0;
                   foreach (var uploadedFile in Upload)
                   {
                       try
                       {
                           var file = Path.Combine(_environment.ContentRootPath, "Uploads", uploadedFile.FileName);
                           using (var fileStream = new FileStream(file, FileMode.Create))
                           {
                               await uploadedFile.CopyToAsync(fileStream);
                           };
                           successfulProcessing++;
                       }
                       catch
                       {
                           failedProcessing++;
                       }
                       if (failedProcessing == 0)
                       {
                           SuccessMessage = "All files has been uploaded successfuly.";
                       }
                       else
                       {
                           ErrorMessage = "There were <b>" + failedProcessing + "</b> errors during uploading and processing of files.";
                       }
                   }
                   return RedirectToPage("/Index");
               }*/

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()
                .Value; // získáme id přihlášeného uživatele
            var successfulProcessing = 0;
            var failedProcessing = 0;

            var uploadAlbum = _context.Albums.Where(a => a.OwnerId == userId && a.Name == UploadAlbumId)
                .FirstOrDefault();

            if (uploadAlbum == null) throw new Exception("couldnt find album to upload to");

            foreach (var uploadedFile in Upload)
                try
                {
                    var fileRecord = new StoredFile
                    {
                        OriginalName = uploadedFile.FileName,
                        UploaderId = userId,
                        UploadedAt = DateTime.Now,
                        ContentType = uploadedFile.ContentType
                    };

                    if (uploadedFile.ContentType.StartsWith("image")) // je soubor obrázek?

                    {
                        fileRecord.Thumbnails = new List<ThumbnailBlob>();

                        var ims = new MemoryStream(); // proud pro příchozí obrázek

                        var oms1 = new MemoryStream(); // proud pro čtvercový náhled

                        var oms2 = new MemoryStream(); // proud pro obdélníkový náhled

                        uploadedFile.CopyTo(ims); // vlož obsah do vstupního proudu

                        IImageFormat
                            format; // zde si uložíme formát obrázku (JPEG, GIF, ...), budeme ho potřebovat při ukládání

                        using (var image = Image.Load(ims.ToArray(), out format)) // vytvoříme čtvercový náhled
                        {
                            var largestSize = Math.Max(image.Height, image.Width); // jaká je orientace obrázku?

                            if (image.Width > image.Height) // podle orientace změníme velikost obrázku
                                image.Mutate(x => x.Resize(0, _squareSize));
                            else
                                image.Mutate(x => x.Resize(_squareSize, 0));

                            image.Mutate(x => x.Crop(new Rectangle((image.Width - _squareSize) / 2,
                                (image.Height - _squareSize) / 2, _squareSize, _squareSize)));

                            // obrázek ořízneme na čtverec

                            image.Save(oms1, format); // vložíme ho do výstupního proudu

                            fileRecord.Thumbnails.Add(new ThumbnailBlob
                            {
                                File = fileRecord,
                                FileId = fileRecord.Id,
                                Type = ThumbnailType.Square,
                                Blob = oms1.ToArray()
                            }); // a uložíme do databáze jako pole bytů
                        }

                        using (var image = Image.Load(ims.ToArray(), out format)) // obdélníkový náhled začíná zde

                        {
                            image.Mutate(x => x.Resize(0, _sameAspectRatioHeigth)); // stačí jen změnit jeho velikost

                            image.Save(oms2, format); // a přes proud ho uložit do databáze

                            fileRecord.Thumbnails.Add(new ThumbnailBlob
                            {
                                File = fileRecord,
                                FileId = fileRecord.Id,
                                Type = ThumbnailType.SameAspectRatio,
                                Blob = oms2.ToArray()
                            });
                        }
                    }

                    _context.Files.Add(fileRecord);
                    var album = _context.Albums
                        .Where(a => a.OwnerId == uploadAlbum.OwnerId && a.Name == uploadAlbum.Name).FirstOrDefault();
                    _context.Entry(album).Collection(a => a.Files).Load();
                    if (album != null)
                        album.Files.Add(fileRecord);
                    else
                        throw new Exception("nenaslo album");

                    //Files.Add(fileRecord);


                    ///*Find(uploadAlbum.OwnerId, uploadAlbum.Name)/**/.FirstOrDefault().Files.Add(fileRecord);

                    if (!Directory.Exists(Path.Combine(_environment.ContentRootPath, "Uploads")))
                        Directory.CreateDirectory(Path.Combine(_environment.ContentRootPath, "Uploads"));

                    var file = Path.Combine(_environment.ContentRootPath, "Uploads",
                        fileRecord.Id + "." + uploadedFile.FileName.Split(".").Last());
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }

                    ;
                    successfulProcessing++;
                }
                catch
                {
                    failedProcessing++;
                    //...
                }

            //show how many 

            //...

            await _context.SaveChangesAsync();
            return RedirectToPage("/Index");
        }
    }
}