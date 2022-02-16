using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
    public class RiverDiversionOrderRatioDAL
    {
        /// <summary>
        /// 获取I再生水可利用量优先序分水比例
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByYear(int Year)
        {
            string strSql = $"select * from I河道引水可利用量优先序分水比例 where 年 = {Year} order by 计算单元编号,历时 asc";
            return DbHelper.ExecuteDataTable(strSql);
        }
    }
}
