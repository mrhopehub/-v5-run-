using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using 实习项目之多线程.BirdfarmRelatedEntity;

namespace 实习项目之多线程.BirdfarmRelatedEntity
{
    //海滩，用于存放航母、部件
    public class Beach
    {
        //存储完成的航母
        private DataTable BirdfarmTable;

        public DataTable BirdfarmTable1
        {
            get { return BirdfarmTable; }
            set { BirdfarmTable = value; }
        }
        //存储完成的舰体部分
        private DataTable ManSectionTable;

        public DataTable ManSectionTable1
        {
            get { return ManSectionTable; }
            set { ManSectionTable = value; }
        }
        //存储完成的飞机部分
        private DataTable AirplaneTable;

        public DataTable AirplaneTable1
        {
            get { return AirplaneTable; }
            set { AirplaneTable = value; }
        }
        //存储完成的舰炮部分
        private DataTable NavalGunTable;

        public DataTable NavalGunTable1
        {
            get { return NavalGunTable; }
            set { NavalGunTable = value; }
        }
        public Beach()
        {
            DataColumn dc = null;
            this.BirdfarmTable = new DataTable();
            dc = this.BirdfarmTable.Columns.Add("航母简称", Type.GetType("System.String"));
            dc = this.BirdfarmTable.Columns.Add("生产日期", Type.GetType("System.DateTime"));
            dc = this.BirdfarmTable.Columns.Add("图像路径", Type.GetType("System.String"));
            this.ManSectionTable = new DataTable();
            dc = this.ManSectionTable.Columns.Add("舰体简称", Type.GetType("System.String"));
            dc = this.ManSectionTable.Columns.Add("生产日期", Type.GetType("System.DateTime"));
            dc = this.ManSectionTable.Columns.Add("图像路径", Type.GetType("System.String"));
            this.AirplaneTable = new DataTable();
            dc = this.AirplaneTable.Columns.Add("飞机简称", Type.GetType("System.String"));
            dc = this.AirplaneTable.Columns.Add("生产日期", Type.GetType("System.DateTime"));
            dc = this.AirplaneTable.Columns.Add("图像路径", Type.GetType("System.String"));
            this.NavalGunTable = new DataTable();
            dc = this.NavalGunTable.Columns.Add("舰炮简称", Type.GetType("System.String"));
            dc = this.NavalGunTable.Columns.Add("生产日期", Type.GetType("System.DateTime"));
            dc = this.NavalGunTable.Columns.Add("图像路径", Type.GetType("System.String"));
        }
        public void DeliveryProduct(AEntity entity)
        {
            DataRow row;
            switch (entity.Type)
            {
                case "航母":
                    row = this.BirdfarmTable.NewRow();
                    row["航母简称"] = entity.Name;
                    row["生产日期"] = entity.Productiondate;
                    row["图像路径"] = entity.ImgName;
                    this.BirdfarmTable.Rows.Add(row);
                    break;
                case "舰体":
                    row = this.ManSectionTable.NewRow();
                    row["舰体简称"] = entity.Name;
                    row["生产日期"] = entity.Productiondate;
                    row["图像路径"] = entity.ImgName;
                    this.ManSectionTable.Rows.Add(row);
                    break;
                case "飞机":
                    row = this.AirplaneTable.NewRow();
                    row["飞机简称"] = entity.Name;
                    row["生产日期"] = entity.Productiondate;
                    row["图像路径"] = entity.ImgName;
                    this.AirplaneTable.Rows.Add(row);
                    break;
                case "舰炮":
                    row = this.NavalGunTable.NewRow();
                    row["舰炮简称"] = entity.Name;
                    row["生产日期"] = entity.Productiondate;
                    row["图像路径"] = entity.ImgName;
                    this.NavalGunTable.Rows.Add(row);
                    break;
            }
        }
    }
}
