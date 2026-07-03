# Diskussionspunkter — nästa möte

Punkter från Issue 3-arbetet som behöver gruppbeslut innan de kan mergas eller implementeras.

---

## 1. Seeder i `Program.cs` istället för SQL-fil 🌱

**Nuläge:** Jag har lagt `SeedData/test-vehicles.sql` som teamet kan köra manuellt med `sqlite3`. Fungerar men klumpigt.

**Förslag:** Flytta seed-logiken till `Program.cs` — körs automatiskt vid app-start om DB är tom.

**Fördelar:**
- Ingen manuell SQL-körning
- Fungerar för alla utan att kunna sqlite CLI
- Alla ser samma testdata → lättare felsökning tillsammans

**Att bestämma:**
- Ska seedern köras alltid i dev, eller bara första gången?
- Använd `if (env.IsDevelopment())` så det inte hamnar i "produktion"
- Vem tar ansvar för implementationen? Egen issue?

---

## 2. `[Display(Name="...")]` på modellen 🇸🇪

**Nuläge:** Jag har lagt svenska `[Display]`-attribut på alla properties i `ParkedVehicle.cs`. Detta påverkar Index, Create, Edit — inte bara Details.

**Fördel:** En källa till sanning. Alla vyer får svenska etiketter automatiskt.

**Att bestämma:**
- OK att jag mergar detta som del av Issue 3?
- Vill någon justera texter (t.ex. "Utcheckningstid" — behövs den överhuvudtaget i denna version?)

---

## 3. `color`: `short` → `string` 🎨

**Nuläge:** Scaffoldningen satte `color` som `short` (siffra). Jag har ändrat till `string` för att stödja Fredriks HTML5 color picker (`type="color"` → hex-sträng).

**Konsekvens:** Migration behövs. Gammal DB-data (siffror) fungerar inte längre.

**Att bestämma:**
- Alla OK med `string`?
- Vem kör `dotnet ef migrations add ChangeColorToString` och committar migrationen?
- Efter merge → alla raderar sin lokala DB och kör `dotnet ef database update`

---

## 4. Namngivning: `Color` vs `color` (PascalCase vs camelCase) 📛

**Nuläge:** Fredriks Create-vy använder `asp-for="Color"` (stort C). Min modell har `color` (litet c). Detta krockar.

**Standard i C#:** Properties ska vara **PascalCase** (`Color`, `RegistrationNumber`, `VehicleType`).

**Vår kod:** Har blandat — scaffoldningen genererade camelCase.

**Att bestämma:**
- Byta HELA modellen till PascalCase? (`registrationNumber` → `RegistrationNumber` osv.)
- Detta påverkar ALLA vyer + controllern
- Bör bli en egen refactor-issue med tydlig ägare

---

## 5. Enum `VehicleType` — svenska namn 🚗

**Nuläge:** Jag har lagt `[Display(Name="Bil")]` osv. på enum-värden. Text visas snyggt i vyer.

**Att bestämma:**
- OK att detta mergas?
- Kolla stavning på "Motorcykel" (var "Motorcycle" av misstag i min fil — behöver dubbelkolla)

---

## 6. Ankomsttid får inte manipuleras via formulär ⚠️

**Nuläge:** Scaffoldade controllern har `[Bind("...arrivalTime...")]` i både `Create` och `Edit`. Detta bryter mot spec:

> "Ankomsttid kan inte manipuleras via formulär"

**Konsekvens:** Idag kan en illvillig användare posta annan ankomsttid genom att lägga till hidden fält.

**Att bestämma:**
- Fredrik (Issue 1) → ta bort `arrivalTime` från `[Bind]` i `Create`, sätt `vehicle.arrivalTime = DateTime.Now` i controller
- George (Issue 4) → samma för `Edit`, behåll ursprunglig `arrivalTime` från DB

---

## 7. `.obsidian`-mapp i git 🗂️

**Nuläge:** Jag har lagt `.obsidian/` i `.gitignore` (efter att ha råkat committa den).

**Att bestämma:**
- OK för alla? Ingen använder Obsidian för att versionera projektnoter?

---

## 8. `.db`-filer i git 💾

**Nuläge:** `_1.db` har trackats. Jag föreslår `*.db` i `.gitignore` + `git rm --cached _1.db`.

**Konsekvens:** Alla får tom DB efter pull → kör `dotnet ef database update` + `test-vehicles.sql` (eller framtida seeder från punkt 1).

**Att bestämma:**
- OK att jag genomför detta?

---

## Sammanfattning — vem gör vad efter mötet

| Punkt | Beslut behövs | Vem genomför |
|-------|--------------|--------------|
| 1. Seeder i Program.cs | Alla | Ny issue, tilldelas |
| 2. `[Display]` på modell | Alla | Javier (i Issue 3-PR) |
| 3. `color` som string + migration | Alla | Javier (migration commit) |
| 4. PascalCase-refactor | Alla | Ny issue, tilldelas |
| 5. Enum svenska namn | Alla | Javier (i Issue 3-PR) |
| 6. Skydda arrivalTime | Fredrik + George | Respektive issue |
| 7. `.obsidian` ignoreras | Alla | Javier |
| 8. `.db` ignoreras | Alla | Javier |
