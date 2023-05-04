namespace MVCAngularShortener.Models
{
    public class Url
    {
        public int Id { get; set; }
        public string FullUrl { get; set; }
        public string ShortUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
