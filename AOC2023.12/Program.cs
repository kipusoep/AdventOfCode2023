using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync("input.txt");

var springsRows = input
    .Select(line => line.Split(' '))
    .Select(splitted => new SpringsRow
    {
        Springs = splitted[0],
        GroupSizes = splitted[1].Split(',').Select(int.Parse).ToList(),
    })
    .ToList();

Exercise1();

Exercise2();

return;

void Exercise1()
{
    CalculateValidCombinations();

    Console.WriteLine("Total valid mutations: {0}.", springsRows.Sum(x => x.ValidCombinations));
}

void Exercise2()
{
    foreach (var springsRow in springsRows)
    {
        springsRow.Springs = string.Join('?', Enumerable.Repeat(springsRow.Springs, 5));
        springsRow.GroupSizes = Enumerable.Repeat(springsRow.GroupSizes, 5).SelectMany(g => g).ToList();
        springsRow.ResetTemporaryData();
    }

    CalculateValidCombinations();

    Console.WriteLine("Total valid mutations: {0}.", springsRows.Sum(x => x.ValidCombinations));
}

void CalculateValidCombinations()
{
    var springRowCounter = 0;
    springsRows.AsParallel().ForAll(springRow =>
    {
        Console.WriteLine("Spring row {0} of {1}.", ++springRowCounter, springsRows.Count);
        springRow.Mutations = GenerateMutations(springRow.Springs, '?', '.', '#');

        springRow.Mutations.AsParallel().ForAll(mutation =>
        {
            var singleDots = Regex.Replace(mutation, @"(\.+)", ".");
            var groups = singleDots.Split('.').Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (groups.Count != springRow.GroupSizes.Count)
            {
                return;
            }

            var isValid = true;
            for (var i = 0; i < groups.Count; i++)
            {
                var group = groups[i];
                var size = springRow.GroupSizes.ElementAt(i);
                if (group.Length != size)
                {
                    isValid = false;
                }
            }

            if (isValid)
            {
                springRow.ValidCombinations++;
            }
        });
    });
}

List<string> GenerateMutations(string input, char marker, params char[] variants)
{
    List<string> result = new();
    var markerIndex = input.IndexOf(marker);
    if (markerIndex >= 0)
    {
        foreach (var variant in variants)
        {
            var newVariant = $"{input[..markerIndex]}{variant}{input[(markerIndex + 1)..]}";
            result.AddRange(GenerateMutations(newVariant, marker, variants));
        }
    }
    else
    {
        result.Add(input);
    }

    return result;
}

public class SpringsRow
{
    public string Springs { get; set; }
    public List<int> GroupSizes { get; set; }
    public List<string> Mutations { get; set; }
    public int ValidCombinations { get; set; }

    public void ResetTemporaryData()
    {
        Mutations = new();
        ValidCombinations = 0;
    }
}