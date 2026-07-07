# Garage 2.0 – Grupp 4

ASP.NET Core MVC-baserad garageapplikation. Första MVC-versionen av garaget — fokus på gränssnitt och funktionalitet.

## Gruppmedlemmar

| Namn                      | Ansvarsområde |
| ------------------------- | ------------- |
| Peter Laan                | Issue 7       |
| Fredrik Selander          | Issue 1,4,8   |
| George Cristian Ciolponea | Issue 2,5     |
| Javier Diaz Urbano        | Issue 3       |

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

| #   | Arbetskort                                                    | Ansvarig    | Status     |
| --- | ------------------------------------------------------------- | ----------- | ---------- |
| –   | Gemensam uppstart                                             | Alla        | Klar       |
| –   | Gemensam singelmodell                                         | Alla        | Klar       |
| –   | Migrationer & lokal databas                                   | Alla        | Klar       |
| –   | Scaffoldad controller & vyer                                  | Alla        | Klar       |
| 1   | [Anpassa parkera/checka in-vyn](issues/issue-01-parkera.md)   | **Fredrik** | Pågår      |
| 2   | [Anpassa översiktsvyn](issues/issue-02-oversikt.md)           | **Peter**   | Pågår      |
| 3   | [Anpassa detaljvyn](issues/issue-03-detaljvyn.md)             | **Javier**  | För Review |
| 4   | [Anpassa edit-vyn](issues/issue-04-edit.md)                   | **George**  | Pågår      |
| 5   | [Hämta ut / checka ut](issues/issue-05-checkout.md)           | –           | Ej startad |
| 6   | [Kvitto](issues/issue-06-kvitto.md)                           | –           | Ej startad |
| 7   | [Sökning](issues/issue-07-sokning.md)                         | –           | Ej startad |
| 8   | [Feedback & användarvänlighet](issues/issue-08-feedback.md)   | –           | Ej startad |
| 9   | [Bonus: utökad sökning & sortering](issues/issue-09-bonus.md) | –           | Ej startad |

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
# 1. Klona repot
git clone git@github.com:GeorgeCristian110/Garage2.0.git
cd Garage2.0

# 2. (Valfritt) Byt till en specifik branch — annars körs master
git switch issue3-Details-javier   # exempel

# 3. Gå in i projektmappen och installera paket
cd Garage2.0
dotnet restore

# 4. Kör appen — databasen skapas och seedas automatiskt
dotnet run
# eller med auto-reload:
dotnet watch
```

Appen körs på `http://localhost:5213` (portnumret kan variera — se terminalen).

**Notera:** Projektet ligger i undermappen `Garage2.0/Garage2.0/` — steg 3–4 körs därifrån.

## Automatisk databas & seed

Vid appstart körs `Data/SeedData.cs` automatiskt och:

1. Skapar `_1.db` om den saknas (kör pending migrations)
2. Fyller tabellen med **10 testfordon** (bilar, motorcyklar, buss, lastbilar) — men **bara om tabellen är tom**

Idempotent: säkert att köra hur många gånger som helst. Ingen manuell `dotnet ef database update` eller `sqlite3`-kommando behövs.

Den lokala databasen (`_1.db`) är **inte** versionshanterad — varje utvecklare har sin egen.

### Återställa databasen

```bash
rm _1.db
dotnet run
```

## Felsökning

### `PendingModelChangesWarning` vid appstart

Betyder att någon har ändrat `ParkedVehicle.cs` utan att skapa en migration. Skapa den lokalt:

```bash
dotnet ef migrations add BeskrivningAvÄndring
dotnet run
```

**Committa alltid migrationsfilen** i samma PR som modellen ändras. Annars kraschar teamet vid nästa pull.

### Databas ur synk efter branch-byte

Enklaste vägen — släng och börja om:

```bash
rm _1.db
dotnet run
```

## Tech stack

- ASP.NET Core MVC
- Entity Framework Core
- SQLite (lokal databas per utvecklare)
