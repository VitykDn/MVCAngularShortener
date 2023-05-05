using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Data;

namespace MVCAngularShortener.Views.Pages
{
    [AllowAnonymous]
    public class AboutModel : PageModel
    {
        private readonly ILogger<AboutModel> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AboutModel(ILogger<AboutModel> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }
        [BindProperty]
        public string AlgorithmDescription { get; set; }


        public void OnGet()
        {
            string filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Infrastructure", "Data", "Algorithm.json");

            if (System.IO.File.Exists(filePath))
            {
                string jsonData = System.IO.File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<dynamic>(jsonData);
                AlgorithmDescription = data?.AlgorithmDescription;
            }
            else
            {
                AlgorithmDescription = "Something went wrong";
            }
        }


        public IActionResult OnPost()
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }
            string filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Infrastructure", "Data", "Algorithm.json");

            dynamic data = new
            {
                AlgorithmDescription = AlgorithmDescription
            };

            string jsonData = JsonConvert.SerializeObject(data);
            System.IO.File.WriteAllText(filePath, jsonData);


            return RedirectToPage();
        }
    }
}
