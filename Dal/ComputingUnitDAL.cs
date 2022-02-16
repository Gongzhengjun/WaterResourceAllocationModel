using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
    public class ComputingUnitDAL
    {
        /// <summary>
        /// 获取计算单元信息表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            string strSql = "select * from R计算单元信息表  order by 计算单元编号 asc";
            return DbHelper.ExecuteDataTable(strSql);
        }
    }
}
