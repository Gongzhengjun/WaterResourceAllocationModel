using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
    public class NodeInformationDAL
    {
        /// <summary>
        /// 获取节点信息表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            string strSql = "select * from R节点信息表 order by 所有节点总编号 asc";
            return DbHelper.ExecuteDataTable(strSql);
        }

        /// <summary>
        /// 根据河流编号获取节点信息表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByRiverNumber(int RiverNumber) 
        {
            string strSql = $"select * from R节点信息表 where 河流编号 = {RiverNumber} order by 节点编号 asc";
            return DbHelper.ExecuteDataTable(strSql);
        }
    }
}
