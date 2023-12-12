var input = await File.ReadAllLinesAsync("input.txt");

var coordinates = new List<Coordinate>();
for (var i = 0; i < input.Length; i++)
{
    var line = input[i];
    for (var j = 0; j < line.Length; j++)
    {
        coordinates.Add(new Coordinate(line[j], j, i));
    }
}

Exercise1();

Exercise2();

return;

void Exercise1()
{
    var start = coordinates.Single(x => x.Character == 'S');
    var up = start.Y > 0 ? coordinates.Single(x => x.Y == start.Y - 1 && x.X == start.X) : null;
    var down = start.Y < coordinates.Max(x => x.Y) ? coordinates.Single(x => x.Y == start.Y + 1 && x.X == start.X) : null;
    var left = start.X > 0 ? coordinates.Single(x => x.Y == start.Y && x.X == start.X - 1) : null;
    var right = start.X < coordinates.Max(x => x.X) ? coordinates.Single(x => x.Y == start.Y && x.X == start.X + 1) : null;

    var prev = start;
    Coordinate next;
    if (up != null && up.ConnectsSouth)
    {
        next = up;
    }
    else if (right != null && right.ConnectsWest)
    {
        next = right;
    }
    else if (down != null && down.ConnectsNorth)
    {
        next = down;
    }
    else
    {
        next = left!;
    }

    var steps = 2;
    while (next != start)
    {
        steps++;
        var newPrev = next;
        var nextNorth = next.ConnectsNorth ? TryGetCoordinate(next.Y - 1, next.X) : null;
        var nextEast = next.ConnectsEast ? TryGetCoordinate(next.Y, next.X + 1) : null;
        var nextSouth = next.ConnectsSouth ? TryGetCoordinate(next.Y + 1, next.X) : null;
        var nextWest = next.ConnectsWest ? TryGetCoordinate(next.Y, next.X - 1) : null;

        if (nextNorth != null && nextNorth != prev)
        {
            next = nextNorth;
        }
        else if (nextEast != null && nextEast != prev)
        {
            next = nextEast;
        }
        else if (nextSouth != null && nextSouth != prev)
        {
            next = nextSouth;
        }
        else if (nextWest != null && nextWest != prev)
        {
            next = nextWest;
        }

        prev = newPrev;
    }

    Console.WriteLine(steps / 2);
}

void Exercise2()
{
}

Coordinate? TryGetCoordinate(long y, long x)
{
    if (y >= 0 && x >= 0 && y < coordinates.Max(c => c.Y) && x < coordinates.Max(c => c.X))
    {
        return coordinates.Single(c => c.Y == y && c.X == x);
    }

    return null;
}

public class Coordinate
{
    public Coordinate(char character, long x, long y)
    {
        Character = character;
        X = x;
        Y = y;
    }

    public char Character { get; set; }
    public long X { get; set; }
    public long Y { get; set; }

    public bool ConnectsNorth => Character is '|' or 'L' or 'J';
    public bool ConnectsEast => Character is '-' or 'L' or 'F';
    public bool ConnectsSouth => Character is '|' or '7' or 'F';
    public bool ConnectsWest => Character is '-' or 'J' or '7';

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Y} {X} {Character}";
    }
}