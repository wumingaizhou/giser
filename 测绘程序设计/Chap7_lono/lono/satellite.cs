using System;
using System.Text.RegularExpressions;

namespace lono
{
    class satellite
    {
        public string name;
        public decimal X;
        public decimal Y;
        public decimal Z;
        public decimal sate_x;
        public decimal sate_y;
        public decimal sate_z;
        public Time satellitetime;
        public double Bp = 30.0 / 180.0 * Math.PI;
        public double Lp = 114.0 / 180 * Math.PI;
        public decimal A;//高度角
        public decimal E;//方位角
        public decimal AIP;//地磁维度
        public decimal BIP;//地磁维度
        public decimal CIP;//地磁维度
        public decimal conIP;//地磁维度

        //以下是计算电离层延迟量的变量
        public decimal[] alist = { (decimal)0.1397 * (decimal)Math.Pow(10, -7), (decimal)-0.7451 * (decimal)Math.Pow(10, -8), (decimal)-0.5960 * (decimal)Math.Pow(10, -7), (decimal)0.1192 * (decimal)Math.Pow(10, -6) };
        public decimal[] blist = { (decimal)0.1270 * (decimal)Math.Pow(10, 6), (decimal)-0.1966 * (decimal)Math.Pow(10, 6), (decimal)0.6554 * (decimal)Math.Pow(10, 5), (decimal)0.2621 * (decimal)Math.Pow(10, 6), };
        public decimal A1 = (decimal)(5 * Math.Pow(10, -9));
        public decimal A3 = 50400;
        public decimal A2, A4;
        public decimal t;
        public decimal F;
        public decimal Tion;
        public decimal Dion;
        public decimal C = 299792458;

        public satellite(Time time,string linedata)
        {
            satellitetime = time;
            var buf = Regex.Split(linedata, @"\s+");//正则表达式的内容；
            name = buf[0];
            sate_x = Convert.ToDecimal(buf[1]) * 1000;
            sate_y = Convert.ToDecimal(buf[2]) * 1000;
            sate_z = Convert.ToDecimal(buf[3]) * 1000;
            var XYZ = CalXYZ();
            X = XYZ[0,0];
            Y = XYZ[1,0];
            Z = XYZ[2,0];
            if(Y > 0)
            {
                A = (decimal)(Math.Atan((double)Y / (double)X) * 180.0 / Math.PI);
                if (X > 0)
                {

                }
                else
                {
                    A = 180 + A;
                }
            }
            else
            {
                A = (decimal)(Math.Atan((double)Y / (double)X) * 180.0 / Math.PI);
                if (X > 0)
                {
                    A = 360 + A;
                }
                else
                {
                    A = 180 + A;
                } 
            }
            E = (decimal)(Math.Atan((double)Z / (Math.Sqrt((double)X * (double)X + ((double)Y * (double)Y)))) * 180 /Math.PI);
            calIP();//计算IP的地磁维度

            GotoAlgo();//计算卫星的电离层延迟量
        }
        public decimal[,] CalXYZ()
        {
            //先搞定矩阵相乘，这里采用纯手写，先锻炼思维
            decimal[,] left = { { -(decimal)Math.Sin(Bp) * (decimal)Math.Cos(Lp) , -(decimal)Math.Sin(Bp) * (decimal)Math.Sin(Lp) , (decimal)Math.Cos(Bp) }, 
                               { -(decimal)Math.Sin(Lp) , (decimal)Math.Cos(Lp) , 0}, 
                               {(decimal)Math.Cos(Bp) * (decimal)Math.Cos(Lp) , (decimal)Math.Cos(Bp) * (decimal)Math.Sin(Lp) , (decimal)Math.Sin(Bp)} };
            decimal[,] right = { { sate_x + (decimal)2225669.7744}, { sate_y - (decimal)4998936.1598 }, { sate_z - (decimal)3265908.9678 } };
            var XYZ = CalleftXright(left,right);//矩阵相乘
            return XYZ;
        }

        public decimal[,] CalleftXright(decimal[,] left, decimal[,] right)//矩阵相乘的函数
        {
            int rowLeft = left.GetLength(0);//左矩阵的行数
            int colLeft = left.GetLength(1);//左矩阵的列数
            int rowRight = right.GetLength(0);//右矩阵的行数
            int colRight = right.GetLength(1);//右矩阵的列数
            decimal[,] result = new decimal[rowLeft,colRight];
            if(colLeft == rowRight)
            {
                for(int i = 0;i < rowLeft;i++)
                {
                    
                    for(int j = 0;j < colRight;j++)
                    {
                        decimal res = 0;
                        for (int k = 0; k < rowRight;k++)
                        {
                            res += left[i, k] * right[k, j];
                        }
                        result[i, j] = res;
                    }

                }
                
            }
            else
            {
                Console.WriteLine("左矩阵行数不等于右矩阵的列数！");
            }
            return result;
        }

        public void calIP()
        {
            conIP = (decimal)(0.0137 / ((double)E / 360.0 * 2 * Math.PI + 0.11) - 0.022);
            AIP = (decimal)(Bp + (double)conIP * Math.Cos((double)A / 360 * 2 * Math.PI));
            BIP = (decimal)Lp + conIP * (decimal)Math.Sin((double)A / 360 * 2 * Math.PI) / (decimal)Math.Cos((double)AIP);
            CIP = AIP + (decimal)(0.064 * Math.Cos((double)BIP - 1.617));
        }

        public void GotoAlgo()
        {
            A2 = alist[0] + alist[1] * CIP + alist[2] * CIP * CIP + alist[3] * CIP * CIP * CIP;
            A4 = blist[0] + blist[1] * CIP + blist[2] * CIP * CIP + blist[3] * CIP * CIP * CIP;
            t = 43200 * BIP + (decimal)satellitetime.second_of_day;
            F = 1 + 16 * ((decimal)0.53 - E / 180 * (decimal)Math.PI) * ((decimal)0.53 - E / 180 * (decimal)Math.PI) * ((decimal)0.53 - E / 180 * (decimal)Math.PI);

            decimal temp = (decimal)(2 * Math.PI * (double)(t - A3) / (double)A4);
            if(Math.Abs(temp) < (decimal)1.57)
            {
                Tion = F * (A1 + A2 * (decimal)Math.Cos((double)temp));
            }
            else
            {
                Tion = F * A1;
            }
            Dion = Tion * C;
            decimal eps = (decimal)Math.Pow(10,-5);
            if(E < eps)
            {
                Tion = 0;
                Dion = 0;
            }

        }
    }
}