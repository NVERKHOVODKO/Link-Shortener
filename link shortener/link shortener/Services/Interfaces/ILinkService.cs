using link_shortener.DTO;
using TestApplication.Models;

namespace TestApplication.Services;

public interface ILinkService
{
    Task<IEnumerable<LinkEntity>> GetAllLinks();
    public Task<string> ShortenUrlAsync(string longUrl);
    public Task<LinkEntity> GetLinkAsync(Guid id);
    public Task DeleteLinkAsync(Guid id);
    public Task<string> GetFullUrl(string shortUrl);
    public Task EditLinkAsync(EditLinkRequest request);
}