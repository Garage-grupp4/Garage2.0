# Garage 2.0 – Grupp 4

ASP.NET Core MVC-baserad garageapplikation. Första MVC-versionen av garaget — fokus på gränssnitt och funktionalitet.

## Gruppmedlemmar

| Namn | Ansvarsområde |
|------|---------------|
| Peter Laan | – |
| Fredrik Selander | – |
| George Cristian Ciolponea | – |
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
| – | Gemensam uppstart | Alla | Ej startad |
| – | Gemensam singelmodell | Alla | Ej startad |
| – | Migrationer & lokal databas | Alla | Ej startad |
| – | Scaffoldad controller & vyer | Alla | Ej startad |
| 1 | Anpassa parkera/checka in-vyn | – | Ej startad |
| 2 | Anpassa översiktsvyn | – | Ej startad |
| 3 | Anpassa detaljvyn | **Javier** | Ej startad |
| 4 | Anpassa edit-vyn | – | Ej startad |
| 5 | Hämta ut / checka ut | – | Ej startad |
| 6 | Kvitto | – | Ej startad |
| 7 | Sökning | – | Ej startad |
| 8 | Feedback & användarvänlighet | – | Ej startad |
| 9 | Bonus: utökad sökning & sortering | – | Ej startad |

**Statusvärden:** Ej startad · Pågår · Behöver hjälp · För Review · Klar

## Git-strategi

- Branch per issue (`feature/issue-3-detaljvyn` etc.)
- PR till `development` — inte direkt till `main`
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
