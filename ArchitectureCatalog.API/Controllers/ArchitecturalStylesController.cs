using ArchitectureCatalog.API.DTOs.ArchitecturalStyles;
using ArchitectureCatalog.API.DTOs.Common;
using ArchitectureCatalog.API.Hateoas;
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
    public async Task<ActionResult<PagedResult<ArchitecturalStyleDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetAllAsync(page, pageSize);

        foreach (var item in result.Items)
        {
            item.Links = HateoasLinks.ForArchitecturalStyle(Url, item.Id);
        }

        result.Links = HateoasLinks.ForPagedCollection(
            Url,
            nameof(GetAll),
            "ArchitecturalStyles",
            result.Page,
            result.PageSize,
            result.TotalPages);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ArchitecturalStyleDto>> GetById(int id)
    {
        var style = await _service.GetByIdAsync(id);
        if (style is null)
        {
            return NotFound();
        }

        style.Links = HateoasLinks.ForArchitecturalStyle(Url, style.Id);
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
        created.Links = HateoasLinks.ForArchitecturalStyle(Url, created.Id);
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
        try
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return Conflict();
        }
    }
}
