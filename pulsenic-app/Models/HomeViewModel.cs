using System;
using System.Drawing;

namespace pulsenic_app.Models
{
	public class HomeViewModel
	{
        public Curve Curve { get;}
        public string Points;
        public string CurvePoints;
        public string Equation;

        public HomeViewModel(
			Curve curve,
            string points,
            string curvePoints,
            string equation)
		{
            this.Curve = curve;
			this.Points = points;
            this.CurvePoints = curvePoints;
			this.Equation = equation;
        }
	}
}

