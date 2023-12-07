using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync("input.txt");

Exercise1();

Exercise2();

return;

void Exercise1()
{
    var times = Regex.Matches(input[0][(input[0].IndexOf(":") + 1)..], @"\d+").Select(x => long.Parse(x.Groups[0].Value)).ToList();
    var distances = Regex.Matches(input[1][(input[0].IndexOf(":") + 1)..], @"\d+").Select(x => long.Parse(x.Groups[0].Value)).ToList();

    CalculateWinningOptions(times, distances, 1);
}

void Exercise2()
{
    var time = Regex.Matches(input[0][(input[0].IndexOf(":") + 1)..].Replace(" ", string.Empty), @"\d+").Select(x => long.Parse(x.Groups[0].Value)).ToList();
    var distance = Regex.Matches(input[1][(input[0].IndexOf(":") + 1)..].Replace(" ", string.Empty), @"\d+").Select(x => long.Parse(x.Groups[0].Value)).ToList();

    CalculateWinningOptions(time, distance, 2);
}

void CalculateWinningOptions(IReadOnlyList<long> times, IReadOnlyList<long> distances, int exercise)
{
    var totalWinningOptions = 0;
    for (var i = 0; i < times.Count; i++)
    {
        var time = times[i];
        var distance = distances[i];
        var winningOptions = 0;
        for (var ms = 1; ms < time; ms++)
        {
            var travelTime = time - ms;
            var distanceTravelled = travelTime * ms;
            if (distanceTravelled > distance)
            {
                winningOptions++;
            }
        }

        if (totalWinningOptions == 0)
        {
            totalWinningOptions = winningOptions;
        }
        else
        {
            totalWinningOptions *= winningOptions;
        }
    }

    Console.WriteLine("Total winning options for exercise {0}: {1}.", exercise, totalWinningOptions);
}