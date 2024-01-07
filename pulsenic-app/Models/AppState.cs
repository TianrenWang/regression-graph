using System;
using System.Collections.Generic;
using System.Text.Json;
using MathNet.Numerics;
namespace pulsenic_app.Models
{
	public class AppState
	{
        private static readonly List<Point> Points = new();
        private static List<Point> LinearPoints = new();
        private static List<Point> QuadraticPoints = new();
        private static List<Point> CubicPoints = new();
        private static string LinearEquation = "Need at least 2 points";
        private static string QuadraticEquation = "Need at least 3 points";
        private static string CubicEquation = "Need at least 4 points";

        public static void AddPoint(Point point)
        {
            Points.Add(point);
            if (Points.Count < 2) return;
            double[] xData = new double[Points.Count];
            double[] yData = new double[Points.Count];
            float minX = Points[0].X;
            float maxX = Points[0].X;
            for (int i = 0; i < Points.Count; i++)
            {
                xData[i] = Points[i].X;
                yData[i] = Points[i].Y;
                if (Points[i].X < minX) minX = Points[i].X;
                if (Points[i].X > maxX) maxX = Points[i].X;
            }

            // Get best fit for linear regression
            (double b, double a) = Fit.Line(xData, yData);
            double[] linearCoefficients = { b, a };
            LinearEquation = GetEquationFromCoefficients(linearCoefficients);
            LinearPoints = calculateCurvePoints(minX, maxX, linearCoefficients);

            // Get best fit for quadratic regression
            if (Points.Count < 3) return;
            double[] quadraicCoefficients = Fit.Polynomial(xData, yData, 2);
            QuadraticEquation = GetEquationFromCoefficients(quadraicCoefficients);
            QuadraticPoints = calculateCurvePoints(minX, maxX, quadraicCoefficients);

            // Get best fit for cubic regression
            if (Points.Count < 4) return;
            double [] cubicCoefficients = Fit.Polynomial(xData, yData, 3);
            CubicEquation = GetEquationFromCoefficients(cubicCoefficients);
            CubicPoints = calculateCurvePoints(minX, maxX, cubicCoefficients);
        }

        private static string GetEquationFromCoefficients(double[] co)
        {
            string result = "";
            for (int i = 0; i < co.Length; i++)
            {
                double coefficient = Math.Round(co[i], 2);
                if (Math.Abs(coefficient - 0) < 1e-10) continue;
                if (result != "")
                    if (coefficient > 0)
                        result += " + ";
                    else if (coefficient < 0)
                        result += " - ";
                if (Math.Abs(coefficient - 1) > 1e-10)
                    result += Math.Abs(coefficient);
                if (i == 1) result += "x";
                else if (i > 1) result += "x^" + i;
            }
            return "y = " + result;
        }

        public static HomeViewModel GetModel(Curve curve)
        {
            string serializedPoints = JsonSerializer.Serialize(Points);
            if (curve == Curve.LINEAR)
                return new(Curve.LINEAR, serializedPoints,
                    JsonSerializer.Serialize(LinearPoints), LinearEquation);
            else if (curve == Curve.QUADRATIC)
                return new(Curve.QUADRATIC, serializedPoints,
                    JsonSerializer.Serialize(QuadraticPoints), QuadraticEquation);
            else
                return new(Curve.CUBIC, serializedPoints,
                    JsonSerializer.Serialize(CubicPoints), CubicEquation);

        }

        private static float calculateYPoly(float x, double[] coefficients)
        {
            double result = 0;
            for(int i = 0; i < coefficients.Length; i++)
            {
                result += Math.Pow(x, i) * coefficients[i];
            }
            return (float)result;
        }

        private static List<Point> calculateCurvePoints(
            float xStart,
            float xEnd,
            double[] coefficients)
        {
            List<Point> result = new();
            float gapDistance = (xEnd - xStart) / 400;
            float curveStart = xStart - 50 * gapDistance;
            float curveEnd = xEnd + 50 * gapDistance;
            for(float x = curveStart; x <= curveEnd; x += gapDistance)
            {
                result.Add(new Point(x, calculateYPoly(x, coefficients)));
            }
            return result;
        }
    }
}

