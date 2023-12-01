using System.Text;
using System.Text.RegularExpressions;

var textToNumbers = new Dictionary<string, int>
{
    {"one", 1},
    {"two", 2},
    {"three", 3},
    {"four", 4},
    {"five", 5},
    {"six", 6},
    {"seven", 7},
    {"eight", 8},
    {"nine", 9},
};

var input = await File.ReadAllLinesAsync("input.txt");

Exercise1();

Exercise2();

return;

void Exercise1()
{
    CalculateSum(input, 1);
}

void Exercise2()
{
    var newLines = new List<string>();
    foreach (var line in input)
    {
        var parsedNumbers = new StringBuilder();
        for (var i = 0; i < line.Length; i++)
        {
            foreach (var (text, number) in textToNumbers)
            {
                var singleChar = line.Substring(i, 1);
                if (line.Length >= i + text.Length && line.Substring(i, text.Length) == text)
                {
                    parsedNumbers.Append(number);
                }
                else if (Regex.IsMatch(singleChar, @"\d"))
                {
                    parsedNumbers.Append(int.Parse(singleChar));
                }
            }
        }
        newLines.Add(parsedNumbers.ToString());
    }

    CalculateSum(newLines, 2);
}

void CalculateSum(IEnumerable<string> input, int exercise)
{
    var numbers = input.Select(line => Regex.Matches(line, @"\d")).Select(digits => int.Parse($"{digits.First().Value}{digits.Last().Value}")).ToList();
    var sum = numbers.Sum();

    Console.WriteLine("Exercise{0} | The sum is: {1}.", exercise, sum);
}