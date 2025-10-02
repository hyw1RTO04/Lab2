
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
