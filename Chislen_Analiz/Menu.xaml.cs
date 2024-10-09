using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chislen_Analiz
{
	/// <summary>
	/// Логика взаимодействия для Menu.xaml
	/// </summary>
	public partial class Menu : Window
	{
		public Menu()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();

		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Chapter1_2 chapter1_2 = new Chapter1_2();
			chapter1_2.Show();
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			Chapter_3 Chapter_3 = new Chapter_3();
			Chapter_3.Show();

		}
    }
}
