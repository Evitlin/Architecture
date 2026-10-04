using ArchitectureCatalog.API.DTOs.ArchitecturalStyles;
using ArchitectureCatalog.API.Services.ArchitecturalStyles;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectureCatalog.API.Controllers;

[ApiController]
[Route("api/architectural-styles")]
public class ArchitecturalStylesController : ControllerBase
{
    private readonly IArchitecturalStyleService _service;

    public ArchitecturalStylesController(IArchitecturalStyleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArchitecturalStyleDto>>> GetAll()
    {
        var styles = await _service.GetAllAsync();
        return Ok(styles);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ArchitecturalStyleDto>> GetById(int id)
    {
        var style = await _service.GetByIdAsync(id);
        if (style is null)
        {
            return NotFound();
        }

        return Ok(style);
    }

    [HttpPost]
    public async Task<ActionResult<ArchitecturalStyleDto>> Create([FromBody] CreateArchitecturalStyleDto dto)
    {
        if (dto is null)
        {
            return BadRequest();
        }

        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateArchitecturalStyleDto dto)
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
