using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ProjectX.Exceptions;
using Repository;
using TestApplication.Models;

namespace TestApplication.Services;

public class LinkService : ILinkService
{
    private readonly IDbRepository _repository;

    public LinkService(IDbRepository repository)
    {
        _repository = repository;
    }


    public async Task<LinkEntity> GetLinkAsync(Guid id)
    {
        var link = await _repository.Get<LinkEntity>(x => x.Id == id).FirstOrDefaultAsync();
        if (link == null) throw new EntityNotFoundException("Link not found");
        return link;
    }


    public async Task<IEnumerable<LinkEntity>> GetAllLinks()
    {
        return _repository.GetAll<LinkEntity>();
    }


    public async Task DeleteLinkAsync(Guid id)
    {
        var user = await _repository.Get<LinkEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (user == null) throw new EntityNotFoundException("Link not found");
        await _repository.Delete<LinkEntity>(id);
        await _repository.SaveChangesAsync();
    }
    

    public async Task<string> ShortenUrlAsync(string longUrl)
    {
        if (!await IsLinkExists(longUrl))
        {
            var shortUrl = await GenerateShortUrl(longUrl);

            var entity = new LinkEntity
            {
                LongUrl = longUrl,
                ShortUrl = $"http://localhost:5180/{shortUrl}",
                //ShortUrl = shortUrl,
                DateCreated = DateTime.UtcNow,
                ClickCount = 0
            };

            await _repository.Add(entity);
            await _repository.SaveChangesAsync();
            return entity.ShortUrl;
        }

        var link = await _repository.Get<LinkEntity>(x => x.LongUrl == longUrl).FirstOrDefaultAsync();
        await _repository.SaveChangesAsync();
        return link.ShortUrl;
    }

    
    public async Task<string> GetFullUrl(string shortUrl)
    {
        var link = await _repository.Get<LinkEntity>(x => x.ShortUrl == $"http://localhost:5180/{shortUrl}")
            .FirstOrDefaultAsync();
        await IncrementClickCount(shortUrl);
        return link.LongUrl;
    }
    

    public async Task IncrementClickCount(string shortUrl)
    {
        var link = await _repository.Get<LinkEntity>(x => x.ShortUrl == $"http://localhost:5180/{shortUrl}")
            .FirstOrDefaultAsync();
        if (link != null)
        {
            link.ClickCount++;
            await _repository.SaveChangesAsync();
        }
    }

    
    private async Task<string> GenerateShortUrl(string longUrl)
    {
        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes(longUrl);
            var hashBytes = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (var i = 0; i < hashBytes.Length; i++) sb.Append(hashBytes[i].ToString("x2"));

            return sb.ToString().Substring(0, 8);
        }
    }
    

    public async Task<bool> IsLinkExists(string longUrl)
    {
        var like = await _repository.Get<LinkEntity>(x => x.LongUrl == longUrl).FirstOrDefaultAsync();
        return like != null;
    }
}