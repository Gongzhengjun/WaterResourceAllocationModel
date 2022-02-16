using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
    public class RiverDiversionRatioDAL
    {
        /// <summary>
        /// 获取河道引水开发利用率及向计算单元分水比例
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            string strSql = "select * from I河道引水开发利用率及向计算单元分水比例 where 历时 = 1 order by 引提水点编号 asc";
            return DbHelper.ExecuteDataTable(strSql);
        }
        /// <summary>
        /// 获取河道引水开发利用率及向计算单元分水比例
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByYear(int Year)
        {
            string strSql = $"select * from I河道引水开发利用率及向计算单元分水比例 where 年 = {Year} order by 引提水点编号,月旬号 asc";
            return DbHelper.ExecuteDataTable(strSql);
        }
    }
}
