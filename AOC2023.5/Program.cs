using System.Diagnostics;

var input = await File.ReadAllLinesAsync("input.txt");

var seedMappings = new SeedMappings
{
    Seeds = input[0][7..].Split(' ').Select(long.Parse).ToList(),
};

var mappingBlocks = input.Skip(2).ToList();
while (true)
{
    var mappingBlock = mappingBlocks.TakeWhile(x => !string.IsNullOrEmpty(x)).ToList();
    if (mappingBlock.Count == 0)
    {
        break;
    }

    mappingBlocks = mappingBlocks.Skip(mappingBlock.Count + 1).ToList();
    var mapping = new SeedMappings.Mapping
    {
        MappingType = Enum.Parse<MappingType>(mappingBlock.First().Replace(" map:", string.Empty).Replace("-", string.Empty), true),
    };
    foreach (var mappingLine in mappingBlock.Skip(1))
    {
        var splittedMappingLine = mappingLine.Split(' ').ToList();
        var entry = new SeedMappings.Mapping.Entry(long.Parse(splittedMappingLine[1]), long.Parse(splittedMappingLine[0]), long.Parse(splittedMappingLine[2]));
        mapping.Entries.Add(entry);
    }

    seedMappings.Mappings.Add(mapping);
}

//Exercise1();

Exercise2();

return;

void Exercise1()
{
    var lowestLocationNumber = long.MaxValue;
    double i = 0;
    var percent = 0;
    var sw = Stopwatch.StartNew();
    foreach (var seed in seedMappings.Seeds)
    {
        var number = seed;
        //Console.WriteLine("Seed: {0}.", seed);
        foreach (var mappingType in Enum.GetNames<MappingType>())
        {
            var entries = seedMappings.Mappings.Single(x => x.MappingType.ToString() == mappingType).Entries;
            foreach (var entry in entries)
            {
                var start = entry.SourceStart;
                var end = entry.SourceStart + (entry.Length - 1);
                if (number >= start && number <= end)
                {
                    var diff = number - start;
                    number = entry.DestinationStart + diff;
                    break;
                }
            }

            //Console.WriteLine("Mapping type: {0}. New number: {1}.", mappingType, number);
        }

        lowestLocationNumber = Math.Min(lowestLocationNumber, number);

        var newPercent = (int)Math.Round(++i / seedMappings.Seeds.Count * 100);
        if (percent != newPercent)
        {
            percent = newPercent;
            Console.WriteLine("Percent done: {0}. Total will take approx: {1} minutes.", percent, $"{sw.Elapsed.TotalMinutes * 100:F1}");
            sw.Restart();

        }
    }

    Console.WriteLine("The lowest location number is: {0}.", lowestLocationNumber);
}

void Exercise2()
{
    var newSeeds = new List<long>();
    var seedChunks = seedMappings.Seeds.Chunk(2).ToList();
    foreach (var seedChunk in seedChunks)
    {
        var startSeed = seedChunk[0];
        var length = seedChunk[1];
        for (var i = 0L; i < length; i++)
        {
            newSeeds.Add(startSeed + i);
        }
    }

    seedMappings.Seeds = newSeeds;

    Exercise1();
}

public class SeedMappings
{
    public List<long> Seeds { get; set; }
    public List<Mapping> Mappings { get; set; } = new();

    public class Mapping
    {
        public MappingType MappingType { get; set; }
        public List<Entry> Entries { get; set; } = new();

        /// <inheritdoc />
        public override string ToString()
        {
            return MappingType.ToString();
        }

        public class Entry(long sourceStart, long destinationStart, long length)
        {
            public long SourceStart { get; set; } = sourceStart;
            public long DestinationStart { get; set; } = destinationStart;
            public long Length { get; set; } = length;

            /// <inheritdoc />
            public override string ToString()
            {
                return $"{DestinationStart} {SourceStart} {Length}";
            }
        }
    }
}

public enum MappingType
{
    SeedToSoil,
    SoilToFertilizer,
    FertilizerToWater,
    WaterToLight,
    LightToTemperature,
    TemperatureToHumidity,
    HumidityToLocation,
}