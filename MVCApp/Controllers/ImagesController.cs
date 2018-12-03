using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVCApp.Models;

namespace MVCApp.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ImageStore _imageStore = new ImageStore();
        private readonly GallerySearch _search = new GallerySearch();

        public async Task<ActionResult> Index()
        {
            var res = (await CosmosDb<GalleryItem>.GetItemsAsync((item) => true)).Where(item => item.IsPublic != false || item.AuthorId == User.Identity.GetUserId());
            return View(res);
        }

        public async Task<ActionResult> RemoveImage(Uri uri)
        {
            await _imageStore.RemoveFile(uri);
            await CosmosDb<GalleryItem>.DeleteItemAsync(uri.LocalPath.Remove(0, 8));
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> EditImage(string id)
        {
            var item = await CosmosDb<GalleryItem>.GetItemAsync(id);
            return View(item);
        }

        [HttpPost]
        public async Task<ActionResult> EditImage([Bind(Include = "Id,Name,Description,Uri")] GalleryItem galleryItem)
        {
            if (ModelState.IsValid)
            {
                await CosmosDb<GalleryItem>.UpdateItemAsync(galleryItem.Id, galleryItem);
                return RedirectToAction("Index");
            }
            return View(galleryItem);
        }

        public async Task<ActionResult> ViewImage(Uri uri)
        {
            var image = await CosmosDb<GalleryItem>.GetItemAsync(uri.LocalPath.Remove(0, 7));
            return View(image);
        }

        public async Task<ActionResult> Search(string term)
        {
            var result = _search.Search(term).Select(i => i.Id);
            var model = new List<GalleryItem>();
            foreach (var item in result)
            {
                model.Add(await CosmosDb<GalleryItem>.GetItemAsync(item));
            }
            return View(model.Where(item => item.AuthorId == null || item.IsPublic == true || item.AuthorId == User.Identity.GetUserId()));
        }

        [Authorize]
        public async Task<ActionResult> MyImages()
        {
            var res = (await CosmosDb<GalleryItem>.GetItemsAsync((item) => true)).Where(item => item.IsPublic != false || item.AuthorId == User.Identity.GetUserId()).Where(item => item.AuthorId == User.Identity.GetUserId());
            var model = new List<GalleryItem>();
            foreach (var item in res)
            {
                model.Add(item);
            }
            return View(model);
        }
    }
}