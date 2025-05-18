using System;
using System.Collections.Generic;
using System.Linq;

namespace ComboGen;

[Flags]
public enum TravelComponentType
{
    None = 0,
    Flight = 1 << 0,    // 1
    Hotel = 1 << 1,     // 2
    Car = 1 << 2,       // 4
    Rail = 1 << 3,      // 8
    Transfer = 1 << 4,  // 16
    Activity = 1 << 5   // 32
}

public class TravelComponentCombination
{
    public int Id { get; set; } // Bitmask value
    public TravelComponentType Components { get; set; }
    public string Name { get; set; } = string.Empty;
}

public static class TravelComponentComboGenerator
{
    public static List<TravelComponentCombination> GetAllCombinations()
    {
        var values = Enum.GetValues(typeof(TravelComponentType))
                         .Cast<TravelComponentType>()
                         .Where(v => v != TravelComponentType.None)
                         .ToArray();

        var combos = new List<TravelComponentCombination>();

        int total = 1 << values.Length; // 2^N combinations

        for (int i = 1; i < total; i++) // skip 0 = None
        {
            var combo = TravelComponentType.None;
            var parts = new List<string>();

            for (int bit = 0; bit < values.Length; bit++)
            {
                if ((i & (1 << bit)) != 0)
                {
                    combo |= values[bit];
                    parts.Add(values[bit].ToString());
                }
            }

            combos.Add(new TravelComponentCombination
            {
                Id = (int)combo,
                Components = combo,
                Name = string.Join("_", parts)
            });
        }

        return combos.OrderBy(c => c.Id).ToList();
    }
}

public class Program
{
    public static void Main()
    {
        var combos = TravelComponentComboGenerator.GetAllCombinations();

        foreach (var combo in combos)
        {
            Console.WriteLine($"{combo.Name} = 1 << {combo.Id.ToString()},");
        }
    }
}
