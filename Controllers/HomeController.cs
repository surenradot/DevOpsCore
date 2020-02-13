using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreApp.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace CoreApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;
        public HomeController(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;

            if (!UserList.usertList.Any())
            {
                UserList.usertList = GetUserList();
            }
        }
      
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult AddEdit(int? id)
        {
            UserDto user = id.HasValue?UserList.usertList.FirstOrDefault(a => a.Id == id):new UserDto();
            return View(user);
        } 

        [HttpPost]
        public async Task<IActionResult> AddEdit(UserDto model)
        {
            int id = model.Id == 0 ? (UserList.usertList.Max(x => x.Id) + 1) : model.Id;

           
            string imagName = model.Image;
            if (Request.Form!=null &&Request.Form.Files != null  && Request.Form.Files.Any())
            {
                var file = Request.Form.Files[0];
                imagName = GetImageName(id, file);          
            }



            model.Id = id;
            model.Image = imagName;
            if (UserList.usertList.Any(a => a.Id == id))
            {
                var user = UserList.usertList.FirstOrDefault(a => a.Id == id);
                UserList.usertList.Remove(user);
            }
            UserList.usertList.Add(model);
            return RedirectToAction("User");
        }
        public IActionResult User()
        {
            return View(UserList.usertList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<UserDto> GetUserList()
        {
            List<UserDto> useList = new List<UserDto>();
            useList.Add(new UserDto()
            {
                Id = 1,
                DOB = new DateTime(1992, 08, 5),
                Email = "surendra@gmail.com",
                Name = "Surendra"
            });
            useList.Add(new UserDto()
            {
                Id = 2,
                DOB = new DateTime(1990, 12, 30),
                Email = "Vikash@gmail.com",
                Name = "Vikash"
            });
            useList.Add(new UserDto()
            {
                Id = 3,
                DOB = new DateTime(1988, 05, 08),
                Email = "Yogendra@gmail.com",
                Name = "Yogendra"
            });
            useList.Add(new UserDto()
            {
                Id = 3,
                DOB = new DateTime(1995, 02, 28),
                Email = "Hemendra@gmail.com",
                Name = "Hemendra"
            });
            useList.Add(new UserDto()
            {
                Id = 4,
                DOB = new DateTime(1993, 02, 5),
                Email = "Arvind@gmail.com",
                Name = "Arvind"
            });

            return useList;
        }

        private string GetImageName(int propertyId, IFormFile Image)
        {
            string imgNames = string.Empty;           
            var webRoot = _appEnvironment.WebRootPath;
            int index = 1;

                if (Image != null && Image.Length > 0)
                {
                    var file = Image;
                    var pathWithFolderName = Path.Combine(webRoot, $"images");
                    if (!Directory.Exists(pathWithFolderName))
                    {
                        Directory.CreateDirectory(pathWithFolderName);
                    }
                    if (file.Length > 0)
                    {
                        var fileName = $"{propertyId}-{index}_" + Path.GetFileName(file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(pathWithFolderName, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                            imgNames=fileName;
                        }
                    }
                
                index++;
            }

            return imgNames;
        }
    }
}
