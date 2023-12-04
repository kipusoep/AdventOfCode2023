using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync("input.txt");

var cards = input.Select(line => new Card(line)).ToList();

Exercise1();

Exercise2();

return;

void Exercise1()
{
    Console.WriteLine("Total points: {0}.", cards.Sum(x => x.Points));
}

void Exercise2()
{
    var cardInstances = cards.Select(x => 1).ToArray();
    for (var i = 0; i < cards.Count; i++)
    {
        var card = new Card(input[i]);
        for (var j = i + 1; j <= i + card.Matches; j++)
        {
            cardInstances[j] += cardInstances[i];
        }
    }

    Console.WriteLine("Total cards: {0}.", cardInstances.Sum());
}

public class Card
{
    public Card(string line)
    {
        Id = int.Parse(Regex.Match(line, @"^Card\s+(\d+):").Groups[1].Value);
        var rest = line[(line.IndexOf(':') + 1)..];
        var winningNumbers = rest[..rest.IndexOf('|')];
        WinningNumbers = winningNumbers.Split(' ').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToList();
        var myNumbers = rest[(rest.IndexOf('|') + 1)..];
        MyNumbers = myNumbers.Split(' ').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Select(int.Parse).ToList();
    }

    public int Id { get; set; }
    public List<int> WinningNumbers { get; set; }
    public List<int> MyNumbers { get; set; }
    public int Matches => MyNumbers.Intersect(WinningNumbers).Count();
    public int Points => DoubleValueXTimes(Matches);

    /// <inheritdoc />
    public override string ToString()
    {
        return Id.ToString();
    }

    private static int DoubleValueXTimes(int times)
    {
        if (times == 0)
        {
            return 0;
        }

        var total = 1;
        for (var i = 1; i < times; i++)
        {
            total *= 2;
        }
        return total;
    }
}