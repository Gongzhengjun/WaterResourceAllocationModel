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
    public class UnitsStatisticBLL
    {
        private UnitsStatisticDAL dal = new UnitsStatisticDAL();
        /// <summary>
        /// O11计算单元 历时 结果统计（原水资源分区套地级市）
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
                        for (int ii = 1; ii < com.Units_Numb; ii++)
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
                                        string Value = Math.Round(com.Units_Waterneed[year, time, ii, jj], 2).ToString();
                                        strValues.Add(Value);
                                        Field = com.Users_Name[jj] + "缺水率";
                                        strField.Add(Field);
                                        Value = $"'{string.Format("{0:P}", com.units_water_usershortR[year, time, ii, jj])}'";
                                        strValues.Add(Value);
                                    }
                                    if (com.locatedwater_unit_supply[year, time, ii] > 0)
                                    {
                                        string Field = "本地地表径流供水";
                                        strField.Add(Field);
                                        string Value = Math.Round(com.Units_Water_Supply[year, time, ii] - com.riverwater_unit_supply[year, time, ii] - com.groundwater_unit_supply[year, time, ii] - com.recycledwater_unit_supply[year, time, ii], 2).ToString();
                                        strValues.Add(Value);
                                    }
                                    else
                                    {
                                        string Field = "本地地表径流供水";
                                        strField.Add(Field);
                                        string Value = Math.Round(com.locatedwater_unit_supply[year, time, ii], 2).ToString();
                                        strValues.Add(Value);
                                    }
                                    string strSql = $" INSERT INTO O11计算单元供需平衡结果表(计算单元编号,计算单元名称,年,历时,月旬,总需水,综合平均缺水率,{string.Join(",", strField)}) VALUES ({ii},'{com.UnitsName[ii]}',{(year + com.First_Year - 1)},{time},'{com.YueXun[time]}',{Math.Round(com.units_waterneedsum[year, time, ii], 2)},'{string.Format("{0:P}", com.units_water_shortR[year, time, ii])}',{string.Join(",", strValues)})";
                                    result = dal.Increase(strSql, trans);
                                    if (!result)
                                    {
                                        goto result;
                                    }
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
