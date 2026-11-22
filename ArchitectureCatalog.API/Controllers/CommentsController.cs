using System.Security.Claims;
using ArchitectureCatalog.API.Auth;
using ArchitectureCatalog.API.DTOs.Comments;
using ArchitectureCatalog.API.DTOs.Common;
using ArchitectureCatalog.API.Hateoas;
using ArchitectureCatalog.API.Services.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectureCatalog.API.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _service;

    public CommentsController(ICommentService service)
    {
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PagedResult<CommentDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? buildingId = null)
    {
        var result = await _service.GetAllAsync(page, pageSize, buildingId);

        foreach (var item in result.Items)
        {
            item.Links = HateoasLinks.ForComment(Url, item.Id, item.BuildingId);
        }

        result.Links = HateoasLinks.ForPagedCollection(
            Url,
            nameof(GetAll),
            "Comments",
            result.Page,
            result.PageSize,
            result.TotalPages,
            new { buildingId });

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetById(int id)
    {
        var comment = await _service.GetByIdAsync(id);
        if (comment is null)
        {
            return NotFound();
        }

        comment.Links = HateoasLinks.ForComment(Url, comment.Id, comment.BuildingId);
        return Ok(comment);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create([FromBody] CreateCommentDto dto)
    {
        if (dto is null)
        {
            return BadRequest();
        }

        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var created = await _service.CreateAsync(dto, userId);
        if (created is null)
        {
            return NotFound();
        }

        created.Links = HateoasLinks.ForComment(Url, created.Id, created.BuildingId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCommentDto dto)
    {
        if (dto is null)
        {
            return BadRequest();
        }

        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        try
        {
            var updated = await _service.UpdateAsync(id, dto, userId, User.IsInRole(Roles.Administrator));
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        try
        {
            var deleted = await _service.DeleteAsync(id, userId, User.IsInRole(Roles.Administrator));
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    private bool TryGetUserId(out int userId)
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out userId);
    }
}
