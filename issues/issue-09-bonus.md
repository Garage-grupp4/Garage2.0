# Issue 9: Bonus — utökad sökning & sortering

**Ansvarig:** –
**Status:** Ej startad
**Branch:** `feature/issue-9-bonus`

## User story
Som användare vill jag kunna söka och sortera på fler sätt, så att översikten blir lättare att använda när garaget innehåller många fordon.

## Tasks
- [ ] Utöka sökning: regnr, färg, märke, modell, fordonstyp
- [ ] Sortering på minst en kolumn (helst flera)
- [ ] Stigande OCH fallande sortering
- [ ] Sökning + sortering ska fungera samtidigt

## Klart när
- [ ] Sökning fungerar på fler fält
- [ ] Sortering fungerar båda riktningarna
- [ ] Kombination fungerar

## Tips: sortering med .AsQueryable()
```csharp
public async Task<IActionResult> Index(string? searchTerm, string? sortBy, bool desc = false) {
    var query = _context.ParkedVehicles.AsQueryable();

    if (!string.IsNullOrWhiteSpace(searchTerm)) {
        var t = searchTerm.ToUpper();
        query = query.Where(v =>
            v.RegistrationNumber.ToUpper().Contains(t) ||
            v.Color!.ToUpper().Contains(t) ||
            v.Brand!.ToUpper().Contains(t) ||
            v.Model!.ToUpper().Contains(t));
    }

    query = sortBy switch {
        "regnr" => desc ? query.OrderByDescending(v => v.RegistrationNumber)
                        : query.OrderBy(v => v.RegistrationNumber),
        "type"  => desc ? query.OrderByDescending(v => v.VehicleType)
                        : query.OrderBy(v => v.VehicleType),
        "time"  => desc ? query.OrderByDescending(v => v.ArrivalTime)
                        : query.OrderBy(v => v.ArrivalTime),
        _ => query.OrderBy(v => v.ArrivalTime)
    };

    return View(await query.Select(...).ToListAsync());
}
```

## Vy: klickbara kolumnrubriker
```razor
<th>
    <a asp-action="Index"
       asp-route-searchTerm="@ViewBag.SearchTerm"
       asp-route-sortBy="regnr"
       asp-route-desc="@(ViewBag.SortBy == "regnr" && !ViewBag.Desc)">
       Regnr
    </a>
</th>
```

## Beroenden
- Bäst att köra EFTER Issues 2 & 7 är klara — bygger vidare på dem
