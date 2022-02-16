using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RiverInitialWaterBLL
    {
        private RiverInitialWaterDAL dal = new RiverInitialWaterDAL();
        /// <summary>
        /// 获取灌区地下水年可开采量
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByYear(int Year)
        {
            return dal.GetTableByYear(Year);
        }
    }
}
