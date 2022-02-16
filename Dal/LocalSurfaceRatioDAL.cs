using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
   public class LocalSurfaceRatioDAL
    {
        /// <summary>
        /// 获取I本地地表径流可利用量优先序分水比例
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByYear(int Year)
        {
            string strSql = $"select * from I本地地表径流可利用量优先序分水比例 where 年 = {Year} order by 计算单元编号,历时 asc";
            return DbHelper.ExecuteDataTable(strSql);
        }
    }
}
