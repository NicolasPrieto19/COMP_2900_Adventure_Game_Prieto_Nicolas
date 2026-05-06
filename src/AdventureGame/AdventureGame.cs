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

    private bool isChestOpen;
    private bool hasPlayerQuit;

    public AdventureGame()
    {
        
    }

    public void Start()
    {
        Init();

        ShowGameStartScreen();

        string input;
        do
        {
            ShowScene();

            do
            {
                ShowInputOptions();

                input = GetInput();
            }
            while(!IsValidInput(input));

            ProcessInput(input);

            UpdateGameState();
        }
        while(!IsGameOver());

        ShowGameOverScreen();
    }

    private void Init()
    {
        adventurer = new Adventurer();

        Room r1 = new Room();
        r1.SetDescription("Room 1");
        r1.SetSouth(true);
        r1.SetEast(true);
        r1.SetLamp(true);

        Room r2 = new Room();
        r2.SetDescription("Room 2");
        r2.SetSouth(true);
        r2.SetWest(true);
        r2.SetKey(true);

        Room r3 = new Room();
        r3.SetDescription("Room 3");
        r3.SetNorth(true);
        r3.SetEast(true);
        r3.SetChest(true);

        Room r4 = new Room();
        r4.SetDescription("Room 4");
        r4.SetNorth(true);
        r4.SetWest(true);

        dungeon = new Room[,]
        {
            {r1, r2},
            {r3, r4}
        };

        aRow = 1;
        aCol = 0;

        isChestOpen= false;
        hasPlayerQuit = false;

    }

    private void ShowGameStartScreen()
    {
        Console.WriteLine("Welcome to Adventure Game! ");
    }

    private void ShowScene()
    {
        var r = dungeon[aRow, aCol];

        Console.WriteLine(r.GetDescription());
    }

    private void ShowInputOptions()
    {
        string options = ""

        +$"GO NORTH [{GO_NORTH}] | GO EAST [{GO_EAST}] | GET LAMP [{GET_LAMP}] | OPEN CHEST [{OPEN_CHEST}]\n"
        +$"GO SOUTH [{GO_SOUTH}] | GO WEST [{GO_WEST}] | GET KEY  [{GET_KEY}] | QUIT [{QUIT}]\n"
        +$"> ";

        Console.Write(options);
    }

    private string GetInput()
    {
        return Console.ReadLine()!.ToUpper();
    }

    private bool IsValidInput(string input)
    {
        string[] validInput = {GO_NORTH, GO_SOUTH, GO_EAST, GO_WEST, GET_LAMP, GET_KEY, OPEN_CHEST, QUIT};
        if(!validInput.Contains(input))
        {
            Console.WriteLine("ERROR: Invalid input. Please Try Again");
            return false;
        }

        return true;
    }

    private string ProcessInput(string input)
    {
        Room r = dungeon[aRow, aCol];

        if (input == GO_NORTH)
        {
            GoNorth(r);
        }
        else if (input == GO_SOUTH)
        {
            GoSouth(r);
        }
        else if (input == GO_EAST)
        {
            GoEast(r);
        }
        else if (input == GO_WEST)
        {
            GoWest(r);
        }
        else if (input == GET_LAMP)
        {
            GetLamp(r);
        }
        else if (input == GET_KEY)
        {
            GetKey(r);
        }
        else if (input == OPEN_CHEST)
        {
            OpenChest(r);
        }
        else
        {
            Quit();
        }
        return input;
    }

    private void UpdateGameState()
    {
        return;
    }

    private bool IsGameOver()
    {
        return isChestOpen || hasPlayerQuit;
    }

    private void ShowGameOverScreen()
    {

    }

    private void GoNorth(Room r)
    {
        if (r.HasNorth())
        {
            aRow -= 1;
        }
        else
        {
            Console.WriteLine("You cannot go north!!\a");
        }
    }

    private void GoSouth(Room r)
    {
        if (r.HasSouth())
        {
            aRow += 1;
        }
        else
        {
            Console.WriteLine("You cannot go south!!\a");
        }
    }

    private void GoEast(Room r)
    {
        if (r.HasEast())
        {
            aCol += 1;
        }
        else
        {
            Console.WriteLine("You cannot go east!!\a");
        }
    }

    private void GoWest(Room r)
    {
        if (r.HasWest())
        {
            aCol -= 1;
        }
        else
        {
            Console.WriteLine("You cannot go west!!\a");
        }
    }

    private void GetLamp(Room r)
    {
        if (r.HasLamp())
        {
            Console.WriteLine("You got the lamp!!\a");
            adventurer.SetLamp(true);
            r.SetLamp(false);
        }
        else
        {
            Console.WriteLine("There is no lamp in this room!!\a");
        }
    }

    private void GetKey(Room r)
    {
        if (r.HasKey())
        {
            Console.WriteLine("You got the key!!\a");
            adventurer.SetKey(true);
            r.SetKey(false);
        }
        else
        {
            Console.WriteLine("There is no key in this room!!\a");
        }
    }
    
    private void OpenChest(Room r)
    {
        if (r.HasChest())
        {
            if (adventurer.HasKey())
            {
                Console.WriteLine("You opened the chest and found the treasure!!\a");
                isChestOpen = true;
            }
            else
            {
                Console.WriteLine("You need a key to open the chest!!\a");
            }
        }
        else
        {
            Console.WriteLine("There is no chest in this room!!\a");
        }
    }

     private void Quit()
    {
        Console.WriteLine("Thanks for playing Adventure Game!!");
        hasPlayerQuit = true;
    }
}