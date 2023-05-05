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

        public UrlController(IUrlsRepository urlsRepository, IShortener shortener, ILogger<Url> logger)
        {
            _urlsRepository = urlsRepository;
            _shortenerService = shortener;
            _logger = logger;
        }


        // GET: Url
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching URLs from the repository for Index action.");

            string domain = HttpContext.Request.Host.Value;
            var urls = await _urlsRepository.GetAllAsync();
            var urlsViewModels = urls.Select(u => u.ToViewModelWithAddress(domain)).ToList();

            _logger.LogInformation("Returning URLs to the Index view.");

            return View(urlsViewModels);
        }

        [Authorize]
        // GET: Url/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await _urlsRepository.GetAllAsync() == null)
            {
                _logger.LogError("Invalid ID or empty URL repository for Details action.");

                return NotFound();
            }

            var url = await _urlsRepository.GetByIdAsync(id);

            if (url == null)
            {
                _logger.LogError("URL not found for ID: {Id}", id);

                return NotFound();
            }

            var urlViewModel = url.ToViewModel();

            _logger.LogInformation("Returning URL details for ID: {Id}", id);

            return View(urlViewModel);
        }
        [Authorize]
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
            _logger.LogInformation("Create action called with fullUrl: {FullUrl}", fullUrl);

            if (!await _urlsRepository.CheckUrl(fullUrl))
            {
                string loggedInUserName = User.Identity.Name;
                string shortUrl = _shortenerService.Encode(fullUrl);

                _logger.LogInformation("Creating new URL with fullUrl: {FullUrl}, shortUrl: {ShortUrl}, and createdBy: {CreatedBy}", fullUrl, shortUrl, loggedInUserName);

                var url = new Url
                {
                    FullUrl = fullUrl,
                    ShortUrl = shortUrl,
                    CreatedBy = loggedInUserName,
                    CreatedDate = DateTime.Now
                };

                await _urlsRepository.CreateAsync(url);
                _logger.LogInformation("New URL created successfully.");

                return RedirectToAction("Index");
            }

            _logger.LogInformation("URL already exists. Displaying the Create view again.");
            return View();
        }




        [Authorize]

        // GET: Url/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _urlsRepository.GetAllAsync() == null)
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
        [Authorize]

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

                    _logger.LogInformation("URL deleted for ID: {Id}", id);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the URL.");

                return Problem("An error occurred while deleting the URL.", statusCode: 500);
            }
        }

        [HttpGet("/Url/RedirectTo/{pathShort:required}", Name = "Url_RedirectTo")]
        public async Task<IActionResult> RedirectToAsync(string pathShort)
        {
            if (pathShort == null)
            {
                _logger.LogError("Invalid path for RedirectTo action.");

                return NotFound();
            }

            var url = await _urlsRepository.GetUrlByPath(pathShort);

            if (url == null)
            {
                _logger.LogError("URL not found for path: {Path}", pathShort);

                return NotFound();
            }

            _logger.LogInformation("Redirecting to URL: {Url}", url.FullUrl);

            return Redirect(url.FullUrl);
        }
    }
}
