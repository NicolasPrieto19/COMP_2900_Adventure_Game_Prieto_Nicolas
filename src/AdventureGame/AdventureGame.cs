using System.ComponentModel;

namespace AdventureGame;

public class AdventureGame
{
    
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

        Room r = new Room();
         r.SetDescription("Room 0");
        dungeon = new Room[,]
        {
            {r}
        };

        aRow = 0;
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
        return;
    }

    private string GetInput()
    {
        return string.Empty;
    }

    private bool IsValidInput(string input)
    {
        return true;
    }

    private string ProcessInput(string input)
    {
        return string.Empty;
    }

    private void UpdateGameState()
    {
        return;
    }

    private bool IsGameOver()
    {
        return true;
    }

    private void ShowGameOverScreen()
    {
        Console.WriteLine("Thanks for playing!");
    }
}