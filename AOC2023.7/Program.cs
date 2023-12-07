var input = await File.ReadAllLinesAsync("input.txt");

var hands = input.Select(line => new Hand(line)).ToList();

var orderedByRank = hands.OrderBy(x => x, new HandRankComparer()).ToList();

for (var rank = 1; rank <= orderedByRank.Count; rank++)
{
    orderedByRank[rank - 1].Rank = rank;
}

Console.WriteLine("Total winnings: {0}.", orderedByRank.Sum(x => x.Winnings));

return;

public class Hand
{
    public List<char> Cards { get; set; }
    public int Bid { get; set; }
    public HandType HandType
    {
        get
        {
            var groupedCards = Cards.GroupBy(x => x).OrderByDescending(x => x.Count()).ToList();
            if (Cards.Any(x => x == 'J') && groupedCards.Count > 1)
            {
                // Remove this if statement for exercise 1
                var newCards = string.Join("", Cards).Replace('J', groupedCards.First(x => x.Key != 'J').Key);
                groupedCards = newCards.Select(x => x).GroupBy(x => x).OrderByDescending(x => x.Count()).ToList();
            }
            return groupedCards.Count switch
            {
                1 => HandType.FiveOfAKind,
                2 when groupedCards.Any(x => x.Count() == 4) => HandType.FourOfAKind,
                2 when groupedCards.Any(x => x.Count() == 3) && groupedCards.Any(x => x.Count() == 2) => HandType.FullHouse,
                3 when groupedCards.Any(x => x.Count() == 3) => HandType.ThreeOfAKind,
                3 when groupedCards[0].Count() == 2 && groupedCards[1].Count() == 2 => HandType.TwoPair,
                _ => groupedCards.Any(x => x.Count() == 2) ? HandType.OnePair : HandType.HighCard
            };
        }
    }

    public int Rank { get; set; }
    public int Winnings => Rank * Bid;

    public Hand(string line)
    {
        var splitted = line.Split(' ');
        Cards = splitted[0].Select(x => x).ToList();
        Bid = int.Parse(splitted[1]);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{string.Join("", Cards)} {HandType} {Bid}";
    }
}

public enum HandType
{
    HighCard = 1,
    OnePair = 2,
    TwoPair = 3,
    ThreeOfAKind = 4,
    FullHouse = 5,
    FourOfAKind = 6,
    FiveOfAKind = 7,
}

public class HandRankComparer : IComparer<Hand>
{
    /// <inheritdoc />
    public int Compare(Hand x, Hand y)
    {
        var handTypeCompare = x.HandType.CompareTo(y.HandType);
        if (handTypeCompare != 0)
        {
            return handTypeCompare;
        }

        for (var i = 0; i < x.Cards.Count; i++)
        {
            var cardX = x.Cards[i];
            var cardY = y.Cards[i];

            var cardXValue = (int)cardX;
            if (_cardValues.TryGetValue(cardX, out var value))
            {
                cardXValue = value;
            }

            var cardYValue = (int)cardY;
            if (_cardValues.TryGetValue(cardY, out value))
            {
                cardYValue = value;
            }

            var cardCompare = cardXValue.CompareTo(cardYValue);
            if (cardCompare != 0)
            {
                return cardCompare;
            }
        }

        return 0;
    }

    private readonly Dictionary<char, int> _cardValues = new()
    {
        { 'A', 62 },
        { 'K', 61 },
        { 'Q', 60 },
        { 'J', 1 }, // Change to 59 for exercise 1
        { 'T', 58 },
    };
}