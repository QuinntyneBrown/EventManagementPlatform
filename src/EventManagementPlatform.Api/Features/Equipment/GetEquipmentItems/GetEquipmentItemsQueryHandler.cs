// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using EventManagementPlatform.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventManagementPlatform.Api.Features.Equipment.GetEquipmentItems;

public class GetEquipmentItemsQueryHandler : IRequestHandler<GetEquipmentItemsQuery, GetEquipmentItemsResponse>
{
    private readonly IEventManagementPlatformContext _context;

    public GetEquipmentItemsQueryHandler(IEventManagementPlatformContext context)
    {
        _context = context;
    }

    public async Task<GetEquipmentItemsResponse> Handle(GetEquipmentItemsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.EquipmentItems.AsQueryable();

        if (request.Category.HasValue)
            query = query.Where(x => x.Category == request.Category.Value);

        if (request.Status.HasValue)
            query = query.Where(x => x.Status == request.Status.Value);

        if (request.Condition.HasValue)
            query = query.Where(x => x.Condition == request.Condition.Value);

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(x => x.Name.Contains(request.Search) ||
                                     (x.Manufacturer != null && x.Manufacturer.Contains(request.Search)) ||
                                     (x.Model != null && x.Model.Contains(request.Search)));

        var totalCount = await query.CountAsync(cancellationToken);

        var equipmentItems = await query
            .OrderBy(x => x.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new EquipmentItemDto(
                x.EquipmentItemId,
                x.Name,
                x.Description,
                x.Category,
                x.Condition,
                x.Status,
                x.PurchaseDate,
                x.PurchasePrice,
                x.CurrentValue,
                x.Manufacturer,
                x.Model,
                x.SerialNumber,
                x.WarehouseLocation,
                x.IsActive,
                x.CreatedAt,
                x.ModifiedAt))
            .ToListAsync(cancellationToken);

        return new GetEquipmentItemsResponse(equipmentItems, totalCount, request.Page, request.PageSize);
    }
}
