
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

	}

}
