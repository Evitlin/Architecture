using ArchitectureCatalog.API.DTOs.Buildings;
using ArchitectureCatalog.API.DTOs.Common;
using ArchitectureCatalog.API.Services.Buildings;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectureCatalog.API.Controllers;

[ApiController]
[Route("api/buildings")]
public class BuildingsController : ControllerBase
{
    private readonly IBuildingService _service;

    public BuildingsController(IBuildingService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<BuildingDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? city = null,
        [FromQuery] int? architecturalStyleId = null)
    {
        var result = await _service.GetAllAsync(page, pageSize, city, architecturalStyleId);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BuildingDto>> GetById(int id)
    {
        var building = await _service.GetByIdAsync(id);
        if (building is null)
        {
            return NotFound();
        }

        return Ok(building);
    }

    [HttpGet("{id:int}/details")]
    public async Task<ActionResult<BuildingDetailsDto>> GetDetails(int id)
    {
        var details = await _service.GetDetailsAsync(id);
        if (details is null)
        {
            return NotFound();
        }

        return Ok(details);
    }

    [HttpPost]
    public async Task<ActionResult<BuildingDto>> Create([FromBody] CreateBuildingDto dto)
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
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBuildingDto dto)
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
