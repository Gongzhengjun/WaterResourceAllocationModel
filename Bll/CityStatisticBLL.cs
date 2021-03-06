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
    public class CityStatisticBLL
    {
        private CityStatisticDAL dal = new CityStatisticDAL();
        /// <summary>
        /// O14 地级区统计输出
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
                        for (int ii = 1; ii < com.City_Numb; ii++)
                        {
                            for (int year = 1; year < com.Years; year++)
                            {
                                for (int time = 1; time < com.Times; time++) //只统计5-8月
                                {
                                    List<string> strField = new List<string>();
                                    List<string> strValues = new List<string>();
                                    string Field = "所属地级区编号";
                                    strField.Add(Field);
                                    string Value = ii.ToString();
                                    strValues.Add(Value);

                                    Field = "所属地级区名称";
                                    strField.Add(Field);
                                    Value = $"'{com.CityName[ii]}'";
                                    strValues.Add(Value);

                                    Field = "年";
                                    strField.Add(Field);
                                    Value = (year + com.First_Year - 1).ToString();
                                    strValues.Add(Value);

                                    Field = "月旬";
                                    strField.Add(Field);
                                    Value = $"'{com.YueXun[time]}'";
                                    strValues.Add(Value);

                                    Field = "月旬号";
                                    strField.Add(Field);
                                    Value = time.ToString();
                                    strValues.Add(Value);

                                    Field = "历时";
                                    strField.Add(Field);
                                    Value = (time + (year - 1) * com.YueXuns).ToString();
                                    strValues.Add(Value);

                                    Field = "总需水";
                                    strField.Add(Field);
                                    Value = Math.Round(com.city_needO_ture[year, time, ii, com.Users], 2).ToString();
                                    strValues.Add(Value);

                                    Field = "总供水";
                                    strField.Add(Field);
                                    Value = Math.Round(com.city_needO_ture[year, time, ii, com.Users] - com.city_short_ture[year, time, ii, com.Users], 2).ToString();
                                    strValues.Add(Value);

                                    Field = "总缺水量";
                                    strField.Add(Field);
                                    Value = Math.Round(com.city_short_ture[year, time, ii, com.Users], 2).ToString();
                                    strValues.Add(Value);

                                    Field = "综合平均缺水率";
                                    strField.Add(Field);
                                    Value = $"'{string.Format("{0:P}", com.city_shortR[year, time, ii, com.Users])}'";
                                    strValues.Add(Value);

                                    Field = "本地地表径流供水";
                                    strField.Add(Field);
                                    Value = Math.Round(com.locatedwater_city_supply[year, time, ii], 2).ToString();
                                    strValues.Add(Value);

                                    Field = "再生水供水";
                                    strField.Add(Field);
                                    Value = Math.Round(com.recycledwater_city_supply[year, time, ii], 2).ToString();
                                    strValues.Add(Value);

                                    Field = "地下水供水";
                                    strField.Add(Field);
                                    Value = Math.Round(com.groundwater_city_supply[year, time, ii], 2).ToString();
                                    strValues.Add(Value);

                                    Field = "界河水供水";
                                    strField.Add(Field);
                                    Value = Math.Round(com.boundaryriver_city_supply[year, time, ii], 2).ToString();
                                    strValues.Add(Value);
                                    for (int jj = 1; jj < com.Users; jj++)
                                    {
                                        Field = com.Users_Name[jj] + "缺水量";
                                        strField.Add(Field);
                                        Value = Math.Round(com.city_short_ture[year, time, ii, jj], 2).ToString();
                                        strValues.Add(Value);
                                        Field = com.Users_Name[jj] + "缺水率";
                                        strField.Add(Field);
                                        Value = $"'{string.Format("{0:P}", com.city_shortR[year, time, ii, jj])}'";
                                        strValues.Add(Value);
                                    }
                                    string strSql = $" INSERT INTO O14地级区供需平衡结果表({string.Join(",", strField)}) VALUES ({string.Join(",", strValues)})";
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
