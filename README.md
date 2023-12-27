# BCSH2 Semestrální práce

## Zadání
varianta: (a) Jednoduchá databázová aplikace
* Sledované entity: Zvire, Ošetrovatel, Druh, Chovatelské potřeby, Show
* Evidováno bude:
  * Zvire - jméno, datum narození, datum příjmu, historie Krmění, historie Medikamentů, plán Show
  * Ošetřovatel - jméno, příjmení, kontaktní informace 
  * Druh - název
  * Chovatelské potřeby - druh potřeby (strava, medikament,...), datum podání
  * Show - datum konání, Zvíře, Ošetřovatel
* aplikace umožňuje uživateli vytvářet, editovat a mazat veškeré uvedené typy entit
* aplikace umožňuje filtrovat entity podle jejich atributů: 
  * Zvíře - podle jméno, datum narození, ...
  * Show - podle Zvíře, Oštřovatel, datum konání
  * a podobně...

Externí knihovny: 
* [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/7.0.12?_src=template) - DB
* [CommunityToolkit.Mvvm](https://www.nuget.org/packages/CommunityToolkit.Mvvm/8.2.2?_src=template) - MVVM
* [DataGridExtensions](https://www.nuget.org/packages/DataGridExtensions/2.6.0?_src=template) - DataGrid Filtry

## Diagram
![DB Diagram](DbDiagram.png)

## Inicializace dat
Aplikační Menu obsahuje v nabídce inicializaci databáze, pokud je prázdná.

## Known Issues
Když jsou v DataGrid zadané špatné údaje (červená buňka) a aplikační Menu je otevřené poprvé od spuštění aplikace, její MenuItemy zůstanou disabled.