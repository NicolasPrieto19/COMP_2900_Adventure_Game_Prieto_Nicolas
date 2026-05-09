# Adventure Game

Proyecto universitario para la clase de Estructura de Datos para la Universida Interamericana de Puerto Rico. Es un juego de texto por consola donde el jugador debe escapar de un dungeon recogiendo objetos y evitando al enemigo.

## Como jugar

El jugador empieza en un cuarto oscuro. El objetivo es recoger la lampara, encontrar la llave y abrir el cofre para ganar. Si el Grue te alcanza, pierdes.

El juego termina cuando:
- Abres el cofre (ganas)
- El Grue llega a tu cuarto (pierdes)
- Sales voluntariamente

## Controles

| Tecla | Accion |
|-------|--------|
| W | Ir al norte |
| S | Ir al sur |
| D | Ir al este |
| A | Ir al oeste |
| L | Recoger lampara |
| K | Recoger llave |
| O | Abrir cofre |
| Q | Salir |

## Mapa

```
[C1: Lampara] --- [C2: Llave] --- [C5]
      |               |             |
[C3: Cofre*]  --- [C4]        --- [C6]
```

El jugador empieza en C3. El Grue empieza en C6.

## Estructura del codigo

| Archivo | Descripcion |
|---------|-------------|
| `Program.cs` | Punto de entrada del programa |
| `AdventureGame.cs` | Logica principal del juego, loop de turnos |
| `Adventurer.cs` | Estado del jugador (inventario) |
| `Room.cs` | Representa cada cuarto del dungeon |
| `Grue.cs` | Enemigo con pathfinding usando BFS |

## El algoritmo del Grue

El Grue usa BFS (Breadth-First Search) para encontrar la ruta mas corta hacia el jugador. Se mueve un paso cada 3 turnos del jugador. La logica completa esta documentada en el video explicativo del codigo.