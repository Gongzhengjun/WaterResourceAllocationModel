using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
    public class RegressionCoefficientDAL
    {
        /// <summary>
        /// 获取代表站灌溉制度
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByYear(int Year)
        {
            string strSql = $"select * from I计算单元退水系数 where 年 = {Year} order by 计算单元编号,历时 asc";
            return DbHelper.ExecuteDataTable(strSql);
        }
    }
}
