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
    public class ValueYearsOutputBLL
    {
        private UnitsDAL dal = new UnitsDAL();
        /// <summary>
        /// O11计算单元供需平衡结果表_年值  输出
        /// </summary>
        /// <returns></returns>
        public bool Add(Common com, double[,] units_groundwater_xuefengY, double[,] units_groundwater_shiyongY)
        {
            var result = false;
            using (var trans = DbHelper.BeginTransaction())
            {
                try
                {
                    result = dal.DeleteALL("O11计算单元供需平衡结果表_年值", trans);
                    if (result)
                    {
                        for (int ii = 1; ii < com.Units_Numb; ii++)//根据计算单元编号 依次输出计算结果
                        {
                            for (int year = 1; year < com.Years; year++)
                            {
                                List<string> strField = new List<string>();
                                List<string> strValues = new List<string>();
                                string Field = "计算单元编号";
                                strField.Add(Field);
                                string Value = $"{ii}";
                                strValues.Add(Value);
                                Field = "计算单元名称";
                                strField.Add(Field);
                                Value = $"'{com.UnitsName[ii]}'";
                                strValues.Add(Value);
                                Field = "年";
                                strField.Add(Field);
                                Value = (year + com.First_Year - 1).ToString();
                                strValues.Add(Value);
                                Field = "总需水";
                                strField.Add(Field);
                                Value = Math.Round(com.units_waterneedOY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                if (com.units_locatedwater_supplyY[year, ii] > 0)
                                {
                                    Field = "本地地表径流供水";
                                    strField.Add(Field);
                                    Value = Math.Round(com.units_watersupplyY[year, ii] - com.units_riverwater_supplyY[year, ii] - com.units_groundwater_supplyY[year, ii] - com.units_recycledwater_supplyY[year, ii], 2).ToString();
                                    strValues.Add(Value);
                                }
                                else
                                {
                                    Field = "本地地表径流供水";
                                    strField.Add(Field);
                                    Value = Math.Round(com.units_locatedwater_supplyY[year, ii], 2).ToString();
                                    strValues.Add(Value);
                                }
                                Field = "河道引提水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.units_riverwater_supplyY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "地下水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.units_groundwater_supplyY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "再生水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.units_recycledwater_supplyY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "总供水";
                                strField.Add(Field);
                                Value = Math.Round(com.units_watersupplyY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "总缺水量";
                                strField.Add(Field);
                                Value = Math.Round(com.units_watershortQY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "地下水占比";
                                strField.Add(Field);
                                Value = Math.Round(units_groundwater_xuefengY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "地下水开采率";
                                strField.Add(Field);
                                Value = Math.Round(units_groundwater_shiyongY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                Field = "界河水供水";
                                strField.Add(Field);
                                Value = Math.Round(com.units_boundaryriver_supplyY[year, ii], 2).ToString();
                                strValues.Add(Value);
                                for (int iii = 1; iii < com.Users; iii++)
                                {
                                    Field = com.Users_Name[iii] + "缺水量";
                                    strField.Add(Field);
                                    Value = Math.Round(com.unitsusers_watershortQY[year, ii,iii], 2).ToString();
                                    strValues.Add(Value);
                                    Field = com.Users_Name[iii] + "缺水率";
                                    strField.Add(Field);
                                    Value = $"'{string.Format("{0:P}", com.unitsusers_watershortRY[year, ii, iii])}'";
                                    strValues.Add(Value);
                                }
                                Field = "综合平均缺水率";
                                strField.Add(Field);
                                Value = $"'{string.Format("{0:P}", com.units_watershortRY[year, ii])}'";
                                strValues.Add(Value);
                                string strSql = $" INSERT INTO O11计算单元供需平衡结果表_年值({string.Join(",", strField)}) VALUES ({string.Join(",", strValues)})";
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
