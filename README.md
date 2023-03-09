# Bioscoop_Simulatie
## Threading in C#

- Auteurs: Kariem de Vries, Sam Echten & Arjan Vijn
- Module: Threading in C#
- Docent: Rob Smit & Rob Loves
- Datum: 9-3-2023
- Versie: 2.0
## Projectleden
| Naam	| Email	| StudentNummer |
| ----- | ----- | ------------- |
| Kariem de Vries	| kariem.de.vries@student.nhlstenden.com | 4716876 | 
| Sam Echten | sam.echten@student.nhlstenden.com | 4687779 |
| Arjan Vijn | arjan.vijn@student.nhlstenden.com | 4675134 |
## Product omschrijving
Het product dat het projectteam gaat maken voor threading in C# is een simulatie van een bioscoop. Binnen deze simulatie worden:
-	Tickets verkocht;
-	Eten en drinken verkocht;
-	Films tegelijkertijd afgespeeld, in verschillende zalen;
-	Na het afspelen van een film wordt de zaal schoongemaakt.

Ook wordt er ook informatie bijgehouden over een aantal onderdelen binnen de simulatie namelijk:
-	Hoe vaak een film afgespeeld wordt;
-	Hoeveel mensen de film bezoeken;
-	Hoeveel mensen de bioscoop bezoeken;
-	Hoeveel eten en drinken verkocht wordt.

Binnen de simulatie gaat threading toegepast worden, hierbij wordt er gebruik gemaakt van de volgende threading technieken:
-	Vanilla threads;
Deze techniek zal gebruikt worden voor de zalen van de bioscoop. Elke zaal zal zijn eigen thread krijgen, waarbij de films en aantal bezoekers meegegeven kunnen worden.

-	Async/await;
Deze threading techniek zal ingezet worden tijdens het wisselen van films in een zaal. Zo zal de zaal bijvoorbeeld eerst schoongemaakt moeten worden voordat er een nieuwe film gedraaid kan worden.

-	Lock;
De lock wordt toegepast op het aantal verkochte kaartjes per zaal en film. Er zal namelijk per zaal en film bijgehouden worden hoeveel kaartjes verkocht zijn tussen alle kassa’s, waardoor dit een shared resource wordt. Om deze resource correct aan te passen kan een lock ingezet worden.

-	Threadpool;
Verkoop van kaarten gaat via de threadpool gedaan worden, als een klant een kaartje koopt wordt dit bij het totaal aantal kaartjes voor die film opgeteld.

-	Semaphore;
Dit zal gebruikt worden om te zorgen dat er maar een bepaald aantal zalen tegelijk een film zullen afspelen. 

-	Async I/O.
Dit wordt toegepast zodat de UI onderdelen niet vastlopen wanneer de gebruiker ergens op klikt. Denk hierbij aan het opstarten van de kassa’s, terwijl er ondertussen ook klanten naar een zaal toe lopen.

## User interface
Voor het weergeven van de bioscoop simulatie zal er een UI worden gebouwd, hierin zullen de verschillende zalen te zien zijn. Ook worden hier de kassa en lobby laten zien. Bij zowel de kassa als de lobby zal bijgehouden worden hoeveel klanten zich er op dat moment bevinden. De klanten kunnen zich langzamerhand verplaatsen van de kassa naar de lobby, en uiteindelijk naar de zaal. Zodra een film wordt gestart zal deze worden weergeven op het scherm van de zaal.

![Mockup bioscoop simulatie](https://cdn.discordapp.com/attachments/1072855744644399156/1083359090270539787/mockups_threading.PNG "Mockup bioscoop simulatie")
FIGUUR 1 - USER INTERFACE
 
## MoSCoW
Hieronder worden de functionaliteiten binnen de applicatie uitgewerkt aan de hand van het MoSCoW model, hierbij kan gelezen worden per hoofdstuk wat de prioriteit per functionaliteit is.
### Must have
-	Films moeten (tegelijkertijd) afgespeeld kunnen worden;
-	Er moeten tickets verkocht kunnen worden;
-	Er moet bijgehouden worden hoe veel tickets er per film en zaal verkocht worden om ervoor te zorgen dat er niet te veel tickets verkocht worden;
-	De UI voor de bioscoop simulatie;
-	Zalen moeten worden schoongemaakt zodra een film is afgelopen.
### Should have
-	Mogelijkheid om zelf films toe te voegen;
-	Eten en drinken moet verkocht / gekocht kunnen worden.
### Could have
-	Mogelijkheid om verschillende soorten eten en drinken toe te voegen en aan te passen.
### Would be cool to implement if we have the time to do it
-	Er tycoon stijl game van maken.
