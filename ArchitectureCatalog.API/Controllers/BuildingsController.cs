using ArchitectureCatalog.API.DTOs.Buildings;
using ArchitectureCatalog.API.DTOs.Common;
using ArchitectureCatalog.API.Hateoas;
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

        foreach (var item in result.Items)
        {
            item.Links = HateoasLinks.ForBuilding(Url, item.Id, item.ArchitecturalStyleId);
        }

        result.Links = HateoasLinks.ForPagedCollection(
            Url,
            nameof(GetAll),
            "Buildings",
            result.Page,
            result.PageSize,
            result.TotalPages,
            new { city, architecturalStyleId });

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

        building.Links = HateoasLinks.ForBuilding(Url, building.Id, building.ArchitecturalStyleId);
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

        details.Building.Links = HateoasLinks.ForBuilding(
            Url,
            details.Building.Id,
            details.Building.ArchitecturalStyleId);
        details.ArchitecturalStyle.Links = HateoasLinks.ForArchitecturalStyle(
            Url,
            details.ArchitecturalStyle.Id);

        foreach (var comment in details.Comments)
        {
            comment.Links = HateoasLinks.ForComment(Url, comment.Id, comment.BuildingId);
        }

        details.Links = HateoasLinks.ForBuildingDetails(
            Url,
            details.Building.Id,
            details.ArchitecturalStyle.Id);

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

        created.Links = HateoasLinks.ForBuilding(Url, created.Id, created.ArchitecturalStyleId);
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
