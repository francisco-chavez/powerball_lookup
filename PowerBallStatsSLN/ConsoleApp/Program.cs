
using System.IO;


namespace PowerBallStatsCalculator;

internal class Program
{

	public static readonly string INPUT_FILE_HEADER			= "Date(YYYY-MM-DD),N0,N1,N2,N3,N4,P0,Power Play Mult";


	private static void Main(string[] args)
	{

		var powerballDrawings = new List<PowerBallDrawing>();


		///
		/// Start Parsing Input
		/// 
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

					var splitDate = split[0].Split('-');
					if (splitDate.Length != 3)
					{
						Console.WriteLine($"Failed to parse date from: {line}");
						Console.WriteLine("Skipping drawing.");
						Console.WriteLine();
						continue;
					}
					var keepGoing = int.TryParse(splitDate[0], out int year);
					if (!keepGoing)
					{
						Console.WriteLine($"Failed to parse date from: {line}");
						Console.WriteLine("Skipping drawing.");
						Console.WriteLine();
						continue;
					}
					keepGoing = int.TryParse(splitDate[1], out int month);
					if (!keepGoing)
					{
						Console.WriteLine($"Failed to parse date from: {line}");
						Console.WriteLine("Skipping drawing.");
						Console.WriteLine();
						continue;
					}
					keepGoing = int.TryParse(splitDate[2], out int dayOfMonth);
					if (!keepGoing)
					{
						Console.WriteLine($"Failed to parse date from: {line}");
						Console.WriteLine("Skipping drawing.");
						Console.WriteLine();
						continue;
					}

					var drawDate = new DateOnly(year, month, dayOfMonth);


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
		///
		/// Finish Parsing Input
		/// 

		powerballDrawings.Sort();

		int[] winningNumberCounts	= new int[70];
		int[] powerballCounts		= new int[27];

		int ticketCost = 2;
		int powerplayCost = 1;

		int winnings = 0;
		int spendings = 0;
		int winningsWithPowerPlay = 0;
		int spendingsWithPowerPlay = 0;
		int timesPlayed = 0;

		for (int i = 0; i < powerballDrawings.Count; i++)
		{
			var winningNumbers  = powerballDrawings[i].WinningNumbers;
			var powerballNumber = powerballDrawings[i].PowerBallNumber;
			var multiplier      = powerballDrawings[i].PowerPlayMultiplier;

			if (i > 9)
			{
				timesPlayed++;

				var predictedNumbers = PredictNumbers(winningNumberCounts, powerballCounts);
				var drawWinnings = powerballDrawings[i].CalculateWinnings(predictedNumbers.Item1, predictedNumbers.Item2, false);
				var drawWinningsWithPowerplay = powerballDrawings[i].CalculateWinnings(predictedNumbers.Item1, predictedNumbers.Item2, true);
				winnings += drawWinnings;
				spendings += ticketCost;
				winningsWithPowerPlay += drawWinningsWithPowerplay;
				spendingsWithPowerPlay += (ticketCost + powerplayCost);
				

				Console.WriteLine($"Date: {powerballDrawings[i].Date}");
				Console.WriteLine($"\tWinning Numbers: {{{string.Join(", ", winningNumbers)}}}");
				Console.WriteLine($"\tPowerball Number: {powerballNumber}");
				Console.WriteLine($"\tPowerplay Multiplier: {multiplier}");
				Console.WriteLine();
				Console.WriteLine($"\tPredicted Winning Numbers: {{{string.Join(", ", predictedNumbers.Item1)}}}");
				Console.WriteLine($"\tPredicted Powerball Number: {predictedNumbers.Item2}");
				Console.WriteLine();
				Console.WriteLine("\tNegative values for standard USD are embedded inside of '()'.");
				Console.WriteLine($"\tProfit made this draw: {drawWinnings - ticketCost:C}");
				Console.WriteLine($"\t\tWith Powerplay: ${drawWinningsWithPowerplay - (ticketCost + powerplayCost):C}");
				Console.WriteLine();
				Console.WriteLine($"\tNumber of times played: {timesPlayed}");
				Console.WriteLine($"\tTotal running profit: ${winnings - spendings:C}");
				Console.WriteLine($"\t\tWith Powerplay: ${winningsWithPowerPlay - spendingsWithPowerPlay:C}");
				Console.WriteLine();
			}


			foreach (var n in winningNumbers)
				winningNumberCounts[n]++;
			powerballCounts[powerballNumber]++;
		}
	}

	private static Tuple<int[], int> PredictNumbers(int[] winningNumberWeights, int[] powerballWeights)
	{
		var weightedWinningNumbers = new List<Tuple<int, int>>(70);
		var weightedPowerballNumbers = new List<Tuple<int, int>>(27);

		for (int i = 0; i < winningNumberWeights.Length; i++)
			weightedWinningNumbers.Add(new Tuple<int, int>(i, winningNumberWeights[i]));
		for (int i = 0; i < powerballWeights.Length; i++)
			weightedPowerballNumbers.Add(new Tuple<int, int>(i, powerballWeights[i]));

		weightedWinningNumbers.Sort(SortByGreatestWeightFirst);
		weightedPowerballNumbers.Sort(SortByGreatestWeightFirst);

		var predictedNumbers = new int[5];

		for (int i = 0; i < 5; i++)
			predictedNumbers[i] = weightedWinningNumbers[i].Item1;

		return new Tuple<int[], int>(predictedNumbers, weightedPowerballNumbers[0].Item1);
	}

	private static int SortByGreatestWeightFirst(Tuple<int, int> x, Tuple<int, int> y)
	{
		return -1 * x.Item2.CompareTo(y.Item2);
	}

}
