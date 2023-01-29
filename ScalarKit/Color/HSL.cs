// using System.Text.RegularExpressions;

// namespace ScalarKit.Color;

// public readonly record struct HSL : IScalar<HSL, string>
// {
//     private static readonly Regex VALID_CRITERIA = new Regex(@"^hsl\(\s*(-?\d+|-?\d*.\d+)\s*,\s*(-?\d+|-?\d*.\d+)%\s*,\s*(-?\d+|-?\d*.\d+)%\s*\)$");

//     public byte HueComponent { get; }

//     public byte SaturationComponent { get; }

//     public byte LightnessComponent { get; }

//     public string Value { get; }

//     private HSL(string hsl)
//     {
//         Value = hsl;

//         var components = hsl.Split('(', ')', ',');

//         HueComponent = byte.Parse(components[1]);

//         SaturationComponent = byte.Parse(components[2]);

//         LightnessComponent = byte.Parse(components[3]);
//     }

//     public static implicit operator HSL(string hsl)
//         => VALID_CRITERIA.IsMatch(hsl)
//             ? new HSL(hsl)
//             : throw new InvalidHSLException(hsl);
// }
