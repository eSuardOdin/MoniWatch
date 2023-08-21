using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using MoniWatch.Entities;
using MoniWatch.DataContext;



namespace MoniWatch.Api.Controllers;

[ApiController]
[Route("tag")]
public class TagController : ControllerBase
{
    private readonly ILogger<TagController> _logger;
    public TagController(ILogger<TagController> logger)
    {
        _logger = logger;
    }


    // o--------------o
    // | GET ALL TAGS |
    // o--------------o
    /// <summary>
    /// Get all tags for a specific user with URL: /root/tag?MoniId={moniId}</br>
    /// Or all tags with URL: /root/tag
    /// </summary>
    /// <param name="moniId">The id of tag's owner</param>
    /// <returns>An array of tags</returns>
    [HttpGet(Name="GetAllTags")]
    public async Task<IEnumerable<Tag>> GetAllTags(int? moniId)
    {
        using(MoniWatchDbContext db = new())
        {
            // Get all tags associated with a specific user
            if(moniId.HasValue)
            {
                return await db.Tags.Where(t => t.MoniId == moniId).ToArrayAsync();
            }
            // Get all tags (I don't see any usecase yet, may remove null forgiving operator on parameter moniId)
            else
            {
                return await db.Tags.ToArrayAsync();
            }
        }
    }
}