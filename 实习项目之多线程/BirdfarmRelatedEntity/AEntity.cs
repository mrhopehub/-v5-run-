using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 实习项目之多线程.BirdfarmRelatedEntity
{
    public class AEntity
    {
        protected string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        
        protected string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        protected DateTime productiondate;

        public DateTime Productiondate
        {
            get { return productiondate; }
            set { productiondate = value; }
        }
        protected string imgName;

        public string ImgName
        {
            get { return imgName; }
            set { imgName = value; }
        }
    }
}
