using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
    public class SystemDAL
    {
        /// <summary>
        /// 获取系统信息表实体
        /// </summary>
        /// <returns></returns>
        public SystemInfo GetEntity()
        {
            string strSql = "SELECT ID1,ID,地级区数目 as PrefectureNumber,工程分区数目 as ProjectNumber,计算单元数目 as CalculationtNumber,计算周期_年 as Years,起始年份 as StartYear,省级区数目 as ProvincialNumber,省级区数目 as CountyNumber,用户数目 as UserNumber,月旬数 as MonthlyNumber,用户1名称 as UserName1,用户2名称 as UserName2,用户3名称 as UserName3,用户4名称 as UserName4,用户5名称 as UserName5,用户6名称 as UserName6    FROM R系统信息表";
            return DbHelper.ExecuteScalar<SystemInfo>(strSql);
        }
    }
}
