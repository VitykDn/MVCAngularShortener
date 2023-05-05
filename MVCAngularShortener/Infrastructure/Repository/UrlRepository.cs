using Microsoft.EntityFrameworkCore;
using MVCAngularShortener.Data;
using MVCAngularShortener.Infrastructure.Interfaces;
using MVCAngularShortener.Infrastructure.Services;
using MVCAngularShortener.Models;

namespace MVCAngularShortener.Infrastructure.Repository
{
    public class UrlRepository : RepositoryBase<Url>, IUrlsRepository
    {
        public UrlRepository(ApplicationDbContext dbContext, ILogger<UrlRepository> logger) : base(dbContext,logger)
        {
        }

        public async Task<bool> CheckUrl(string newUrl)
        {
            _logger.LogInformation("Checking URL: {NewUrl}", newUrl);

            bool check = await _dbContext.Set<Url>().AnyAsync(u => u.FullUrl == newUrl);

            _logger.LogInformation("URL check result: {CheckResult}", check);

            return check;
        }

        public async Task<IEnumerable<Url>> GetAllUserUrls()
        {
            _logger.LogInformation("Getting all user URLs.");

            throw new NotImplementedException();
        }

        public async Task<Url> GetUrlByPath(string path)
        {
            _logger.LogInformation("Getting URL by path: {Path}", path);

            var url = await _dbContext.Set<Url>().FirstOrDefaultAsync(u => u.ShortUrl == path);

            if (url != null)
            {
                _logger.LogInformation("URL found: {Url}", url);
            }
            else
            {
                _logger.LogInformation("URL not found for path: {Path}", path);
            }

            return url;
        }
    }
}

