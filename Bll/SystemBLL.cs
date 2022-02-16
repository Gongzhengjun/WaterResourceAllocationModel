using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SystemBLL
    {
        private SystemDAL dal = new SystemDAL();
        /// <summary>
        /// 获取系统信息表实体
        /// </summary>
        /// <returns></returns>
        public SystemInfo GetEntity() 
        {
            return dal.GetEntity();
        }
    }
}
