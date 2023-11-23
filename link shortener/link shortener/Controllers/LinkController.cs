using Microsoft.AspNetCore.Mvc;
using TestApplication.Services;

namespace TestApplication.Controllers;

//[Authorize]
[ApiController]
[Produces("application/json")]
[Route("[controller]")]
public class LinkController : ControllerBase
{
    private readonly ILinkService _linkService;

    public LinkController(ILinkService linkService)
    {
        _linkService = linkService;
    }


    [HttpGet("links")]
    public async Task<IActionResult> GetAllLinks()
    {
        var links = await _linkService.GetAllLinks();
        return Ok(links);
    }


    [HttpGet("links/{id}")]
    public async Task<IActionResult> GetLink(Guid id)
    {
        var link = await _linkService.GetLinkAsync(id);
        return Ok(link);
    }


    [HttpDelete("links/{id}")]
    public async Task<IActionResult> DeleteLink(Guid id)
    {
        await _linkService.DeleteLinkAsync(id);
        return Ok("Deleted");
    }


    [HttpPost("shorten")]
    public async Task<IActionResult> ShortenUrl([FromBody] string longUrl)
    {
        var shortenUrl = await _linkService.ShortenUrlAsync(longUrl);
        return Ok(new { RedirectUrl = shortenUrl });
    }
    
    
    [HttpGet("/{shortUrl}")]
    public async Task<IActionResult> RedirectShortUrl(string shortUrl)
    {
        string fullUrl = await _linkService.GetFullUrl(shortUrl);
        Console.WriteLine(fullUrl);
        if (fullUrl != null)
        {
            return Redirect(fullUrl);
        }

        return NotFound();
    }
}