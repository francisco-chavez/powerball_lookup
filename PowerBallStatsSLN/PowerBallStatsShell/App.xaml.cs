
using System.Configuration;
using System.Data;
using System.Windows;


namespace PowerBallStatsShell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App 
		: PrismApplication
    {
		protected override void RegisterTypes(IContainerRegistry containerRegistry)
		{
		}

		protected override Window CreateShell()
		{
			var window = Container.Resolve<MainWindow>();
			return window;
		}
    }

}
