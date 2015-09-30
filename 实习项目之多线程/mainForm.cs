using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using 实习项目之多线程.BirdfarmRelatedEntity;
using 实习项目之多线程.Common;
using 实习项目之多线程.Common.Interface;
using 实习项目之多线程.Thread;

namespace 实习项目之多线程
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
            //去掉选项卡的片头
            this.tabControl1.Appearance = TabAppearance.FlatButtons;
            this.tabControl1.ItemSize = new Size(0, 1);
            this.tabControl1.SizeMode = TabSizeMode.Fixed;

            this.beach = new Beach();

            this.dataGridView1.DataSource = this.beach.BirdfarmTable1;
            this.setGridViewHeaderSize(this.dataGridView1);
            this.setGridViewHeaderEvent(this.dataGridView1);
            this.dataGridView1.Columns[1].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            this.dataGridView1.Columns[2].Visible = false;
            this.dataGridView2.DataSource = this.beach.ManSectionTable1;
            this.setGridViewHeaderSize(this.dataGridView2);
            this.setGridViewHeaderEvent(this.dataGridView2);
            this.dataGridView2.Columns[1].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            this.dataGridView2.Columns[2].Visible = false;
            this.dataGridView3.DataSource = this.beach.AirplaneTable1;
            this.setGridViewHeaderSize(this.dataGridView3);
            this.setGridViewHeaderEvent(this.dataGridView3);
            this.dataGridView3.Columns[1].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            this.dataGridView3.Columns[2].Visible = false;
            this.dataGridView4.DataSource = this.beach.NavalGunTable1;
            this.setGridViewHeaderSize(this.dataGridView4);
            this.setGridViewHeaderEvent(this.dataGridView4);
            this.dataGridView4.Columns[1].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
            this.dataGridView4.Columns[2].Visible = false;

            Secondarythread tmp = new Secondarythread(this, this.cmdSemaphore);
            tmp.createProductThreads();
            this.thread = new System.Threading.Thread(tmp.run);
            this.thread.IsBackground = true;
            this.thread.Start();
        }

        //要生产的航母数量
        private int _birdfarm_num;
        public Beach beach;
        private System.Threading.Thread thread;

        public int Birdfarm_num
        {
            get { return this._birdfarm_num; }
            set { this._birdfarm_num = value; }
        }
        public SemaphoreSlim cmdSemaphore = new SemaphoreSlim(0);
        private System.Collections.Queue cmdQueue = new System.Collections.Queue();

        //发送命令，注意queue的线程安全
        private void sendCommand(int command)
        {
            lock(this.cmdQueue.SyncRoot)
            {
                this.cmdQueue.Enqueue(command);
            }
            this.cmdSemaphore.Release();
        }
        //有辅助线程调用，用于获取命令，注意线程安全
        public int checkCommand()
        {
            lock (this.cmdQueue.SyncRoot)
            {
                return Convert.ToInt32(this.cmdQueue.Dequeue());
            }
        }
        public void updateEntity(AEntity entity)
        {
            this.beach.DeliveryProduct(entity);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab != this.tabPage1)
            {
                this.tabControl1.SelectedTab = this.tabPage1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab != this.tabPage2)
            {
                this.tabControl1.SelectedTab = this.tabPage2;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab != this.tabPage3)
            {
                this.tabControl1.SelectedTab = this.tabPage3;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab != this.tabPage4)
            {
                this.tabControl1.SelectedTab = this.tabPage4;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab != this.tabPage5)
            {
                this.tabControl1.SelectedTab = this.tabPage5;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string rule = @"^[1-9]\d{0,2}$|^$";
            string tip = "请输入1000（不包括1000）以内的整数";
            IVerify verify = new RegularExpVerify(rule,tip);
            ArrayList verifyArgs = new ArrayList();
            verifyArgs.Add(sender);
            verifyArgs.Add(e);
            verifyArgs.Add(this.errorProvider1);
            verify.Verify((sender as TextBox).Text, verifyArgs);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (this.errorProvider1.GetError(this.textBox1) != string.Empty)
            {
                MessageBox.Show("输出错误，请仔细检查");
            }
            else
            {
                this._birdfarm_num = Convert.ToInt32(this.textBox1.Text);
                this.sendCommand(0);
                this.tabControl1.SelectedTab = this.tabPage2;
                this.button1.Enabled = false;
            }
        }
        //去掉点击标题排序的功能
        private void setGridViewHeaderEvent(DataGridView gridView)
        {
            for (int i = 0; i < gridView.Columns.Count; i++)
            {
                gridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        //单元格宽度自适应
        private void setGridViewHeaderSize(DataGridView gridView)
        {
            //禁止标题自动换行
            gridView.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            for (int i = 0; i < gridView.Columns.Count; i++)
            {
                gridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }
    }
}
