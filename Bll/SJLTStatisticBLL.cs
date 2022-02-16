using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace BLL
{
    public class SJLTStatisticBLL
    {
        private SJLTStatisticDAL dal = new SJLTStatisticDAL();
        /// <summary>
        /// 016 三江连通工程区 统计输出
        /// </summary>
        /// <returns></returns>
        public bool Add(Common com)
        {
            var result = false;
            using (var trans = DbHelper.BeginTransaction())
            {
                try
                {
                    result = dal.DeleteALL(trans);
                    if (result)
                    {
                        for (int year = 1; year < com.Years; year++)
                        {

                            for (int time = 1; time < com.Times; time++) //只统计5-8月
                            {
                                List<string> strField = new List<string>();
                                List<string> strValues = new List<string>();
                                for (int jj = 1; jj < com.Users; jj++)
                                {
                                    string Field = com.Users_Name[jj] + "缺水量";
                                    strField.Add(Field);
                                    string Value = Math.Round(com.water_usershort[year, time, jj], 2).ToString();
                                    strValues.Add(Value);
                                    Field = com.Users_Name[jj] + "缺水率";
                                    strField.Add(Field);
                                    Value = $"'{string.Format("{0:P}", com.water_usershortR[year, time, jj])}'";
                                    strValues.Add(Value);
                                }
                                string strSql = $" INSERT INTO O16三江连通工程区供需平衡结果表(名称,年,月旬号,历时,月旬,总需水,总供水,总缺水量,本地地表径流供水,再生水供水,河道引提水供水,地下水供水,界河水供水,综合平均缺水率,{string.Join(",", strField)}) VALUES ('三江连通工程区',{(year + com.First_Year - 1)},{time},'{time + (year - 1) * com.YueXuns}','{com.YueXun[time]}',{Math.Round(com.SJLT_waterneed[year, time], 2)},{Math.Round(com.SJLT_watersupply[year, time], 2)},{Math.Round(com.SJLT_watershortage[year, time], 2)},{Math.Round(com.locatedwater_SJLT_supply[year, time], 2)},{Math.Round(com.recycledwater_SJLT_supply[year, time], 2)},{Math.Round(com.riverwater_SJLT_supply[year, time], 2)},{Math.Round(com.groundwater_SJLT_supply[year, time], 2)},{Math.Round(com.boundaryriver_SJLT_supply[year, time], 2)},'{string.Format("{0:P}", com.water_usershortR[year, time, com.Users])}',{string.Join(",", strValues)})";
                                result = dal.Increase(strSql, trans);
                                if (!result)
                                {
                                    goto result;
                                }
                            }
                        }


                    }
                result:
                    if (result)
                    {
                        trans.Commit();
                    }
                    else
                    {
                        trans.Rollback();
                    }

                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }

            }
            return result;
        }
    }
}
