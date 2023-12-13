var input = await File.ReadAllLinesAsync("input.txt");

var blocks = new List<List<string>>();
var curBlock = new List<string>();
foreach (var line in input)
{
    if (string.IsNullOrEmpty(line))
    {
        blocks.Add(curBlock);
        curBlock = new();
    }
    else
    {
        curBlock.Add(line);
    }
}
blocks.Add(curBlock);

var resultPerBlock = new List<(int column, int row)>();

Exercise1();

Exercise2();

return;

void Exercise1()
{
    var columns = new List<int>();
    var rows = new List<int>();
    foreach (var block in blocks)
    {
        var (column, row) = FindReflections(block);
        columns.Add(column);
        rows.Add(row);
        resultPerBlock.Add((column, row));
    }

    Console.WriteLine("Summary: {0}.", (rows.Sum() * 100) + columns.Sum());
}

void Exercise2()
{
    var columns = new List<int>();
    var rows = new List<int>();
    foreach (var block in blocks)
    {
        //        var str = @"
        //.#.#..#.#.#
        //.####..###.
        //#...##.#...
        //#...##.#...
        //.####..###.
        //.#.#..#.#.#
        //##.##...#.#
        //.#.##..##.#
        //.##...##.#.
        //.....#.##.#
        //.....#.##.#
        //..#...##.#.
        //.#.##..##.#
        //##.##...#.#
        //.#.#..#.#.#";

        //        if (!block.SequenceEqual(str.Split(Environment.NewLine).Skip(1).ToList()))
        //        {
        //            continue;
        //        }

        var matchFound = false;
        var allCharacters = new List<char>();
        foreach (var line in block)
        {
            allCharacters.AddRange(line.ToCharArray());
            allCharacters.Add('\n');
        }

        var originalAllCharacters = allCharacters.ToList();
        for (var i = 0; i < allCharacters.Count; i++)
        {
            //if (i != 97)
            //{
            //    continue;
            //}
            if (allCharacters[i] == '.' || allCharacters[i] == '#')
            {
                allCharacters[i] = allCharacters[i] == '.' ? '#' : '.';
                var (column, row) = FindReflections(
                    new string(allCharacters.ToArray()).Split('\n').Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
                    resultPerBlock[blocks.IndexOf(block)]);
                if (column > 0 || row > 0)
                {
                    matchFound = true;
                    columns.Add(column);
                    rows.Add(row);
                    break;
                }
            }

            allCharacters = originalAllCharacters.ToList();
        }

        if (!matchFound)
        {
            Console.WriteLine("Match not found for block:");
            block.ForEach(Console.WriteLine);
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    Console.WriteLine("Summary: {0}.", (rows.Sum() * 100) + columns.Sum());
}

(int columns, int rows) FindReflections(List<string> block, (int column, int row)? resultToIgnore = null)
{
    // Check columns
    var lineLength = block.First().Length;
    var column = 1;
    var distance = 1;
    while (column < lineLength)
    {
        var isReflection = true;
        for (var i = 0; i < block.Count; i++) // Loop through lines
        {
            var line = block[i];
            var charLeft = line[column - distance];
            var charRight = line[column + (distance - 1)];
            if (charLeft != charRight)
            {
                isReflection = false;
                column++;
                distance = 1;
                break;
            }
        }

        if (isReflection)
        {
            distance++;
        }

        if (column - distance < 0 || column + (distance - 1) >= lineLength)
        {
            if (isReflection)
            {
                if (!resultToIgnore.HasValue || resultToIgnore.Value.column != column)
                {
                    return (column, 0);
                }

                column++;
                distance = 1;
            }
        }
    }

    // Check rows
    var colLength = block.Count;
    var row = 1;
    distance = 1;
    while (true)
    {
        var isReflection = true;
        for (var i = 0; i < block.First().Length; i++) // Loop through columns
        {
            var cols = block.Select(x => x.ElementAt(i)).ToList();
            var charUp = cols[row - distance];
            var charDown = cols[row + (distance - 1)];
            if (charUp != charDown)
            {
                isReflection = false;
                row++;
                distance = 1;
                break;
            }
        }

        if (isReflection)
        {
            distance++;
        }

        if (row - distance < 0 || row + (distance - 1) >= colLength)
        {
            if (isReflection)
            {
                if (!resultToIgnore.HasValue || resultToIgnore.Value.row != row)
                {
                    return (0, row);
                }

                row++;
                distance = 1;
            }
        }

        if (row >= colLength)
        {
            break;
        }
    }

    return (0, 0);
}