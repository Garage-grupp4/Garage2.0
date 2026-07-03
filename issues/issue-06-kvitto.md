# Issue 6: Kvitto

**Ansvarig:** –
**Status:** Ej startad
**Branch:** `feature/issue-6-kvitto`

## User story
Som användare vill jag få ett kvitto när jag hämtar ut ett fordon, så att jag kan se hur länge fordonet varit parkerat och vad det kostade.

## Tasks
- [ ] Skapa `ReceiptViewModel` (fordonsdata, in/ut-tid, duration, pris)
- [ ] Bestäm **prisregel** som grupp (t.ex. 10 kr/påbörjad timme)
- [ ] Skapa `Receipt.cshtml`
- [ ] `Receipt`-action i controllern
- [ ] Visa: regnr, fordonstyp, incheckningstid, utcheckningstid, total tid, pris
- [ ] **Bonus:** utskriftsvänlig (CSS `@media print`)

## Klart när
- [ ] Kvitto visas efter utcheckning
- [ ] Använder ViewModel
- [ ] Alla fält synliga och läsbara

## Förslag: ReceiptViewModel
```csharp
public class ReceiptViewModel {
    public string RegistrationNumber { get; set; } = "";
    public VehicleType VehicleType { get; set; }
    public DateTime ArrivalTime { get; set; }
    public DateTime CheckoutTime { get; set; }
    public TimeSpan Duration => CheckoutTime - ArrivalTime;
    public decimal Price => (decimal)Math.Ceiling(Duration.TotalHours) * PricePerHour;
    public const decimal PricePerHour = 10m;
}
```

## Bonus: utskrift
```html
<style>
@media print {
    .no-print { display: none; }
    body { font-size: 12pt; }
}
</style>
<button onclick="window.print()" class="no-print">Skriv ut</button>
```

## Beroenden
- Samarbeta med Issue 5 för hur data skickas till kvittovyn
