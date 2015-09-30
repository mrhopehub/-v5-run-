using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Collections;
using 实习项目之多线程.BirdfarmRelatedEntity;
using 实习项目之多线程.ImgHelper;
using System.Configuration;
using 实习项目之多线程.Common.Interface;
using 实习项目之多线程.Common;

namespace 实习项目之多线程.Thread
{
    public class buildBirdfarmThread
    {
        private IConsumeTime consumeTime = new consumeByCheckNow();
        private delegate void updateBirdfarm(AEntity entity);
        private Birdfarm _birdfarm = new Birdfarm();
        private mainForm form;
        private int _birdfarm_num;

        public int Birdfarm_num
        {
            get { return _birdfarm_num; }
            set { _birdfarm_num = value; }
        }

        private System.Collections.Queue _MainSectionQueue;

        public System.Collections.Queue MainSectionQueue
        {
            get { return _MainSectionQueue; }
            set { _MainSectionQueue = value; }
        }
        private System.Collections.Queue _AirplaneQueue;

        public System.Collections.Queue AirplaneQueue
        {
            get { return _AirplaneQueue; }
            set { _AirplaneQueue = value; }
        }
        private System.Collections.Queue _NavalGunQueue;

        public System.Collections.Queue NavalGunQueue
        {
            get { return _NavalGunQueue; }
            set { _NavalGunQueue = value; }
        }
        private SemaphoreSlim mainSectionSem;

        public SemaphoreSlim MainSectionSem
        {
            get { return mainSectionSem; }
            set { mainSectionSem = value; }
        }
        private SemaphoreSlim airplaneSem;

        public SemaphoreSlim AirplaneSem
        {
            get { return airplaneSem; }
            set { airplaneSem = value; }
        }
        private SemaphoreSlim navalGunSem;

        public SemaphoreSlim NavalGunSem
        {
            get { return navalGunSem; }
            set { navalGunSem = value; }
        }
        private SemaphoreSlim _productSem;

        public SemaphoreSlim ProductSem
        {
            get { return _productSem; }
            set { _productSem = value; }
        }
        public buildBirdfarmThread(mainForm form)
        {
            this.form = form;
        }
        public void run()
        {
            //System.Windows.Forms.MessageBox.Show("航母线程启动成功");
            Image birdfarmImg;
            Image mainSectionImg;
            Image airPlaneImg;
            Image navalGunImg;
            MainSection mainSection;
            Airplane airplane;
            NavalGun navalGun;
            string name;
            string imgName;
            int tmpx, tmpy;
            tmpx = Convert.ToInt32(ConfigurationManager.AppSettings["airplaneX"]);
            tmpy = Convert.ToInt32(ConfigurationManager.AppSettings["airplaneY"]);
            Point airplanepos = new Point(tmpx, tmpy);
            tmpx = Convert.ToInt32(ConfigurationManager.AppSettings["navalGunX"]);
            tmpy = Convert.ToInt32(ConfigurationManager.AppSettings["navalGunY"]);
            Point navalpos = new Point(tmpx, tmpy);
            while (true)
            {
                //等待辅助进程法令
                this._productSem.Wait();
                for (int i = 0; i < this._birdfarm_num; i++)
                {
                    this.consumeTime.doWork(4);
                    name = @"航空母舰" + i.ToString().PadLeft(2, '0');
                    imgName = ConfigurationManager.AppSettings["birdfarmPath"] + name + @".png";
                    /**********************************Start获取部件*********************************/
                    this.mainSectionSem.Wait();
                    lock (this._MainSectionQueue.SyncRoot)
                    {
                        mainSection = (MainSection)this._MainSectionQueue.Dequeue();
                    }
                    this.airplaneSem.Wait();
                    lock (this._AirplaneQueue.SyncRoot)
                    {
                        airplane = (Airplane)this._AirplaneQueue.Dequeue();
                    }
                    this.navalGunSem.Wait();
                    lock (this._NavalGunQueue.SyncRoot)
                    {
                        navalGun = (NavalGun)this._NavalGunQueue.Dequeue();
                    }
                    /**********************************End获取部件*********************************/
                    //获取图片
                    mainSectionImg = Image.FromFile(mainSection.ImgName);
                    airPlaneImg = Image.FromFile(airplane.ImgName);
                    navalGunImg = Image.FromFile(navalGun.ImgName);

                    //拼接图片
                    birdfarmImg = (Image)mainSectionImg.Clone();
                    SuperimposeImage.Superimpose(birdfarmImg, airPlaneImg, airplanepos);
                    SuperimposeImage.Superimpose(birdfarmImg, navalGunImg, navalpos);
                    birdfarmImg.Save(imgName);
                    //释放图片
                    mainSectionImg.Dispose();
                    airPlaneImg.Dispose();
                    navalGunImg.Dispose();

                    //拼接航母
                    this._birdfarm.Name = name;
                    this._birdfarm.Productiondate = DateTime.Now;
                    this._birdfarm.ImgName = imgName;

                    //更新界面
                    this.form.BeginInvoke(new updateBirdfarm(this.form.updateEntity), this._birdfarm);
                }
                System.Windows.Forms.MessageBox.Show("任务完成");
            }
        }
    }
}
