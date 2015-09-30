using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using 实习项目之多线程.Common.Interface;
using System.Windows.Forms;

namespace 实习项目之多线程.Common
{
    //按照_rule正则表达式验证TextBox的内容，如错误会用args[2]（ErrorProvider）提示错误
    //args:object sender, EventArgs e, ErrorProvider errorProvider1
    class RegularExpVerify : IVerify
    {
        //正则表达式
        private string _rule;
        //验证失败提示信息
        private string _tip;
        public RegularExpVerify(string rule, string tip)
        {
            this._rule = rule;
            this._tip = tip;
        }
        public bool Verify(string text, ArrayList args)
        {
            Regex regex = new Regex(this._rule);
            object sender = args[0];
            ErrorProvider errorProvider = (ErrorProvider)args[2];
            if (regex.IsMatch(text))
            {
                errorProvider.SetError((sender as TextBox), String.Empty);
                return true;
            }
            else
            {
                errorProvider.SetError((sender as TextBox), this._tip);
                return false;
            }
        }
    }
}
