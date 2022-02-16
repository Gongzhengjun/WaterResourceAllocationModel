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
    public class FenqusStatisticBLL
    {
        private FenqusStatisticDAL dal = new FenqusStatisticDAL();
        /// <summary>
        /// O12工程分区供需平衡结果表统计
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
                        for (int ii = 1; ii < com.Fenqus; ii++)
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
                                        string Value = Math.Round(com.fenqu_shortQ[year, time, ii, jj], 2).ToString();
                                        strValues.Add(Value);
                                        Field = com.Users_Name[jj] + "缺水率";
                                        strField.Add(Field);
                                        Value = $"'{string.Format("{0:P}", com.fenqu_shortR[year, time, ii, jj])}'";
                                        strValues.Add(Value);
                                    }
                                    if (com.locatedwater_unit_supply[year, time, ii] > 0)
                                    {
                                        string Field = "本地地表径流供水";
                                        strField.Add(Field);
                                        string Value = Math.Round(com.fenqu_supplyQ[year, time, ii] - com.riverwater_fenqu_supply[year, time, ii] - com.groundwater_fenqu_supply[year, time, ii] - com.recycledwater_fenqu_supply[year, time, ii], 2).ToString();
                                        strValues.Add(Value);
                                    }
                                    else
                                    {
                                        string Field = "本地地表径流供水";
                                        strField.Add(Field);
                                        string Value = Math.Round(com.locatedwater_fenqu_supply[year, time, ii], 2).ToString();
                                        strValues.Add(Value);
                                    }
                                    string strSql = $" INSERT INTO O12工程分区供需平衡结果表(所属工程分区编号,所属工程分区,年,历时,月旬,总需水,河道引提水供水,地下水供水,再生水供水,界河水供水,总供水,总缺水量,综合平均缺水率,{string.Join(",", strField)}) VALUES ({ii},'{com.FenquName[ii]}',{year + com.First_Year - 1},{time},{time + (year - 1) * com.YueXuns},'{com.YueXun[time]}',{Math.Round(com.fenqu_shortQO[year, time, ii, com.Users], 2)},{Math.Round(com.riverwater_fenqu_supply[year, time, ii], 2)},{Math.Round(com.groundwater_fenqu_supply[year, time, ii], 2)},{Math.Round(com.recycledwater_fenqu_supply[year, time, ii], 2)},{Math.Round(com.boundaryriver_fenqu_supply[year, time, ii], 2)},{Math.Round(com.fenqu_supplyQ[year, time, ii], 2)},'{string.Format("{0:P}", com.fenqu_shortQ[year, time, ii, com.Users])}',{string.Join(",", strValues)})";
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
