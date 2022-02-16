using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public class RiverDiversionOrderRatioBLL
    {
        private RiverDiversionOrderRatioDAL dal = new RiverDiversionOrderRatioDAL();
        /// <summary>
        /// 获取河道引水可利用量优先序分水比例
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByYear(int Year)
        {
            return dal.GetTableByYear(Year);
        }
    }
}
