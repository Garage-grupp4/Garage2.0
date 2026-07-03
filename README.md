# Garage 2.0 – Grupp 4

ASP.NET Core MVC-baserad garageapplikation. Första MVC-versionen av garaget — fokus på gränssnitt och funktionalitet.

## Gruppmedlemmar

| Namn | Ansvarsområde |
|------|---------------|
| Peter Laan | Issue 2: Översiktsvyn |
| Fredrik Selander | Issue 1: Parkera/checka in-vyn |
| George Cristian Ciolponea | Issue 4: Edit-vyn |
| Javier Diaz Urbano | Issue 3: Detaljvyn |

## Grundflöde

Parkera fordon → Översikt → Detaljer → Editera → Hämta ut → Kvitto

## Singelmodell

En enda modell: `ParkedVehicle` med följande properties:

- Fordonstyp (enum)
- Registreringsnummer (unikt)
- Färg
- Märke
- Modell
- Antal hjul
- Ankomsttid (sätts automatiskt, kan ej ändras)

## Arbetskort / Statustavla

| # | Arbetskort | Ansvarig | Status |
|---|------------|----------|--------|
| – | Gemensam uppstart | Alla | Klar |
| – | Gemensam singelmodell | Alla | Klar |
| – | Migrationer & lokal databas | Alla | Klar |
| – | Scaffoldad controller & vyer | Alla | Klar |
| 1 | [Anpassa parkera/checka in-vyn](issues/issue-01-parkera.md) | **Fredrik** | Pågår |
| 2 | [Anpassa översiktsvyn](issues/issue-02-oversikt.md) | **Peter** | Pågår |
| 3 | [Anpassa detaljvyn](issues/issue-03-detaljvyn.md) | **Javier** | För Review |
| 4 | [Anpassa edit-vyn](issues/issue-04-edit.md) | **George** | Pågår |
| 5 | [Hämta ut / checka ut](issues/issue-05-checkout.md) | – | Ej startad |
| 6 | [Kvitto](issues/issue-06-kvitto.md) | – | Ej startad |
| 7 | [Sökning](issues/issue-07-sokning.md) | – | Ej startad |
| 8 | [Feedback & användarvänlighet](issues/issue-08-feedback.md) | – | Ej startad |
| 9 | [Bonus: utökad sökning & sortering](issues/issue-09-bonus.md) | – | Ej startad |

**Statusvärden:** Ej startad · Pågår · Behöver hjälp · För Review · Klar

## Git-strategi

- Feature branch per issue — mönster: `issue{N}-{beskrivning}-{namn}` (t.ex. `issue3-Details-javier`)
- PR till `development` — inte direkt till `master`
- `git pull --rebase origin development` innan push för ren historik
- Prata med gruppen innan ändringar i delade delar (modell, propertynamn, fordonstyper, prisregel, actions/vyer)

## Daglig avstämning (max 15 min)

Varje person svarar på:
1. Vad har jag gjort sedan sist?
2. Vad ska jag göra idag?
3. Är det något som hindrar mig?

## Kom igång lokalt

```bash
git clone git@github.com:GeorgeCristian110/Garage2.0.git
cd Garage2.0
dotnet restore
dotnet ef database update
dotnet run
```

## Tech stack

- ASP.NET Core MVC
- Entity Framework Core
- SQLite (lokal databas per utvecklare)
