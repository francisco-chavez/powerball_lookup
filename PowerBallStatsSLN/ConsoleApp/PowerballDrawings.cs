
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PowerBallStatsCalculator;

public class PowerballDrawings
	: List<PowerBallDrawing>
{
	/// <summary>
	/// When given a set of numbers to search for, this will look the latest number that is a 
	/// winning number before the stop date. If nothing can be found, it will return 0. If 
	/// multiple numbers are within the same drawing, it will return the first one it finds.
	/// </summary>
	/// <remarks>
	/// This is using a binary search to speed things up. Due to using the generic nature of the 
	/// algorithm used, this will return 0 if you enter a stop date that isn't a drawing date.
	/// </remarks>
	// Todo: Switch to a binary search algorithm that doesn't fail just because someone entered a
	//		 stop date that falls between two drawings.
	public int FindMostRecentWinningNumber(DateOnly stopDate, params int[] winningNumbers)
	{
		// If winning numbers are given, then return 0.
		if ((winningNumbers?.Length ?? 0) == 0)
			return 0;

		// If only one value is given, then there's nothing to compare it to. Therefore it
		// automatically wins.
		if (winningNumbers.Length == 1)
			return winningNumbers[0];

		int index_of_stop_date = FindIndexOfDate(stopDate);

		// We are looking for numbers that were drawn before the stop date. The drawing from the stop
		// date does NOT count as "before the stop date".
		if (index_of_stop_date < 1)
			return 0;


		for (int i = index_of_stop_date - 1; i > -1; i--)
			foreach (var n in winningNumbers)
				if (this[i].ContainsWinningNumber(n))
					return n;
		return 0;
	}

	public int FindIndexOfDate(DateOnly date)
	{
		int low = 0;
		int high = this.Count - 1;

		while (low < high)
		{
			int mid = (low + high) / 2;

			if (this[mid].Date == date)
				return mid;
			if (this[mid].Date < date)
				low = mid + 1;
			else
				high = mid - 1;
		}

		return -1;
	}

}
