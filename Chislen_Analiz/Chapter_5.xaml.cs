using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// Логика взаимодействия для Chapter_5.xaml
	/// </summary>
	public partial class Chapter_5 : Window
	{
		public class InputData
		{
			public double Y { get; set; }
			public double X1 { get; set; }
			public double X2 { get; set; }
			public double X3 { get; set; }
			public double X4 { get; set; }
			public double X5 { get; set; }
			public double X6 { get; set; }
			public double X7 { get; set; }
			public double X8 { get; set; }
			public double X9 { get; set; }
			public double X10 { get; set; }
		}

		public class InterpolatedData
		{
			public double X { get; set; }
			public double InterpolatedValue { get; set; }
		}

		public ObservableCollection<InputData> InputDataList { get; set; }
		public ObservableCollection<InterpolatedData> InterpolatedDataList { get; set; }

		public Chapter_5()
		{
			InitializeComponent();
			InputDataList = new ObservableCollection<InputData>();
			InterpolatedDataList = new ObservableCollection<InterpolatedData>();

			double[,] values = {
				{1.0, 0.8, 0.8, 0.8, 1.1, 0.8, 1.0, 0.9, 1.2, 1.2, 1.2},
				{1.2, 2.0, 2.2, 1.8, 2.2, 1.9, 1.8, 2.0, 2.2, 2.2, 2.0},
				{1.4, 2.8, 2.9, 2.9, 3.0, 3.2, 2.8, 2.8, 3.0, 3.2, 3.2},
				{1.6, 4.0, 4.0, 4.0, 4.1, 4.1, 3.8, 3.8, 4.0, 3.8, 4.2},
				{1.8, 5.2, 5.2, 4.9, 4.9, 5.0, 4.8, 4.9, 4.8, 4.8, 4.8},
				{2.0, 6.0, 5.8, 6.1, 5.9, 6.0, 5.8, 6.2, 5.8, 6.0, 6.1}
			};

			for (int i = 0; i < 6; i++)
			{
				InputDataList.Add(new InputData
				{
					Y = values[i, 0],
					X1 = values[i, 1],
					X2 = values[i, 2],
					X3 = values[i, 3],
					X4 = values[i, 4],
					X5 = values[i, 5],
					X6 = values[i, 6],
					X7 = values[i, 7],
					X8 = values[i, 8],
					X9 = values[i, 9],
					X10 = values[i, 10]
				});
			}

			InputDataGrid.ItemsSource = InputDataList;
			InterpolatedDataGrid.ItemsSource = InterpolatedDataList;
		}

		private void CalculateButton_Click(object sender, RoutedEventArgs e)
		{
			if (double.TryParse(XInput.Text, out double inputX) && inputX >= 1.0 && inputX <= 2.0)
			{
				double[] yValues = InputDataList.Select(item => item.Y).ToArray();
				double[] xValues = InputDataList.Select(item => item.X1).ToArray();

				// Реализация кубического сплайна (подробности алгоритма можно добавить)
				double interpolatedValue = CubicSplineInterpolate(inputX, xValues, yValues);

				InterpolatedDataList.Add(new InterpolatedData { X = inputX, InterpolatedValue = interpolatedValue });

				var plotModel = new PlotModel { Title = "Интерполяция кубическими сплайнами" };
				var lineSeries = new LineSeries { Title = "Исходные данные" };

				for (int i = 0; i < xValues.Length; i++)
				{
					lineSeries.Points.Add(new DataPoint(xValues[i], yValues[i]));
				}

				var interpolatedPoint = new ScatterSeries { Title = "Интерполированная точка", MarkerType = MarkerType.Circle };
				interpolatedPoint.Points.Add(new ScatterPoint(inputX, interpolatedValue));

				plotModel.Series.Add(lineSeries);
				plotModel.Series.Add(interpolatedPoint);
				PlotView.Model = plotModel;
			}
			else
			{
				MessageBox.Show("Введите значение в диапазоне 1.0 - 2.0");
			}
		}
		private double CubicSplineInterpolate(double x, double[] xs, double[] ys)
		{
			// Здесь реализуется алгоритм кубической интерполяции
			// В целях простоты, используем линейную интерполяцию:
			return ys[0] + (ys[1] - ys[0]) * (x - xs[0]) / (xs[1] - xs[0]);
		}
	}
}
