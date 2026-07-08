# Issue 5: Hämta ut / checka ut

**Ansvarig:** –
**Status:** Ej startad
**Branch:** `feature/issue-5-checkout`

## User story
Som användare vill jag kunna hämta ut ett fordon, så att det inte längre står kvar som parkerat.

## Kommentar
Utgå från scaffoldad `Delete`. Byt namn/språk och skicka data vidare till kvitto (Issue 6) INNAN raderingen.

## Tasks
- [ ] Byt actionnamn: `Delete` → `Checkout`, `DeleteConfirmed` → `CheckoutConfirmed`
- [ ] Byt UI-språk: "Radera" → "Hämta ut"
- [ ] Hämta fordonet
- [ ] Spara data till `ReceiptViewModel` (eller `TempData`) INNAN `Remove`
- [ ] Ta bort fordonet från DB
- [ ] Redirect till `Receipt`-action med kvittodatan

## Klart när
- [ ] Fordonet försvinner från översikten efter utcheckning
- [ ] Kvitto-data finns kvar (i TempData eller skickas som route)
- [ ] UI säger "Hämta ut" — aldrig "Delete"

## Tips: skicka data till kvitto
```csharp
[HttpPost, ActionName("Checkout")]
public async Task<IActionResult> CheckoutConfirmed(int id) {
    var vehicle = await _context.ParkedVehicles.FindAsync(id);
    if (vehicle == null) return NotFound();

    var receipt = new ReceiptViewModel {
        RegistrationNumber = vehicle.RegistrationNumber,
        VehicleType = vehicle.VehicleType,
        ArrivalTime = vehicle.ArrivalTime,
        CheckoutTime = DateTime.Now
    };
    // beräkna Duration + Price här eller i ViewModel

    _context.ParkedVehicles.Remove(vehicle);
    await _context.SaveChangesAsync();

    TempData["Receipt"] = System.Text.Json.JsonSerializer.Serialize(receipt);
    return RedirectToAction(nameof(Receipt));
}
```

## Beroenden
- Samarbeta tight med Issue 6 (kvitto) — bestäm `ReceiptViewModel`-form tillsammans
