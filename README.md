# Assault Cube Aimbot DIPL
- Projekt je softver za varanje koji radi na pucačkoj videoigri Assault Cube
- Softver otkriva pozicije drugih igrača (crta kvadrat oko njihovih modela te vuče linije od dna prozora do njih) te
 automatski cilja na najbližeg neprijateljskog igrača na pritisak određene tipke (LCTRL)

# Instalacija i korištenje Assault Cube 
- Preuzeti videoigru s ovog linka - https://assault.cubers.net/download.html 
- Nakon završene instalacije igra se pokreće dvoklikom na "assaultcube.bat" datoteku
- Pri paljenju igra otvara meni gdje se podešava nadimak i rezolucija
- Nije potrebno kreirati nikakve account-ove za igru, to se može preskočiti odabiranjem "Ok" opcije ispod
- Otvaranje i zatvaranje menu-a se radi ESC tipkom, a navigiranje po menu-u sa strelicama gore i dolje, ENTER za odabiranje opcija te strelice lijevo i desno za bilo kakve opcije sa slider-om
- Kretanje u video igri se radi sa WASD tipkama, pucanje sa lijevim klikom miša
- Za testiranje softvera protiv AI igrača: Singleplayer -> Team Deathmatch (ili bilo koji drugi game mode, no za svrhu jednostavnosti) -> Odabrati težinu AI igrača -> Odabrati broj igrača po timu te će runda početi (u slučaju umiranja u rundi, lijevi klik miša radi respawn (ponovno vraćanje u rudnu) igrača)

# Korištenje softvera za varanje
- Prvo je potrebno extract-ati ezOverlay projekt iz ezOverlay.dll.zip
- Tada pokrenuti ezOverlay.sln i build-ati projekt
- Nakon toga projekt se može zatvoriti i može se otvoriti originalni projekt gdje je potrebno ubaciti ezOverlay.dll
- U Aimbot projektu desni klik na References -> Add Reference -> Browse te otići u extract-ani projekt ezOverlay->bin->Debug i odabrati ezOverlay.dll
- Nakon pokretanja Assault Cube videoigre, softver je dovoljno pokrenuti iz Visual Studio-a
- Prozor aplikacije je nevidljiv i napravljen je da se nalazi na istoj poziciji kao i Assault Cube prozor u slučaju pomicanja po ekranu (također da bude iste veličine)


![slika](https://github.com/psJustDoit/DIPL-Aimbot-ESP-Projekt/assets/86831771/b910d478-b249-40a2-bbf0-539705103dee)
![slika](https://github.com/psJustDoit/DIPL-Aimbot-ESP-Projekt/assets/86831771/85d867da-7ac6-4809-8383-5c71c70e1a29)
