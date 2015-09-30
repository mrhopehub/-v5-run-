using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace 实习项目之多线程.ImgHelper
{
    public static class addStrToImg
    {
        private static Font _font = new Font("Times New Roman", 30, FontStyle.Bold);
        //添加字符串到图片，默认字体为"Times New Roman", 30, FontStyle.Bold
        public static void addStr(Image img, string text, Point postion)
        {
            Graphics g = Graphics.FromImage(img);
            SolidBrush sbrush = new SolidBrush(Color.Red);
            g.DrawString(text, _font, sbrush, postion);
            g.Flush();
            g.Dispose();
        }
        //添加字符串到图片，指定字体
        public static void addStr(Image img, string text, Point postion, Font font)
        {
            Graphics g = Graphics.FromImage(img);
            SolidBrush sbrush = new SolidBrush(Color.Red);
            g.DrawString(text, font, sbrush, postion);
            g.Flush();
            g.Dispose();
        }
    }
}
