using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class WaterTransferProjectBLL
    {
        private WaterTransferProjectDAL dal = new WaterTransferProjectDAL();
        /// <summary>
        /// 获取调水工程规模
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByYear(int Year) 
        {
            return dal.GetTableByYear(Year);
        }
    }
}
