using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LocalSurfaceRatioBLL
    {
        private LocalSurfaceRatioDAL dal = new LocalSurfaceRatioDAL();
        /// <summary>
        /// 获取I本地地表径流可利用量优先序分水比例
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByYear(int Year)
        {
            return dal.GetTableByYear(Year);
        }
    }
}
