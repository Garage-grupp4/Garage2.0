# Issue 2: Anpassa översiktsvyn

**Ansvarig:** –
**Status:** Ej startad
**Branch:** `feature/issue-2-oversikt`

## User story
Som användare vill jag se alla parkerade fordon i en översikt, så att jag snabbt ser vad som finns i garaget.

## Kommentar
Utgå från scaffoldad `Index`. Skapa en `OverviewViewModel` — översikten ska INTE visa alla properties.

## Tasks
- [ ] Skapa `OverviewViewModel` med endast: `VehicleType`, `RegistrationNumber`, `ArrivalTime`
- [ ] Anpassa `Index`-action: mappa `ParkedVehicle` → `OverviewViewModel`
- [ ] Visa fordonstyp, regnr, ankomsttid
- [ ] Länkar/knappar per rad: Detaljer, Editera, Hämta ut
- [ ] **Bonus:** kolumn "Parkerad tid" (räkna ut `DateTime.Now - ArrivalTime`)

## Klart när
- [ ] Alla parkerade fordon visas
- [ ] Vyn använder ViewModel (inte modellen direkt)
- [ ] Navigation till detaljer/edit/hämta ut fungerar

## Beroenden
- Modellen klar
- Bra att synka med Issue 3, 4, 5 för action-namn (`Details`, `Edit`, `Checkout`)

## Tips
```csharp
// Mappning i controller
var model = await _context.ParkedVehicles
    .Select(v => new OverviewViewModel {
        VehicleType = v.VehicleType,
        RegistrationNumber = v.RegistrationNumber,
        ArrivalTime = v.ArrivalTime
    }).ToListAsync();
```
