using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RiverInformationBLL
    {
        private RiverInformationDAL dal = new RiverInformationDAL();
        /// <summary>
        /// 获取河流信息表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable() 
        {
            return dal.GetTable();
        }
    }
}
