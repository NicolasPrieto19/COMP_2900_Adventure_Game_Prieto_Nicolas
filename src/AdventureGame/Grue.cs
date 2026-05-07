namespace AdventureGame;
public class Grue
{
    private int gRow; 
    private int gCol; 

    public Grue(int startRow, int startCol)
    {
        gRow = startRow;
        gCol = startCol;
    }

    public int GetRow() { return gRow; }
    public int GetCol() { return gCol; }

    // Mueve el Grue un paso hacia el jugador usando BFS.
    public void MoveTowardsPlayer(int playerRow, int playerCol, Room[,] dungeon)
    {

        if (gRow == playerRow && gCol == playerCol) return;

        var queue = new Queue<(int row, int col)>();

        var cameFrom = new Dictionary<(int, int), (int, int)>();

        var start = (gRow, gCol);
        queue.Enqueue(start);
        cameFrom[start] = (-1, -1); // el punto de inicio no tiene origen

        bool found = false;

        // Exploramos cuarto por cuarto hasta encontrar al jugador
        while (queue.Count > 0 && !found)
        {
            var current = queue.Dequeue();
            int r = current.row;
            int c = current.col;
            Room room = dungeon[r, c];

            // Revisamos los 4 vecinos posibles segun las puertas del cuarto actual
            // Solo podemos ir a un vecino si la puerta existe Y el vecino no fue visitado

            // Norte: fila - 1
            if (room.HasNorth() && !cameFrom.ContainsKey((r - 1, c)))
            {
                // Registramos que el vecino (r-1, c) viene de (r, c)
                cameFrom[(r - 1, c)] = (r, c);
                // Si el vecino es el jugador, terminamos la busqueda
                if (r - 1 == playerRow && c == playerCol) { found = true; break; }
                //Si no es el jugador, lo agregamos a la cola para seguir explorando
                queue.Enqueue((r - 1, c));
            }
            // Sur: fila + 1
            if (room.HasSouth() && !cameFrom.ContainsKey((r + 1, c)))
            {
                cameFrom[(r + 1, c)] = (r, c);
                if (r + 1 == playerRow && c == playerCol) { found = true; break; }
                queue.Enqueue((r + 1, c));
            }
            // Este: columna + 1
            if (room.HasEast() && !cameFrom.ContainsKey((r, c + 1)))
            {
                cameFrom[(r, c + 1)] = (r, c);
                if (r == playerRow && c + 1 == playerCol) { found = true; break; }
                queue.Enqueue((r, c + 1));
            }
            // Oeste: columna - 1
            if (room.HasWest() && !cameFrom.ContainsKey((r, c - 1)))
            {
                cameFrom[(r, c - 1)] = (r, c);
                if (r == playerRow && c - 1 == playerCol) { found = true; break; }
                queue.Enqueue((r, c - 1));
            }
        }

        if (!found) return;

        // Reconstruimos la ruta desde el jugador hacia atras hasta el Grue,
        // para saber cual es el PRIMER paso que debe dar el Grue.
        var path = new List<(int, int)>();
        var step = (playerRow, playerCol);

        while (step != start)
        {
            path.Add(step);
            step = cameFrom[step];
        }

        // El ultimo elemento de path (antes de start) es el primer paso del Grue
        // path esta en orden inverso, asi que el primer paso es el ultimo elemento
        var nextStep = path[path.Count - 1];
        gRow = nextStep.Item1;
        gCol = nextStep.Item2;
    }

    // Devuelve una descripcion de donde se escucha el rugido del Grue
    public string GetGrueHint(int playerRow, int playerCol)
    {
        int dr = gRow - playerRow;
        int dc = gCol - playerCol;

        if (dr == 0 && dc == 0) return "";

        bool adjacent = (Math.Abs(dr) + Math.Abs(dc)) == 1;
        string intensity = adjacent ? "un rugido FUERTE" : "un rugido lejano";

        string direction = "";
        if (dr < 0 && dc == 0) direction = "al norte";
        else if (dr > 0 && dc == 0) direction = "al sur";
        else if (dr == 0 && dc > 0) direction = "al este";
        else if (dr == 0 && dc < 0) direction = "al oeste";
        else if (dr < 0 && dc > 0) direction = "al noreste";
        else if (dr < 0 && dc < 0) direction = "al noroeste";
        else if (dr > 0 && dc > 0) direction = "al sureste";
        else direction = "al suroeste";

        return $"Escuchas {intensity} proveniente {direction}...";
    }
}