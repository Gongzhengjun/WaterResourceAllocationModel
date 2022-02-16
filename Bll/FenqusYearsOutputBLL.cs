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
    public class FenqusYearsOutputBLL
    {
        private UnitsDAL dal = new UnitsDAL();
        /// <summary>
        /// O11计算单元供需平衡结果表_年值  输出
        /// </summary>
        /// <returns></returns>
        public bool Add(Common com, double[] locatedwater_fenqu_supplyY_avg, double[,] fenqu_users_shortQOY_avg, double[,] fenqu_users_shortQY_avg, double[] recycledwater_fenqu_supplyY_avg, double[] riverwater_fenqu_supplyY_avg, double[] groundwater_fenqu_supplyY_avg, double[] boundaryriver_fenqu_supplyY_avg, double[,] fenqu_users_shortRY_avg)
        {
            var result = false;
            using (var trans = DbHelper.BeginTransaction())
            {
                try
                {
                    result = dal.DeleteALL("O12工程分区供需平衡结果表_年值", trans);
                    if (result)
                    {
                        for (int ii = 1; ii < com.Fenqus; ii++)//根据计算单元编号 依次输出计算结果
                        {
                            for (int year = 1; year < com.Years; year++)
                            {
                                List<string> strField = new List<string>();
                                List<string> strValues = new List<string>();
                                string Field = "所属工程分区编号";
                                strField.Add(Field);
                                string Value = $"{ii}";
                                strValues.Add(Value);
                                Field = "所属工程分区";
                                strField.Add(Field);
                                Value = $"'{com.FenquName[ii]}'"; 
                                strValues.Add(Value);
                                Field = "年";
                                strField.Add(Field);
                                Value = (year + com.First_Year - 1).ToString();
                                strValues.Add(Value);
                                Field = "总需水";
                                strField.Add(Field);
                                Value = Math.Round(com.fenqu_users_shortQOY[year, ii, com.Users], 2).ToString();
                                strValues.Add(Value);
                                if (locatedwater_fenqu_supplyY_avg[ii] > 0)
                                {
                                    Field = "本地地表径流供水";
                                    strField.Add(Field);
                                    Value = Math.Round(com.fenqu_users_shortQOY[year, ii, com.Users] - com.fenqu_users_shortQY[year, ii, com.Users] - com.riverwater_fenqu_supplyY[year, ii] - com.groundwater_fenqu_supplyY[year, ii] - com.recycledwater_fenqu_supplyY[year, ii] - com.boundaryriver_fenqu_supplyY[year, ii], 2).ToString();
                                    strValues.Add(Value);
                                }
                                else
                                {
                                    Field = "本地地表径流供水";
                                    strField.Add(Field);
                                    Value = Math.Round(locatedwater_fenqu_supplyY_avg[ii], 2).ToString();
                                    strValues.Add(Value);
                                }
                                Field = "河道引提水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.riverwater_fenqu_supplyY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "地下水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.groundwater_fenqu_supplyY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "再生水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.recycledwater_fenqu_supplyY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "界河水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.boundaryriver_fenqu_supplyY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "总供水";
                                strField.Add(Field);
                                Value = Math.Round(com.fenqu_users_shortQOY[year, ii, com.Users] - com.fenqu_users_shortQY[year, ii, com.Users], 2).ToString();
                                strValues.Add(Value);
                                Field = "总缺水量";
                                strField.Add(Field);
                                Value = Math.Round(com.fenqu_users_shortQY[year, ii, com.Users], 2).ToString();
                                strValues.Add(Value);

                                for (int iii = 1; iii < com.Users; iii++)
                                {
                                    Field = com.Users_Name[iii] + "缺水量";
                                    strField.Add(Field);
                                    Value = Math.Round(com.fenqu_users_shortQY[year, ii, iii], 2).ToString();
                                    strValues.Add(Value);
                                    Field = com.Users_Name[iii] + "缺水率";
                                    strField.Add(Field);
                                    Value = $"'{string.Format("{0:P}", com.fenqu_users_shortRY[year, ii, iii])}'";
                                    strValues.Add(Value);
                                }
                                Field = "综合平均缺水率";
                                strField.Add(Field);
                                Value = $"'{string.Format("{0:P}", com.fenqu_users_shortRY[year, ii, com.Users])}'";
                                strValues.Add(Value);
                                string strSql = $" INSERT INTO O12工程分区供需平衡结果表_年值({string.Join(",", strField)}) VALUES ({string.Join(",", strValues)})";
                                result = dal.Increase(strSql, trans);
                                if (!result)
                                {
                                    goto result;
                                }
                            }
                        }
                        if (result)
                        {
                            //接下来写入 平均值
                            for (int ii = 1; ii < com.Fenqus; ii++)
                            {
                                List<string> strField = new List<string>();
                                List<string> strValues = new List<string>();
                                string Field = "所属工程分区编号";
                                strField.Add(Field);
                                string Value = $"{ii}";
                                strValues.Add(Value);
                                Field = "所属工程分区";
                                strField.Add(Field);
                                Value = $"'{com.FenquName[ii]}'"; ;
                                strValues.Add(Value);
                                Field = "年";
                                strField.Add(Field);
                                Value = (0).ToString();
                                strValues.Add(Value);
                                Field = "总需水";
                                strField.Add(Field);
                                Value = Math.Round(fenqu_users_shortQOY_avg[ii, com.Users], 2).ToString();
                                strValues.Add(Value);
                                Field = "总供水";
                                strField.Add(Field);
                                Value = Math.Round(fenqu_users_shortQOY_avg[ii, com.Users] - fenqu_users_shortQY_avg[ii, com.Users], 2).ToString();
                                strValues.Add(Value);
                                Field = "总缺水量";
                                strField.Add(Field);
                                Value = Math.Round(fenqu_users_shortQY_avg[ii, com.Users], 2).ToString();
                                strValues.Add(Value);
                                Field = "本地地表径流供水";
                                strField.Add(Field);
                                Value = Math.Round(locatedwater_fenqu_supplyY_avg[ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "再生水供水";
                                strField.Add(Field);
                                Value = Math.Round(recycledwater_fenqu_supplyY_avg[ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "河道引提水供水";
                                strField.Add(Field);
                                Value = Math.Round(riverwater_fenqu_supplyY_avg[ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "地下水供水";
                                strField.Add(Field);
                                Value = Math.Round(groundwater_fenqu_supplyY_avg[ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "界河水供水";
                                strField.Add(Field);
                                Value = Math.Round(boundaryriver_fenqu_supplyY_avg[ii], 2).ToString();
                                strValues.Add(Value);
                                for (int iii = 1; iii < com.Users; iii++)
                                {
                                    Field = com.Users_Name[iii] + "缺水量";
                                    strField.Add(Field);
                                    Value = Math.Round(fenqu_users_shortQY_avg[ii, iii], 2).ToString();
                                    strValues.Add(Value);
                                    Field = com.Users_Name[iii] + "缺水率";
                                    strField.Add(Field);
                                    Value = $"'{string.Format("{0:P}", fenqu_users_shortRY_avg[ii, iii])}'";
                                    strValues.Add(Value);
                                }
                                Field = "综合平均缺水率";
                                strField.Add(Field);
                                Value = $"'{string.Format("{0:P}", fenqu_users_shortRY_avg[ii, com.Users])}'";
                                strValues.Add(Value);
                                string strSql = $" INSERT INTO O12工程分区供需平衡结果表_年值({string.Join(",", strField)}) VALUES ({string.Join(",", strValues)})";
                                result = dal.Increase(strSql, trans);
                                if (!result)
                                {
                                    goto result;
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
