# Issue 3: Anpassa detaljvyn ⭐

**Ansvarig:** **Javier**
**Status:** Ej startad
**Branch:** `feature/issue-3-detaljvyn`

## User story
Som användare vill jag kunna se all information om ett parkerat fordon, så att jag kan kontrollera att uppgifterna stämmer.

## Kommentar
Utgå från scaffoldad `Details(int? id)`-action. Denna vy är "read-only" — ingen ViewModel strikt nödvändig, men kan användas för att formatera ankomsttid eller lägga till "parkerad tid".

## Tasks
- [ ] Anpassa `Details.cshtml` till garage-språk
- [ ] Visa registreringsnummer
- [ ] Visa fordonstyp (som text, inte enum-siffra)
- [ ] Visa färg
- [ ] Visa märke
- [ ] Visa modell
- [ ] Visa antal hjul
- [ ] Visa ankomsttid (formaterad, t.ex. `yyyy-MM-dd HH:mm`)
- [ ] Tillbaka-länk till översikten
- [ ] Länkar till Edit och Hämta ut från detaljvyn

## Klart när
- [ ] All fordonsinformation visas
- [ ] Öppnas från översikten via länk
- [ ] Tydlig tillbaka-navigation
- [ ] Fordonstyp visas som text (inte "0", "1", "2")
- [ ] Ankomsttid är läsbar

## Beroenden
- **Blockerar inte andra** — kan starta så fort scaffolding är klar
- Behöver synka med Issue 2 för länkstruktur (`asp-route-id`)

## Tekniska tips

### Formatera enum till text
```razor
<dd>@Html.DisplayFor(model => model.VehicleType)</dd>
```
Eller med `Display`-attribut på enum-värden:
```csharp
public enum VehicleType {
    [Display(Name = "Bil")] Car,
    [Display(Name = "Motorcykel")] Motorcycle,
    [Display(Name = "Lastbil")] Truck
}
```

### Formatera datum
```csharp
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
public DateTime ArrivalTime { get; set; }
```

### Bonus: "Parkerad tid" i detaljvyn
```razor
<dt>Parkerad i</dt>
<dd>@((DateTime.Now - Model.ArrivalTime).ToString(@"d\d\ hh\:mm"))</dd>
```

### Navigation
```razor
<a asp-action="Edit" asp-route-id="@Model.Id" class="btn btn-primary">Editera</a>
<a asp-action="Checkout" asp-route-id="@Model.Id" class="btn btn-warning">Hämta ut</a>
<a asp-action="Index" class="btn btn-secondary">Tillbaka</a>
```

## Definition of done (för mig personligen)
1. Kör lokalt utan varningar
2. Alla properties syns
3. Klickbar från Index → tillbaka fungerar
4. Commit med tydligt meddelande: `feat(details): anpassa detaljvy för parkerat fordon`
5. PR till `development`
6. Status → "För Review" i statustavlan
