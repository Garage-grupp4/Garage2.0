# Issue 4: Anpassa edit-vyn

**Ansvarig:** –
**Status:** Ej startad
**Branch:** `feature/issue-4-edit`

## User story
Som användare vill jag kunna ändra information om ett parkerat fordon, så att felaktig information kan rättas.

## Kommentar
Utgå från scaffoldad `Edit`. Skydda `ArrivalTime` — den får ALDRIG ändras via formuläret.

## Tasks
- [ ] Anpassa Edit-vyn
- [ ] Visa nuvarande värden i formulärfälten
- [ ] Låt användaren ändra: regnr, fordonstyp, färg, märke, modell, antal hjul
- [ ] `ArrivalTime` visas som read-only text (inte input)
- [ ] Valideringsfel visas
- [ ] Unikt regnr — får inte krocka med annat fordon (men OK om det är samma fordon)

## Klart när
- [ ] Editering fungerar
- [ ] `ArrivalTime` kan inte ändras
- [ ] Regnr kan inte dubbleras
- [ ] Ogiltiga värden sparas inte

## Tips: skydda ArrivalTime
```csharp
[HttpPost]
public async Task<IActionResult> Edit(int id, EditViewModel vm) {
    var vehicle = await _context.ParkedVehicles.FindAsync(id);
    if (vehicle == null) return NotFound();

    // ArrivalTime kopieras INTE från vm — behålls som det var
    vehicle.RegistrationNumber = vm.RegistrationNumber;
    vehicle.Color = vm.Color;
    // ...
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}
```

## Tips: unikhet vid edit
```csharp
bool exists = await _context.ParkedVehicles
    .AnyAsync(v => v.RegistrationNumber == vm.RegistrationNumber && v.Id != id);
if (exists) ModelState.AddModelError(nameof(vm.RegistrationNumber), "Regnr används redan");
```
