using ArchitectureCatalog.API.DTOs.Comments;
using ArchitectureCatalog.API.DTOs.Common;
using ArchitectureCatalog.API.Hateoas;
using ArchitectureCatalog.API.Services.Comments;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectureCatalog.API.Controllers;

[ApiController]
[Route("api/architectural-styles/{styleId:int}/buildings/{buildingId:int}/comments")]
public class StyleBuildingCommentsController : ControllerBase
{
    private readonly ICommentService _service;

    public StyleBuildingCommentsController(ICommentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CommentDto>>> GetAll(int styleId, int buildingId)
    {
        var comments = await _service.GetByStyleAndBuildingAsync(styleId, buildingId);
        if (comments is null)
        {
            return NotFound();
        }

        foreach (var comment in comments)
        {
            comment.Links = HateoasLinks.ForComment(Url, comment.Id, comment.BuildingId);
            comment.Links.Add(new LinkDto(
                "collection",
                Url.Action(nameof(GetAll), new { styleId, buildingId })!,
                "GET"));
        }

        return Ok(comments);
    }
}
