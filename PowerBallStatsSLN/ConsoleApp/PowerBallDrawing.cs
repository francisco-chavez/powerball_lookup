
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PowerBallStatsCalculator;

public class PowerBallDrawing
	: IComparable<PowerBallDrawing>
{

	private DateOnly	_date;
	private int[]		_winningNumbers			= new int[5];
	private int			_powerballNumber;
	private int			_powerplayMultiplier;


	public PowerBallDrawing(
		DateOnly date, 
		int n0, int n1, int n2, int n3, int n4, 
		int powerballNumber, 
		int powerplayMultiplier)
	{
		_date					= date;

		_winningNumbers[0]		= n0;
		_winningNumbers[1]		= n1;
		_winningNumbers[2]		= n2;
		_winningNumbers[3]		= n3;
		_winningNumbers[4]		= n4;

		_powerballNumber		= powerballNumber;
		_powerplayMultiplier	= powerplayMultiplier;


		for (int i = 0; i < _winningNumbers.Length; i++)
		{
			if (!IsValidWinningNumber(_winningNumbers[i]))
				throw new ArgumentOutOfRangeException($"n{i}");
		}

		if (!IsValidPowerballNumber(powerballNumber))
			throw new ArgumentOutOfRangeException(nameof(powerballNumber));

		if (!IsValidPowerplayMultiplier(powerplayMultiplier))
			throw new ArgumentOutOfRangeException(nameof(powerplayMultiplier));
	}


	public int		PowerBallNumber		{ get { return _powerballNumber; } }
	public int[]	WinningNumbers		{ get { return _winningNumbers; } }
	public DateOnly Date				{ get { return _date; } }
	public int		PowerPlayMultiplier { get { return _powerplayMultiplier; }}


	public int CompareTo(PowerBallDrawing other)
	{
		return this.Date.CompareTo(other.Date);
	}


	public static bool IsValidWinningNumber(int n)
	{
		return 0 < n && n < 70;
	}

	public static bool IsValidPowerballNumber(int n)
	{
		return 0 < n && n < 27;
	}

	public static bool IsValidPowerplayMultiplier(int n)
	{
		switch (n)
		{
			case 2:
			case 3:
			case 4:
			case 5:
			case 10:
				return true;
			default:
				return false;
		}
	}

}
