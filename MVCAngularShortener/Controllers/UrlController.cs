using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCAngularShortener.Data;
using MVCAngularShortener.Infrastructure.Interfaces;
using MVCAngularShortener.Models;
using MVCAngularShortener.Models.ViewModels;

namespace MVCAngularShortener.Controllers
{
    public class UrlController : Controller
    {
        private readonly IUrlsRepository _urlsRepository;
        private readonly ILogger<Url> _logger;
        private readonly IShortener _shortenerService;

        public UrlController(IUrlsRepository urlsRepository, IShortener shortener)
        {
            _urlsRepository = urlsRepository;
            _shortenerService = shortener;
        }


        // GET: Url
        public async Task<IActionResult> Index()
        {
            string domain = HttpContext.Request.Host.Value;
            var urls = await _urlsRepository.GetAllAsync();
            var urlsViewModels = urls.Select(u =>u.ToViewModelWithAddress(domain)).ToList();
            return View(urlsViewModels);
        }

        // GET: Url/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _urlsRepository.GetAllAsync() == null)
            {
                return NotFound();
            }

            var url = await _urlsRepository.GetByIdAsync(id);
            if (url == null)
            {
                return NotFound();
            }
            var urlViewModel = url.ToViewModel();
            return View(urlViewModel);
        }

        // GET: Url/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Url/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string fullUrl)
        {
            if (!await _urlsRepository.CheckUrl(fullUrl))
            {
                string loggedInUserName = User.Identity.Name;
                string shortUrl = _shortenerService.Encode(fullUrl);
                var url = new Url
                {
                    FullUrl = fullUrl,
                    ShortUrl = shortUrl,
                    CreatedBy = loggedInUserName,
                    CreatedDate = DateTime.Now
                };
                await _urlsRepository.CreateAsync(url);
                return RedirectToAction("Index");
            }
            return View();
        }





        // GET: Url/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _urlsRepository.GetAllAsync()== null)
            {
                return NotFound();
            }

            var url = await _urlsRepository.GetByIdAsync(id);
            var urlModel = url.ToViewModel();
            if (url == null)
            {
                return NotFound();
            }

            return View(urlModel);
        }

        // POST: Url/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var url = await _urlsRepository.GetByIdAsync(id);
                if (url != null)
                {
                    await _urlsRepository.DeleteAsync(url);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return Problem("An error occurred while deleting the URL.", statusCode: 500);
            }
        }
        [HttpGet("/Url/RedirectTo/{pathShort:required}", Name = "Url_RedirectTo")]
        public async Task<IActionResult> RedirectToAsync(string pathShort)
        {
            if (pathShort == null)
            {
                return NotFound();
            }

            var url =  await _urlsRepository.GetUrlByPath(pathShort);
            if (url == null)
            {
                return NotFound();
            }

            return  Redirect(url.FullUrl);
        }
    }
}
