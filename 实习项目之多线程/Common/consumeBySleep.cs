using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using 实习项目之多线程.Common.Interface;

namespace 实习项目之多线程.Common
{
    public class consumeBySleep : IConsumeTime
    {
        public void doWork(double seconds)
        {
            if (seconds > 0)
            {
                int mSecond = Convert.ToInt32(seconds) * 1000;
                Console.WriteLine("Sleeping...");
                System.Threading.Thread.Sleep(mSecond);
            }
        }
    }
}
