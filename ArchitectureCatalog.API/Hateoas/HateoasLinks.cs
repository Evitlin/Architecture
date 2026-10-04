using ArchitectureCatalog.API.DTOs.Common;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectureCatalog.API.Hateoas;

public static class HateoasLinks
{
    public static List<LinkDto> ForArchitecturalStyle(IUrlHelper url, int id)
    {
        return
        [
            new LinkDto("self", url.Action("GetById", "ArchitecturalStyles", new { id })!, "GET"),
            new LinkDto("update", url.Action("Update", "ArchitecturalStyles", new { id })!, "PUT"),
            new LinkDto("delete", url.Action("Delete", "ArchitecturalStyles", new { id })!, "DELETE"),
            new LinkDto("buildings", url.Action("GetAll", "Buildings", new { architecturalStyleId = id })!, "GET")
        ];
    }

    public static List<LinkDto> ForBuilding(IUrlHelper url, int id, int architecturalStyleId)
    {
        return
        [
            new LinkDto("self", url.Action("GetById", "Buildings", new { id })!, "GET"),
            new LinkDto("details", url.Action("GetDetails", "Buildings", new { id })!, "GET"),
            new LinkDto("update", url.Action("Update", "Buildings", new { id })!, "PUT"),
            new LinkDto("delete", url.Action("Delete", "Buildings", new { id })!, "DELETE"),
            new LinkDto("style", url.Action("GetById", "ArchitecturalStyles", new { id = architecturalStyleId })!, "GET"),
            new LinkDto("comments", url.Action("GetAll", "Comments", new { buildingId = id })!, "GET"),
            new LinkDto(
                "scoped-comments",
                url.Action("GetAll", "StyleBuildingComments", new { styleId = architecturalStyleId, buildingId = id })!,
                "GET")
        ];
    }

    public static List<LinkDto> ForComment(IUrlHelper url, int id, int buildingId)
    {
        return
        [
            new LinkDto("self", url.Action("GetById", "Comments", new { id })!, "GET"),
            new LinkDto("update", url.Action("Update", "Comments", new { id })!, "PUT"),
            new LinkDto("delete", url.Action("Delete", "Comments", new { id })!, "DELETE"),
            new LinkDto("building", url.Action("GetById", "Buildings", new { id = buildingId })!, "GET")
        ];
    }

    public static List<LinkDto> ForBuildingDetails(IUrlHelper url, int buildingId, int architecturalStyleId)
    {
        return
        [
            new LinkDto("self", url.Action("GetDetails", "Buildings", new { id = buildingId })!, "GET"),
            new LinkDto("building", url.Action("GetById", "Buildings", new { id = buildingId })!, "GET"),
            new LinkDto("style", url.Action("GetById", "ArchitecturalStyles", new { id = architecturalStyleId })!, "GET"),
            new LinkDto("comments", url.Action("GetAll", "Comments", new { buildingId })!, "GET")
        ];
    }

    public static List<LinkDto> ForPagedCollection(
        IUrlHelper url,
        string actionName,
        string controllerName,
        int page,
        int pageSize,
        int totalPages,
        object? extraRouteValues = null)
    {
        var lastPage = totalPages < 1 ? 1 : totalPages;

        var links = new List<LinkDto>
        {
            new("self", BuildCollectionHref(url, actionName, controllerName, page, pageSize, extraRouteValues), "GET"),
            new("first", BuildCollectionHref(url, actionName, controllerName, 1, pageSize, extraRouteValues), "GET")
        };

        if (page > 1)
        {
            links.Add(new LinkDto(
                "prev",
                BuildCollectionHref(url, actionName, controllerName, page - 1, pageSize, extraRouteValues),
                "GET"));
        }

        if (page < totalPages)
        {
            links.Add(new LinkDto(
                "next",
                BuildCollectionHref(url, actionName, controllerName, page + 1, pageSize, extraRouteValues),
                "GET"));
        }

        links.Add(new LinkDto(
            "last",
            BuildCollectionHref(url, actionName, controllerName, lastPage, pageSize, extraRouteValues),
            "GET"));

        return links;
    }

    private static string BuildCollectionHref(
        IUrlHelper url,
        string actionName,
        string controllerName,
        int page,
        int pageSize,
        object? extraRouteValues)
    {
        var values = new RouteValueDictionary(extraRouteValues)
        {
            ["page"] = page,
            ["pageSize"] = pageSize
        };

        return url.Action(actionName, controllerName, values)!;
    }
}
