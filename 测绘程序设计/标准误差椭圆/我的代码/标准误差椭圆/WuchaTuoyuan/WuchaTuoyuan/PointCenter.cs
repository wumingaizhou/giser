using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuchaTuoyuan
{
    /// <summary>
    /// 计算协方差
    /// </summary>
    class PointCenter
    {
        public double qxx, qyy, qxy,so;//so是单位权的标准偏差
        public double t;
        public double Su, Sv;//新坐标系下的长半轴短半轴
        public PointCenter(List<Point> points)
        {
            double[] xlist = new double[points.Count];
            double[] ylist = new double[points.Count];
            double Xavr = 0;//X坐标的均值
            double Yavr = 0;//Y坐标的均值
            for(int i = 0;i < points.Count;i++)
            {
                Xavr += points[i].x;
                Yavr += points[i].y;
            }
            Xavr /= points.Count;
            Yavr /= points.Count;
            for(int i = 0;i < points.Count;i++)//去均值
            {
                xlist[i] = points[i].x - Xavr;
                ylist[i] = points[i].y - Yavr;
            }
            //接下来就可以计算协方差了
            for(int i = 0;i < points.Count;i++)
            {
                qxx += xlist[i] * xlist[i];
                qyy += ylist[i] * ylist[i];
                qxy += xlist[i] * ylist[i];
            }
            qxx /= (points.Count - 1);
            qyy /= (points.Count - 1);
            qxy /= (points.Count - 1);
            so = Math.Sqrt(qxx + qyy) / Math.Sqrt(points.Count);
            //接下来计算t
            t = Math.Atan(2 * qxy / (qyy - qxx));
            t = t / 2;
            //计算新坐标系下的协方差
            double quu = Math.Sin(t) * Math.Sin(t) * qxx + 2 * Math.Cos(t) * Math.Sin(t) * qxy + Math.Cos(t) * Math.Cos(t) * qyy;
            double qvv = Math.Cos(t) * Math.Cos(t) * qxx - 2 * Math.Cos(t) * Math.Sin(t) * qxy + Math.Sin(t) * Math.Sin(t) * qyy;
            Su = so * Math.Sqrt(quu);
            Sv = so * Math.Sqrt(qvv);
        }
    }
}
