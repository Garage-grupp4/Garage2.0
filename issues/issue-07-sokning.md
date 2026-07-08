# Issue 7: Sökning på registreringsnummer

**Ansvarig:** –
**Status:** Ej startad
**Branch:** `feature/issue-7-sokning`

## User story
Som användare vill jag kunna söka på registreringsnummer, så att jag snabbt kan hitta rätt fordon.

## Tasks
- [ ] Sökfält i översiktsvyn (`Index.cshtml`)
- [ ] Skicka söktermen till `Index`-action som query-parameter
- [ ] Filtrera på regnr (`Contains`, case-insensitive)
- [ ] Visa matchande fordon
- [ ] "Inga fordon hittades" om resultatet är tomt

## Klart när
- [ ] Sökning fungerar
- [ ] Tom sökning → visa alla
- [ ] Tydligt meddelande vid 0 träffar

## Tips
```csharp
public async Task<IActionResult> Index(string? searchTerm) {
    var query = _context.ParkedVehicles.AsQueryable();

    if (!string.IsNullOrWhiteSpace(searchTerm)) {
        query = query.Where(v =>
            v.RegistrationNumber.ToUpper().Contains(searchTerm.ToUpper()));
    }

    ViewBag.SearchTerm = searchTerm;
    var model = await query.Select(...).ToListAsync();
    return View(model);
}
```

```razor
<form asp-action="Index" method="get" class="mb-3">
    <input name="searchTerm" value="@ViewBag.SearchTerm"
           placeholder="Sök på registreringsnummer" class="form-control" />
    <button type="submit" class="btn btn-primary">Sök</button>
</form>

@if (!Model.Any()) {
    <div class="alert alert-info">Inga fordon matchade sökningen.</div>
}
```

## Beroenden
- Issue 2 (översikten) bör vara klar först — annars merge-konflikter i `Index.cshtml`
