# Issue 1: Anpassa parkera/checka in-vyn

**Ansvarig:** –
**Status:** Ej startad
**Branch:** `feature/issue-1-parkera`

## User story
Som användare vill jag kunna parkera ett fordon via ett tydligt formulär, så att fordonet hamnar i garaget.

## Kommentar
Utgå från scaffoldad `Create`-action i `ParkedVehiclesController`. Fundera på om en `ViewModel` behövs (troligen ja — vi vill inte att `ArrivalTime` ska kunna postas via formuläret).

## Tasks
- [ ] Anpassa Create-vyn till garage-språk (t.ex. "Parkera fordon" istället för "Create")
- [ ] Fält: registreringsnummer, fordonstyp, färg, märke, modell, antal hjul
- [ ] Fordonstyp som `<select>` (dropdown) — bind mot `VehicleType` enum
- [ ] Ankomsttid syns INTE i formuläret, sätts i controllern: `vehicle.ArrivalTime = DateTime.Now`
- [ ] Visa valideringsfel via `asp-validation-for`
- [ ] Kontrollera unikt registreringsnummer före `SaveChanges`

## Klart när
- [ ] Fordon kan parkeras
- [ ] Rätt språk i UI
- [ ] Dropdown för fordonstyp
- [ ] `ArrivalTime` sätts av systemet
- [ ] Ogiltig input → valideringsfel, inte crash

## Beroenden
- Singelmodellen `ParkedVehicle` klar
- `VehicleType` enum klar

## Tips
```csharp
// Dropdown i vyn
<select asp-for="VehicleType" asp-items="Html.GetEnumSelectList<VehicleType>()">
    <option value="">-- Välj fordonstyp --</option>
</select>
```
