var input = await File.ReadAllLinesAsync("input.txt");

var sequences = input.Select(x => new Sequence(x)).ToList();

Exercise1();

Exercise2();

return;

void Exercise1()
{
    var sum = sequences.Sum(x => x.AnswerExercise1);
    Console.WriteLine("The sum of all new answers is: {0}.", sum);
}

void Exercise2()
{
    var sum = sequences.Sum(x => x.AnswerExercise2);
    Console.WriteLine("The sum of all new answers is: {0}.", sum);
}

public class Sequence(string line)
{
    public List<int> MainSequence { get; set; } = line.Split(' ').Select(int.Parse).ToList();
    public List<List<int>> SubSequences
    {
        get
        {
            var subSequences = new List<List<int>>();
            var currentSequence = MainSequence;
            while (true)
            {
                var subSequence = new List<int>();
                for (var i = 0; i < currentSequence.Count - 1; i++)
                {
                    subSequence.Add(currentSequence[i + 1] - currentSequence[i]);
                }
                subSequences.Add(subSequence);
                currentSequence = subSequence;
                if (currentSequence.All(x => x == 0))
                {
                    break;
                }
            }

            return subSequences;
        }
    }
    public int AnswerExercise1
    {
        get
        {
            if (!_answer1.HasValue)
            {
                var copyOfSubSequences = SubSequences.ToList();
                for (var i = copyOfSubSequences.Count - 1; i >= 0; i--)
                {
                    var subSequence = copyOfSubSequences[i];
                    var nextSequence = i == 0 ? MainSequence : copyOfSubSequences[i - 1];
                    nextSequence.Add(nextSequence.Last() + subSequence.Last());
                }

                _answer1 = MainSequence.Last();
            }

            return _answer1.Value;
        }
    }
    public int AnswerExercise2
    {
        get
        {
            if (!_answer2.HasValue)
            {
                var copyOfSubSequences = SubSequences.ToList();
                for (var i = copyOfSubSequences.Count - 1; i >= 0; i--)
                {
                    var subSequence = copyOfSubSequences[i];
                    var nextSequence = i == 0 ? MainSequence : copyOfSubSequences[i - 1];
                    nextSequence.Insert(0, nextSequence.First() - subSequence.First());
                }

                _answer2 = MainSequence.First();
            }

            return _answer2.Value;
        }
    }

    private int? _answer1;
    private int? _answer2;

    /// <inheritdoc />
    public override string ToString()
    {
        return string.Join(' ', MainSequence);
    }
}