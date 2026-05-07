using System.ComponentModel;

namespace AdventureGame;

public class AdventureGame
{
    public readonly string GO_NORTH = "W";
    public readonly string GO_SOUTH = "S";
    public readonly string GO_EAST = "D";
    public readonly string GO_WEST = "A";
    public readonly string GET_LAMP = "L";
    public readonly string GET_KEY = "K";
    public readonly string OPEN_CHEST = "O";
    public readonly string QUIT = "Q";

    private Adventurer adventurer;
    private Room[,] dungeon;
    private int aRow;
    private int aCol;

    private Grue grue; 
    private int grueMoveCooldown; 
    private bool isEatenByGrue;
    private bool isChestOpen;
    private bool hasPlayerQuit;

    public AdventureGame()
    {
    }

    public void Start()
    {
        Init();

        ShowGameStartScreen();
        ShowMap();

        Console.WriteLine("\nPresiona cualquier tecla para comenzar...");
        Console.ReadKey();
        Console.Clear();

        string input;
        string lastMessage = ""; 
        do
        {
            Console.Clear();

            if (lastMessage != "")
            {
                Console.WriteLine(lastMessage);
                Console.WriteLine();
                lastMessage = "";
            }

            ShowScene();

            Console.WriteLine(grue.GetGrueHint(aRow, aCol));
            Console.WriteLine();

            do
            {
                ShowInputOptions();
                input = GetInput();
            }
            while (!IsValidInput(input));

            lastMessage = ProcessInput(input);

            UpdateGameState();
        }
        while (!IsGameOver());

        ShowGameOverScreen();
    }

    private void Init()
    {
        adventurer = new Adventurer();

        Room r1 = new Room();
        r1.SetDescription("Cuarto 1 - Noroeste");
        r1.SetSouth(true);
        r1.SetEast(true);
        r1.SetLamp(true);

        Room r2 = new Room();
        r2.SetDescription("Cuarto 2 - Norte Centro");
        r2.SetSouth(true);
        r2.SetEast(true);
        r2.SetWest(true);
        r2.SetKey(true); 
        Room r5 = new Room();
        r5.SetDescription("Cuarto 5 - Noreste");
        r5.SetSouth(true);
        r5.SetWest(true);

        Room r3 = new Room();
        r3.SetDescription("Cuarto 3 - Suroeste");
        r3.SetNorth(true);
        r3.SetEast(true);
        r3.SetChest(true);

        Room r4 = new Room();
        r4.SetDescription("Cuarto 4 - Sur Centro");
        r4.SetNorth(true);
        r4.SetEast(true);
        r4.SetWest(true);

        Room r6 = new Room();
        r6.SetDescription("Cuarto 6 - Sureste");
        r6.SetNorth(true);
        r6.SetWest(true);

        dungeon = new Room[,]
        {
            { r1, r2, r5 },
            { r3, r4, r6 }
        };

        aRow = 1;
        aCol = 0;

        grue = new Grue(1, 2);

        grueMoveCooldown = 0;

        isEatenByGrue = false;
        isChestOpen = false;
        hasPlayerQuit = false;
    }

    private void ShowGameStartScreen()
    {
        Console.Clear();
        Console.WriteLine("===========================================");
        Console.WriteLine("       BIENVENIDO A ADVENTURE GAME        ");
        Console.WriteLine("===========================================");
        Console.WriteLine();
        Console.WriteLine("Estas atrapado en un dungeon oscuro.");
        Console.WriteLine("Busca la lampara, encuentra la llave,");
        Console.WriteLine("y abre el cofre para escapar.");
        Console.WriteLine("Pero cuidado... algo acecha en la oscuridad.");
        Console.WriteLine();
    }    private void ShowMap()
    {
        Console.WriteLine("--- MAPA DEL DUNGEON ---");
        Console.WriteLine();
        Console.WriteLine("  [Cuarto 1]---[Cuarto 2]---[Cuarto 5]");
        Console.WriteLine("      |             |             |    ");
        Console.WriteLine("  [Cuarto 3]---[Cuarto 4]---[Cuarto 6]");
        Console.WriteLine();
        Console.WriteLine("  Cuarto 1: Lampara");
        Console.WriteLine("  Cuarto 2: Llave");
        Console.WriteLine("  Cuarto 3: Cofre  <- TU ESTAS AQUI");
        Console.WriteLine("  Cuarto 4: (vacio)");
        Console.WriteLine("  Cuarto 5: (vacio)");
        Console.WriteLine("  Cuarto 6: (vacio)");
        Console.WriteLine();
        Console.WriteLine("  Controles: W=Norte  S=Sur  A=Oeste  D=Este");
        Console.WriteLine("             L=Lampara  K=Llave  O=Cofre  Q=Salir");
        Console.WriteLine("------------------------");
    }
    private void ShowScene()
    {
        var r = dungeon[aRow, aCol];
        Console.WriteLine("-------------------------------------------");

        if (!adventurer.HasLamp())
        {
            Console.WriteLine("Estas en un cuarto oscuro. No puedes ver nada.");
            Console.WriteLine("(Necesitas encontrar una lampara para ver el cuarto)");
        }
        else
        {
            Console.WriteLine(r.GetDescription());

            if (r.HasLamp()) Console.WriteLine("  - Hay una LAMPARA en el suelo.");
            if (r.HasKey())  Console.WriteLine("  - Hay una LLAVE colgando de la pared.");
            if (r.HasChest()) Console.WriteLine("  - Hay un COFRE en la esquina.");

            string salidas = "  Salidas: ";
            if (r.HasNorth()) salidas += "[W]Norte ";
            if (r.HasSouth()) salidas += "[S]Sur ";
            if (r.HasEast())  salidas += "[D]Este ";
            if (r.HasWest())  salidas += "[A]Oeste ";
            Console.WriteLine(salidas);
        }

        Console.WriteLine($"  Inventario: Lampara = {adventurer.HasLamp()} | Llave = {adventurer.HasKey()}");
        Console.WriteLine("-------------------------------------------");
    }

    private void ShowInputOptions()
    {
        string options = ""
            + $"Moverse: [{GO_NORTH}]Norte [{GO_SOUTH}]Sur [{GO_EAST}]Este [{GO_WEST}]Oeste\n"
            + $"Accion:  [{GET_LAMP}]Lampara [{GET_KEY}]Llave [{OPEN_CHEST}]Cofre [{QUIT}]Salir\n"
            + "> ";

        Console.Write(options);
    }

    private string GetInput()
    {
        return Console.ReadLine()!.ToUpper();
    }

    private bool IsValidInput(string input)
    {
        string[] validInput = { GO_NORTH, GO_SOUTH, GO_EAST, GO_WEST, GET_LAMP, GET_KEY, OPEN_CHEST, QUIT };
        if (!validInput.Contains(input))
        {
            Console.WriteLine("ERROR: Entrada invalida. Por favor intenta de nuevo.");
            return false;
        }
        return true;
    }
    private string ProcessInput(string input)
    {
        Room r = dungeon[aRow, aCol];

        if (input == GO_NORTH)        return GoNorth(r);
        else if (input == GO_SOUTH)   return GoSouth(r);
        else if (input == GO_EAST)    return GoEast(r);
        else if (input == GO_WEST)    return GoWest(r);
        else if (input == GET_LAMP)   return GetLamp(r);
        else if (input == GET_KEY)    return GetKey(r);
        else if (input == OPEN_CHEST) return OpenChest(r);
        else                          return Quit();
    }
    private void UpdateGameState()
    {
        grueMoveCooldown++;

        if (grueMoveCooldown >= 3)
        {
            grue.MoveTowardsPlayer(aRow, aCol, dungeon);
            grueMoveCooldown = 0; 
        }
        if (grue.GetRow() == aRow && grue.GetCol() == aCol)
        {
            isEatenByGrue = true;
        }
    }

    private bool IsGameOver()
    {
        return isChestOpen || hasPlayerQuit || isEatenByGrue;
    }

    private void ShowGameOverScreen()
    {
        Console.WriteLine();
        Console.WriteLine("===========================================");

        if (isEatenByGrue)
        {
            Console.WriteLine("   EL GRUE TE HA DEVORADO. GAME OVER.");
            Console.WriteLine("   TODO SE HA VUELTO OSCURO...");
            Console.WriteLine("   PARECE QUE EL GRUE TE HA ENCONTRADO DESPUES DE TODO...");
            Console.WriteLine("   LO SABES PORQUE ESTAS EN SU PANZA!!");
        }
        else if (isChestOpen)
        {
            Console.WriteLine("  FELICIDADES! ENCONTRASTE EL TESORO!");
            Console.WriteLine("  Has escapado del dungeon. GANASTE!");
        }
        else
        {
            Console.WriteLine("  Has abandonado el dungeon. Hasta la proxima.");
        }

        Console.WriteLine("===========================================");
    }

    private string GoNorth(Room r)
    {
        if (r.HasNorth()) { aRow -= 1; return ""; }
        return "No puedes ir al norte desde aqui!";
    }

    private string GoSouth(Room r)
    {
        if (r.HasSouth()) { aRow += 1; return ""; }
        return "No puedes ir al sur desde aqui!";
    }

    private string GoEast(Room r)
    {
        if (r.HasEast()) { aCol += 1; return ""; }
        return "No puedes ir al este desde aqui!";
    }

    private string GoWest(Room r)
    {
        if (r.HasWest()) { aCol -= 1; return ""; }
        return "No puedes ir al oeste desde aqui!";
    }

    private string GetLamp(Room r)
    {
        if (r.HasLamp())
        {
            adventurer.SetLamp(true);
            r.SetLamp(false);
            return "Recogiste la lampara! Ahora puedes ver el dungeon.";
        }
        else if (adventurer.HasLamp())
        {
            return "Ya tienes la lampara contigo.";
        }
        return "No hay ninguna lampara en este cuarto.\n(Pista: busca la lampara en otro cuarto para poder ver)";
    }

    private string GetKey(Room r)
    {
        if (r.HasKey())
        {
            adventurer.SetKey(true);
            r.SetKey(false);
            return "Recogiste la llave! Ahora puedes abrir el cofre.";
        }
        else if (adventurer.HasKey())
        {
            return "Ya tienes la llave contigo.";
        }
        return "No hay ninguna llave en este cuarto.\n(Pista: busca la llave en otro cuarto para poder abrir el cofre)";
    }

    private string OpenChest(Room r)
    {
        if (r.HasChest())
        {
            if (adventurer.HasKey())
            {
                isChestOpen = true;
                return "Abriste el cofre y encontraste el tesoro!";
            }
            return "El cofre esta cerrado con llave.\n(Necesitas encontrar una llave antes de poder abrirlo)";
        }
        return "No hay ningun cofre en este cuarto.";
    }

    private string Quit()
    {
        hasPlayerQuit = true;
        return "Gracias por jugar Adventure Game!";
    }
}