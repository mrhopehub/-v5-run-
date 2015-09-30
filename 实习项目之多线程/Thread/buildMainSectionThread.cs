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
    public class buildMainSectionThread
    {
        private IConsumeTime consumeTime = new consumeByCheckNow();
        private delegate void updateMainSection(AEntity entity);
        private mainForm form;
        private MainSection _mainSection;
        private int _mainSection_Num;

        public int MainSection_Num
        {
            get { return _mainSection_Num; }
            set { _mainSection_Num = value; }
        }
        private SemaphoreSlim _productSem;

        public SemaphoreSlim ProductSem
        {
            get { return _productSem; }
            set { _productSem = value; }
        }
        private SemaphoreSlim _mainSectionSem;

        public SemaphoreSlim MainSectionSem
        {
            get { return _mainSectionSem; }
            set { _mainSectionSem = value; }
        }
        private System.Collections.Queue _mainSectionQueue;

        public System.Collections.Queue MainSectionQueue
        {
            get { return _mainSectionQueue; }
            set { _mainSectionQueue = value; }
        }
        public buildMainSectionThread(mainForm form)
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
                for (int i = 0; i < this._mainSection_Num; i++)
                {
                    this.consumeTime.doWork(3);
                    //System.Windows.Forms.MessageBox.Show("舰体线程启动成功");
                    string modelPath = ConfigurationManager.AppSettings["modelPath"] + @"mainSection.png";
                    Image model = Image.FromFile(modelPath);
                    Image mainSectionImg = (Image)model.Clone();
                    model.Dispose();
                    _mainSection = new MainSection();
                    name = @"舰体" + i.ToString().PadLeft(2, '0');
                    imgName = ConfigurationManager.AppSettings["mainSectionPath"] + name + @".png";
                    addStrToImg.addStr(mainSectionImg, i.ToString().PadLeft(2, '0'),pos);
                    mainSectionImg.Save(imgName);

                    //生产mainSection
                    this._mainSection.Name = name;
                    this._mainSection.Productiondate = DateTime.Now;
                    this._mainSection.ImgName = imgName;

                    //加入队列
                    lock (this._mainSectionQueue.SyncRoot)
                    {
                        this._mainSectionQueue.Enqueue(this._mainSection);
                    }

                    //通知拼接线程
                    this._mainSectionSem.Release();

                    //更新界面
                    this.form.BeginInvoke(new updateMainSection(this.form.updateEntity),this._mainSection);
                }
            }
        }
    }
}