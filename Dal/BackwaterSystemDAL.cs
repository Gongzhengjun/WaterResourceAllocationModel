using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
    public class BackwaterSystemDAL
    {
        /// <summary>
        /// 获取退水系统信息表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            string strSql = "select * from R退水系统信息表";
            return DbHelper.ExecuteDataTable(strSql);
        }
        /// <summary>
        /// 获取退水系统信息表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableSortNumber()
        {
            string strSql = "select * from R退水系统信息表 order by 退水点编号 asc";
            return DbHelper.ExecuteDataTable(strSql);
        }
    }
}
