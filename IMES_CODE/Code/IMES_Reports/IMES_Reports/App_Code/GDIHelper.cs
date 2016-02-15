using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace IMES_Reports.App_Code
{
    class GDIHelper
    {
        private void AddBeziersExample(PaintEventArgs e, Point[] myArray)
        {

            // Adds two Bezier curves.
            //Point[] myArray =
            // {
            //     new Point(20, 100),
            //     new Point(40, 75),
            //     new Point(60, 125),
            //     new Point(80, 100),
            //     new Point(100, 50),
            //     new Point(120, 150),
            //     new Point(140, 100)
            // };

            // Create the path and add the curves.
            GraphicsPath myPath = new GraphicsPath();
            myPath.AddBeziers(myArray);

            // Draw the path to the screen.
            Pen myPen = new Pen(Color.Black, 2);
            e.Graphics.DrawPath(myPen, myPath);
        }
    }
}
