# Azuria - Ein Proxer.Me API in .NET (inoffiziell)

##Status
Master-Branch: [![Build status](https://ci.appveyor.com/api/projects/status/eenr5ksrjakegl0e/branch/master?svg=true)](https://ci.appveyor.com/project/InfiniteSoul/massive-octo-wookie/branch/master)

Neuester Commit: [![Build status](https://ci.appveyor.com/api/projects/status/eenr5ksrjakegl0e?svg=true)](https://ci.appveyor.com/project/InfiniteSoul/massive-octo-wookie)

Issues: [![Stories in Ready](https://badge.waffle.io/InfiniteSoul/azuria.svg?label=ready&title=Ready)](http://waffle.io/InfiniteSoul/azuria)

Dokumentation: http://azuria.infinitesoul.de/help

---

##Was ist das?
Azuria ist eine **inoffizielle** Klassenbibliothek, die sowohl die Funktionen der offiziellen Proxer API, als auch einige weitere Funktionen von Proxer.Me für, wie der Titel schon vermuten mag, .NET Sprachen zur Verfügung stellt. Im Moment existiert nur eine "normale" Version, jedoch ist eine portable und eine für Mono bereits geplant. 


##Wie installiere ich es?
Entweder du gehst auf die [Website](http://azuria.infinitesoul.de) und lädst dir entweder die stabile oder die neueste Version runter oder du gibtst folgendes in die NuGet-Konsole ein:
```
PM> Install-Package Azuria
```

##Die Klasse `ProxerResult`
Diese Klasse ist eine Hilfsklasse und sie tritt fast überall auf, insbesondere, wenn die Klassenbibliothek mit Proxer kommuniziert. Sie tritt immer als Rückgabewert auf und gibt dem Anwender jede menge Möglichkeiten zu überprüfen, ob die Methode planmäßig verlaufen ist, indem sie die folgenden Eigenschaften und Methoden bereitstellt:

####Die `Success` Eigenschaft 
Diese Eigenschaft gibt an, ob die Methode erfolgreich war, die dieses Objekt zurückgegeben hat. Wenn diese Eigenschaft einen falschen Wahrheitswert zurückgibt, so kann die `Exceptions` Eigenschaft weiterhelfen.

####Die `Exceptions` Eigenschaft
Hier werden alle Ausnahmen gesammelt, die während der Ausführung der Methode und der dazugehörigen Untermethoden aufgerufen werde. Diese kann aber auch Ausnahmen enthalten, wenn die `Success` Eigenschaft einen wahren Wahrheitswert zurückgibt, wie bei den `Init()` Methoden einiger Klassen.

####Die `Result` Eigenschaft (Nur in der Unterklasse `ProxerResult<T>`)
Diese Eigenschaft gibt das Resultat der Methode zurück und ist vom Typ `T`. Hier muss jedoch beachtet werden, dass wenn die `Success` Eigenschaft einen falschen Wahrheitswert zurückgibt diese Eigenschaft auch einen NULL-Wert zurückgeben kann.

####Die `OnError(T)` Methode (Nur in der Unterklasse `ProxerResult<T>`)
Diese Methode gibt einen festgelegten Wert zurück, wenn nach der Ausführung der Methode die `Success` Eigenschaft einen falschen Wahrheitswert zurückgibt. Ansonsten wird der Rückgabewert der Methode zurückgegeben. Verwendung in C#:
```csharp
bool loggedIn = (await senpai.Login("benutzername", "passwort")).OnError(false);
```
In diesem Beispiel wird der Senpai durch die Methode eingeloggt und es wird zurückgegeben, ob die Aktion erfolgreich war. Wenn nun aber ein Fehler bei der Ausführung der Methode aufgetreten ist, z.B. ist der Proxer-Server nicht verfügbar, dann wird automatisch `false` zurückgegeben. Dies kann gut benutzt werden, um den Quellcode zu vereinfachen und keine zu großen `if` Blöcke zu bauen.


##Noch Fragen? 
Dann schaue dir am besten mal das Beispielprojekt `Azuria.Example` an oder schau in der [Dokumentation] (http://azuria.infinitesoul.de/help) nach. Diese sind immer auf dem aktuellsten Stand mit der Master-Branch. 

## Externe Abhängigkeiten

[JSON .NET](https://www.nuget.org/packages/Newtonsoft.Json/)

[HtmlAgilityPack](https://htmlagilitypack.codeplex.com/)

[RestSharp](http://restsharp.org/)

Diese können auch mit dem Befehl `nuget restore` heruntergeladen werden.
