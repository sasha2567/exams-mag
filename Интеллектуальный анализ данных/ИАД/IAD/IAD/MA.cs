using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAD
{
    public class MA
    {
        public List<PointF> Solve(List<PointF> inmass, int q, int steps)
        {
            List<PointF> result = new List<PointF>(inmass.Count + steps);
            List<PointF> res = new List<PointF>(steps + 1);
            res.Add(new PointF(inmass[inmass.Count - 1].X, inmass[inmass.Count - 1].Y));
            for (int i = 0; i < inmass.Count; i++)
            {
                result.Add(inmass[i]);
            }
            float step = 0;
            for (int i = 1; i < inmass.Count; i++)
                step += inmass[i].X - inmass[i - 1].X;
            step /= inmass.Count - 1;
            for (int i = inmass.Count; i < inmass.Count + steps; i++)
            {
                List<float> sum = new List<float>();
                for (int j = 0; j <= q; j++)
                {
                    sum.Add(result[i - j - 1].Y);
                }
                result.Add(new PointF(result[i - 1].X + step,sum.Average()));
            }
            for (int i = 1; i < steps + 1; i++)
            {
                res.Add(new PointF(res[i - 1].X + step, result[i + inmass.Count - 1].Y));
            }
            return res;
        }
    }
}
