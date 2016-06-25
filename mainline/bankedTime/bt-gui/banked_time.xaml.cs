// bankedTime : bt-gui
// Écrit par : jfgratton (), 2015.02.13 @ 16:36
// 
// banked_time.xaml.cs : fenêtre principale

using System.Windows;

namespace JFG.FX
{
	/// <summary>
	///     Interaction logic for banked_time.xaml
	/// </summary>
	public partial class bt_gui : Window
	{
		/// <summary>
		///     Interaction logic for banked_time.xaml
		/// </summary>
		public bt_gui()
		{
			InitializeComponent();
		}

		private void BankedTimeTabMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			tabctrl1.SelectedIndex = 1;
			tabBanked.IsSelected = true;
			tabConnection.Visibility = Visibility.Hidden;
		}

		private void OvertimeTabMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			tabctrl1.SelectedIndex = 2;
			tabOver.IsSelected = true;
			tabConnection.Visibility = Visibility.Collapsed;
		}

		private void DBTabMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			tabctrl1.SelectedIndex = 3;
			tabDatabase.IsSelected = true;
			tabConnection.Visibility = Visibility.Hidden;
		}

		private void CONNECTION(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			tabConnection.Visibility = Visibility.Visible;
			tabConnection.IsSelected = true;
			tabctrl1.SelectedIndex = 4;
		}
	}
}