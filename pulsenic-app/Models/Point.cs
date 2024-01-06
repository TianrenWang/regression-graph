using System;
namespace pulsenic_app.Models
{
    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }

        // This fixes an issue where binding this model to the POST
        // endpoint requires a parameter-less constructor
        public Point()
        {

        }
        public Point (float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}

