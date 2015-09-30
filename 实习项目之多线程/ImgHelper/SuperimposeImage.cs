using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace 实习项目之多线程.ImgHelper
{
    public static class SuperimposeImage
    {
        //把fore图片叠加到back图片的postion位置
        public static void Superimpose(Image back, Image fore, Point postion)
        {
            using (Graphics g = Graphics.FromImage(back))
            {
                //draw other image on top of main Image
                g.DrawImage(fore, postion);
            }
        }
    }
}
