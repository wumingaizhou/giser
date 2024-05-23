using System;
using System.Text.RegularExpressions;
namespace WuchaTuoyuan
{
    class Point
    {
        public double x;
        public double y;
        public Point(string line)
        {
            var buf = Regex.Split(line, @"\s+");
            x = Convert.ToDouble(buf[0]);
            y = Convert.ToDouble(buf[1]);
        }
    }
}