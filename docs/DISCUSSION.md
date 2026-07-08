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

## 4. Namngivning: camelCase → PascalCase 📛

**Nuläge:** Min modell använder camelCase (`color`, `registrationNumber`) från scaffoldningen. C#-standard är PascalCase — Fredrik följde detta korrekt i Create.cshtml med `asp-for="Color"`. Min modell bör anpassas till standard.

**Konsekvens:** Byta HELA modellen till PascalCase påverkar alla vyer + controllern.

**Att bestämma:**
- Byta till PascalCase på alla properties? (`registrationNumber` → `RegistrationNumber` osv.)
- Bör bli en egen refactor-issue med tydlig ägare — inte ändras mitt i pågående feature-arbete
- Vem tar ansvar för refactoren?

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

## 7. `.gitignore` städning 🗂️

**Nuläge:** `.gitignore` saknar mönster för flera IDE:er och lokala filer teamet använder. Just nu trackas t.ex. `_1.db` och `.obsidian/` som inte borde ligga i git.

**Förslag:**
```
# IDE / editor
.idea/          # Fredrik — IntelliJ IDEA / Rider
.vs/            # Visual Studio
.vscode/        # VS Code
.obsidian/      # Javier — projektnoter

# Databas (lokal per utvecklare)
*.db
*.db-shm
*.db-wal

# OS
.DS_Store       # macOS
```

**Att bestämma:**
- Vem använder vilken IDE? (Fråga runt bordet)
- OK att jag genomför uppdateringen?
- Efter merge: `git rm --cached _1.db .idea/ .vscode/ .obsidian/` för att sluta tracka det som redan finns i git

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
| 7. `.gitignore` städning | Alla | Javier |
