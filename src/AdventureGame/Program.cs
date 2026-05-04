namespace AdventureGame;

public class Program
{
    public static void Main()
    {
        Adventurer a = new Adventurer();
        a.SetLamp(true);
        a.SetKey(true);

        Room r = new Room();
        r.SetDescription("Room 1");
        r.SetLamp(true);
        Console.WriteLine(a);
        Console.WriteLine(r);
    }
}