
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace PowerBallStatsShell;

public  class MainWindowViewModel
	: ViewModelBase
{
	
	public MainWindowViewModel()
	{
		LoadRawDataCommand = new DelegateCommand(LoadRawDataCommand_Execute);
	}


	public ICommand LoadRawDataCommand { get; private set; }
	public void LoadRawDataCommand_Execute() 
	{
		var dialog = new Microsoft.Win32.OpenFileDialog()
		{
			FileName	= "Document",
			DefaultExt	= ".csv",
			Filter		= "Comma-Separated Values (.csv)|*.csv",
			Multiselect = false,
			Title		= "Import Powerball Data"
		};

		bool keepGoing = dialog.ShowDialog() == true;

		if (!keepGoing)
			return;

		var pathname = dialog.FileName;
	}

}
