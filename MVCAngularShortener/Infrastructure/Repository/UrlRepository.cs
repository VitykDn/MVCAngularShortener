using Microsoft.EntityFrameworkCore;
using MVCAngularShortener.Data;
using MVCAngularShortener.Infrastructure.Interfaces;
using MVCAngularShortener.Infrastructure.Services;
using MVCAngularShortener.Models;

namespace MVCAngularShortener.Infrastructure.Repository
{
    public class UrlRepository : RepositoryBase<Url>, IUrlsRepository
    {
        public UrlRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CheckUrl(string newUrl)
        {
            bool check = await _dbContext.Set<Url>().AnyAsync(u => u.FullUrl == newUrl);
            return check;
        }

        public Task<IEnumerable<Url>> GetAllUserUrls()
        {
            throw new NotImplementedException();
        }

        public async Task<Url> GetUrlByPath(string path)
        {

                return await _dbContext.Set<Url>().FirstOrDefaultAsync(u => u.ShortUrl == path);

        }
    }
}
