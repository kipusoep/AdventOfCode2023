var input = await File.ReadAllLinesAsync("input.txt");

Exercise1();

Exercise2();

return;

void Exercise1()
{
    var partNumbers = new List<int>();
    var currentNumber = string.Empty;
    for (var i = 0; i < input.Length; i++)
    {
        var line = input[i];
        for (var j = 0; j < line.Length; j++)
        {
            var character = line[j];
            if (char.IsDigit(character))
            {
                currentNumber = $"{currentNumber}{character}";
            }

            var isEndOfLine = j + 1 == line.Length;
            if (!string.IsNullOrEmpty(currentNumber) && (!char.IsDigit(character) || isEndOfLine))
            {
                if (isEndOfLine)
                {
                    j++;
                }

                // Number is complete
                var number = int.Parse(currentNumber);
                var numberLength = currentNumber.Length;

                var lineBefore = i > 0 ? input[i - 1] : string.Empty;
                var lineAfter = i + 1 < input.Length ? input[i + 1] : string.Empty;
                var leftSideReal = j - numberLength - 1;
                var leftSideSafe = Math.Max(leftSideReal, 0);
                var rightSideReal = j + 1;
                var rightSideSafe = Math.Min(rightSideReal, line.Length);

                lineBefore = !string.IsNullOrEmpty(lineBefore) ? lineBefore[leftSideSafe..rightSideSafe] : string.Empty;
                lineAfter = !string.IsNullOrEmpty(lineAfter) ? lineAfter[leftSideSafe..rightSideSafe] : string.Empty;
                var charBefore = leftSideReal >= 0 ? line.Substring(leftSideReal, 1) : string.Empty;
                var charAfter = rightSideReal - 1 < line.Length ? line.Substring(rightSideReal - 1, 1) : string.Empty;

                var allAdjacents = lineBefore + lineAfter + charBefore + charAfter;
                if (allAdjacents.Any(x => x != '.' && !char.IsDigit(x)))
                {
                    Console.WriteLine("Part number: {0}.", number);
                    partNumbers.Add(number);
                }
                else
                {
                    //Console.WriteLine("Normal number: {0}.", number);
                }

                currentNumber = string.Empty;
            }
        }
    }

    Console.WriteLine("The sum of all part numbers is: {0}.", partNumbers.Sum());
    Console.WriteLine($"[{string.Join(", ", partNumbers)}]");
}

void Exercise2()
{
}