using System.ComponentModel;

namespace AdventureGame;

public class AdventureGame
{
    public readonly string GO_NORTH = "W";
    public readonly string GO_SOUTH = "S";
    public readonly string GO_EAST = "A";
    public readonly string GO_WEST = "D";
    public readonly string GET_LAMP = "L";
    public readonly string GET_KEY = "K";
    public readonly string OPEN_CHEST = "O";
    public readonly string QUIT = "Q";

    private Adventurer adventurer;
    private Room[,] dungeon;
    private int aRow;
    private int aCol;

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

        Room rCenter = new Room();
        rCenter.SetDescription("Room Center");
        rCenter.SetNorth(true);

        Room rNorth = new Room();
        rNorth.SetDescription("Room North");
        rNorth.SetSouth(true);

        dungeon = new Room[,]
        {
            {rNorth},
            {rCenter}
        };

        aRow = 1;
        aCol = 0;
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
        string[] validInput = {GO_NORTH, GO_SOUTH, GO_EAST, GO_WEST, GET_LAMP, GET_KEY};
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
        return input;
    }

    private void UpdateGameState()
    {
        return;
    }

    private bool IsGameOver()
    {
        return false;
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
}