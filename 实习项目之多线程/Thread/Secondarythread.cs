using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace 实习项目之多线程.Thread
{
    //辅助线程，用于创建拼接航母、生产各个部件的线程
    //然后接受主线程命令，进而控制其他线程（辅助线程创建的线程）
    public class Secondarythread
    {
        private mainForm form;
        //同步信号
        public SemaphoreSlim cmdSemaphore;
        public SemaphoreSlim productBirdfarmSem;
        public SemaphoreSlim productMainSectionSem;
        public SemaphoreSlim productAirplaneSem;
        public SemaphoreSlim productNavalGunSem;

        public SemaphoreSlim mainSectionSem;
        public SemaphoreSlim airplaneSem;
        public SemaphoreSlim navalGunSem;

        //生产线程
        private System.Threading.Thread BirdfarmThread;
        private System.Threading.Thread ManSectionThread;
        private System.Threading.Thread AirplaneThread;
        private System.Threading.Thread NavalGunThread;

        private buildBirdfarmThread birdfarmThreadobj;
        private buildMainSectionThread mainSectionThreadobj;
        private buildAirplaneThread airplaneThreadobj;
        private buildNavalGunThread navalGunThreadobj;
        //entity队列
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
        public Secondarythread(mainForm form, SemaphoreSlim semaphore)
        {
            this.form = form;
            this.cmdSemaphore = semaphore;
            this.productBirdfarmSem = new SemaphoreSlim(0);
            this.productMainSectionSem = new SemaphoreSlim(0);
            this.productAirplaneSem = new SemaphoreSlim(0);
            this.productNavalGunSem = new SemaphoreSlim(0);

            this.mainSectionSem = new SemaphoreSlim(0);
            this.airplaneSem = new SemaphoreSlim(0);
            this.navalGunSem = new SemaphoreSlim(0);

            this._MainSectionQueue = new System.Collections.Queue();
            this._AirplaneQueue = new System.Collections.Queue();
            this._NavalGunQueue = new System.Collections.Queue();
        }
        //线程启动
        public void run()
        {
            while (true)
            {
                //等待命令
                this.cmdSemaphore.Wait();
                //命令
                switch (this.form.checkCommand())
                {
                    case 0://发布命令
                        this.birdfarmThreadobj.Birdfarm_num = this.form.Birdfarm_num;
                        this.mainSectionThreadobj.MainSection_Num = this.form.Birdfarm_num;
                        this.airplaneThreadobj.Airplane_Num = this.form.Birdfarm_num;
                        this.navalGunThreadobj.NavalGun_Num = this.form.Birdfarm_num;

                        this.productBirdfarmSem.Release();
                        this.productMainSectionSem.Release();
                        this.productAirplaneSem.Release();
                        this.productNavalGunSem.Release();

                        break;
                    case 1://开始生产航母命令
                        this.BirdfarmThread.Resume();
                        break;
                    case 2://暂停航母明林
                        this.BirdfarmThread.Suspend();
                        break;
                    case 3://开始所有部件命令
                        this.ManSectionThread.Resume();
                        this.AirplaneThread.Resume();
                        this.NavalGunThread.Resume();
                        break;
                    case 4://暂停所有部件命令
                        this.ManSectionThread.Suspend();
                        this.AirplaneThread.Suspend();
                        this.NavalGunThread.Suspend();
                        break;
                }
            }
        }
        public void createProductThreads()
        {
            this.birdfarmThreadobj = new buildBirdfarmThread(this.form);
            birdfarmThreadobj.ProductSem = this.productBirdfarmSem;
            birdfarmThreadobj.MainSectionSem = this.mainSectionSem;
            birdfarmThreadobj.AirplaneSem = this.airplaneSem;
            birdfarmThreadobj.NavalGunSem = this.navalGunSem;
            birdfarmThreadobj.MainSectionQueue = this._MainSectionQueue;
            birdfarmThreadobj.AirplaneQueue = this._AirplaneQueue;
            birdfarmThreadobj.NavalGunQueue = this._NavalGunQueue;
            this.BirdfarmThread = new System.Threading.Thread(birdfarmThreadobj.run);
            this.BirdfarmThread.IsBackground = true;
            this.BirdfarmThread.Start();

            this.mainSectionThreadobj = new buildMainSectionThread(this.form);
            mainSectionThreadobj.ProductSem = this.productMainSectionSem;
            this.mainSectionThreadobj.MainSectionSem = this.mainSectionSem;
            mainSectionThreadobj.MainSectionQueue = this._MainSectionQueue;
            this.ManSectionThread = new System.Threading.Thread(mainSectionThreadobj.run);
            this.ManSectionThread.IsBackground = true;
            this.ManSectionThread.Start();

            this.airplaneThreadobj = new buildAirplaneThread(this.form);
            airplaneThreadobj.ProductSem = this.productAirplaneSem;
            this.airplaneThreadobj.AirplaneSem = this.airplaneSem;
            airplaneThreadobj.AirplaneQueue = this._AirplaneQueue;
            this.AirplaneThread = new System.Threading.Thread(airplaneThreadobj.run);
            this.AirplaneThread.IsBackground = true;
            this.AirplaneThread.Start();

            this.navalGunThreadobj = new buildNavalGunThread(this.form);
            this.navalGunThreadobj.ProductSem = this.productNavalGunSem;
            this.navalGunThreadobj.NavalGunSem = this.navalGunSem;
            this.navalGunThreadobj.NavalGunQueue = this._NavalGunQueue;
            this.NavalGunThread = new System.Threading.Thread(navalGunThreadobj.run);
            this.NavalGunThread.IsBackground = true;
            this.NavalGunThread.Start();
        }
    }
}
