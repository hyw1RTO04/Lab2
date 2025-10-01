using System;
using System.IO;

enum State
{
    Winner,
    Looser,
    Playing,
    NotInGame
}

class Player
{
    public string name;
    public int location;
    public State state = State.NotInGame;
    public int distanceTraveled = 0;

    public Player(string name)
    {
        this.name = name;
        this.location = -1;
    }

    public void Move(int steps, int boardSize)
    {
        if (state == State.Playing)
        {
            location = (location + steps) % boardSize;
            if (location < 0)
                location += boardSize;

            distanceTraveled += Math.Abs(steps);
        }
    }

    public bool IsInGame()
    {
        return state == State.Playing || state == State.Winner || state == State.Looser;
    }

    public int GetLocation()
    {
        return location;
    }
}

enum GameState
{
    Start,
    End
}

class Game
{
    public static string InputFile;
    public static string OutFile;

    public int size;
    public Player cat;
    public Player mouse;
    public GameState state;
    private int? caughtLocation = null;

    public Game(int size)
    {
        this.size = size;
        cat = new Player("Cat");
        mouse = new Player("Mouse");
        state = GameState.Start;
    }

    private void DoMoveCommand(char command, int steps)
    {
        Player player = (command == 'C') ? cat : mouse;

        if (player.state == State.NotInGame)
        {
            // первая установка позиции — дистанция не считается
            player.location = steps % size;
            if (player.location < 0)
                player.location += size;

            player.state = State.Playing;
            player.distanceTraveled = 0;
        }
        else
        {
            player.Move(steps, size);
        }

        CheckGameStatus();
    }

    private void CheckGameStatus()
    {
        if (state != GameState.End && cat.IsInGame() && mouse.IsInGame() && cat.location == mouse.location)
        {
            cat.state = State.Winner;
            mouse.state = State.Looser;
            state = GameState.End;
            caughtLocation = cat.location;
        }
    }

    private void DoPrintCommand(StreamWriter writer)
    {
        string cat_location, mouse_location, distance;

        if (cat.IsInGame())
            cat_location = cat.GetLocation().ToString();
        else
            cat_location = "??";

        if (mouse.IsInGame())
            mouse_location = mouse.GetLocation().ToString();
        else
            mouse_location = "??";

        if (cat.IsInGame() && mouse.IsInGame())
            distance = GetDistance().ToString();
        else
            distance = "";

        string output = $"{cat_location,3} {mouse_location,5} {distance,9}";
        Console.WriteLine(output);
        writer.WriteLine(output);
    }

    private int GetDistance()
    {
        return Math.Abs(cat.GetLocation() - mouse.GetLocation());
    }

    private void DoStartCommand(char player, int position)
    {
        int loc = position % size;
        if (loc < 0)
            loc += size;

        if (player == 'C')
        {
            cat.location = loc;
            cat.state = State.Playing;
        }
        else if (player == 'M')
        {
            mouse.location = loc;
            mouse.state = State.Playing;
        }
    }

    private void WriteLine(string message, StreamWriter writer)
    {
        Console.WriteLine(message);
        writer.WriteLine(message);
    }

    public void Run(string[] lines)
    {
        StreamWriter writer = new StreamWriter(OutFile);

        WriteLine(" Cat and Mouse", writer);
        WriteLine("", writer);
        WriteLine("Cat Mouse  Distance", writer);
        WriteLine("-------------------", writer);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0 || string.IsNullOrEmpty(parts[0]))
                continue;

            char command = parts[0][0];

            if (command == 'S' && parts.Length >= 3)
            {
                char player = parts[1][0];
                int position = int.Parse(parts[2]);
                DoStartCommand(player, position);
            }
            else if ((command == 'C' || command == 'M') && parts.Length >= 2)
            {
                int steps = int.Parse(parts[1]);
                DoMoveCommand(command, steps);

                if (state == GameState.End)
                    break;
            }
            else if (command == 'P')
            {
                if (state == GameState.End)
                    break;

                DoPrintCommand(writer);
            }
        }

        WriteLine("-------------------", writer);
        WriteLine("", writer);
        WriteLine("", writer);
        WriteLine($"Distance traveled:   Mouse    Cat", writer);
        WriteLine($"                        {mouse.distanceTraveled,2}     {cat.distanceTraveled,2}", writer);
        WriteLine("", writer);

        if (caughtLocation.HasValue)
        {
            WriteLine($"Mouse caught at: {caughtLocation.Value}", writer);
        }
        else if (state == GameState.End)
        {
            if (cat.state == State.Winner)
                WriteLine("Cat wins!", writer);
            else if (mouse.state == State.Winner)
                WriteLine("Mouse wins!", writer);
        }
        else
        {
            WriteLine("Mouse evaded Cat", writer);
        }

        writer.Close();
    }

}

class Program
{
    static void Main(string[] args)
    {
        Game.InputFile = "3.ChaseData.txt";
        Game.OutFile = "PursuitLog.txt";

        string[] allLines = File.ReadAllLines(Game.InputFile);
        int size = int.Parse(allLines[0]);    // первая строка — размер поля
        string[] commands = allLines.Skip(1).ToArray();

        Game game = new Game(size);
        game.Run(commands);
    }
}
