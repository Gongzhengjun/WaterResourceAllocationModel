using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public  class BackwaterSystemBLL
    {
        private BackwaterSystemDAL dal = new BackwaterSystemDAL();
        /// <summary>
        /// 获取退水系统信息表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            return dal.GetTable();
        }
        /// <summary>
        /// 获取退水系统信息表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableSortNumber()
        {
            return dal.GetTableSortNumber();
        }
    }
}
