using System;
using System.Collections.Generic;

public static class LagrangeInterpolation
{
	public static double Interpolate(List<double> xValues, List<double> yValues, double x)
	{
		double result = 0.0;

		for (int i = 0; i < xValues.Count; i++)
		{
			double term = yValues[i];
			for (int j = 0; j < xValues.Count; j++)
			{
				if (j != i)
				{
					term *= (x - xValues[j]) / (xValues[i] - xValues[j]);
				}
			}
			result += term;
		}

		return result;
	}
}
