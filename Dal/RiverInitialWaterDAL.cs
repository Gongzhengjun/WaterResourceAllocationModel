using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
   public class RiverInitialWaterDAL
    {
        /// <summary>
        /// 获取灌区地下水年可开采量
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByYear(int Year)
        {
            string strSql = $"select * from I河流初始水量  where 年 = {Year} order by 河流编号,历时 asc";
            return DbHelper.ExecuteDataTable(strSql);
        }
    }
}
