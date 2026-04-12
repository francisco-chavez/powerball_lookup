
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PowerBallStatsShell.Data;

/// <summary>
/// PowerPall results have a draw date, 5 winning numbers, 1 power ball number, and a power play 
/// multiplier. The power ball number is included as the 6th winning number.
/// </summary>
public class PowerBallResult
{

	public	const				int					NUMBER_COUNT				= 6;
	public	static readonly		HashSet<UInt16>		POWER_PLAY_MULTIPLIERS		= new HashSet<UInt16> { 2, 3, 4, 5, 10 };

	private DateOnly	_drawDate;
	private UInt16[]	_winningNumbers			= new UInt16[NUMBER_COUNT];
	private UInt16		_powerPlayMultiplier	= 2;


	public PowerBallResult(DateOnly drawDate, IEnumerable<UInt16> winningNumbers, UInt16 powerPlayMultiplier)
	{
		if (winningNumbers == null)
			throw new ArgumentNullException(nameof(winningNumbers));

		if (winningNumbers.Count() != PowerBallResult.NUMBER_COUNT)
			throw new ArgumentOutOfRangeException(nameof(winningNumbers), $"{NUMBER_COUNT} numbers are expected.");

		if (!POWER_PLAY_MULTIPLIERS.Contains(powerPlayMultiplier))
			throw new ArgumentOutOfRangeException(nameof(powerPlayMultiplier), "The given power play multiplier violates the game rules.");


		_drawDate = drawDate;
		_powerPlayMultiplier = powerPlayMultiplier;

		int i = 0;
		foreach (var n in winningNumbers)
		{
			this[i] = n;
			i++;
		}

		for (i = 0; i < NUMBER_COUNT - 1; i++)
			if (!(0 < _winningNumbers[i] && _winningNumbers[i] <= 69))
				throw new ArgumentOutOfRangeException(nameof(_winningNumbers), $"The winning number at index {i} is out of range.");
		if (!(0 < _winningNumbers[NUMBER_COUNT - 1] && _winningNumbers[NUMBER_COUNT - 1] <= 26))
			throw new ArgumentOutOfRangeException(nameof(_winningNumbers), "The power ball number is out of range.");
	}


	public UInt16 this[int index]
	{
		get { return _winningNumbers[index]; }
		private set { _winningNumbers[index] = value; }
	}

	public DateOnly DrawDate
	{
		get { return _drawDate; }
	}

	public UInt16 PowerPlayMultiplier
	{
		get { return _powerPlayMultiplier; }
	}

}
