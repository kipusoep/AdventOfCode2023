using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync("input.txt");

var games = input.Select(line => new Game(line)).ToList();

Exercise1();

Exercise2();

return;

void Exercise1()
{
    var gameSetup = new
    {
        Reds = 12,
        Greens = 13,
        Blues = 14,
    };

    var validGames = games.Where(x => x.Rounds.All(y => y.Reds <= gameSetup.Reds && y.Greens <= gameSetup.Greens && y.Blues <= gameSetup.Blues)).ToList();
    var sumOfValidIds = validGames.Sum(x => x.Id);

    Console.WriteLine("Sum of valid game id's: {0}.", sumOfValidIds);
}

void Exercise2()
{
    foreach (var game in games)
    {
        var maxRed = game.Rounds.Max(x => x.Reds);
        var maxGreen = game.Rounds.Max(x => x.Greens);
        var maxBlue = game.Rounds.Max(x => x.Blues);
        game.Power = maxRed * maxGreen * maxBlue;
    }

    var powerSum = games.Sum(x => x.Power);

    Console.WriteLine("Sum of powers: {0}.", powerSum);
}

public class Game
{
    public Game(string line)
    {
        Id = int.Parse(Regex.Match(line, @"^Game (\d+):").Groups[1].Value);
        var rest = line[(line.IndexOf(':') + 1)..];
        var rounds = rest.Split(';');
        Rounds = rounds.Select(x => new Round(x)).ToList();
    }

    public int Id { get; set; }
    public List<Round> Rounds { get; set; }
    public int Power { get; set; }

    public class Round
    {
        public Round(string line)
        {
            var colors = line.Split(',').Select(x => x.Trim()).ToList();
            foreach (var color in colors)
            {
                var match = Regex.Match(color, @"(\d+) (\w+)");
                var cubes = int.Parse(match.Groups[1].Value);
                switch (match.Groups[2].Value)
                {
                    case "red":
                        Reds = cubes;
                        break;
                    case "green":
                        Greens = cubes;
                        break;
                    case "blue":
                        Blues = cubes;
                        break;
                }
            }
        }

        public int Reds { get; set; }
        public int Greens { get; set; }
        public int Blues { get; set; }
    }
}