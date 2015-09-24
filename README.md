## Build status
[![Build status][appveyor_build_badge]][appveyor_build_link]
[![Test status][appveyor_tests_badge]][appveyor_tests_link]
[![Coverage Status][coveralls_badge]][coveralls_link]

[![Coverity Scan Build Status][coverity_badge]][coverity_link] *Coverity funktioniert noch nicht mit VS2015 / C# 6*

# Bankleitzahlen-Tool

## Ziel dieses Projekts

Das Bankleitzahlen-Tool hilft beim Download und Durchsuchen der auf der Webseite
der [Bundesbank][bundesbank_link] hinterlegten *Bankleitzahlenänderungsdateien*:

![Screenshot][screenshot]

## Hä?

Okay, kleiner Scherz ;-). Das Projekt dient originär der Demonstration des WPF-Frameworks
[Caliburn.Micro][caliburn_link]. Hierzu wurde eine GUI entworfen und sowohl mit Caliburn.Micro
als auch in "Pure-WPF" umgesetzt.

Die "Pure-WPF"-Implementierung zeigt:
* Databindung und Actions über XAML
* Databindung und Actions über Code-behind
* rund 18k `UserControls` in der mit einer `ListBox` verbundenen `Collection` (ohne Virtualisierung): flutscht nicht
* Viele Smells ...

Die Caliburn.Micro-Implementierung demonstriert:
* MVVM-Pattern
* Konventionen für die automatische Zuordnung von ViewModel und View
* Konventionen fürs Databindung
* Konventionen für Actions und der Verwendung von `cal:Message.Attach`
* Konventionen für Eigenschaften wie IsEnabled und SelectedItem
* Nützliche Basisklassen wie `PropertyChangedBase` und `ViewAware`
* rund 18k ViewModels in der mit einer `ListBox` verbundenen `Collection`: flutscht dennoch
* sehr einfacher Einstieg

Caliburn.Micro bietet noch andere, hier bisher nicht eingesetzte Features:
* Erweiterung um eigene Konventionen
* Einfacher IoC-Container
* Event-Bus
* Abstraktion für die verschiedenen Plattformen: .NET, Silverlight und WinRT


[bundesbank_link]: https://www.bundesbank.de/Redaktion/DE/Standardartikel/Aufgaben/Unbarer_Zahlungsverkehr/bankleitzahlen_download.html
[caliburn_link]: http://caliburnmicro.com/
[screenshot]: https://raw.githubusercontent.com/chkpnt/Bankleitzahlen-Tool/master/screenshot.png
[appveyor_build_badge]: https://ci.appveyor.com/api/projects/status/i7phbw50uln995mc?svg=true
[appveyor_build_link]: https://ci.appveyor.com/project/chkpnt/bankleitzahlen-tool
[appveyor_tests_badge]: http://teststatusbadge.azurewebsites.net/api/status/chkpnt/bankleitzahlen-tool
[appveyor_tests_link]: https://ci.appveyor.com/project/chkpnt/bankleitzahlen-tool/build/tests
[coverity_badge]: https://scan.coverity.com/projects/6440/badge.svg
[coverity_link]: https://scan.coverity.com/projects/6440
[coveralls_badge]: https://coveralls.io/repos/chkpnt/Bankleitzahlen-Tool/badge.svg 
[coveralls_link]: https://coveralls.io/github/chkpnt/Bankleitzahlen-Tool