
using System.IO;


namespace PowerBallStatsCalculator;

internal class Program
{

	public static readonly string INPUT_FILE_HEADER = "Date(YYYY-MM-DD),N0,N1,N2,N3,N4,P0,Power Play Mult";


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
