﻿using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVCApp.Models;

namespace MVCApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ImageStore _imageStore = new ImageStore();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Index(string name, string description, HttpPostedFileBase image, int isPublic)
        {
            var result = await _imageStore.SaveImage(image.InputStream);
            var item = new GalleryItem { Id = result, Description = description, Name = name, Uri = $"{ConfigurationManager.AppSettings["baseUri"]}/{ConfigurationManager.AppSettings["containerName"]}/{result}", AuthorId = User.Identity.GetUserId(), IsPublic = isPublic == 1};
            await CosmosDb<GalleryItem>.CreateItemAsync(item);
            return View((object)item.Name);
        }
    }
}