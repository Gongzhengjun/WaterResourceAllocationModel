using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
    public class IrrigationSystemDAL
    {
        /// <summary>
        /// 获取代表站灌溉制度
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByYear(int Year)
        {
            string strSql = $"select * from I代表站灌溉制度 where 年 = {Year} order by 灌溉制度编号,年,历时 asc";
            return DbHelper.ExecuteDataTable(strSql);
        }
    }
}
