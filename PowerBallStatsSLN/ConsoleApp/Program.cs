
using System.IO;


namespace PowerBallStatsCalculator;

internal class Program
{

	public static readonly string INPUT_FILE_HEADER			= "Date(YYYY-MM-DD),N0,N1,N2,N3,N4,P0,Power Play Mult";
	public static readonly string PARSE_DATE_FORMAT_STRING	= "yyyy-mm-dd";


	private static void Main(string[] args)
	{
		if (args.Length == 0)
		{
			Console.WriteLine("No file entered.");
			Console.WriteLine("Exiting application.");
			return;
		}

		if (!File.Exists(args[0]))
		{
			Console.WriteLine($"Entered file does not exist: {args[0]}");
			Console.WriteLine("Exiting application.");
			return;
		}

		var powerballDrawings = new List<PowerBallDrawing>();

		try
		{
			using (var fileStreamReader = File.OpenText(args[0]))
			{
				var header = fileStreamReader.ReadLine();
				if (!INPUT_FILE_HEADER.Equals(header))
				{
					Console.WriteLine("Input file header does not match.");
					Console.WriteLine("Exiting application.");
					return;
				}

				var inputsPerDrawing = INPUT_FILE_HEADER.Split(',').Length;

				while (!fileStreamReader.EndOfStream)
				{
					var line = fileStreamReader.ReadLine();
					var split = line.Split(',');
					if (split.Length != inputsPerDrawing) {
						Console.WriteLine($"Row error, skipping: {line}");
						Console.WriteLine();
						continue;
					}

					bool keepGoing = DateOnly.TryParseExact(split[0], PARSE_DATE_FORMAT_STRING, out DateOnly drawDate);
					if (!keepGoing)
					{
						Console.WriteLine($"Failed to parse date from: {line}");
						Console.WriteLine("Skipping drawing.");
						Console.WriteLine();
						continue;
					}

					keepGoing = int.TryParse(split[1], out int n0);
					if (!keepGoing)
					{
						Console.WriteLine($"Failed to parse winning number 0 from: {line}");
						Console.WriteLine("Skipping drawing.");
						continue;
					}

					keepGoing = int.TryParse(split[2], out int n1);
					if (!keepGoing)
					{
						Console.WriteLine($"Failed to parse winning number 1 from: {line}");
						Console.WriteLine("Skipping drawing.");
						continue;
					}

					keepGoing = int.TryParse(split[3], out int n2);
					if (!keepGoing)
					{
						Console.WriteLine($"Failed to parse winning number 2 from: {line}");
						Console.WriteLine("Skipping drawing.");
						continue;
					}

					keepGoing = int.TryParse(split[4], out int n3);
					if (!keepGoing)
					{
						Console.WriteLine($"Failed to parse winning number 3 from: {line}");
						Console.WriteLine("Skipping drawing.");
						continue;
					}

					keepGoing = int.TryParse(split[5], out int n4);
					if (!keepGoing)
					{
						Console.WriteLine($"Failed to parse winning number 4 from: {line}");
						Console.WriteLine("Skipping drawing.");
						continue;
					}

					keepGoing = int.TryParse(split[6], out int powerballNumber);
					if (!keepGoing)
					{
						Console.WriteLine($"Failed to parse power ball number from: {line}");
						Console.WriteLine("Skipping drawing.");
						continue;
					}

					keepGoing = int.TryParse(split[7], out int powerplayMult);
					if (!keepGoing)
					{
						Console.WriteLine($"Failed to parse powerplay multiplier from: {line}");
						Console.WriteLine("Skipping drawing.");
						continue;
					}

					try
					{
						var drawing = new PowerBallDrawing(drawDate, n0, n1, n2, n3, n4, powerballNumber, powerplayMult);
						powerballDrawings.Add(drawing);
					}
					catch (Exception ex) 
					{
						Console.WriteLine($"Failed to parse line: {line}");
						Console.WriteLine(ex.Message);
						Console.WriteLine();
						continue;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("Problem reading input file.");
			Console.WriteLine(ex.Message);
			return;
		}
	}
}
