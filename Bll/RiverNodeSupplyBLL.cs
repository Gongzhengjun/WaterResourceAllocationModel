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
    public class RiverNodeSupplyBLL
    {
        private RiverNodeSupplyDAL dal = new RiverNodeSupplyDAL();
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
                        for (int rivercode = 1; rivercode < com.River_Numb; rivercode++)
                        {
                            for (int nodecode = 1; nodecode < com.River_Node[com.River_Info[rivercode, 1]]; nodecode++)
                            {
                                for (int year = 1; year < com.Years; year++)
                                {
                                    for (int time = 1; time < com.Times; time++) //只统计5-8月
                                    {
                                        string strSql = $" INSERT INTO O河流节点径流量供水量（竖向）(所有节点总编号,河流编号,河流名称,节点编号,节点名称,年,月旬号,历时,月旬,节点径流量,节点供水量) VALUES ({com.River_Totalnode[com.River_Info[rivercode, 1], nodecode]},{com.River_Info[rivercode, 1]},'{com.RiverName[com.River_Info[rivercode, 1]]}',{nodecode},'{com.NodeName[com.River_Totalnode[com.River_Info[rivercode, 1], nodecode]]}',{(year + com.First_Year - 1)},{time},{time + (year - 1) * (com.YueXuns - 1)},'{com.YueXun[time]}',{ Math.Round(com.RiverQ[year, time, com.River_Info[rivercode, 1], nodecode], 2)},{Math.Round(com.Riverwater_Node_Supply[year, time, com.River_Totalnode[com.River_Info[rivercode, 1], nodecode]], 2)})";
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
