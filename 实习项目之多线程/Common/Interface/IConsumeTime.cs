using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 实习项目之多线程.Common.Interface
{
    public interface IConsumeTime
    {
        //消耗时间的doWork
        //seconds:消耗的时间，单位秒
        void doWork(double seconds);
    }
}
