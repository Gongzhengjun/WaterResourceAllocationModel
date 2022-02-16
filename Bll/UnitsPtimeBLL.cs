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
    public class UnitsPtimeBLL
    {
        private UnitsDAL dal = new UnitsDAL();
        /// <summary>
        /// O31  历时保证率 结果
        /// </summary>
        /// <returns></returns>
        public bool Add(Common com)
        {
            var result = false;
            using (var trans = DbHelper.BeginTransaction())
            {
                try
                {
                    result = dal.DeleteALL("O31历时保证率计算结果表", trans);
                    if (result)
                    {
                        for (int j = 1; j <= com.Units_Numb + 2; j++)//根据计算单元编号 依次输出计算结果
                        {
                            List<string> strField = new List<string>();
                            List<string> strValues = new List<string>();
                            if (j == com.Units_Numb)
                            {
                                string Field = "计算单元编号";
                                strField.Add(Field);
                                string Value = $"{j}";
                                strValues.Add(Value);
                                Field = "计算单元名称";
                                strField.Add(Field);
                                Value = $"'整个区域'";
                                strValues.Add(Value);
                                Field = "综合平均历时保证率";
                                strField.Add(Field);
                                Value = $"'{string.Format("{0:P}", com.unitsum_Ptime)}'";
                                strValues.Add(Value);
                                for (int ii = 1; ii < com.Users; ii++)
                                {
                                    Field = com.Users_Name[ii] + "历时保证率";
                                    strField.Add(Field);
                                    Value = $"'{string.Format("{0:P}", com.unitsum_user_Ptime[ii])}'";
                                    strValues.Add(Value);
                                }
                                
                            }
                            else if (j == com.Units_Numb + 1)
                            {
                                string Field = "计算单元编号";
                                strField.Add(Field);
                                string Value = $"{j}";
                                strValues.Add(Value);
                                Field = "计算单元名称";
                                strField.Add(Field);
                                Value = $"'三江连通工程区'";
                                strValues.Add(Value);
                                Field = "综合平均历时保证率";
                                strField.Add(Field);
                                Value = $"'{string.Format("{0:P}", com.SJLT_Ptime)}'";
                                strValues.Add(Value);
                                for (int ii = 1; ii < com.Users; ii++)
                                {
                                    Field = com.Users_Name[ii] + "历时保证率";
                                    strField.Add(Field);
                                    Value = $"'{string.Format("{0:P}", com.SJLT_users_Ptime[ii])}'";
                                    strValues.Add(Value);
                                }
                            }
                            else if (j == com.Units_Numb + 2)
                            {
                                string Field = "计算单元编号";
                                strField.Add(Field);
                                string Value = $"{j}";
                                strValues.Add(Value);
                                Field = "计算单元名称";
                                strField.Add(Field);
                                Value = $"'所有灌区'";
                                strValues.Add(Value);
                                Field = "综合平均历时保证率";
                                strField.Add(Field);
                                Value = $"'{string.Format("{0:P}", com.guanqu_Ptime)}'";
                                strValues.Add(Value);
                                for (int ii = 1; ii < com.Users; ii++)
                                {
                                    Field = com.Users_Name[ii] + "历时保证率";
                                    strField.Add(Field);
                                    Value = $"'{string.Format("{0:P}", com.guanqu_user_Ptime[ii])}'";
                                    strValues.Add(Value);
                                }
                            }
                            else
                            {
                                string Field = "计算单元编号";
                                strField.Add(Field);
                                string Value = $"{j}";
                                strValues.Add(Value);
                                Field = "计算单元名称";
                                strField.Add(Field);
                                Value = $"'{com.UnitsName[j]}'";
                                strValues.Add(Value);
                                Field = "综合平均历时保证率";
                                strField.Add(Field);
                                Value = $"'{string.Format("{0:P}", com.units_waterPtime[j])}'";
                                strValues.Add(Value);
                                for (int ii = 1; ii < com.Users; ii++)
                                {
                                    Field = com.Users_Name[ii] + "历时保证率";
                                    strField.Add(Field);
                                    Value = $"'{string.Format("{0:P}", com.unitsusers_waterPtime[j,ii])}'";
                                    strValues.Add(Value);
                                }
                              
                            }
                            string strSql = $" INSERT INTO O31历时保证率计算结果表({string.Join(",", strField)}) VALUES ({string.Join(",", strValues)})";
                            result = dal.Increase(strSql, trans);
                            if (!result)
                            {
                                break;
                            }
                        }
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
