using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using 实习项目之多线程.Common.Interface;

namespace 实习项目之多线程.Common
{
    public class consumeByCheckNow : IConsumeTime
    {
        public void doWork(double seconds)
        {
            if (seconds > 0)
            {
                int mSecond = Convert.ToInt32(seconds) * 1000;
                DateTime over = DateTime.Now.AddMilliseconds(mSecond);
                while (DateTime.Now < over)
                {
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
        }
    }
}
