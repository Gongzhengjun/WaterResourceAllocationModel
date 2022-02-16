using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
    public class ShippingFlowDAL
    {
        /// <summary>
        /// 删除表数据
        /// </summary>
        /// <returns></returns>
        public bool DeleteALL(OleDbTransaction? trans = null)
        {
            string strSql = $"delete from O33航运流量保证率";
            return DbHelper.ExecuteNonQuery(strSql, trans) > 0;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public bool Increase(int RiverNumber, string RiverName,int NodeNumber,string NodeName,string HangyunPtime,string HangyunPyear, OleDbTransaction? trans = null)
        {
            string strSql = $" INSERT INTO O33航运流量保证率(河流编号,河流名称,节点编号,节点名称,历时保证率,年保证率) VALUES ({RiverNumber},'{RiverName}',{NodeNumber},'{NodeName}','{HangyunPtime}','{HangyunPyear}')";
            return DbHelper.ExecuteNonQuery(strSql, trans) > 0;
        }
    }
}
