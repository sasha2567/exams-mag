using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IAD
{
    class AR
    {
        public float coefA;
        public float coefB;
        public float coefC;

        private float[] determinant2x2(PointF y, PointF y1, PointF y2)
        {
            float[] result = new float[2];
            float det1 = (y.Y - y1.Y) / (y1.Y - y2.Y);
            float det2 = (y1.Y * y1.Y - y.Y * y2.Y) / (y1.Y - y2.Y);
            result[0] = det1;
            result[1] = det2;
            return result;
        }

        private float[] determinant3x3(PointF y, PointF y1, PointF y2, PointF y3, PointF y4)
        {
            float[] result = new float[3];
            float det1 = (y4.Y * y.Y * y.Y - 2 * y1.Y * y2.Y * y3.Y + y2.Y * y2.Y * y2.Y - y.Y * y2.Y * y4.Y + y.Y * y3.Y * y3.Y) /
                (y2.Y * y2.Y - y2.Y * y3.Y - y4.Y * y2.Y + y3.Y * y3.Y - y1.Y * y3.Y + y1.Y * y4.Y);
            float det2 = (y2.Y * y2.Y + y.Y * y3.Y - y.Y * y4.Y - y1.Y * y2.Y + y1.Y * y4.Y - y2.Y * y3.Y) /
                (y2.Y * y2.Y - y2.Y * y3.Y - y4.Y * y2.Y + y3.Y * y3.Y - y1.Y * y3.Y + y1.Y * y4.Y);
            float det3 = (y1.Y * y1.Y - y1.Y * y2.Y - y1.Y * y3.Y + y2.Y * y2.Y - y.Y * y2.Y + y.Y * y3.Y) /
                (y2.Y * y2.Y - y2.Y * y3.Y - y4.Y * y2.Y + y3.Y * y3.Y - y1.Y * y3.Y + y1.Y * y4.Y);
            result[0] = det1;
            result[1] = det2;
            result[2] = det3;
            return result;
        }

        public List<PointF> Forecast(List<PointF> inmass, int steps, int exponent)
        {
            if (steps > 0)
            {
                List<PointF> result = new List<PointF>(steps + 1);
                List<PointF> temp = new List<PointF>(inmass.Count + steps);
                result.Add(new PointF(inmass[inmass.Count - 1].X, inmass[inmass.Count - 1].Y));
                float step = 0;
                for (int i = 1; i < inmass.Count; i++)
                    step += inmass[i].X - inmass[i - 1].X;
                step /= inmass.Count - 1;
                if (exponent == 1)
                {

                    float[] res = determinant2x2(result[0], inmass[inmass.Count - 2], inmass[inmass.Count - 3]);
                    coefA = res[0];
                    coefB = res[1];
                    result.Add(new PointF(result[0].X + step, coefB + coefA * result[0].Y));
                    
                    res = determinant2x2(result[1], result[0], inmass[inmass.Count - 2]);
                    coefA = res[0];
                    coefB = res[1];
                    result.Add(new PointF(result[1].X + step, coefB + coefA * result[1].Y));
                    for (int i = 2; i <= steps; i++)
                    {
                        res = determinant2x2(result[i], result[i - 1], result[i - 2]);
                        coefA = res[0];
                        coefB = res[1];
                        result.Add(new PointF(result[i].X + step,
                            coefB + coefA * result[i].Y));
                    }
                    return result;
                }
                if (exponent == 2 && inmass.Count >= 5)
                {
                    float[] res = determinant3x3(result[0], inmass[inmass.Count - 2], inmass[inmass.Count - 3], inmass[inmass.Count - 4], inmass[inmass.Count - 5]);
                    coefA = res[0];
                    coefB = res[1];
                    coefC = res[2];
                    result.Add(new PointF(result[0].X + step, coefA + coefB * result[0].Y + coefC * inmass[inmass.Count - 2].Y));
                    
                    res = determinant3x3(result[1], result[0], inmass[inmass.Count - 2], inmass[inmass.Count - 3], inmass[inmass.Count - 4]);
                    coefA = res[0];
                    coefB = res[1];
                    coefC = res[2];
                    result.Add(new PointF(result[1].X + step, coefA + coefB * result[1].Y + coefC * result[0].Y));
                    
                    res = determinant3x3(result[2], result[1], result[1], inmass[inmass.Count - 2], inmass[inmass.Count - 3]);
                    coefA = res[0];
                    coefB = res[1];
                    coefC = res[2];
                    result.Add(new PointF(result[2].X + step, coefA + coefB * result[2].Y + coefC * result[1].Y));
                    
                    res = determinant3x3(result[3], result[2], result[1], result[1], inmass[inmass.Count - 2]);
                    coefA = res[0];
                    coefB = res[1];
                    coefC = res[2];
                    result.Add(new PointF(result[3].X + step, coefA + coefB * result[3].Y + coefC * result[2].Y));
                    for (int i = 4; i <= steps; i++)
                    {
                        res = determinant3x3(result[i], result[i - 1], result[i - 2], result[i - 3], result[i - 4]);
                        coefA = res[0];
                        coefB = res[1];
                        coefC = res[2];
                        result.Add(new PointF(result[i].X + step, coefA + coefB * result[i].Y + coefC * result[i - 1].Y));
                    }
                    return result;
                }
            }
            return inmass;
        }

        public List<PointF> Model(List<PointF> inmass, int exponent)
        {
            List<PointF> result = new List<PointF>(inmass.Count);
            result.Add(new PointF(inmass[0].X, inmass[0].Y));
            if (exponent == 1)
            {
                float[] res = determinant2x2(inmass[2], inmass[1], inmass[0]);
                coefA = res[0];
                coefB = res[1];
                result.Add(new PointF(inmass[1].X, coefB + coefA * inmass[0].Y));
                for (int i = 2; i < inmass.Count; i++)
                {
                    res = determinant2x2(inmass[i], inmass[i - 1], inmass[i - 2]);
                    coefA = res[0];
                    coefB = res[1];
                    result.Add(new PointF(inmass[i].X, coefB + coefA * inmass[i - 1].Y));
                }
                return result;
            }
            if (exponent == 2)
            {
                result.Add(new PointF(inmass[1].X, inmass[1].Y));
                result.Add(new PointF(inmass[2].X, inmass[2].Y));
                result.Add(new PointF(inmass[3].X, inmass[3].Y));
                float[] res = new float[3];
                for (int i = 4; i < inmass.Count; i++)
                {
                    res = determinant3x3(inmass[i], inmass[i-1], inmass[i-2], inmass[i-3], inmass[i-4]);
                    coefA = res[0];
                    coefB = res[1];
                    result.Add(new PointF(inmass[i].X, coefA + coefB * inmass[i - 1].Y + coefC * inmass[i - 2].Y));
                }
                return result;
            }
            return inmass;
        }
    }
}
