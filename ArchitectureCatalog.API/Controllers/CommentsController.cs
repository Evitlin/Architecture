using ArchitectureCatalog.API.DTOs.Comments;
using ArchitectureCatalog.API.DTOs.Common;
using ArchitectureCatalog.API.Services.Comments;
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

    [HttpGet]
    public async Task<ActionResult<PagedResult<CommentDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? buildingId = null)
    {
        var result = await _service.GetAllAsync(page, pageSize, buildingId);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetById(int id)
    {
        var comment = await _service.GetByIdAsync(id);
        if (comment is null)
        {
            return NotFound();
        }

        return Ok(comment);
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create([FromBody] CreateCommentDto dto)
    {
        if (dto is null)
        {
            return BadRequest();
        }

        var created = await _service.CreateAsync(dto);
        if (created is null)
        {
            return NotFound();
        }

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCommentDto dto)
    {
        if (dto is null)
        {
            return BadRequest();
        }

        var updated = await _service.UpdateAsync(id, dto);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
