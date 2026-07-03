# Issue 8: Feedback & användarvänlighet

**Ansvarig:** –
**Status:** Ej startad
**Branch:** `feature/issue-8-feedback`

## User story
Som användare vill jag få tydlig feedback när jag gör något i systemet, så att jag vet om det gick bra eller inte.

## Kommentar
Tvärgående uppgift — påverkar alla actions. Använd `TempData` för flash messages som överlever redirect.

## Tasks
- [ ] Feedback efter lyckad parkering
- [ ] Feedback efter misslyckad parkering
- [ ] Feedback efter lyckad editering
- [ ] Feedback efter misslyckad editering
- [ ] Feedback efter utcheckning
- [ ] Tydligt fel vid dubblett av regnr
- [ ] Enhetliga knappar och navigation genom appen
- [ ] Enhetligt språk (svenska genomgående)

## Klart när
- [ ] Användaren får feedback efter viktiga händelser
- [ ] Språk och stil är konsekvent
- [ ] Appen känns sammanhängande

## Tips: TempData flash pattern

**Controller:**
```csharp
TempData["Success"] = "Fordonet parkerades!";
return RedirectToAction(nameof(Index));
```

**_Layout.cshtml (visas överallt):**
```razor
@if (TempData["Success"] != null) {
    <div class="alert alert-success alert-dismissible fade show">
        @TempData["Success"]
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    </div>
}
@if (TempData["Error"] != null) {
    <div class="alert alert-danger">@TempData["Error"]</div>
}
```

## Koordinering
- Kräver koll med alla issue-ansvariga — sista biten innan release
- Bäst att köra efter Issues 1–7 är i "För Review"
