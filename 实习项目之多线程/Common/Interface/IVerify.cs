using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace 实习项目之多线程.Common.Interface
{
    public interface IVerify
    {
        //args用于传参
        bool Verify(string text, ArrayList args);
    }
}
