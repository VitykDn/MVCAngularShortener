namespace MVCAngularShortener.Models.ViewModels
{
    public static class UrlViewModelExtensions
    {
        public static UrlViewModel ToViewModelWithAddress(this Url entity, string domain)
        {
            return new UrlViewModel
            {
                Id = entity.Id,
                FullUrl = entity.FullUrl,
                ShortUrl  = "https://" + domain + "/Url/RedirectTo/" + entity.ShortUrl,
                CreatedBy = entity.CreatedBy,
                CreatedDate = entity.CreatedDate
            };
        }
        public static UrlViewModel ToViewModel(this Url entity)
        {
            return new UrlViewModel
            {
                Id = entity.Id,
                FullUrl = entity.FullUrl,
                ShortUrl =  entity.ShortUrl,
                CreatedBy = entity.CreatedBy,
                CreatedDate = entity.CreatedDate
            };
        }
    }
    public class UrlViewModel
    {
        public int Id { get; set; }
        public string FullUrl { get; set; }
        public string ShortUrl { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
