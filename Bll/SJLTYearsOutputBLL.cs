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
    public class SJLTYearsOutputBLL
    {
        private UnitsDAL dal = new UnitsDAL();
        /// <summary>
        /// O13  县级区年值结果输出 
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public bool Add(Common com)
        {
            var result = false;
            using (var trans = DbHelper.BeginTransaction())
            {
                try
                {
                    result = dal.DeleteALL("O16三江连通工程区供需平衡结果表_年值", trans);
                    if (result)
                    {

                        for (int year = 1; year < com.Years; year++)
                        {
                            List<string> strField = new List<string>();
                            List<string> strValues = new List<string>();
                            if (year == com.Years)
                            {
                                string Field = "名称";
                                strField.Add(Field);
                                string Value = $"'平均值'";
                                strValues.Add(Value);
                                Field = "总需水";
                                strField.Add(Field);
                                Value = Math.Round(com.SJLT_waterneedYsum, 2).ToString();
                                strValues.Add(Value);
                                if (com.locatedwater_SJLT_supplyYsum > 0)
                                {
                                    Field = "本地地表径流供水";
                                    strField.Add(Field);
                                    Value = Math.Round((com.SJLT_watersupplyYsum - com.riverwater_SJLT_supplyYsum - com.groundwater_SJLT_supplyYsum - com.recycledwater_SJLT_supplyYsum) / (com.Years - 1), 2).ToString();
                                    strValues.Add(Value);
                                }
                                else
                                {
                                    Field = "本地地表径流供水";
                                    strField.Add(Field);
                                    Value = Math.Round(com.locatedwater_SJLT_supplyYsum / (com.Years - 1), 2).ToString();
                                    strValues.Add(Value);
                                }
                                Field = "河道引提水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.riverwater_SJLT_supplyYsum / (com.Years - 1), 2).ToString();
                                strValues.Add(Value);
                                Field = "地下水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.groundwater_SJLT_supplyYsum / (com.Years - 1), 2).ToString();
                                strValues.Add(Value);
                                Field = "再生水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.recycledwater_SJLT_supplyYsum / (com.Years - 1), 2).ToString();
                                strValues.Add(Value);
                                Field = "总供水";
                                strField.Add(Field);
                                Value = Math.Round(com.SJLT_watersupplyYsum / (com.Years - 1), 2).ToString();
                                strValues.Add(Value);
                                Field = "总缺水量";
                                strField.Add(Field);
                                Value = Math.Round(com.SJLT_watershortYsum / (com.Years - 1), 2).ToString();
                                strValues.Add(Value);
                                Field = "界河水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.boundaryriver_SJLT_supplyYsum / (com.Years - 1), 2).ToString();
                                strValues.Add(Value);
                                for (int iii = 1; iii < com.Users; iii++)
                                {
                                    Field = com.Users_Name[iii] + "缺水量";
                                    strField.Add(Field);
                                    Value = Math.Round(com.SJLT_user_watershortQYsum[iii], 2).ToString();
                                    strValues.Add(Value);
                                    Field = com.Users_Name[iii] + "缺水率";
                                    strField.Add(Field);
                                    Value = $"'{string.Format("{0:P}", com.SJLT_user_watershortRYsum[iii])}'";
                                    strValues.Add(Value);
                                }
                                Field = "综合平均缺水率";
                                strField.Add(Field);
                                Value = $"'{string.Format("{0:P}", com.SJLT_watershortRYsum)}'";
                                strValues.Add(Value);
                            }
                            else
                            {
                                string Field = "名称";
                                strField.Add(Field);
                                string Value = $"'三江连通工程区'";
                                strValues.Add(Value);
                                Field = "年";
                                strField.Add(Field);
                                Value = (year + com.First_Year - 1).ToString();
                                strValues.Add(Value);
                                Field = "总需水";
                                strField.Add(Field);
                                Value = Math.Round(com.SJLT_waterneedY[year], 2).ToString();
                                strValues.Add(Value);
                                Field = "总供水";
                                strField.Add(Field);
                                Value = Math.Round(com.SJLT_watersupplyY[year], 2).ToString();
                                strValues.Add(Value);
                                Field = "总缺水量";
                                strField.Add(Field);
                                Value = Math.Round(com.SJLT_watershortY[year], 2).ToString();
                                strValues.Add(Value);
                                Field = "本地地表径流供水";
                                strField.Add(Field);
                                Value = Math.Round(com.locatedwater_SJLT_supplyY[year], 2).ToString();
                                strValues.Add(Value);
                                Field = "再生水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.recycledwater_SJLT_supplyY[year], 2).ToString();
                                strValues.Add(Value);
                                Field = "河道引提水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.riverwater_SJLT_supplyY[year], 2).ToString();
                                strValues.Add(Value);
                                Field = "地下水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.groundwater_SJLT_supplyY[year], 2).ToString();
                                strValues.Add(Value);
                                Field = "界河水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.boundaryriver_SJLT_supplyY[year], 2).ToString();
                                strValues.Add(Value);
                                for (int iii = 1; iii < com.Users; iii++)
                                {
                                    Field = com.Users_Name[iii] + "缺水量";
                                    strField.Add(Field);
                                    Value = Math.Round(com.SJLT_user_watershortQY[year, iii], 2).ToString();
                                    strValues.Add(Value);
                                    Field = com.Users_Name[iii] + "缺水率";
                                    strField.Add(Field);
                                    Value = $"'{string.Format("{0:P}", com.SJLT_user_watershortRY[year, iii])}'";
                                    strValues.Add(Value);
                                }
                                Field = "综合平均缺水率";
                                strField.Add(Field);
                                Value = $"'{string.Format("{0:P}", com.SJLT_watershortY[year]/ com.SJLT_waterneedY[year])}'";
                                strValues.Add(Value);
                            }


                            string strSql = $" INSERT INTO O16三江连通工程区供需平衡结果表_年值({string.Join(",", strField)}) VALUES ({string.Join(",", strValues)})";
                            result = dal.Increase(strSql, trans);
                            if (!result)
                            {
                                goto result;
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
