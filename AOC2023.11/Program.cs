var input = await File.ReadAllLinesAsync("input.txt");

var parsedGalaxies = new List<Coordinate>();
var galaxiesXIndex = new Dictionary<long, List<int>>();
var galaxiesYIndex = new Dictionary<long, List<int>>();
for (var i = 0; i < input.Length; i++)
{
    galaxiesYIndex[i] = [];
    for (var j = 0; j < input[i].Length; j++)
    {
        if (i == 0)
            galaxiesXIndex[j] = [];

        switch (input[i][j])
        {
            case '#':
                parsedGalaxies.Add(new Coordinate
                {
                    X = j,
                    Y = i,
                });
                galaxiesXIndex[j].Add(parsedGalaxies.Count - 1);
                galaxiesYIndex[i].Add(parsedGalaxies.Count - 1);
                break;
        }
    }
}

var galaxies = parsedGalaxies.ToList();

Exercise1();

galaxies = parsedGalaxies.ToList(); // Reset to original state

Exercise2();

return;

void Exercise1()
{
    ExpandGalaxy(galaxiesXIndex, 2 - 1, true);
    ExpandGalaxy(galaxiesYIndex, 2 - 1, false);

    var totalSteps = CalculateStepsBetweenGalaxies();

    Console.WriteLine(totalSteps);
}

void Exercise2()
{
    ExpandGalaxy(galaxiesXIndex, 1000000 - 1, true);
    ExpandGalaxy(galaxiesYIndex, 1000000 - 1, false);

    var totalSteps = CalculateStepsBetweenGalaxies();

    Console.WriteLine(totalSteps);
}

long CalculateStepsBetweenGalaxies()
{
    var l = 0L;
    var galaxyArray = galaxies.ToArray();
    for (var i = 0; i < galaxyArray.Length; i++)
    {
        var coordinate = galaxyArray[i];
        for (var j = i + 1; j < galaxyArray.Length; j++)
        {
            var coordinate2 = galaxyArray[j];
            var dx = coordinate.X - coordinate2.X;
            var dy = coordinate.Y - coordinate2.Y;
            l += (dx >= 0 ? dx : -dx) + (dy >= 0 ? dy : -dy);
        }
    }

    return l;
}

void ExpandGalaxy(Dictionary<long, List<int>> galaxyDimensionIndexes, long expansionScale, bool inX)
{
    var expansionAmount = 0L;
    foreach (var (_, galaxyIndexes) in galaxyDimensionIndexes)
    {
        if (galaxyIndexes.Count == 0)
        {
            expansionAmount += expansionScale;
            continue;
        }

        foreach (var galaxyIndex in galaxyIndexes)
        {
            var c = new Coordinate
            {
                X = galaxies[galaxyIndex].X,
                Y = galaxies[galaxyIndex].Y,
            };
            if (inX)
                c.X += expansionAmount;
            else
                c.Y += expansionAmount;
            galaxies[galaxyIndex] = c;
        }
    }
}

public class Coordinate
{
    public long X { get; set; }
    public long Y { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Y} {X}";
    }
}