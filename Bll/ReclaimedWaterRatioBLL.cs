using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ReclaimedWaterRatioBLL
    {
        private ReclaimedWaterRatioDAL dal = new ReclaimedWaterRatioDAL();
        /// <summary>
        /// 获取I再生水可利用量优先序分水比例
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByYear(int Year) 
        {
            return dal.GetTableByYear(Year);
        }
    }
}
