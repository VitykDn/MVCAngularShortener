namespace MVCAngularShortener.Infrastructure.Interfaces
{
    public interface IShortener
    {
        string Encode(string originalUrl);
        string Decode(string shortUrl);
    }
}
