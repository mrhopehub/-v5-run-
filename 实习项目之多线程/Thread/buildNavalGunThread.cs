using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using 实习项目之多线程.BirdfarmRelatedEntity;
using 实习项目之多线程.ImgHelper;
using System.Configuration;
using 实习项目之多线程.Common.Interface;
using 实习项目之多线程.Common;

namespace 实习项目之多线程.Thread
{
    public class buildNavalGunThread
    {
        private IConsumeTime consumeTime = new consumeByCheckNow();
        private delegate void updateNavalGun(AEntity entity);
        private mainForm form;
        private NavalGun _navalGun;

        private int _navalGun_Num;

        public int NavalGun_Num
        {
            get { return _navalGun_Num; }
            set { _navalGun_Num = value; }
        }
        private SemaphoreSlim _productSem;

        public SemaphoreSlim ProductSem
        {
            get { return _productSem; }
            set { _productSem = value; }
        }
        private SemaphoreSlim _navalGunSem;

        public SemaphoreSlim NavalGunSem
        {
            get { return _navalGunSem; }
            set { _navalGunSem = value; }
        }
        private System.Collections.Queue _navalGunQueue;

        public System.Collections.Queue NavalGunQueue
        {
            get { return _navalGunQueue; }
            set { _navalGunQueue = value; }
        }
        public buildNavalGunThread(mainForm form)
        {
            this.form = form;
        }
        public void run()
        {

            Point pos = new Point(0, 0);
            string name;
            string imgName;
            while (true)
            {
                this._productSem.Wait();
                for (int i = 0; i < this._navalGun_Num; i++)
                {
                    this.consumeTime.doWork(1);
                    //System.Windows.Forms.MessageBox.Show("舰炮线程启动成功");
                    string modelPath = ConfigurationManager.AppSettings["modelPath"] + @"navalGun.png";
                    Image model = Image.FromFile(modelPath);
                    Image navalGunImg = (Image)model.Clone();
                    model.Dispose();
                    _navalGun = new NavalGun();
                    name = @"舰炮" + i.ToString().PadLeft(2, '0');
                    imgName = ConfigurationManager.AppSettings["navalGunPath"] + name + @".png";
                    addStrToImg.addStr(navalGunImg, i.ToString().PadLeft(2, '0'), pos);
                    navalGunImg.Save(imgName);

                    //生产mainSection
                    this._navalGun.Name = name;
                    this._navalGun.Productiondate = DateTime.Now;
                    this._navalGun.ImgName = imgName;

                    //加入队列
                    lock (this._navalGunQueue.SyncRoot)
                    {
                        this._navalGunQueue.Enqueue(this._navalGun);
                    }

                    //通知拼接线程
                    this._navalGunSem.Release();

                    //更新界面
                    this.form.BeginInvoke(new updateNavalGun(this.form.updateEntity), this._navalGun);
                }
            }
        }
    }
}
