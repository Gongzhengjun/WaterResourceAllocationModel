using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ComputingUnitBLL
    {
        private ComputingUnitDAL dal = new ComputingUnitDAL();
        /// <summary>
        /// 获取计算单元信息表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable() 
        {
            return dal.GetTable();
        }
    }
}
