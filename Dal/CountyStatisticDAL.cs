using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
    public class CountyStatisticDAL
    {
        /// <summary>
        /// 删除表数据
        /// </summary>
        /// <returns></returns>
        public bool DeleteALL(OleDbTransaction? trans = null)
        {
            string strSql = $"delete from O13县级区供需平衡结果表";
            return DbHelper.ExecuteNonQuery(strSql, trans) > 0;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public bool Increase(string strSql, OleDbTransaction? trans = null)
        {
            return DbHelper.ExecuteNonQuery(strSql, trans) > 0;
        }
    }
}
