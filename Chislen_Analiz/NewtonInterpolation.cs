using System;
using System.Collections.Generic;

public static class NewtonInterpolation
{
	public static double Interpolate(List<double> xValues, List<double> yValues, double x)
	{
		int n = xValues.Count;
		var dividedDifferences = new double[n];
		Array.Copy(yValues.ToArray(), dividedDifferences, n);

		for (int i = 1; i < n; i++)
		{
			for (int j = n - 1; j >= i; j--)
			{
				dividedDifferences[j] = (dividedDifferences[j] - dividedDifferences[j - 1]) / (xValues[j] - xValues[j - i]);
			}
		}

		double result = dividedDifferences[n - 1];
		for (int i = n - 2; i >= 0; i--)
		{
			result = result * (x - xValues[i]) + dividedDifferences[i];
		}

		return result;
	}
}
