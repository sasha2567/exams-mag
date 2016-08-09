using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IAD
{
    class GraphBuilder
    {
        PictureBox pictureBox;
        Graphics graphics;
        public Point center;
        public Point scale;
        public GraphBuilder(PictureBox pictureBox, Point center, Point scale)
        {
            this.pictureBox = pictureBox;
            graphics = this.pictureBox.CreateGraphics();
            this.center = center;
            this.scale = scale;
        }

        public void SetGraphics(PictureBox pictureBox, Point center)
        {
            this.pictureBox = pictureBox;
            graphics = this.pictureBox.CreateGraphics();
            this.center = center;
            graphics.Clear(pictureBox.BackColor);
        }
        public void DrawGrid(Pen pen)
        {
            DrawGrid(pen, this.scale);
        }
        public void DrawGrid(Pen pen, Point scale)
        {
            for (int i = 0; i * scale.Y < pictureBox.Height; i += scale.Y)
            {
                graphics.DrawLine(pen,
                    new Point(0, center.Y + i * scale.Y),
                    new Point(pictureBox.Width, center.Y + i * scale.Y));
                graphics.DrawLine(pen,
                    new Point(0, center.Y - i * scale.Y),
                    new Point(pictureBox.Width, center.Y - i * scale.Y));
                graphics.DrawString("" + i, new Font("Arial", 8), pen.Brush, new Point(center.X + i * scale.X, center.Y));
                graphics.DrawString("" + -i, new Font("Arial", 8), pen.Brush, new Point(center.X - i * scale.X, center.Y));
            }
            for (int j = 0; j * scale.X < pictureBox.Width; j += scale.X)
            {
                graphics.DrawLine(pen,
                    new Point(center.X + j * scale.X, 0),
                    new Point(center.X + j * scale.X, pictureBox.Height));
                graphics.DrawLine(pen,
                    new Point(center.X - j * scale.X, 0),
                    new Point(center.X - j * scale.X, pictureBox.Height));
                graphics.DrawString("" + -j, new Font("Arial", 8), pen.Brush, new Point(center.X, center.Y + j * scale.Y));
                graphics.DrawString("" + j, new Font("Arial", 8), pen.Brush, new Point(center.X, center.Y - j * scale.X));
            }
            pen.Width++;
            graphics.DrawLine(pen,
                    new Point(0, center.Y),
                    new Point(pictureBox.Width, center.Y));
            graphics.DrawLine(pen,
                    new Point(center.X, 0),
                    new Point(center.X, pictureBox.Height));
        }

        public void DrawPlot(Pen pen, float[] y)
        {
            List<PointF> res = new List<PointF>(y.Length);
            for (int i = 0; i < y.Length; i++)
                res.Add(new PointF(i, y[i]));
            DrawPlot(pen, res);
        }
        public void DrawPlot(Pen pen, List<PointF> inmass)
        {
            for (int i = 1; i < inmass.Count; i++)
            {
                graphics.DrawLine(pen,
                    new PointF(center.X + inmass[i - 1].X * scale.X, center.Y - inmass[i - 1].Y * scale.Y),
                    new PointF(center.X + inmass[i].X * scale.X, center.Y - inmass[i].Y * scale.Y));
            }
        }
    }
}
