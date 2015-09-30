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
    public class buildAirplaneThread
    {
        private IConsumeTime consumeTime = new consumeByCheckNow();
        private delegate void updateAirplane(AEntity entity);
        private mainForm form;
        private Airplane _airplane;

        private int _airplane_Num;

        public int Airplane_Num
        {
            get { return _airplane_Num; }
            set { _airplane_Num = value; }
        }
        private SemaphoreSlim _productSem;

        public SemaphoreSlim ProductSem
        {
            get { return _productSem; }
            set { _productSem = value; }
        }
        private SemaphoreSlim _airplaneSem;

        public SemaphoreSlim AirplaneSem
        {
            get { return _airplaneSem; }
            set { _airplaneSem = value; }
        }
        private System.Collections.Queue _airplaneQueue;

        public System.Collections.Queue AirplaneQueue
        {
            get { return _airplaneQueue; }
            set { _airplaneQueue = value; }
        }
        public buildAirplaneThread(mainForm form)
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
                for (int i = 0; i < this._airplane_Num; i++)
                {
                    this.consumeTime.doWork(2);
                    //System.Windows.Forms.MessageBox.Show("飞机线程启动成功");
                    string modelPath = ConfigurationManager.AppSettings["modelPath"] + @"airplane.png";
                    Image model = Image.FromFile(modelPath);
                    Image airplaneImg = (Image)model.Clone();
                    model.Dispose();
                    this._airplane = new Airplane();
                    name = @"飞机" + i.ToString().PadLeft(2, '0');
                    imgName = ConfigurationManager.AppSettings["airplanePath"] + name + @".png";
                    addStrToImg.addStr(airplaneImg, i.ToString().PadLeft(2, '0'), pos);
                    airplaneImg.Save(imgName);

                    //生产mainSection
                    this._airplane.Name = name;
                    this._airplane.Productiondate = DateTime.Now;
                    this._airplane.ImgName = imgName;

                    //加入队列
                    lock (this._airplaneQueue.SyncRoot)
                    {
                        this._airplaneQueue.Enqueue(this._airplane);
                    }

                    //通知拼接线程
                    this._airplaneSem.Release();

                    //更新界面
                    this.form.BeginInvoke(new updateAirplane(this.form.updateEntity), this._airplane);
                }
            }
        }
    }
}
