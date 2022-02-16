using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
    public class RiverInformationDAL
    {
        /// <summary>
        /// 获取河流信息表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            string strSql = "select * from R河流信息表  order by  河道等级 desc,编号 asc";
            return DbHelper.ExecuteDataTable(strSql);
        }
    }
}
