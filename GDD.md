# GDD

Este es el documento de diseño de TennisFightingGame. Esto no es público 
asique lo escribo en castellano.

## Stats

* Sprint speed:

* Stamina regeneration:

* Endurance degenderation:

## Personajes

### Linyera

Es un all-rounder más tirando para defensivo que ofensivo. 

#### Ventajas

+ Tiene la mejor endurance del juego 

+ Tiene muy buena velocidad de sprint

+ Tiene muy buena variedad de ataques, muy buen control horizontal de 
  la trayectoria de la pelota

#### Desventajas

- Gasta más stamina que la mayoría de los personajes, por más que tenga 
  buena endurance

- Su control vertical de la pelota es un poco pobre

- Sus ataques son un poco debiles, aunque suficientemente fuertes como 
  para tener al menos una opción de devolución a cualquier distancia

#### Moveset

* Standing light: Le pega despacio y le baja la gravedad un poco a la 
  pelota para dejarla servidita.
  
* QCF standing/airborne light: La deja bien cortita con un golpecito.

* Standing/Airborne medium: Drive moderadamente fuerte que desde mitad de 
  cancha llega a devolver la pelota. Tiene soft hit-cancel y reduce muy 
  levemente la gravedad.
  Hay una ventana chiquita (0.1 secs, 6 frames) para hacer un sprint y 
  llegar a pegar un standing heavy, y una ventana aun más chica 
  (0,033 secs, 2 frames) para pegar un HCF standing heavy.

* Standing/Airborne heavy: Revés rápido y fuerte, un poquito laggy.

* HCF standing heavy: Drive completo, con la C y todo. Muy fuerte.
