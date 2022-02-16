using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public class RegressionCoefficientBLL
    {
        private RegressionCoefficientDAL dal = new RegressionCoefficientDAL();
        /// <summary>
        /// 获取代表站灌溉制度
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByYear(int Year)
        {
            return dal.GetTableByYear(Year);
        }
    }
}
