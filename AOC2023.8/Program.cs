using System.Collections.Concurrent;
using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync("input.txt");

var steps = input[0].Select(x => x).ToList();
var nodes = input.Skip(2).Select(x => Regex.Match(x, @"(\w+) = \((\w+), (\w+)\)")).Select(x => new
Node
{
    Start = x.Groups[1].Value,
    Left = x.Groups[2].Value,
    Right = x.Groups[3].Value,
}).ToList();

Exercise1();

Exercise2();

return;

void Exercise1()
{
    var stepsCounter = 0;
    var stepsIndex = 0;
    var maxSteps = steps.Count;
    var currentNode = nodes.Single(x => x.Start == "AAA");
    while (currentNode.Start != "ZZZ")
    {
        stepsCounter++;
        var leftOrRight = steps.ElementAt(stepsIndex);
        currentNode = leftOrRight == 'L' ? nodes.Single(x => x.Start == currentNode.Left) : nodes.Single(x => x.Start == currentNode.Right);
        stepsIndex++;
        if (stepsIndex == maxSteps)
        {
            stepsIndex = 0;
        }
    }

    Console.WriteLine("Number of steps needed for exercise 1: {0}.", stepsCounter);
}

void Exercise2()
{
    var maxSteps = steps.Count;
    var currentNodes = nodes.Where(x => x.Start.EndsWith('A')).AsParallel();
    var stepsPerNode = new ConcurrentBag<long>();
    currentNodes.ForAll(node =>
    {
        var stepsCounter = 0;
        var stepsIndex = 0;
        while (true)
        {
            stepsCounter++;
            var leftOrRight = steps.ElementAt(stepsIndex);
            node = leftOrRight == 'L' ? nodes.Single(x => x.Start == node.Left) : nodes.Single(x => x.Start == node.Right);
            if (node.Start.EndsWith('Z'))
            {
                stepsPerNode.Add(stepsCounter);
                break;
            }
            stepsIndex++;
            if (stepsIndex == maxSteps)
            {
                stepsIndex = 0;
            }
        }
    });

    Console.WriteLine("Number of steps needed for exercise 2: {0}.", LCMRange(stepsPerNode));
}

static long LCMRange(IEnumerable<long> numbers)
{
    return numbers.Aggregate(LCM);
}

static long LCM(long a, long b)
{
    return Math.Abs(a * b) / GCD(a, b);
}

static long GCD(long a, long b)
{
    while (true)
    {
        if (b == 0) return a;
        var a1 = a;
        a = b;
        b = a1 % b;
    }
}

public class Node
{
    public string Start { get; set; }
    public string Left { get; set; }
    public string Right { get; set; }
}