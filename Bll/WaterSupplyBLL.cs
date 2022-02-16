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
    public class WaterSupplyBLL
    {
        private WaterSupplyDAL dal = new WaterSupplyDAL();
        /// <summary>
        /// 各个水源分水结果
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
                        for (int j = 1; j < com.Units_Numb; j++)
                        {
                            for (int year = 1; year < com.Years; year++)
                            {
                                for (int time = 1; time < com.Times; time++) //只统计5-8月
                                {
                                    List<string> strField = new List<string>();
                                    List<string> strValues = new List<string>();
                                    string Field = "计算单元编号";
                                    strField.Add(Field);
                                    string Value = j.ToString();
                                    strValues.Add(Value);

                                    Field = "计算单元名称";
                                    strField.Add(Field);
                                    Value = $"'{com.UnitsName[j]}'";
                                    strValues.Add(Value);

                                    Field = "月旬号";
                                    strField.Add(Field);
                                    Value = time.ToString();
                                    strValues.Add(Value);

                                    Field = "年";
                                    strField.Add(Field);
                                    Value = (year + com.First_Year - 1).ToString();
                                    strValues.Add(Value);

                                    Field = "月旬";
                                    strField.Add(Field);
                                    Value = $"'{com.YueXun[time]}'";
                                    strValues.Add(Value);

                                    Field = "总供水量";
                                    strField.Add(Field);
                                    Value = Math.Round(com.Units_Water_Supply[year, time, j], 2).ToString();
                                    strValues.Add(Value);

                                    Field = "总供水量（再生水）";
                                    strField.Add(Field);
                                    Value = Math.Round((com.Recycledwater_Supply_Ture[year, time, j, 1] + com.Recycledwater_Supply_Ture[year, time, j, 2] + com.Recycledwater_Supply_Ture[year, time, j, 3] + com.Recycledwater_Supply_Ture[year, time, j, 4] + com.Recycledwater_Supply_Ture[year, time, j, 5] + com.Recycledwater_Supply_Ture[year, time, j, 6]), 2).ToString();
                                    strValues.Add(Value);

                                    Field = "总供水量（本地地表水）";
                                    strField.Add(Field);
                                    Value = Math.Round((com.Locatedwater_Supply_Ture[year, time, j, 1] + com.Locatedwater_Supply_Ture[year, time, j, 2] + com.Locatedwater_Supply_Ture[year, time, j, 3] + com.Locatedwater_Supply_Ture[year, time, j, 4] + com.Locatedwater_Supply_Ture[year, time, j, 5] + com.Locatedwater_Supply_Ture[year, time, j, 6]), 2).ToString();
                                    strValues.Add(Value);

                                    Field = "总供水量（地下水）";
                                    strField.Add(Field);
                                    Value = Math.Round((com.Groundwater_Supply_Ture[year, time, j, 1] + com.Groundwater_Supply_Ture[year, time, j, 2] + com.Groundwater_Supply_Ture[year, time, j, 3] + com.Groundwater_Supply_Ture[year, time, j, 4] + com.Groundwater_Supply_Ture[year, time, j, 5] + com.Groundwater_Supply_Ture[year, time, j, 6]), 2).ToString();
                                    strValues.Add(Value);

                                    Field = "总供水量（界河水）";
                                    strField.Add(Field);
                                    Value = Math.Round((com.Boundaryriver_Supply_Ture[year, time, j, 1] + com.Boundaryriver_Supply_Ture[year, time, j, 2] + com.Boundaryriver_Supply_Ture[year, time, j, 3] + com.Boundaryriver_Supply_Ture[year, time, j, 4] + com.Boundaryriver_Supply_Ture[year, time, j, 5] + com.Boundaryriver_Supply_Ture[year, time, j, 6]), 2).ToString();
                                    strValues.Add(Value);

                                    Field = "总供水量（河网水）";
                                    strField.Add(Field);
                                    Value = Math.Round(com.riverwater_units_supply[year, time, j], 2).ToString();
                                    strValues.Add(Value);
                                    for (int ii = 1; ii < com.Users; ii++)
                                    {
                                        Field = com.Users_Name[ii] + "供水量（再生水）";
                                        strField.Add(Field);
                                        Value = Math.Round(com.Recycledwater_Supply_Ture[year, time, j, ii], 2).ToString();
                                        strValues.Add(Value);

                                        Field = com.Users_Name[ii] + "供水量（本地地表水）";
                                        strField.Add(Field);
                                        Value = Math.Round(com.Locatedwater_Supply_Ture[year, time, j, ii], 2).ToString();
                                        strValues.Add(Value);

                                        Field = com.Users_Name[ii] + "供水量（地下水）";
                                        strField.Add(Field);
                                        Value = Math.Round(com.Groundwater_Supply_Ture[year, time, j, ii], 2).ToString();
                                        strValues.Add(Value);

                                        Field = com.Users_Name[ii] + "供水量（界河水）";
                                        strField.Add(Field);
                                        Value = Math.Round(com.Boundaryriver_Supply_Ture[year, time, j, ii], 2).ToString();
                                        strValues.Add(Value);

                                        Field = com.Users_Name[ii] + "供水量（河网水）";
                                        strField.Add(Field);
                                        Value = Math.Round(com.riverwater_unitsuser_supply[year, time, j, ii], 2).ToString();
                                        strValues.Add(Value);
                                    }
                                    string strSql = $" INSERT INTO O2各水源供水量({string.Join(",", strField)}) VALUES ({string.Join(",", strValues)})";
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
