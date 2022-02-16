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
    public class ProvinceYearsOutputBLL
    {
        private UnitsDAL dal = new UnitsDAL();
        /// <summary>
        /// O15 省级区年值结果输出 
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
                    result = dal.DeleteALL("O15省级区供需平衡结果表_年值", trans);
                    if (result)
                    {
                        for (int ii = 1; ii < com.Province_Numb; ii++)//根据计算单元编号 依次输出计算结果
                        {
                            for (int year = 1; year < com.Years; year++)
                            {
                                List<string> strField = new List<string>();
                                List<string> strValues = new List<string>();
                                string Field = "所属省级区编号";
                                strField.Add(Field);
                                string Value = $"{ii}";
                                strValues.Add(Value);
                                Field = "所属省级区名称";
                                strField.Add(Field);
                                Value = $"'{com.ProvinceName[ii]}'";
                                strValues.Add(Value);
                                Field = "年";
                                strField.Add(Field);
                                Value = (year + com.First_Year - 1).ToString();
                                strValues.Add(Value);
                                Field = "总需水";
                                strField.Add(Field);
                                Value = Math.Round(com.province_needO_tureY[year, ii, com.Users], 2).ToString();
                                strValues.Add(Value);
                                if (com.locatedwater_province_supplyY[year, ii] > 0)
                                {
                                    Field = "本地地表径流供水";
                                    strField.Add(Field);
                                    Value = Math.Round(com.province_needO_tureY[year, ii, com.Users]- com.province_short_tureY[year, ii, com.Users] - com.riverwater_province_supplyY[year, ii] - com.groundwater_province_supplyY[year, ii] - com.recycledwater_province_supplyY[year, ii], 2).ToString();
                                    strValues.Add(Value);
                                }
                                else
                                {
                                    Field = "本地地表径流供水";
                                    strField.Add(Field);
                                    Value = Math.Round(com.locatedwater_province_supplyY[year, ii], 2).ToString();
                                    strValues.Add(Value);
                                }
                               
                               
                                Field = "河道引提水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.riverwater_province_supplyY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "地下水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.groundwater_province_supplyY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "再生水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.recycledwater_province_supplyY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "界河水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.boundaryriver_province_supplyY[year, ii], 2).ToString();
                                Field = "总供水";
                                strField.Add(Field);
                                Value = Math.Round(com.province_needO_tureY[year, ii, com.Users] - com.province_short_tureY[year, ii, com.Users], 2).ToString();
                                strValues.Add(Value);
                                Field = "总缺水量";
                                strField.Add(Field);
                                Value = Math.Round(com.province_short_tureY[year, ii, com.Users], 2).ToString();
                                strValues.Add(Value);
                                strValues.Add(Value);
                                for (int iii = 1; iii < com.Users; iii++)
                                {
                                    Field = com.Users_Name[iii] + "缺水量";
                                    strField.Add(Field);
                                    Value = Math.Round(com.province_short_tureY[year, ii, iii], 2).ToString();
                                    strValues.Add(Value);
                                    Field = com.Users_Name[iii] + "缺水率";
                                    strField.Add(Field);
                                    Value = $"'{string.Format("{0:P}", com.province_shortRY[year, ii, iii])}'";
                                    strValues.Add(Value);
                                }
                                Field = "综合平均缺水率";
                                strField.Add(Field);
                                Value = $"'{string.Format("{0:P}", com.province_shortRY[year, ii, com.Users])}'";
                                strValues.Add(Value);
                                string strSql = $" INSERT INTO O15省级区供需平衡结果表_年值({string.Join(",", strField)}) VALUES ({string.Join(",", strValues)})";
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
