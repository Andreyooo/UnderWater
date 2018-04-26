## Finale Projektbeschreibung

_Amin Uzdenov, André Lavrinov, Dennis Lavrinov
WF: Spieleentwicklung & Creative Coding_

_19.04.2018_
&nbsp;

### Glooms

#### Der große Kampf um Atlantis

_Glooms ist angelehnt an die Worms-Reihe mit innovativen bahnbrechenden
Gameplaymechaniken._
&nbsp;

#### Story​ :
Im Reich der Glooms gibt es drei verschiedene Völker, welche auf verschiedenen
Inseln des Atlantiks leben. Eines Tages taucht zwischen den drei Inseln aus dem
Ozean eine gigantische Stadt auf, Atlantis. Bisher nur als alter Mythos bekannt,
gehen die Inselbewohner von unzähligen Schätzen und antiken Technologien aus.
Es bricht ein großer Krieg aus, aber die Völker, klug und weise, haben aus
vorherigen Konflikten gelernt und um große Verluste zu vermeiden, haben sich die
Oberhäupter darauf geeinigt, nur ihre drei stärksten und tapfersten Krieger in den
Kampf zu schicken. In diesem legendären Kampf auf Leben und Tod wird sich
entscheiden, welches Volk den atlantischen Reichtümern würdig ist.
&nbsp;

#### Gameplay:
Am Anfang des Spiels erscheint ein Intro, dass kurz die Story erläutert, damit der
Spieler in die Welt von Glooms eintauchen kann. Über das Menü wird die Map und
die Fraktion ausgewählt. Nachdem die Fraktionen der Spieler zufällig auf der Map
verteilt wurden, beginnt eine zufällig ausgewählte Einheit eines Spieler seine Runde.
Sein Gloomy besitzt eine begrenzte Anzahl an Aktionspunkten, mit der er sich
bewegen oder Waffen, Items und Skills nutzen kann. Der Spieler hat außerdem die
Möglichkeit in eine Kameraansicht zu wechseln, um die Map zu überblicken. Beim
Bewegen eines Gloomys kann der Spieler nur eine begrenzte Entfernung
zurücklegen. Mit den restlichen Aktionspunkten wiederholt sich das Ganze. Wählt er
eine Waffe aus, so öffnet sich das Ziel-Menü, in welchem er mit der Maus die
Richtung und die Stärke des Projektils steuern kann. Nach der Benutzung wird
dieses abgefeuert und schlägt hoffentlich am gewünschten Ziel ein.



Die Flugbahn wird hier auch vom Wetter, beispielsweise vom Wind beeinflusst.
Trifft der Spieler einen feindliche Gloomy, verliert dieser Leben abhängig von der
Stärke der Waffe. Zusätzlich erhält der Gloomy des aktuellen Spielers Erfahrung für
den erfolgreichen Treffer. Sammelt ein Gloomy genug Erfahrung, kann er ein
Upgrade durchführen und sich in verschiedenen Bereichen verbessern. Hat ein
Gloomy seine Aktionspunkte aufgebraucht oder ist das Zeitlimit überschritten, ist
sein Zug beendet und ein Gloomy der anderen Fraktion ist an der Reihe. 
Die Gloomys wechseln sich fraktionsweise und ebenfalls untereinander ab, sodass nach
9 Runden jeder Gloomy an der Reihe war. Im Verlaufe des Spiels erhöht sich die
Anzahl an Aktionspunkten, die einem Gloomy zur Verfügung steht, damit die
Gloomys mehr und vor allem mächtigere Aktionen durchführen können. Neue
Waffen werden per Luftunterstützung im Spielfeld abgeworfen und können
eingesammelt werden. Die durchschnittliche Spieldauer beträgt zwischen 15 und 30
Minute. Das Spiel selbst soll sich durch die diversen Upgrades zuspitzen, wodurch
ein deutlicher Kontrast zwischen dem Anfang und dem Ende einer Spielpartie
entstehen soll.
&nbsp;

#### Technische Elemente:

* 2D
* Rundenbasiert
  * Zeitlimit und APS (ActionPointSystem)
* Multiplayer (3 Spieler)
  * lokal mit eventueller Online-Erweiterung
* Tasten- und Maussteuerung
  * Menü, Einheitenbewegung, Zielsystem
* Physik
  * ballistische Elemente (Raketen, Steine)
* Rückstoß bei Treffern
* Beeinflussbare Umgebung (Map)
  * zerstörbar
  * Wind, Wasser, etc.
    &nbsp;


#### Big Idea:

Um Glooms ein einzigartiges Spielgefühl zu geben, haben wir uns zwei große
Gameplay-Elemente überlegt. Zum einen ein RPG- bzw. Leveling-System, bei dem
die Gloomies während des Kampfes Erfahrung sammeln und damit Fähigkeiten und
Upgrades freischalten. Diese können zusätzliche Skills(Schaden oder Utility), als
auch Upgrades, wie z.B. erhöhtes Leben oder eine extra Waffe sein. Grundsätzlich
sollen die PowerUps vielseitig sein und viele Vorteile in verschiedenen Situation
liefern. Das heißt, man sollte grundsätzlich in der Lage sein, sich auch aus
brenzlichen Situationen befreien zu können oder seine Position vorteilhaft
auszunutzen z.B.: ein Gloomy, der auf einem hohen Berg sitzt und sich ein
Scharfschützen-Gewehr verschafft, da er freie Schussbahn auf seine Gegner hat.
Das Leveling-System haben wir uns überlegt, um dem Spiel mehr Tiefe zu geben
und um das Spiel dynamischer zu gestalten.



Beim zweiten Gameplay-Element handelt es sich um das Action-Point-System.
Jeder Gloomy besitzt eine gewisse Anzahl an Aktionspunkten und jede Aktion
kostet Aktionspunkte, somit muss jeder Gloomy seine Handlungen sorgfältig planen.
Zu Aktionen gehören Bewegung und das Nutzen von Waffen, sowie Skills. Die
Höhe der Kosten unterscheiden sich je nach “Mächtigkeit” der eingesetzten Aktion.
Stärkere Waffen bzw. mächtige Skills kosten auch deutlich mehr Aktionspunkte.
Im Verlaufe des Spiels erhöht sich die Anzahl an Aktionspunkten, die jedem Gloomy
zur Verfügung stehen. Wir haben uns das APS überlegt, da wir RPG-Elemente in
das Spiel bringen wollen und es dadurch unterschiedlich starke Fähigkeiten gibt.
Damit das Spiel weiterhin balanced bleibt, finden wir, dass durch eine feste Anzahl
an Aktionspunkten ein Spieler der beispielsweise seinen Gloomy bereits upgegraded
hat und mächtigere Fähigkeiten besitzt, dennoch nicht zu weit vorne liegt, da er
eventuell bei einem starken Angriff nicht mehr genug Punkte hat, um sich zu
bewegen. Zu dem kommt der taktische Aspekt, dass heißt man muss wohl
überlegen welche Aktionen man unternimmt, eventuell muss man sogar etwas
rechnen. Außerdem wollen wir kein zu straffes Zeitlimit verwenden. Der Spieler soll
sich nicht gehetzt fühlen, wenn er seine Aktionen plant. Zusätzlich ergeben sich
durch das APS auch bestimmte Interaktionen bzw. Möglichkeiten, wie z.B. Waffen,
die einem feindlichen Gloomy die Aktionspunkte für einen Zug verringern und dafür
vielleicht wenig Schaden zufügen.
&nbsp;

#### Projekt-Layers:

**Funktionales Minimum (Layer 1):**
* 3 Fraktionen mit je einer Einheit
* 2 unterschiedliche Waffen
* 2D-Map
* funktionierendes PVP System (Leben/Schaden)
* rundenbasiertes Gameplay
* fixe Spawns

**Minimalziel (Layer 2):**
* einfaches Menü
* weitere Waffentypen
* einfaches Art-Design
* unterschiedliches Fraktions-Designs (Passive Skills)
* ActionPointSystem
* Audioeffekte
* ausbalanciertes Spawnsystem
* erste RPG-Elemente

**Ziel (Layer 3):**
* solides Menü
* zerstörbare Umgebung
* Umwelteinflüsse (Wind/Regen, etc)
* Umfangreiche Audioeffekte
* Audio-Synchronisation der Fraktionen
* RPG-System (Leveln, Skills, etc)
* Intro für die Story
* unterschiedliche Maps

**Wunschziel (Layer 4):**
* Online-Multiplayer
* Optionale Ziele/Objectives
* verbessertes Grafikdesign
* Individualisierung jeder Einheit einer Fraktion

**Extras (Layer 5):**
* Startscreen mit Artworks
* erweiterte Story mit Hintergrundinformationen zu den Fraktionen
  * Story Trailer
    &nbsp;

####  Entwicklungszeitplan
* Link zu Agantty: https://app.agantty.com/sharing/8d5aa254c7f989a3175797d66d6447ed

&nbsp;

#### Teamaufteilung:

**Amin​ :**
* Game-Designer
* Programmierer
* Qualitätssicherung
* Map-Designer

**André​ :**
* Game-Designer
* Programmierer
* Künstler
* Synchronsprecher

**Dennis​ :**
* Game-Designer
* Programmierer
* Produzent
* Sound-Engineer