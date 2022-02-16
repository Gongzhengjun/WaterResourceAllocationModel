using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
    public class WaterTransferProjectDAL
    {
        /// <summary>
        /// 获取调水工程规模
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByYear(int Year)
        {
            string strSql = $"select * from I调水工程规模 where 年 = {Year} order by 调入水点编号 ,历时 asc";
            return DbHelper.ExecuteDataTable(strSql);
        }
    }
}
