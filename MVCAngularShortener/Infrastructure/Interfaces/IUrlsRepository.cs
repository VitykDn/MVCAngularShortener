using MVCAngularShortener.Data.Interfaces;
using MVCAngularShortener.Models;

namespace MVCAngularShortener.Infrastructure.Interfaces
{
    public interface IUrlsRepository : IRepositoryBase<Url>
    {
        Task<IEnumerable<Url>> GetAllUserUrls();
        Task<bool> CheckUrl(string newUrl);

        Task<Url> GetUrlByPath(string path);
    }
}
