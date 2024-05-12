using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace Trop
{
    class Satellite
    {
        public string name;
        public int year;
        public int month;
        public int day;
        public decimal B;//经度
        public decimal L;//纬度
        public decimal H;//高程
        public decimal E;//高度角
        //以下是计算湿分量的变量
        public decimal Aw;
        public decimal Bw;
        public decimal Cw;
        public decimal mw;//湿分量
        //以下是计算干分量的变量
        public int days;
        public decimal ad;
        public decimal bd;
        public decimal cd;
        public decimal md;//干分量
        public decimal s;//对流层延迟


        public Satellite(string line)
        {
            var buf = Regex.Split(line, @"\,");
            name = buf[0];
            year = Convert.ToInt16(buf[1].Substring(0, 4));
            month = Convert.ToInt16(buf[1].Substring(4, 2));
            day = Convert.ToInt16(buf[1].Substring(6,2));
            B = Convert.ToDecimal(buf[2]);
            L = Convert.ToDecimal(buf[3]);
            H = Convert.ToDecimal(buf[4]);
            E = Convert.ToDecimal(buf[5]);
            Calmw();//计算湿分量
            Calmd();//计算干分量
            decimal ZHD = (decimal)2.29951 * (decimal)Math.Pow(Math.E, -0.000116 * (double)H);
            s = ZHD * md + (decimal)0.1 * mw;

        }
        private void Calmw()//计算湿分量
        {
            decimal[] alist = {(decimal)5.8021897E-4, (decimal)5.6794847E-4, (decimal)5.8118019E-4, (decimal)5.9727542E-4, (decimal)6.1641693E-4 };
            decimal[] blist = { (decimal)1.4275268E-3, (decimal)1.5138625E-3, (decimal)1.4572752E-3, (decimal)1.5007428E-3, (decimal)1.7599082E-3 };
            decimal[] clist = { (decimal)4.3472961E-2, (decimal)4.6729510E-2, (decimal)4.3908931E-2, (decimal)4.4626982E-2, (decimal)5.4736038E-2 };
            if(L < 15)
            {
                Aw = alist[0];
                Bw = blist[0];
                Cw = clist[0];

            }
            if(L >= 15 && L < 30)
            {
                Aw = alist[0] + (alist[1] - alist[0]) * (L - 15) / 15;
                Bw = blist[0] + (blist[1] - blist[0]) * (L - 15) / 15;
                Cw = clist[0] + (clist[1] - clist[0]) * (L - 15) / 15;
            }
            if(L >= 30 && L < 45)
            {
                Aw = alist[1] + (alist[2] - alist[1]) * (L - 30) / 15;
                Bw = blist[1] + (blist[2] - blist[1]) * (L - 30) / 15;
                Cw = clist[1] + (clist[2] - clist[1]) * (L - 30) / 15;
            }
            if(L >= 45 && L < 60)
            {
                Aw = alist[2] + (alist[3] - alist[2]) * (L - 45) / 15;
                Bw = blist[2] + (blist[3] - blist[2]) * (L - 45) / 15;
                Cw = clist[2] + (clist[3] - clist[2]) * (L - 45) / 15;
            }
            if (L >= 60 && L <= 75)
            {
                Aw = alist[3] + (alist[4] - alist[3]) * (L - 60) / 15;
                Bw = blist[3] + (blist[4] - blist[3]) * (L - 60) / 15;
                Cw = clist[3] + (clist[4] - clist[3]) * (L - 60) / 15;
            }
            if(L > 75)
            {
                Aw = alist[4];
                Bw = blist[4];
                Cw = clist[4];
            }

            decimal above,under;//分子分母
            decimal e = (decimal)Math.Sin((double)E / 180.0 * Math.PI);
            above = 1 / (1 + (Aw / (1 + (Bw / (1 + Cw)))));
            under = 1 / (e + (Aw / (e + (Bw / (e + Cw)))));
            mw = under / above;//题目写错了，分子分母反了.....
        }

        private void Calmd()
        {
            Caldays();//计算年积日
            decimal T = (decimal)Math.Cos(2 * Math.PI * (days - 28) / 365.25);
            decimal[] alist1 = { (decimal)1.2769934E-3, (decimal)1.2683230E-3, (decimal)1.2465397E-3, (decimal)1.2196049E-3, (decimal)1.2045996E-3 } ;
            decimal[] alist2 = { (decimal)0.0, (decimal)1.2709626E-5, (decimal)2.6523662E-5, (decimal)3.4000452E-5, (decimal)4.1202191E-4 } ;
            decimal[] blist1 = { (decimal)2.9153695E-3, (decimal)2.9152299E-3, (decimal)2.9288445E-3, (decimal)2.9022565E-3, (decimal)2.9024912E-3 } ;
            decimal[] blist2 = { (decimal)0.0, (decimal)2.1414979E-5, (decimal)3.0160779E-5, (decimal)7.2562722E-5, (decimal)1.1723375E-5 } ;
            decimal[] clist1 = { (decimal)6.2610505E-2, (decimal)5.2837393E-2, (decimal)6.3721774E-2, (decimal)6.3824265E-2, (decimal)6.4258455E-2 } ;
            decimal[] clist2 = { (decimal)0.0, (decimal)9.0128400E-5, (decimal)4.3497037E-5, (decimal)8.4795348E-4, (decimal)1.7037206E-3 };
            decimal aht = (decimal)2.53E-5;
            decimal bht = (decimal)5.49E-3;
            decimal cht = (decimal)1.14E-3;

            if (L < 15)
            {
                ad = alist1[0] + alist2[0] * T;
                bd = blist1[0] + blist2[0] * T;
                cd = clist1[0] + clist2[0] * T;

            }
            if (L >= 15 && L < 30)
            {
                ad = alist1[0] + (alist1[1] - alist1[0]) * (L - 15) / 15 + alist2[0] + (alist2[1] - alist2[0]) * (L - 15) / 15 * T;
                bd = blist1[0] + (blist1[1] - blist1[0]) * (L - 15) / 15 + blist2[0] + (blist2[1] - blist2[0]) * (L - 15) / 15 * T;
                cd = clist1[0] + (clist1[1] - clist1[0]) * (L - 15) / 15 + clist2[0] + (clist2[1] - clist2[0]) * (L - 15) / 15 * T;
            }
            if (L >= 30 && L < 45)
            {
                ad = alist2[1] + (alist2[2] - alist2[1]) * (L - 30) / 15 + alist2[1] + (alist2[2] - alist2[1]) * (L - 30) / 15 * T;
                bd = blist2[1] + (blist2[2] - blist2[1]) * (L - 30) / 15 + blist2[1] + (blist2[2] - blist2[1]) * (L - 30) / 15 * T;
                cd = clist2[1] + (clist2[2] - clist2[1]) * (L - 30) / 15 + clist2[1] + (clist2[2] - clist2[1]) * (L - 30) / 15 * T;
            }
            if (L >= 45 && L < 60)
            {
                ad = alist1[2] + (alist1[3] - alist1[2]) * (L - 45) / 15 + alist2[2] + (alist2[3] - alist2[2]) * (L - 45) / 15 * T;
                bd = blist1[2] + (blist1[3] - blist1[2]) * (L - 45) / 15 + blist2[2] + (blist2[3] - blist2[2]) * (L - 45) / 15 * T;
                cd = clist1[2] + (clist1[3] - clist1[2]) * (L - 45) / 15 + clist2[2] + (clist2[3] - clist2[2]) * (L - 45) / 15 * T;
            }
            if (L >= 60 && L <= 75)
            {
                ad = alist1[3] + (alist1[4] - alist1[3]) * (L - 60) / 15 + alist2[3] + (alist2[4] - alist2[3]) * (L - 60) / 15 * T;
                bd = blist1[3] + (blist1[4] - blist1[3]) * (L - 60) / 15 + blist2[3] + (blist2[4] - blist2[3]) * (L - 60) / 15 * T;
                cd = clist1[3] + (clist1[4] - clist1[3]) * (L - 60) / 15 + clist2[3] + (clist2[4] - clist2[3]) * (L - 60) / 15 * T;
            }
            if (L > 75)
            {
                ad = alist1[4] + alist2[4] * T;
                bd = blist1[4] + blist2[4] * T;
                cd = clist1[4] + clist2[4] * T;
            }

            decimal above1,above2, under1,under2;//分子分母12
            decimal e = (decimal)Math.Sin((double)E / 180.0 * Math.PI);
            above1 = 1 / (1 + (ad / (1 + (bd / (1 + cd)))));
            above2 = 1 / (1 + (aht / (1 + (bht / (1 + cht)))));
            under1 = 1 / (e + (ad / (e + (bd / (e + cd)))));
            under2 = 1 / (e + (aht / (e + (bht / (e + cht)))));
            md = under1 / above1 + (1 / e - under2 / above2) * H / 1000;
        }
        private void Caldays()
        {
            int[] list = { 0, 31, 58, 89, 119,150,180,211,242,272,303,333 };
            days = list[month - 1] + day;
        }
    }
}