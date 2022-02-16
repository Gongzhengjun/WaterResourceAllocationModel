using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RiverDiversionRatioBLL
    {
        private RiverDiversionRatioDAL dal = new RiverDiversionRatioDAL();
        /// <summary>
        /// 获取河道引水开发利用率及向计算单元分水比例
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            return dal.GetTable();
        }
        /// <summary>
        /// 获取河道引水开发利用率及向计算单元分水比例
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByYear(int Year)
        {
            return dal.GetTableByYear(Year);
        }
    }
}
