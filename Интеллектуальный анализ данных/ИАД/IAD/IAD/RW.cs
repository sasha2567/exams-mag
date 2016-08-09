using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAD
{
    class RW
    {
        public List<PointF> forecast(List<PointF> inmass, int steps, float alfa)
        {
            if (steps > 0)
            {
                List<PointF> result = new List<PointF>(steps + 1);
                result.Add(new PointF(inmass[inmass.Count - 1].X, inmass[inmass.Count - 1].Y));
                float step = 0;
                for (int i = 1; i < inmass.Count; i++)
                    step += inmass[i].X - inmass[i - 1].X;
                step /= inmass.Count - 1;
                Random rand = new Random();
                for (int i = 1; i <= steps; i++)
                {
                    if (alfa != 0)
                        result.Add(new PointF(result[i - 1].X + step,
                            result[i - 1].Y + alfa + 2 * (float)rand.NextDouble() - 1));
                    else
                        result.Add(new PointF(result[i - 1].X + step,
                            result[i - 1].Y + 2 * (float)rand.NextDouble() - 1));
                }
                return result;
            }
            return inmass;
        }
    }
}
