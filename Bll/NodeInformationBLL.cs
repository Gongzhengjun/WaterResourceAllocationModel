using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class NodeInformationBLL
    {
        private NodeInformationDAL dal = new NodeInformationDAL();
        /// <summary>
        /// 获取节点信息表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable() 
        {
            return dal.GetTable();
        }
        /// <summary>
        /// 根据河流编号获取节点信息表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTableByRiverNumber(int RiverNumber) 
        {
            return dal.GetTableByRiverNumber(RiverNumber);
        }
    }
}
