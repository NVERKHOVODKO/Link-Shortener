using Microsoft.AspNetCore.Mvc;
using TestApplication.Models;
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

    
    /// <summary>
    /// Redirect to the full URL based on the provided short URL.
    /// </summary>
    /// <param name="shortUrl">The short URL to redirect to the corresponding full URL.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/links/{shortUrl}
    ///
    /// </remarks>
    /// <returns>
    /// - 302 Redirect to the full URL if the short URL is found.
    /// - 404 Not Found if the short URL is not found.
    /// </returns>
    [HttpGet("/{shortUrl}")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RedirectShortUrl(string shortUrl)
    {
        var fullUrl = await _linkService.GetFullUrl(shortUrl);
        if (fullUrl != null) return Redirect(fullUrl);
        return NotFound();
    }
    
    
    /// <summary>
    /// Get all links.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/links
    ///
    /// </remarks>
    /// <returns>
    /// 200 OK with the list of all links in the response body.
    /// </returns>
    /// <response code="200">Returns the list of all links.</response>
    [HttpGet("links")]
    [ProducesResponseType(typeof(IEnumerable<LinkEntity>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllLinks()
    {
        var links = await _linkService.GetAllLinks();
        return Ok(links);
    }

    
    /// <summary>
    /// Get a link by ID.
    /// </summary>
    /// <param name="id">The ID of the link to retrieve.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/links/{id}
    ///
    /// </remarks>
    /// <returns>
    /// 200 OK with the link details in the response body.
    /// </returns>
    /// <response code="200">Returns the requested link.</response>
    /// <response code="404">If the link with the specified ID is not found.</response>
    [HttpGet("links/{id}")]
    [ProducesResponseType(typeof(LinkEntity), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLink(Guid id)
    {
        var link = await _linkService.GetLinkAsync(id);
        return Ok(link);
    }
    
    
    /// <summary>
    /// Shorten a long URL.
    /// </summary>
    /// <param name="longUrl">The long URL to be shortened.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/links/shorten
    ///     {
    ///         "longUrl": "https://example.com/very/long/url"
    ///     }
    ///
    /// </remarks>
    /// <returns>
    /// 200 OK with the shortened URL in the response body.
    /// </returns>
    [HttpPost("shorten")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> ShortenUrl([FromBody] string longUrl)
    {
        var shortenUrl = await _linkService.ShortenUrlAsync(longUrl);
        return Ok(new { RedirectUrl = shortenUrl });
    }

    
    /// <summary>
    /// Delete a link by ID.
    /// </summary>
    /// <param name="id">The ID of the link to delete.</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     DELETE /api/links/{id}
    ///
    /// </remarks>
    /// <returns>
    /// 200 OK if the link is successfully deleted.
    /// </returns>
    /// <response code="200">Link deleted successfully.</response>
    /// <response code="404">If the link with the specified ID is not found.</response>
    [HttpDelete("links/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLink(Guid id)
    {
        await _linkService.DeleteLinkAsync(id);
        return Ok("Deleted");
    }
}