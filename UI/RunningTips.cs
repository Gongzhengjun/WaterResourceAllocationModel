using BLL;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace UI
{
    public class RunningTips
    {
        public bool RunningTipsClick(Common com)
        {
            bool result = false;
            //先统计各分区需水量，再扣掉地下水大概利用量，估算 渠首的最大引水量，不然会出现最后渠道还有大量水用不完的情况
            double[,,] fenqu_WD = new double[com.Years, com.Times, com.Fenqus];
            double[,,] fenqu_groundwater = new double[com.Years, com.Times, com.Fenqus];
            double[,,] candle_ic = new double[com.Years, com.Times, com.Fenqus];
            com.transfer_rivernode_reservoir = new double[com.Years, com.Times, com.River_Totalnode.GetUpperBound(1), com.Reservoir_Numb];
            DataTable Data = new DataTable();
            for (int year = 1; year < com.Years; year++)
            {
                for (int time = 5; time <= 16; time++)
                {
                    for (int i = 1; i < com.Fenqus; i++)
                    {
                        for (int iii = 1; iii < com.Units_Numb; iii++)
                        {
                            if (com.fenqu_units[iii, 2] == i)
                            {
                                //工程分区 总需水量
                                fenqu_WD[year, time, i] = fenqu_WD[year, time, i] + com.Units_WaterneedO[year, time, iii, 4] / com.unit_surfacewaterK[year, time, i, 0];    //水田
                                //工程分区地下水可开采量
                                if (com.unit_surfacewaterK[year, time, i, 1] > 0)
                                {
                                    fenqu_groundwater[year, time, i] = fenqu_groundwater[year, time, i] + com.Groundwater_Supply[year, 1, iii] / 12 / com.unit_surfacewaterK[year, time, i, 1] * com.unit_surfacewaterK[year, time, i, 0]; //地下水只放在里第一旬  5-8月共12个旬
                                }
                            }
                        }
                    }
                    candle_ic[year, time, 4] = fenqu_WD[year, time, 5] - fenqu_groundwater[year, time, 5] * 0.75;  //挠力河二干
                    candle_ic[year, time, 3] = fenqu_WD[year, time, 4] - fenqu_groundwater[year, time, 4] * 0.75;  //挠力河一干
                    candle_ic[year, time, 2] = candle_ic[year, time, 3] + candle_ic[year, time, 4] + fenqu_WD[year, time, 3] - fenqu_groundwater[year, time, 3] * 0.75; //引松
                    candle_ic[year, time, 1] = candle_ic[year, time, 2] + fenqu_WD[year, time, 2] - fenqu_groundwater[year, time, 2] * 0.75 + fenqu_WD[year, time, 1] - fenqu_groundwater[year, time, 1] * 0.75; //引黑
                    for (int ii = 1; ii <= 4; ii++)
                    {
                        if (candle_ic[year, time, ii] < 0)//有些时段可能不需要调水
                        {
                            candle_ic[year, time, ii] = 0;
                        }
                        if (com.Transferin_Node_Info[year, time, ii, 2] - candle_ic[year, time, ii] > 0)//把估算得到的小值 赋值给 规模
                        {
                            com.Transferin_Node_Info[year, time, ii, 2] = candle_ic[year, time, ii];
                        }
                    }
                }
            }
            //调用本地地表水分配模块
            com = locatedwater_division(com);
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            //***************************** 水资源系统大循环-河网水分配(含水库水、调水)  （顺序 4）
            //__________________________________________________________________________________________________________________________
            var NodeInfoBLL = new NodeInformationBLL();
            for (int year = 1; year < com.Years; year++)
            {
                for (int time = 1; time < com.Times; time++)
                {
                    for (int j = 1; j < com.River_Numb; j++)
                    {
                        Data = NodeInfoBLL.GetTableByRiverNumber(com.River_Info[j, 1]);
                        com.River_Node[com.River_Info[j, 1]] = Data.Rows.Count;
                        com.River_Attribute = Data.Columns.Count + 1;
                        com.River_Node_Info = new int[com.River_Node[com.River_Info[j, 1]] + 1, com.River_Attribute];
                        if (com.River_Info[j, 1] == 2)//2是松花江的编号 .net减1
                        {
                            com.hangyunPtime = new double[com.River_Node[2] + 1];//重定义 存储松花江各个节点的 航运历时保证率
                            com.hangyunPyear = new double[com.River_Node[2] + 1];//航运 年 保证率
                        }
                        for (int i = 1; i <= com.River_Node[com.River_Info[j, 1]]; i++)
                        {
                            com.River_Node_Info[i, 1] = Data.Rows[i - 1]["河流编号"].ToInt();//河流历时编号
                            com.River_Node_Info[i, 2] = Data.Rows[i - 1]["节点编号"].ToInt();
                            com.River_Node_Info[i, 3] = Data.Rows[i - 1]["节点类型代码"].ToInt();
                            com.River_Totalnode[com.River_Info[j, 1], i] = Data.Rows[i - 1]["所有节点总编号"].ToInt();//流域所有节点总编号
                            com.River_Totalnoderank = com.River_Totalnode[com.River_Info[j, 1], i];//河道节点总编号
                            switch (com.River_Node_Info[i, 2])
                            {
                                case 1:
                                    com.River_Node_Info[i, 4] = Data.Rows[i - 1]["引提水点编号"].ToInt();
                                    break;
                                case 2:
                                    com.River_Node_Info[i, 6] = Data.Rows[i - 1]["退水点编号"].ToInt();
                                    break;
                                case 5:
                                    com.River_Node_Info[i, 7] = Data.Rows[i - 1]["调出点编号"].ToInt();
                                    break;
                                case 6:
                                    com.River_Node_Info[i, 8] = Data.Rows[i - 1]["调出点编号"].ToInt();
                                    break;
                                case 3:
                                    com.River_Node_Info[i, 9] = Data.Rows[i - 1]["调出点编号"].ToInt();
                                    break;
                                case 10:
                                    com.River_Node_Info[i, 9] = Data.Rows[i - 1]["调出点编号"].ToInt();
                                    break;
                            }
                            if (com.River_Node_Info[i, 3] == 0)//水文站或者流域出口  直接过流
                            {
                                com.RiverQ[year, time, com.River_Node_Info[i, 1], i] = com.RiverQ[year, time, com.River_Node_Info[i, 1], i - 1] * (1 - com.RiverK[year, time, j, i]); //河道损失
                            }
                            else if (com.River_Node_Info[i, 3] == 1)
                            {
                                //—————————————————————————————————————————————————————
                                //***************************************** 引提水点
                                //—————————————————————————————————————————————————————
                                int iii = 0;
                                for (iii = 1; iii < com.Channel_Node; iii++)
                                {
                                    if (com.Channel_Node_Info[year, time, iii, 1] == com.River_Node_Info[i, 4])
                                    {
                                        break;
                                    }
                                }
                                //###############################################

                                //'''''''''''''' 节点控制流量在此设置
                                if (com.River_Info[j, 1] == 2) // 判断，若是 松花江 的编号2(不是j)，则设置节点控制最小流量为 850 m3/s
                                {
                                    com.RiverQ_Limit[year, time, com.River_Info[j, 1], i] = com.SHJ_limitQ[year, time]; //流量(m3/s)转为历时水量（单位万m3）
                                }
                                else
                                {
                                    com.RiverQ_Limit[year, time, com.River_Info[j, 1], i] = 0;
                                }
                                //'----------------- 河网水可供水量
                                com.Riverwater_Totalsupply[year, time, com.River_Totalnoderank] = com.RiverQ[year, time, com.River_Node_Info[i, 1], i - 1] * (1 - com.RiverK[year, time, j, i]) - com.RiverQ_Limit[year, time, com.River_Info[j, 1], i];
                                //河道节点最大允许取水量（先满足河道流量要求）
                                if (com.Riverwater_Totalsupply[year, time, com.River_Totalnoderank] < 0)//是否满足最小流量约束
                                {
                                    com.Riverwater_Totalsupply[year, time, com.River_Totalnoderank] = 0;
                                }
                                com.RiverQ[year, time, com.River_Node_Info[i, 1], i] = com.RiverQ[year, time, com.River_Node_Info[i, 1], i - 1] * (1 - com.RiverK[year, time, j, i]);
                                //河道理论剩余可供水量（减去损失）

                                //###############################################
                                //--------------------------------- 计算单元供水
                                int unitscode = com.Channel_Node_Info[year, time, iii, 3].ToInt();


                                //*********************************************************************************************************
                                if (com.fenqu_units[unitscode, 2] == 1) //引黑直供区 的引水系数 减小 20210825
                                {
                                    com.Channel_Node_Info[year, time, iii, 6] = com.Channel_Node_Info[year, time, iii, 6] * 1;
                                    //再按灌溉制度统一管理分水比例
                                    if (com.units_ISnumb[unitscode] == 1)//萝北 常规   延军01 延军02
                                    {
                                        com.Channel_Node_Info[year, time, iii, 6] = com.Channel_Node_Info[year, time, iii, 6] * 0.9;
                                    }
                                    else if (com.units_ISnumb[unitscode] == 9)//萝北 控灌  凤翔01-03，梧桐河03、05
                                    {
                                        com.Channel_Node_Info[year, time, iii, 6] = com.Channel_Node_Info[year, time, iii, 6] * 0.7;
                                    }
                                    else if (com.units_ISnumb[unitscode] == 10)//萝北  控灌  新团结01-06 梧桐河01 02 04
                                    {
                                        com.Channel_Node_Info[year, time, iii, 6] = com.Channel_Node_Info[year, time, iii, 6] * 0.75;
                                    }
                                }
                                else if (com.fenqu_units[unitscode, 2] == 3)//引松直供区 的引水系数 减小  20210825
                                {
                                    com.Channel_Node_Info[year, time, iii, 6] = com.Channel_Node_Info[year, time, iii, 6] * 1;
                                    //再按灌溉制度统一管理分水比例
                                    if (com.units_ISnumb[unitscode] == 3)//桦川 常规
                                    {
                                        com.Channel_Node_Info[year, time, iii, 6] = com.Channel_Node_Info[year, time, iii, 6] * 0.8;
                                    }
                                    else if (com.units_ISnumb[unitscode] == 5)//富廷岗 常规
                                    {
                                        com.Channel_Node_Info[year, time, iii, 6] = com.Channel_Node_Info[year, time, iii, 6] * 0.7;
                                    }
                                }

                                //*********************************************************************************************************
                                if (com.fenqu_units[unitscode, 2] == 1) //引黑直供区 的分水流量数 减小 20210902
                                {
                                    com.fenshuiW[year, time, unitscode] = com.fenshuiW[year, time, unitscode] * 1;
                                    if (com.units_ISnumb[unitscode] == 1)//萝北 常规   延军01 延军02
                                    {
                                        com.fenshuiW[year, time, unitscode] = com.fenshuiW[year, time, unitscode] * 0.9;
                                    }
                                    else if (com.units_ISnumb[unitscode] == 9)//萝北 控灌  凤翔01-03，梧桐河03、05
                                    {
                                        com.fenshuiW[year, time, unitscode] = com.fenshuiW[year, time, unitscode] * 0.7;
                                    }
                                    else if (com.units_ISnumb[unitscode] == 10)//萝北  控灌  新团结01-06 梧桐河01 02 04
                                    {
                                        com.fenshuiW[year, time, unitscode] = com.fenshuiW[year, time, unitscode] * 0.75;
                                    }

                                }
                                else if (com.fenqu_units[unitscode, 2] == 3)//引松直供区 的引水系数 减小  20210825
                                {
                                    com.fenshuiW[year, time, unitscode] = com.fenshuiW[year, time, unitscode] * 1;
                                    //再按灌溉制度统一管理分水比例
                                    if (com.units_ISnumb[unitscode] == 3)//桦川 常规
                                    {
                                        com.fenshuiW[year, time, unitscode] = com.fenshuiW[year, time, unitscode] * 0.8;
                                    }
                                    else if (com.units_ISnumb[unitscode] == 5)//富廷岗 常规
                                    {
                                        com.fenshuiW[year, time, unitscode] = com.fenshuiW[year, time, unitscode] * 0.7;
                                    }
                                }

                                //*********************************************************************************************************



                                //---------------------------------- 计算单元河道可引水
                                if (com.Riverwater_Totalsupply[year, time, com.River_Totalnoderank] - com.fenshuiW[year, time, unitscode] > 0)
                                {
                                    com.Riverwater_Supply[year, time, com.River_Totalnoderank, unitscode] = com.fenshuiW[year, time, unitscode];    // 按规模来引
                                }
                                else
                                {
                                    com.Riverwater_Supply[year, time, com.River_Totalnoderank, unitscode] = com.Riverwater_Totalsupply[year, time, com.River_Totalnoderank];
                                }

                                if (com.Riverwater_Supply[year, time, com.River_Totalnoderank, unitscode] > 0)//节点有水可供
                                {
                                    //****************************************************************************************
                                    //---------------------------------- 河网引水分配-----------------------------------------
                                    //****************************************************************************************

                                    //调用 河网水分水 子模块
                                    com = riverwater_supply(com, unitscode, com.Users, com.River_Totalnoderank, year, time);
                                    //把可供的河道水按照分水比和优先级供给用户
                                    com.yintiQ = 0;
                                    for (int jj = 1; jj < com.Users; jj++)
                                    {
                                        //河道剩余水量-实际供水
                                        com.RiverQ[year, time, com.River_Node_Info[i, 1], i] = com.RiverQ[year, time, com.River_Node_Info[i, 1], i] * (1 - com.RiverK[year, time, j, i]) - com.Riverwater_Supply_Ture[year, time, com.River_Totalnoderank, unitscode, jj];
                                        com.yintiQ = com.yintiQ + com.Riverwater_Supply_Ture[year, time, com.River_Totalnoderank, unitscode, jj];
                                    }
                                }
                                //计算河道总供水量占河道流量的比例
                                if (com.yintiQ + com.RiverQ[year, time, com.River_Node_Info[i, 1], i] > 0)
                                {
                                    com.unit_yintiK[year, time, iii] = 0.1 * com.yintiQ / (com.yintiQ + com.RiverQ[year, time, com.River_Node_Info[i, 1], i]) * 10;
                                }
                                else
                                {
                                    com.unit_yintiK[year, time, iii] = 0;
                                }
                                //-----------------地下水水分配(先地表调水再地下供水)
                                //----------------------------------------------------
                                com = groundwater_divisionLH(com, com.Units_Numb, year, time, com.Users, com.guanqu_numb, unitscode);
                                //地下水配置完成后，再转化为 地表毛需水
                                //水田
                                if (com.Units_Waterneed[year, time, unitscode, 0] > 0)
                                {
                                    com.Units_Waterneed[year, time, unitscode, 4] = com.Units_Waterneed[year, time, unitscode, 4] / com.unit_surfacewaterK[year, time, unitscode, 0] * com.unit_groundwaterK[year, time, unitscode, 0];
                                }
                                //这儿开始判断 地下水的使用率   对地下水使用较少的计算单元 下个时刻 少用地表水  hbd
                                for (int ii = 1; ii < com.Users; ii++)//计算单元的地下水使用量=各个用水户的使用量累加
                                {
                                    com.groundwater_unit_supply[year, time, unitscode] = com.groundwater_unit_supply[year, time, unitscode] + com.Groundwater_Supply_Ture[year, time, unitscode, ii];
                                }
                                double hhh = 0;
                                if (com.User_Availability_Groundwater[1, 1, unitscode] > 0)
                                {
                                    hhh = com.groundwater_unit_supply[year, time, unitscode] / com.User_Availability_Groundwater[1, 1, unitscode];     // 地下水实时开采率 = 地下水供水量 / 地下水可开采量
                                }
                                switch (time)
                                {
                                    case 5:
                                        if (0 < hhh && hhh < 0.2)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.5)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 1.05;//增大下个时段的引水比例
                                        }
                                        break;
                                    case 6:
                                        if (0 < hhh && hhh < 0.3)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.6)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 1.05;
                                        }
                                        break;
                                    case 7:
                                        if (0 < hhh && hhh < 0.4)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.7)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 1.05;
                                        }
                                        break;
                                    case 8:
                                        if (0 < hhh && hhh < 0.45)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.75)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 1.05;
                                        }
                                        break;
                                    case 9:
                                        if (0 < hhh && hhh < 0.5)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.8)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 1.05;
                                        }
                                        break;
                                    case 10:
                                        if (0 < hhh && hhh < 0.55)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.85)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 1.05;
                                        }
                                        break;
                                    case 11:
                                        if (0 < hhh && hhh < 0.6)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.9)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 1.05;
                                        }
                                        break;
                                    case 12:
                                        if (0 < hhh && hhh < 0.65)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.9)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 1.05;
                                        }
                                        break;
                                    case 13:
                                        if (0 < hhh && hhh < 0.7)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.95)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 1.05;
                                        }
                                        break;
                                    case 14:
                                        if (0 < hhh && hhh < 0.75)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.95)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 1.05;
                                        }
                                        break;
                                    case 15:
                                        if (0 < hhh && hhh < 0.8)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.95)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 1.05;
                                        }
                                        break;
                                    case 16:
                                        if (0 < hhh && hhh < 0.85)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.99)
                                        {
                                            com.fenshuiW[year, time + 1, unitscode] = com.fenshuiW[year, time + 1, unitscode] * 1.05;
                                        }
                                        break;
                                }

                                //分水比例的控制方式
                                switch (time)//逐个历时判断
                                {
                                    case 5:
                                        if (0 < hhh && hhh < 0.2)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.5)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 1.05;//增大下个时段的引水比例
                                        }
                                        break;
                                    case 6:
                                        if (0 < hhh && hhh < 0.3)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.6)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 1.05;
                                        }
                                        break;
                                    case 7:
                                        if (0 < hhh && hhh < 0.4)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.7)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 1.05;
                                        }
                                        break;
                                    case 8:
                                        if (0 < hhh && hhh < 0.45)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.75)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 1.05;
                                        }
                                        break;
                                    case 9:
                                        if (0 < hhh && hhh < 0.5)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.8)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 1.05;
                                        }
                                        break;
                                    case 10:
                                        if (0 < hhh && hhh < 0.55)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.85)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 1.05;
                                        }
                                        break;
                                    case 11:
                                        if (0 < hhh && hhh < 0.6)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.9)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 1.05;
                                        }
                                        break;
                                    case 12:
                                        if (0 < hhh && hhh < 0.65)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.9)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 1.05;
                                        }
                                        break;
                                    case 13:
                                        if (0 < hhh && hhh < 0.7)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.95)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 1.05;
                                        }
                                        break;
                                    case 14:
                                        if (0 < hhh && hhh < 0.75)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.95)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 1.05;
                                        }
                                        break;
                                    case 15:
                                        if (0 < hhh && hhh < 0.8)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.95)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 1.05;
                                        }
                                        break;
                                    case 16:
                                        if (0 < hhh && hhh < 0.85)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 0.8;              //减少下个时段的引水比例
                                        }
                                        else if (hhh > 0.99)
                                        {
                                            com.Channel_Node_Info[year, time + 1, iii, 6] = com.Channel_Node_Info[year, time + 1, iii, 6] * 1.05;
                                        }
                                        break;
                                }


                            }
                            else if (com.River_Node_Info[i, 3] == 2)
                            {
                                //—————————————————————————————————————————————————————
                                //************************************* 退水点
                                //—————————————————————————————————————————————————————
                                int iii = 0;
                                for (iii = 1; iii < com.return_node; iii++)//从退水点信息表中找到 该行信息
                                {
                                    if (com.Channel_Node_Info[year, time, iii, 1] == com.River_Node_Info[i, 4])
                                    {
                                        break;
                                    }
                                }
                                int unitscode = com.Channel_Node_Info[year, time, iii, 3].ToInt();   //找到退水点对应的计算单元编号

                                //-----------------------------------------------------------------------------------------------------------------

                                for (int jj = 1; jj < com.Users; jj++)
                                {
                                    com.Water_Supply_Ture[year, time, unitscode, jj] = com.Units_WaterneedO[year, time, unitscode, jj] - com.Units_Waterneed[year, time, unitscode, jj];
                                    //各用户总的供水量 = 用户最初需水量-用户当前需水量

                                    com.Units_Waterreturn[year, time, unitscode, jj] = com.Water_Supply_Ture[year, time, unitscode, jj] * com.Units_WaterreturnC[year, time, unitscode, jj];
                                    //各用户总的退水量 = 各用水户的总供水 * 退水系数

                                    com.Units_Watereturn_Total[year, time, unitscode] = com.Units_Watereturn_Total[year, time, unitscode] + com.Units_Waterreturn[year, time, unitscode, jj];
                                    //各用户总的供水引起退水量
                                }

                                com.Waterreturn_Total[year, time, com.River_Node_Info[i, 1], com.River_Node_Info[i, 2]] = com.Waterreturn_Total[year, time, com.River_Node_Info[i, 1], com.River_Node_Info[i, 2]] + com.Units_Watereturn_Total[year, time, unitscode] + com.Recycledwater_Supply[year, time, unitscode] + com.Locatedwater_Supply[year, time, unitscode] + com.Locatedwater_Runoff[year, time, unitscode];
                                //退水点总的退水量 = 使用后退水（总供水 * 退水系数）+ 直接退水（供水结束剩余再生水和本地地表径流直接入河网）+ 本地无法利用的地表径

                                //退水点径流量！
                                com.RiverQ[year, time, com.River_Node_Info[i, 1], i] = com.RiverQ[year, time, com.River_Node_Info[i, 1], i - 1] * (1 - com.RiverK[year, time, j, i]) + com.Waterreturn_Total[year, time, com.River_Node_Info[i, 1], com.River_Node_Info[i, 2]];
                            }
                            else if (com.River_Node_Info[i, 3] == 4)
                            {
                                //—————————————————————————————————————————————————————
                                //************************************** 支流汇入点
                                //—————————————————————————————————————————————————————

                                for (int ii = 1; ii < com.River_Numb; ii++)
                                {
                                    if (com.River_Info[ii, 3] == com.River_Node_Info[i, 1] && com.River_Info[ii, 5] == i)//找到汇入河流对应的编号ii
                                    {
                                        com.RiverQ[year, time, com.River_Node_Info[i, 1], i] = com.RiverQ[year, time, com.River_Node_Info[i, 1], i - 1] * (1 - com.RiverK[year, time, j, i]) + com.RiverQ[year, time, com.River_Info[ii, 1], com.River_Info[ii, 4]];    //子流域汇入计算
                                    }
                                }
                            }
                            else if (com.River_Node_Info[i, 3] == 5)
                            {
                                //—————————————————————————————————————————————————————
                                //*************************************** 调水工程调出点
                                //—————————————————————————————————————————————————————
                                int iii = 0;
                                for (iii = 1; iii < com.Transferin_Node - 1; iii++)//从节点信息表中找到 该行信息
                                {
                                    if (com.Transferin_Node_Info[year, time, iii, 3] == com.River_Node_Info[i, 7])//调水工程中的调出点编号 对应 节点信息表中调出点编号
                                    {
                                        break;
                                    }
                                }

                                com.WaterTransferMAX[year, time, iii, 1] = com.Transferin_Node_Info[year, time, iii, 2];  // 最后一位1代表节点1 即为 渠首

                                //未调之前，本节点与上个节点流量相同
                                com.RiverQ[year, time, com.River_Node_Info[i, 1], i] = com.RiverQ[year, time, com.River_Node_Info[i, 1], i - 1];

                                //根据调水规模写入设计调水量
                                if (j < com.River_Node_Info.GetUpperBound(0) && com.River_Node_Info[j, 1] == 2) //松花江干流的编号 这里需注意是否其他方案也进行了相同的编号设置
                                {
                                    if (com.RiverQ[year, time, com.River_Node_Info[i, 1], i] - com.SHJ_limitQ[year, time] < 0)
                                    {
                                        com.Transferin_Node_Info[year, time, iii, 2] = 0; //小于航运控制流量，则不调水，设计调出水量为0

                                        com.Transferin_Node_Info[year, time, iii, 4] = 0; //小于航运控制流量，则不调水，实际调出水量为0
                                    }
                                    else if (com.RiverQ[year, time, com.River_Node_Info[i, 1], i] - com.SHJ_limitQ[year, time] > 0 && com.RiverQ[year, time, com.River_Node_Info[i, 1], i] - com.SHJ_limitQ[year, time] < com.WaterTransferMAX[year, time, iii, 1])
                                    {
                                        com.Transferin_Node_Info[year, time, iii, 2] = com.RiverQ[year, time, com.River_Node_Info[i, 1], i] - com.SHJ_limitQ[year, time];//实际调出水量
                                        com.Transferin_Node_Info[year, time, iii, 4] = com.Transferin_Node_Info[year, time, iii, 2];
                                        com.RiverQ[year, time, com.River_Node_Info[i, 1], i] = com.SHJ_limitQ[year, time];//调出水之后，河流流量仅为航运控制流量
                                    }
                                    else
                                    {
                                        //上游来水-调出量
                                        com.RiverQ[year, time, com.River_Node_Info[i, 1], i] = com.RiverQ[year, time, com.River_Node_Info[i, 1], i - 1] * (1 - com.RiverK[year, time, j, i]) - com.WaterTransferMAX[year, time, iii, 1];
                                        com.Transferin_Node_Info[year, time, iii, 2] = com.WaterTransferMAX[year, time, iii, 1];
                                    }
                                }
                                else if (com.RiverQ[year, time, com.River_Node_Info[i, 1], i] < com.WaterTransferMAX[year, time, iii, 1])
                                {
                                    com.Transferin_Node_Info[year, time, iii, 2] = com.RiverQ[year, time, com.River_Node_Info[i, 1], i];//设计调出水量
                                    com.RiverQ[year, time, com.River_Node_Info[i, 1], i] = 0;//全部调完，剩余河流水量为0！！
                                }
                                else
                                {
                                    //上游来水-调出量
                                    com.Transferin_Node_Info[year, time, iii, 2] = com.WaterTransferMAX[year, time, iii, 1];
                                    com.RiverQ[year, time, com.River_Node_Info[i, 1], i] = com.RiverQ[year, time, com.River_Node_Info[i, 1], i] * (1 - com.RiverK[year, time, j, i]) - com.Transferin_Node_Info[year, time, iii, 2];
                                }
                            }
                            else if (com.River_Node_Info[i, 3] == 6)
                            {
                                //—————————————————————————————————————————————————————
                                //********************************** 调水工程调入点
                                //—————————————————————————————————————————————————————
                                int iii = 0;
                                for (iii = 1; iii < com.Transferin_Node - 1; iii++)//从调入水点信息表中找到 该行信息
                                {
                                    if (com.Transferin_Node_Info[year, time, iii, 1] == com.River_Node_Info[i, 8])
                                    {
                                        break;
                                    }
                                }
                                //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                //'''''''''上游来水 + 调入量 *（1 - 损失系数0.1）       调水损失在此考虑！！！！
                                //''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                switch (iii)
                                {
                                    case 1:
                                        com.RiverQ[year, time, com.River_Node_Info[i, 1], i] = com.Transferin_Node_Info[year, time, iii, 2] * 0.92;    //引黑损失8%
                                        break;
                                    case 2:
                                        com.RiverQ[year, time, com.River_Node_Info[i, 1], i] = com.Transferin_Node_Info[year, time, iii, 2] * 0.9;    //引松损失10%
                                        break;
                                    default:
                                        if (com.Transferin_Node_Info[year, time, iii, 1] == 4)//万金山渠首调入
                                        {
                                            com.RiverQ[year, time, com.River_Node_Info[i, 1], i] = com.RiverQ[year, time, com.River_Node_Info[i, 1], i - 1] + com.Transferin_Node_Info[year, time, iii, 2];//上个节点水量+调水量
                                        }
                                        else//其他
                                        {
                                            com.RiverQ[year, time, com.River_Node_Info[i, 1], i] = com.Transferin_Node_Info[year, time, iii, 2];//挠力河一干、二干渠等其他的损失不在扣损
                                        }
                                        break;
                                }


                            }
                            else if (com.River_Node_Info[i, 3] == 3) //水库点
                            {
                                //暂不考虑水库作用，直接过流
                                com.RiverQ[year, time, com.River_Node_Info[i, 1], i] = com.RiverQ[year, time, com.River_Node_Info[i, 1], i - 1] * (1 - com.RiverK[year, time, j, i]); //河道损失
                            }
                        }
                    }
                    //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                    //***********************************  界河水水分配 (顺序5)
                    //______________________________________________________________________________________________________________

                    //-------------调用界河水分配模块（界河区域缺水直接有界河水来补给）
                    Data = new ComputingUnitBLL().GetTable();
                    int outputnumb = Data.Rows.Count + 1;

                    for (int j = 1; j < outputnumb; j++)
                    {
                        com.Boundaryriver[j] = Data.Rows[j - 1]["是否可用界河水"].ToInt();
                    }

                    for (int i = 1; i < com.Units_Numb; i++)
                    {
                        if (com.Boundaryriver[i] == 1)
                        {
                            for (int ii = 1; ii < com.Users; ii++)
                            {
                                com.Boundaryriver_Supply_Ture[year, time, i, ii] = com.Units_Waterneed[year, time, i, ii];  //缺水全部有界河水来补充

                                com.Units_Waterneed[year, time, i, ii] = 0;                                                //缺水得到补充
                            }
                        }
                    }
                }
            }
            //''''''''''''''' 计算完毕后，再计算水田和旱田的  原始 地表毛需水
            //'''''''''''''''
            for (int year = 1; year < com.Years; year++)
            {
                for (int time = 1; time < com.Times; time++)
                {
                    for (int j = 1; j < com.Units_Numb; j++)
                    {
                        //水田
                        if (com.unit_surfacewaterK[year, time, j, 0] > 0)
                        {
                            com.Units_WaterneedO[year, time, j, 4] = com.Units_WaterneedO[year, time, j, 4] - com.Groundwater_Supply_Ture[year, time, j, 4] * (com.unit_groundwaterK[year, time, j, 0] / com.unit_surfacewaterK[year, time, j, 0] - 1);
                        }
                    }
                }
            }

            //'''''''''''''''''' 优化配置结束！！

            //'---------------------------------------------------------------------------------------------------
            //'-************************************ 开始循环 整理结果 ******************************************
            //'---------------------------------------------------------------------------------------------------
            for (int year = 1; year < com.Years; year++)
            {
                for (int time = 1; time < com.Times; time++)
                {
                    //OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
                    //------------------------------------------------ 逐历时计算结果汇总分析 ----------------------------------
                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    //------------------------------------------------ 计算单元分析    在大循环算完 就逐个历时整理
                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    for (int i = 1; i < com.Units_Numb; i++)
                    {
                        for (int ii = 1; ii < com.Users; ii++)
                        {
                            //计算单元各个用户的缺水率
                            if (com.Units_WaterneedO[year, time, i, ii] == 0)//不需水的话就不缺水
                            {
                                com.units_water_usershortR[year, time, i, ii] = 0;
                            }
                            else
                            {
                                com.units_water_usershortR[year, time, i, ii] = com.units_water_usershortR[year, time, i, ii] / com.Units_WaterneedO[year, time, i, ii];
                            }
                            //计算单元总的缺水量
                            com.units_water_shortQ[year, time, i] = com.units_water_shortQ[year, time, i] + com.Units_Waterneed[year, time, i, ii];

                            //计算单元总的供水量
                            com.Units_Water_Supply[year, time, i] = com.Units_Water_Supply[year, time, i] + com.Units_WaterneedO[year, time, i, ii] - com.Units_Waterneed[year, time, i, ii];

                            //所有计算单各类用户总的缺水量
                            com.water_usershort[year, time, ii] = com.water_usershort[year, time, ii] + com.Units_Waterneed[year, time, i, ii];

                            //所有计算单各类用户总的缺水量原始值 = 需水量
                            com.water_usershortO[year, time, ii] = com.water_usershortO[year, time, ii] + com.Units_WaterneedO[year, time, i, ii];
                        }

                        if (com.units_water_shortQ[year, time, i] + com.Units_Water_Supply[year, time, i] > 0)
                        {
                            //计算单元缺水率
                            com.units_water_shortR[year, time, i] = com.units_water_shortQ[year, time, i] / (com.units_water_shortQ[year, time, i] + com.Units_Water_Supply[year, time, i]);
                        }
                        else
                        {
                            com.units_water_shortR[year, time, i] = 0;
                        }
                    }

                    for (int i = 1; i < com.Users; i++)
                    {
                        //各用户总的缺水率
                        if (com.water_usershortO[year, time, i] != 0)//判定是否缺水
                        {
                            com.water_usershortR[year, time, i] = com.water_usershort[year, time, i] / com.water_usershortO[year, time, i];
                        }
                        else
                        {
                            com.water_usershortR[year, time, i] = 0;//不缺水则缺水率是 0
                        }

                        //总的缺水量
                        com.water_usershort[year, time, com.Users] = com.water_usershort[year, time, com.Users] + com.water_usershort[year, time, i];
                        //总的缺水量-原始值
                        com.water_usershortO[year, time, com.Users] = com.water_usershortO[year, time, com.Users] + com.water_usershortO[year, time, i];
                    }
                    if (com.water_usershortO[year, time, com.Users] > 0)
                    {
                        com.water_usershortR[year, time, com.Users] = com.water_usershort[year, time, com.Users] / com.water_usershortO[year, time, com.Users];
                    }
                    else
                    {
                        com.water_usershortR[year, time, com.Users] = 0;
                    }
                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    //------------------------------------------------ 河流节点、水库供水分析 -----------------------------------------------------------
                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    //++++++++++++++++++ 河流节点分析 （按计算单元统计）
                    //+++++++++++++++++++++++++++++++++



                    //------------------------统计每个计算单元的河网水供水情况
                    for (int ii = 1; ii < com.Units_Numb; ii++)
                    {
                        for (int jj = 1; jj < com.Users; jj++)
                        {
                            for (int iii = 1; iii < com.River_Totalnode.GetUpperBound(1); iii++)//取数组的第2纬度的长度
                            {
                                com.riverwater_unitsuser_supply[year, time, ii, jj] = com.riverwater_unitsuser_supply[year, time, ii, jj] + com.Riverwater_Supply_Ture[year, time, iii, ii, jj];
                            }
                            com.riverwater_units_supply[year, time, ii] = com.riverwater_units_supply[year, time, ii] + com.riverwater_unitsuser_supply[year, time, ii, jj];
                        }
                    }

                    //'------------------------ 统计每个节点的供水情况(按节点统计) !
                    for (int ii = 1; ii < com.River_Totalnode.GetUpperBound(1); ii++)
                    {
                        //--- 供计算单元水量
                        for (int iii = 1; iii < com.Units_Numb; iii++)
                        {
                            for (int jj = 1; jj < com.Users; jj++)
                            {
                                com.Riverwater_Node_Supply[year, time, ii] = com.Riverwater_Node_Supply[year, time, ii] + com.Riverwater_Supply_Ture[year, time, ii, iii, jj];
                            }
                        }
                        //--- 供水库水量
                        for (int iii = 1; iii < com.Reservoir_Numb; iii++)
                        {
                            com.Riverwater_Node_Supply[year, time, ii] = com.Riverwater_Node_Supply[year, time, ii] + com.transfer_rivernode_reservoir[year, time, ii, iii];
                        }
                    }

                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    //++++++++++++++++++ 各个计算单元的水库供水量 （按计算单元统计）
                    //+++++++++++++++++++++++++++++++++


                    //------------------------统计每个计算单元水库水供水情况
                    for (int ii = 1; ii < com.Units_Numb; ii++)
                    {
                        for (int jj = 1; jj < com.Users; jj++)
                        {
                            for (int iii = 1; iii < com.reservoir_numb; iii++)
                            {
                                com.reservoirwater_unitsuser_supply[year, time, ii, jj] = com.reservoirwater_unitsuser_supply[year, time, ii, jj] + com.Reservoirwater_Supply_Ture[year, time, iii, ii, jj];
                            }
                            com.reservoirwater_units_supply[year, time, ii] = com.reservoirwater_units_supply[year, time, ii] + com.reservoirwater_unitsuser_supply[year, time, ii, jj];
                        }

                    }
                    //------------------------ 统计每个水库的供水情况(按水库统计) !
                    for (int ii = 1; ii < com.reservoir_numb; ii++)
                    {
                        for (int iii = 1; iii < com.Units_Numb; iii++)
                        {
                            for (int jj = 1; jj < com.Users; jj++)
                            {
                                com.reservoirwater_node_supply[year, time, ii, iii] = com.reservoirwater_node_supply[year, time, ii, iii] + com.Reservoirwater_Supply_Ture[year, time, ii, iii, jj];
                            }
                        }
                    }


                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    //-----------------------------------------------------------------------------------------------------------------------------
                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ 工程分区结果统计 +++++++++++++++++++++++++++++++++++++++++++
                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    for (int i = 1; i < com.Fenqus; i++)
                    {
                        for (int iii = 1; iii < com.Units_Numb; iii++)
                        {
                            if (com.fenqu_units[iii, 2] == i)
                            {
                                for (int ii = 1; ii < com.Users; ii++)
                                {
                                    //工程分区各个用户缺水量汇总
                                    com.fenqu_shortQ[year, time, i, ii] = com.fenqu_shortQ[year, time, i, ii] + com.Units_Waterneed[year, time, iii, ii];
                                    //工程分区各个用户总需水量
                                    com.fenqu_shortQO[year, time, i, ii] = com.fenqu_shortQO[year, time, i, ii] + com.Units_WaterneedO[year, time, iii, ii];
                                    //工程分区再生水供水量
                                    com.recycledwater_fenqu_supply[year, time, i] = com.recycledwater_fenqu_supply[year, time, i] + com.Recycledwater_Supply_Ture[year, time, iii, ii];
                                    //工程分区本地地表水供水量
                                    com.locatedwater_fenqu_supply[year, time, i] = com.locatedwater_fenqu_supply[year, time, i] + com.Locatedwater_Supply_Ture[year, time, iii, ii];
                                    //工程分区地下水供水量
                                    com.groundwater_fenqu_supply[year, time, i] = com.groundwater_fenqu_supply[year, time, i] + com.Groundwater_Supply_Ture[year, time, iii, ii];
                                    //工程分区界河水供水量
                                    com.boundaryriver_fenqu_supply[year, time, i] = com.boundaryriver_fenqu_supply[year, time, i] + com.Boundaryriver_Supply_Ture[year, time, iii, ii];
                                    //工程分区河网水供水量
                                    com.riverwater_fenqu_supply[year, time, i] = com.riverwater_fenqu_supply[year, time, i] + com.riverwater_unitsuser_supply[year, time, iii, ii];
                                    //工程分区水库水供水量
                                    com.reservoirwater_fenqu_supply[year, time, i] = com.reservoirwater_fenqu_supply[year, time, i] + com.reservoirwater_unitsuser_supply[year, time, iii, ii];
                                }
                            }
                        }
                        //统计各个计算单元总供水量
                        com.fenqu_supplyQ[year, time, i] = com.fenqu_supplyQ[year, time, i] + com.recycledwater_fenqu_supply[year, time, i] + com.locatedwater_fenqu_supply[year, time, i] + com.groundwater_fenqu_supply[year, time, i] + com.riverwater_fenqu_supply[year, time, i] + com.reservoirwater_fenqu_supply[year, time, i] + com.boundaryriver_fenqu_supply[year, time, i];
                    }

                    for (int i = 1; i < com.Users; i++)//汇总所有用户缺水
                    {
                        for (int ii = 1; ii < com.Fenqus + 1; ii++)
                        {
                            //各工程分区用户的缺水率
                            if (com.fenqu_shortQO[year, time, ii, i] > 0)
                            {
                                com.fenqu_shortR[year, time, ii, i] = com.fenqu_shortQ[year, time, ii, i] / com.fenqu_shortQO[year, time, ii, i];
                            }
                            else
                            {
                                com.fenqu_shortR[year, time, ii, i] = 0;
                            }
                            //-----------------------------------------各个工程分区
                            //各工程分区所有用户的缺水量
                            com.fenqu_shortQ[year, time, ii, com.Users] = com.fenqu_shortQ[year, time, ii, com.Users] + com.fenqu_shortQ[year, time, ii, i];
                            //各工程分区所有用户的总需水量
                            com.fenqu_shortQO[year, time, ii, com.Users] = com.fenqu_shortQO[year, time, ii, com.Users] + com.fenqu_shortQO[year, time, ii, i];
                            if (com.fenqu_shortQO[year, time, ii, com.Users] > 0)
                            {
                                //各工程分区所有用户的总缺水率
                                com.fenqu_shortR[year, time, ii, com.Users] = com.fenqu_shortQ[year, time, ii, com.Users] / com.fenqu_shortQO[year, time, ii, com.Users];
                            }
                            else
                            {
                                com.fenqu_shortR[year, time, ii, com.Users] = 0;
                            }
                            //-----------------------------------------所有工程分区汇总
                            //所有工程分区各用户的总缺水量
                            com.fenqu_shortQ[year, time, com.Fenqus, i] = com.fenqu_shortQ[year, time, com.Fenqus, i] + com.fenqu_shortQ[year, time, ii, i];

                            //所有工程分区各个用户的总需水量
                            com.fenqu_shortQO[year, time, com.Fenqus, i] = com.fenqu_shortQO[year, time, com.Fenqus, i] + com.fenqu_shortQO[year, time, ii, i];
                        }
                        //所有工程分区各个用户的总缺水率
                        if (com.fenqu_shortQO[year, time, com.Fenqus, i] > 0)
                        {
                            com.fenqu_shortR[year, time, com.Fenqus, i] = com.fenqu_shortQ[year, time, com.Fenqus, i] / com.fenqu_shortQO[year, time, com.Fenqus, i];
                        }
                        else
                        {
                            com.fenqu_shortR[year, time, com.Fenqus, i] = 0;
                        }
                    }

                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    //------------------------------------------------------------------------------------------------------------------------------
                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ O11计算单元结果统计 +++++++++++++++++++++++++++++++++++++++++
                    //------------------------------------------------------------------------------------------------------------------------------
                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    for (int i = 1; i < com.Units_Numb; i++)
                    {
                        for (int ii = 1; ii < com.Users; ii++)
                        {
                            com.recycledwater_unit_supply[year, time, i] = com.recycledwater_unit_supply[year, time, i] + com.Recycledwater_Supply_Ture[year, time, i, ii];
                            com.locatedwater_unit_supply[year, time, i] = com.locatedwater_unit_supply[year, time, i] + com.Locatedwater_Supply_Ture[year, time, i, ii];
                            com.boundaryriver_unit_supply[year, time, i] = com.boundaryriver_unit_supply[year, time, i] + com.Boundaryriver_Supply_Ture[year, time, i, ii];

                            com.riverwater_unit_supply[year, time, i] = com.riverwater_unit_supply[year, time, i] + com.riverwater_unitsuser_supply[year, time, i, ii];

                            com.reservoirwater_unit_supply[year, time, i] = com.reservoirwater_unit_supply[year, time, i] + com.reservoirwater_unitsuser_supply[year, time, i, ii];

                            com.units_waterneedsum[year, time, i] = com.units_waterneedsum[year, time, i] + com.Units_WaterneedO[year, time, i, ii];
                        }
                        com.SJLT_waterneed[year, time] = com.SJLT_waterneed[year, time] + com.units_waterneedsum[year, time, i];                     //整个三江连通工程区统计

                        com.SJLT_watersupply[year, time] = com.SJLT_watersupply[year, time] + com.Units_Water_Supply[year, time, i];

                        com.SJLT_watershortage[year, time] = com.SJLT_watershortage[year, time] + com.units_water_shortQ[year, time, i];

                        com.recycledwater_SJLT_supply[year, time] = com.recycledwater_SJLT_supply[year, time] + com.recycledwater_unit_supply[year, time, i];

                        com.locatedwater_SJLT_supply[year, time] = com.locatedwater_SJLT_supply[year, time] + com.locatedwater_unit_supply[year, time, i];

                        com.groundwater_SJLT_supply[year, time] = com.groundwater_SJLT_supply[year, time] + com.groundwater_unit_supply[year, time, i];

                        com.boundaryriver_SJLT_supply[year, time] = com.boundaryriver_SJLT_supply[year, time] + com.boundaryriver_unit_supply[year, time, i];

                        com.riverwater_SJLT_supply[year, time] = com.riverwater_SJLT_supply[year, time] + com.riverwater_unit_supply[year, time, i];

                        com.reservoirwater_SJLT_supply[year, time] = com.reservoirwater_SJLT_supply[year, time] + com.reservoirwater_unit_supply[year, time, i];
                    }

                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    //------------------------------------------------------------------------------------------------------------------------------
                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++  县级区结果统计 +++++++++++++++++++++++++++++++++++++++++++++
                    //------------------------------------------------------------------------------------------------------------------------------

                    for (int i = 1; i < com.County_Numb; i++)
                    {
                        for (int ii = 1; ii < com.Units_Numb; ii++)
                        {
                            if (com.county_units[ii, 2] == i)//统计各个县级区各个用户总的供水量 和 原始需水量
                            {
                                for (int iii = 1; iii < com.Users; iii++)
                                {
                                    com.county_short_ture[year, time, i, iii] = com.county_short_ture[year, time, i, iii] + com.Units_Waterneed[year, time, ii, iii];
                                    com.county_needO_ture[year, time, i, iii] = com.county_needO_ture[year, time, i, iii] + com.Units_WaterneedO[year, time, ii, iii];

                                    //统计各个水源向各个县级区供水量
                                    com.recycledwater_county_supply[year, time, i] = com.recycledwater_county_supply[year, time, i] + com.Recycledwater_Supply_Ture[year, time, ii, iii];

                                    com.locatedwater_county_supply[year, time, i] = com.locatedwater_county_supply[year, time, i] + com.Locatedwater_Supply_Ture[year, time, ii, iii];

                                    com.groundwater_county_supply[year, time, i] = com.groundwater_county_supply[year, time, i] + com.Groundwater_Supply_Ture[year, time, ii, iii];

                                    com.riverwater_county_supply[year, time, i] = com.riverwater_county_supply[year, time, i] + com.riverwater_unitsuser_supply[year, time, ii, iii];

                                    com.reservoirwater_county_supply[year, time, i] = com.reservoirwater_county_supply[year, time, i] + com.reservoirwater_unitsuser_supply[year, time, ii, iii];

                                    com.boundaryriver_county_supply[year, time, i] = com.boundaryriver_county_supply[year, time, i] + com.Boundaryriver_Supply_Ture[year, time, ii, iii];
                                }
                            }
                        }
                        for (int iii = 1; iii < com.Users; iii++) //统计各个县级区所有用户总的缺水量 和 原始需水量
                        {
                            //各个用户缺水率
                            if (com.county_needO_ture[year, time, i, iii] > 0)
                            {
                                com.county_shortR[year, time, i, iii] = com.county_short_ture[year, time, i, iii] / com.county_needO_ture[year, time, i, iii];
                            }
                            else
                            {
                                com.county_needO_ture[year, time, i, iii] = 0;
                            }
                            com.county_short_ture[year, time, i, com.Users] = com.county_short_ture[year, time, i, com.Users] + com.county_short_ture[year, time, i, iii];
                            com.county_needO_ture[year, time, i, com.Users] = com.county_needO_ture[year, time, i, com.Users] + com.county_needO_ture[year, time, i, iii];
                        }
                        if (com.county_needO_ture[year, time, i, com.Users] > 0)
                        {
                            //各个县级区总的缺水率
                            com.county_shortR[year, time, i, com.Users] = com.county_short_ture[year, time, i, com.Users] / com.county_needO_ture[year, time, i, com.Users];
                        }
                        else
                        {
                            com.county_shortR[year, time, i, com.Users] = 0;
                        }
                    }

                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    //------------------------------------------------------------------------------------------------------------------------------
                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++  地级区结果统计 +++++++++++++++++++++++++++++++++++++++++++++
                    //------------------------------------------------------------------------------------------------------------------------------

                    for (int i = 1; i < com.City_Numb; i++)
                    {
                        for (int ii = 1; ii < com.Units_Numb; ii++)
                        {
                            if (com.city_units[ii, 2] == i)//统计各个地级区各个用户总的供水量 和 原始需水量
                            {
                                for (int iii = 1; iii < com.Users; iii++)
                                {
                                    com.city_short_ture[year, time, i, iii] = com.city_short_ture[year, time, i, iii] + com.Units_Waterneed[year, time, ii, iii];
                                    com.city_needO_ture[year, time, i, iii] = com.city_needO_ture[year, time, i, iii] + com.Units_WaterneedO[year, time, ii, iii];

                                    //统计各个水源向各个地级区供水量
                                    com.recycledwater_city_supply[year, time, i] = com.recycledwater_city_supply[year, time, i] + com.Recycledwater_Supply_Ture[year, time, ii, iii];

                                    com.locatedwater_city_supply[year, time, i] = com.locatedwater_city_supply[year, time, i] + com.Locatedwater_Supply_Ture[year, time, ii, iii];

                                    com.groundwater_city_supply[year, time, i] = com.groundwater_city_supply[year, time, i] + com.Groundwater_Supply_Ture[year, time, ii, iii];

                                    com.riverwater_city_supply[year, time, i] = com.riverwater_city_supply[year, time, i] + com.riverwater_unitsuser_supply[year, time, ii, iii];

                                    com.reservoirwater_city_supply[year, time, i] = com.reservoirwater_city_supply[year, time, i] + com.reservoirwater_unitsuser_supply[year, time, ii, iii];

                                    com.boundaryriver_city_supply[year, time, i] = com.boundaryriver_city_supply[year, time, i] + com.Boundaryriver_Supply_Ture[year, time, ii, iii];
                                }
                            }
                        }
                        for (int iii = 1; iii < com.Users; iii++) //统计各个地级区所有用户总的缺水量 和 原始需水量
                        {
                            //各个用户缺水率
                            if (com.city_needO_ture[year, time, i, iii] != 0)
                            {
                                com.city_shortR[year, time, i, iii] = com.city_short_ture[year, time, i, iii] / com.city_needO_ture[year, time, i, iii];
                            }
                            else
                            {
                                com.city_needO_ture[year, time, i, iii] = 0;                      //如果需水为0 则缺水率为0
                            }
                            com.city_short_ture[year, time, i, com.Users] = com.city_short_ture[year, time, i, com.Users] + com.city_short_ture[year, time, i, iii];
                            com.city_needO_ture[year, time, i, com.Users] = com.city_needO_ture[year, time, i, com.Users] + com.city_needO_ture[year, time, i, iii];
                        }
                        if (com.city_needO_ture[year, time, i, com.Users] > 0)
                        {
                            //各个县级区总的缺水率
                            com.city_shortR[year, time, i, com.Users] = com.city_short_ture[year, time, i, com.Users] / com.city_needO_ture[year, time, i, com.Users];
                        }
                        else
                        {
                            com.city_shortR[year, time, i, com.Users] = 0;
                        }
                    }

                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    //-------------------------------------------------------------------------------------------------------------------------------
                    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++  省级区结果统计 ++++++++++++++++++++++++++++++++++++++++++++++
                    //-------------------------------------------------------------------------------------------------------------------------------
                    for (int i = 1; i < com.Province_Numb; i++)
                    {
                        for (int ii = 1; ii < com.Units_Numb; ii++)
                        {
                            if (com.province_units[ii, 2] == i) //统计各个省级区各个用户总的供水量 和 原始需水量
                            {
                                for (int iii = 1; iii < com.Users; iii++)
                                {
                                    com.province_short_ture[year, time, i, iii] = com.province_short_ture[year, time, i, iii] + com.Units_Waterneed[year, time, ii, iii];
                                    com.province_needO_ture[year, time, i, iii] = com.province_needO_ture[year, time, i, iii] + com.Units_WaterneedO[year, time, ii, iii];

                                    //统计各个水源向各个省级区供水量
                                    com.recycledwater_province_supply[year, time, i] = com.recycledwater_province_supply[year, time, i] + com.Recycledwater_Supply_Ture[year, time, ii, iii];

                                    com.locatedwater_province_supply[year, time, i] = com.locatedwater_province_supply[year, time, i] + com.Locatedwater_Supply_Ture[year, time, ii, iii];

                                    com.groundwater_province_supply[year, time, i] = com.groundwater_province_supply[year, time, i] + com.Groundwater_Supply_Ture[year, time, ii, iii];

                                    com.riverwater_province_supply[year, time, i] = com.riverwater_province_supply[year, time, i] + com.riverwater_unitsuser_supply[year, time, ii, iii];

                                    com.reservoirwater_province_supply[year, time, i] = com.reservoirwater_province_supply[year, time, i] + com.reservoirwater_unitsuser_supply[year, time, ii, iii];

                                    com.boundaryriver_province_supply[year, time, i] = com.boundaryriver_province_supply[year, time, i] + com.Boundaryriver_Supply_Ture[year, time, ii, iii];
                                }
                            }
                        }
                        for (int iii = 1; iii < com.Users; iii++) //统计各个省级区所有用户总的缺水量 和 原始需水量
                        {
                            //各个用户缺水率
                            if (com.province_needO_ture[year, time, i, iii] != 0)
                            {
                                com.province_shortR[year, time, i, iii] = com.province_short_ture[year, time, i, iii] / com.province_needO_ture[year, time, i, iii];
                            }
                            else
                            {
                                com.province_needO_ture[year, time, i, iii] = 0; //如果需水为0 则缺水率为0
                            }
                            com.province_short_ture[year, time, i, com.Users] = com.province_short_ture[year, time, i, com.Users] + com.province_short_ture[year, time, i, iii];
                            com.province_needO_ture[year, time, i, com.Users] = com.province_needO_ture[year, time, i, com.Users] + com.province_needO_ture[year, time, i, iii];
                        }
                        //各个省级区总的缺水率
                        if (com.province_needO_ture[year, time, i, com.Users] > 0)
                        {
                            com.province_shortR[year, time, i, com.Users] = com.province_short_ture[year, time, i, com.Users] / com.province_needO_ture[year, time, i, com.Users];
                        }
                        else
                        {
                            com.province_shortR[year, time, i, com.Users] = 0;
                        }
                    }
                }
            }
            //OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
            //OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO                              OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
            //OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO       年值统计及保证率       OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
            //OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO                              OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
            //OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
            //O11 计算单元  定义中间变量
            long[,] qs1 = new long[com.Units_Numb, com.Users];//统计缺水次数，为计算保证率做准备
            long[,] qs2 = new long[com.Units_Numb, com.Users];
            double s1 = 0;
            double s2 = 0;
            double s3 = 0;
            double s4 = 0;
            double s5 = 0;
            double s6 = 0;
            double s7 = 0;
            double s8 = 0;
            double s9 = 0;
            double s10 = 0;
            double s11 = 0;
            //各计算单元 各用水户 年值统计
            for (int i = 1; i < com.Units_Numb; i++)
            {
                for (int j = 1; j < com.Users; j++)
                {
                    for (int year = 1; year < com.Years; year++)
                    {
                        for (int time = 1; time < com.Times; time++)
                        {
                            s1 = s1 + com.Units_Waterneed[year, time, i, j];  // 逆序计算，最后的需水为缺水

                            s2 = s2 + com.Units_WaterneedO[year, time, i, j];  //  原始需水
                            if (com.units_water_usershortR[year, time, i, j] > 0.001)//统计各历时 缺水的个数 即不满足需求 0.1%
                            {
                                qs1[i, j] = qs1[i, j] + 1;
                            }
                        }

                        com.unitsusers_watershortQY[year, i, j] = s1;

                        com.unitsusers_waterneedOY[year, i, j] = s2;
                        if (com.unitsusers_waterneedOY[year, i, j] > 0)
                        {
                            com.unitsusers_watershortRY[year, i, j] = com.unitsusers_watershortQY[year, i, j] / com.unitsusers_waterneedOY[year, i, j];
                        }
                        else
                        {
                            com.unitsusers_watershortRY[year, i, j] = 0;   //如果不需水的话缺水率直接为 0
                        }
                        if (com.unitsusers_watershortRY[year, i, j] > 0.001)//统计各年 缺水的个数
                        {
                            qs2[i, j] = qs2[i, j] + 1;
                        }
                        s1 = 0;
                        s2 = 0;
                    }

                    com.unitsusers_waterPtime[i, j] = ((com.Years - 1) * (com.Times - 1) - qs1[i, j]) / ((com.Years - 1) * (com.Times - 1)); //历时保证率      减去缺水的就是不缺水的了

                    com.unitsusers_waterPyear[i, j] = ((com.Years - 1) - qs2[i, j]) / (com.Years); // 年保证率
                    s1 = 0;
                    s2 = 0;
                    s3 = 0;
                    s4 = 0;
                }
            }
            //统计 所有计算单元 加和 起来的 保证率
            double ss1 = 0;
            double ss2 = 0;
            long[] qss1 = new long[com.Users];
            long[] qss2 = new long[com.Users];
            //整个区域 各个用水户的 历时和年保证率，   整个区域的 下面计算
            for (int j = 1; j < com.Users; j++)
            {
                for (int i = 1; i < com.Units_Numb; i++)
                {
                    qss1[j] = qss1[j] + qs1[i, j];   //各个单元不同用水户 缺水的 历时 加和
                    qss2[j] = qss2[j] + qs2[i, j];    //各个单元不同用水户 缺水的 年 加和
                }
                com.unitsum_user_Ptime[j] = ((com.Units_Numb - 1) * (com.Years - 1) * (com.Times - 1) - qss1[j]) / ((com.Units_Numb - 1) * (com.Years - 1) * (com.Times - 1) + 1); //这里+1存在一定的问题，导致面上比单个计算单元的保证率高
                com.unitsum_user_Pyear[j] = ((com.Units_Numb - 1) * (com.Years - 1) - qss2[j]) / ((com.Units_Numb - 1) * (com.Years - 1) + 1);  // 这里考虑 是否 改为 单元数+1
            }
            //所有灌区 各个用水户的 历时和年保证率，   所有灌区平均的 下面计算
            for (int j = 1; j < com.Users; j++)
            {
                qss1[j] = 0;
                qss2[j] = 0;
                for (int i = 1; i < com.guanqu_numb + 1; i++)//不同方案，灌区的个数不一样
                {
                    qss1[j] = qss1[j] + qs1[i, j];//各个单元不同用水户 缺水的 历时 加和
                    qss2[j] = qss2[j] + qs2[i, j];//各个单元不同用水户 缺水的 年 加和
                }
                com.unitsum_user_Ptime[j] = ((com.Units_Numb - 1) * (com.Years - 1) * (com.Times - 1) - qss1[j]) / ((com.Units_Numb - 1) * (com.Years - 1) * (com.Times - 1) + 1); //这里+1存在一定的问题，导致面上比单个计算单元的保证率高
                com.unitsum_user_Pyear[j] = ((com.Units_Numb - 1) * (com.Years - 1) - qss2[j]) / ((com.Units_Numb - 1) * (com.Years - 1) + 1);  // 这里考虑 是否 改为 单元数+1
            }
            long[] qs3 = new long[com.Units_Numb];
            long[] qs4 = new long[com.Units_Numb];
            s1 = 0;
            s2 = 0;
            s3 = 0;
            s4 = 0;
            s5 = 0;
            s6 = 0;
            s7 = 0;
            s8 = 0;
            s9 = 0;
            s10 = 0;
            s11 = 0;
            for (int ii = 1; ii < com.Units_Numb; ii++)
            {
                for (int year = 1; year < com.Years; year++)
                {
                    for (int time = 1; time < com.Times; time++)
                    {
                        s3 = s3 + com.Units_Water_Supply[year, time, ii];
                        s4 = s4 + com.units_water_shortQ[year, time, ii];
                        s5 = s5 + com.units_waterneedsum[year, time, ii];
                        if (com.units_water_shortR[year, time, ii] > 0.001)
                        {
                            qs3[ii] = qs3[ii] + 1;
                        }
                        s6 = s6 + com.locatedwater_unit_supply[year, time, ii];
                        s7 = s7 + com.recycledwater_unit_supply[year, time, ii];
                        s8 = s8 + com.groundwater_unit_supply[year, time, ii];
                        s9 = s9 + com.riverwater_unit_supply[year, time, ii];
                        s10 = s10 + com.reservoirwater_unit_supply[year, time, ii];
                        s11 = s11 + com.boundaryriver_unit_supply[year, time, ii];
                    }
                    com.units_watersupplyY[year, ii] = s3;
                    com.units_watershortQY[year, ii] = s4;
                    com.units_waterneedOY[year, ii] = s5;
                    if (s5 > 0)//计算单元综合平均缺水率
                    {
                        com.units_watershortRY[year, ii] = s4 / s5;
                    }
                    else
                    {
                        com.units_watershortRY[year, ii] = 0;
                    }
                    if (com.units_watershortRY[year, ii] > 0.001)//缺水年数  都用 0.1%
                    {
                        qs4[ii] += 1;
                    }
                    com.units_locatedwater_supplyY[year, ii] = s6;
                    com.units_recycledwater_supplyY[year, ii] = s7;
                    com.units_groundwater_supplyY[year, ii] = s8;
                    com.units_riverwater_supplyY[year, ii] = s9;
                    com.units_reservoirdwater_supplyY[year, ii] = s10;
                    com.units_boundaryriver_supplyY[year, ii] = s11;

                    s1 = s1 + s3;  //累计供水量
                    s2 = s2 + s5;  //总的需水量

                    s3 = 0;
                    s4 = 0;
                    s5 = 0;
                    s6 = 0;
                    s7 = 0;
                    s8 = 0;
                    s9 = 0;
                    s10 = 0;
                    s11 = 0;
                }
                com.units_waterPtime[ii] = ((com.Years - 1) * (com.Years - 1) - qs3[ii]) / ((com.Years - 1) * (com.Years - 1) + 1);     //单元综合平均保证率

                com.units_waterPyear[ii] = ((com.Years - 1) - qs4[ii]) / ((com.Years - 1) + 1);


                ss1 = ss1 + qs3[ii];    //整个区域 所有用水户 缺水的 历时数 加和
                ss2 = ss2 + qs4[ii];    //整个区域 所有用水户 缺水的 年数 加和

                s1 = 0;
                s2 = 0;
                s3 = 0;
                s4 = 0;
                s5 = 0;
                s6 = 0;
                s7 = 0;
                s8 = 0;
                s9 = 0;
                s10 = 0;
                s11 = 0;
            }
            com.unitsum_Ptime = ((com.Units_Numb - 1) * (com.Years - 1) * (com.Times - 1) - ss1) / ((com.Units_Numb - 1) * (com.Years - 1) * (com.Times - 1) + 1); //整个区域的 历时 保证率
            com.unitsum_Pyear = ((com.Units_Numb - 1) * (com.Years - 1) - ss2) / ((com.Units_Numb - 1) * (com.Years - 1) + 1); //整个区域的 年 保证率
            //所有灌区 的 综合平均保证率
            for (int i = 1; i < com.guanqu_numb + 1; i++) //不同方案，灌区的个数不一样
            {
                ss1 = ss1 + qs3[i];      //整个区域 所有用水户 缺水的 历时数 加和
                ss2 = ss2 + qs4[i];      //整个区域 所有用水户 缺水的 年数 加和
            }
            com.guanqu_Ptime = (com.guanqu_numb * (com.Years - 1) * (com.Times - 1) - ss1) / (com.guanqu_numb * (com.Years - 1) * (com.Times - 1) + 1); //这里+1存在一定的问题，导致所有灌区比单个灌区的保证率高
            com.guanqu_Pyear = (com.guanqu_numb * (com.Years - 1) - ss2) / (com.guanqu_numb * (com.Years - 1) + 1);  // 这里考虑 是否 改为 单元数+1
            double[,] units_groundwater_xuefengY = new double[com.Years, com.Units_Numb];//地下水削峰比例
            double[,] units_groundwater_shiyongY = new double[com.Years, com.Units_Numb];//地下水开采比例

            //地下水使用情况统计  削峰和 使用
            for (int ii = 1; ii < com.Units_Numb; ii++)
            {
                for (int year = 1; year < com.Years; year++)
                {
                    for (int time = 1; time < com.Times; time++)
                    {
                        if (com.units_waterneedOY[year, ii] > 0)
                        {
                            units_groundwater_xuefengY[year, ii] = com.units_groundwater_supplyY[year, ii] / com.units_waterneedOY[year, ii];
                        }
                        else
                        {
                            units_groundwater_xuefengY[year, ii] = 0;
                        }
                        if (com.User_Availability_Groundwater[year, 1, ii] > 0)
                        {
                            units_groundwater_shiyongY[year, ii] = com.units_groundwater_supplyY[year, ii] / com.User_Availability_Groundwater[year, 1, ii];
                        }
                        else
                        {
                            units_groundwater_shiyongY[year, ii] = 0;
                        }
                    }
                }
            }
            //整个三江连通区   各 水源 年值统计
            int qs9 = 0;
            int qs10 = 0;
            s1 = 0;
            s2 = 0;
            s3 = 0;
            s4 = 0;
            s5 = 0;
            s6 = 0;
            s7 = 0;
            s8 = 0;
            s9 = 0;
            for (int year = 1; year < com.Years; year++)
            {
                for (int time = 1; time < com.Times; time++)
                {
                    s1 = s1 + com.SJLT_waterneed[year, time];
                    s2 = s2 + com.SJLT_watersupply[year, time];
                    s3 = s3 + com.SJLT_watershortage[year, time];
                    if (com.water_usershortR[year, time, com.Users] > 0.001)//区域缺水率小于0.1%视为不缺水
                    {
                        qs9 = qs9 + 1;
                    }
                    s4 = s4 + com.locatedwater_SJLT_supply[year, time];
                    s5 = s5 + com.recycledwater_SJLT_supply[year, time];
                    s6 = s6 + com.groundwater_SJLT_supply[year, time];
                    s7 = s7 + com.riverwater_SJLT_supply[year, time];
                    s8 = s8 + com.reservoirwater_SJLT_supply[year, time];
                    s9 = s9 + com.boundaryriver_SJLT_supply[year, time];
                    com.SJLT_waterneedY[year] = s1;              //三江连通规划区年总需水量年值
                    com.SJLT_watersupplyY[year] = s2;//三江连通规划区年总供水量年值
                    com.SJLT_watershortY[year] = s3;//三江连通规划区年总缺水量年值

                    com.locatedwater_SJLT_supplyY[year] = s4;//三江连通规划区本地水总供水量
                    com.recycledwater_SJLT_supplyY[year] = s5; //三江连通规划区再生水总供水量
                    com.groundwater_SJLT_supplyY[year] = s6; //三江连通规划区地下水总供水量
                    com.riverwater_SJLT_supplyY[year] = s7; //三江连通规划区河网（引提水）总供水量
                    com.reservoirwater_SJLT_supplyY[year] = s8; //三江连通规划区水库水总供水量
                    com.boundaryriver_SJLT_supplyY[year] = s9; //三江连通规划区界河水总供水量
                    if (s1 > 0)
                    {
                        com.SJLT_watershortRY[year] = s3 / s1;
                    }
                    else
                    {
                        com.SJLT_watershortRY[year] = 0;
                    }
                    if (com.SJLT_watershortRY[year] > 0.001)//区域缺水率小于0.1%视为不缺水
                    {
                        qs10 = qs10 + 1;
                    }
                    //整个三江连通工程区 年值 再累加求和
                    com.SJLT_waterneedYsum = com.SJLT_waterneedYsum + s1;

                    com.SJLT_watersupplyYsum = com.SJLT_watersupplyYsum + s2;

                    com.SJLT_watershortYsum = com.SJLT_watershortYsum + s3;

                    com.locatedwater_SJLT_supplyYsum = com.locatedwater_SJLT_supplyYsum + s4;
                    com.recycledwater_SJLT_supplyYsum = com.recycledwater_SJLT_supplyYsum + s5;
                    com.groundwater_SJLT_supplyYsum = com.groundwater_SJLT_supplyYsum + s6;
                    com.riverwater_SJLT_supplyYsum = com.riverwater_SJLT_supplyYsum + s7;
                    com.reservoirwater_SJLT_supplyYsum = com.reservoirwater_SJLT_supplyYsum + s8;
                    com.boundaryriver_SJLT_supplyYsum = com.boundaryriver_SJLT_supplyYsum + s9;

                    s1 = 0;
                    s2 = 0;
                    s3 = 0;
                    s4 = 0;
                    s5 = 0;
                    s6 = 0;
                    s7 = 0;
                    s8 = 0;
                    s9 = 0;
                }
            }
            com.SJLT_Ptime = ((com.Times - 1) * (com.Years - 1) - qs9) / ((com.Times - 1) * (com.Years - 1) + 1);    //三江连通面上的保证率 （每个历时内 任意区域任意时刻缺了任意量的水都算缺水）
            com.SJLT_Pyear = ((com.Years - 1) - qs10) / ((com.Years - 1) + 1);

            com.SJLT_watershortRYsum = com.SJLT_watershortYsum / com.SJLT_waterneedYsum;    //整体缺水率
            int[] qs11 = new int[com.Users];
            int[] qs12 = new int[com.Users];
            //历时需单独计算 之前直接在单元里的
            for (int i = 0; i < com.Users; i++)
            {
                for (int year = 1; year < com.Years; year++)
                {
                    for (int time = 1; time < com.Times; time++)
                    {
                        if (com.water_usershortR[year, time, i] > 0.001)//只要整个区域有任意一个地方缺水，就算该区域在该历时缺水！！缺水率小于0.1%
                        {
                            qs11[i] = qs11[i] + 1;
                        }
                    }
                }
                com.SJLT_users_Ptime[i] = ((com.Times - 1) * (com.Years - 1) - qs11[i]) / ((com.Times - 1) * (com.Years - 1) + 1);       // 三江连通面上区域 各个用户 历时保证率
            }
            //年保证率
            for (int year = 1; year < com.Years; year++)
            {
                for (int i = 1; i < com.Users; i++)
                {
                    for (int ii = 1; ii < com.Units_Numb; ii++)
                    {
                        com.SJLT_user_watershortQY[year, i] = com.SJLT_user_watershortQY[year, i] + com.unitsusers_watershortQY[year, ii, i];

                        com.SJLT_user_waterneedOY[year, i] = com.SJLT_user_waterneedOY[year, i] + com.unitsusers_waterneedOY[year, ii, i];
                        if (com.SJLT_user_waterneedOY[year, i] > 0)
                        {
                            com.SJLT_user_watershortRY[year, i] = com.SJLT_user_watershortQY[year, i] / com.SJLT_user_waterneedOY[year, i];
                        }
                        else
                        {
                            com.SJLT_user_watershortRY[year, i] = 0;
                        }
                    }

                    com.SJLT_user_watershortQYsum[i] = com.SJLT_user_watershortQYsum[i] + com.SJLT_user_watershortQY[year, i];
                    com.SJLT_user_waterneedOYsum[i] = com.SJLT_user_waterneedOYsum[i] + com.SJLT_user_waterneedOY[year, i];
                    if (com.SJLT_user_waterneedOYsum[i] > 0)
                    {
                        com.SJLT_user_watershortRYsum[i] = com.SJLT_user_watershortQYsum[i] / com.SJLT_user_waterneedOYsum[i];
                    }
                    else
                    {
                        com.SJLT_user_watershortRYsum[i] = 0;
                    }
                }
            }
            //整个三江连通工程区 面上 各个用户 年 保证率
            for (int i = 1; i < com.Users; i++)
            {
                for (int year = 1; year < com.Years; year++)
                {
                    if (com.SJLT_user_watershortRY[year, i] > 0.001)//缺水率小于0.1%
                    {
                        qs12[i] = qs12[i] + 1;
                    }
                }
                com.SJLT_users_Pyear[i] = ((com.Years - 1) - qs12[i]) / (com.Years);
            }
            //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            //'''''''''''''''''''''''''''''''''''''''''工程分区 年值统计
            //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            //各工程分区 分水源供水量 年值统计
            double[] recycledwater_fenqu_supplyY_avg = new double[com.Fenqus];
            double[] locatedwater_fenqu_supplyY_avg = new double[com.Fenqus];
            double[] groundwater_fenqu_supplyY_avg = new double[com.Fenqus];
            double[] boundaryriver_fenqu_supplyY_avg = new double[com.Fenqus];
            double[] riverwater_fenqu_supplyY_avg = new double[com.Fenqus];
            double[] reservoirwater_fenqu_supplyY_avg = new double[com.Fenqus];
            s1 = 0;
            s2 = 0;
            s3 = 0;
            s4 = 0;
            s5 = 0;
            s6 = 0;
            s7 = 0;
            s8 = 0;
            s9 = 0;
            s10 = 0;
            s11 = 0;
            double s12 = 0;
            for (int i = 1; i < com.Fenqus; i++)
            {
                for (int year = 1; year < com.Years; year++)
                {
                    for (int time = 1; time < com.Times; time++)
                    {
                        //工程分区再生水供水量
                        s1 = s1 + com.recycledwater_fenqu_supply[year, time, i];

                        //工程分区本地地表水供水量
                        s2 = s2 + com.locatedwater_fenqu_supply[year, time, i];

                        //工程分区地下水供水量
                        s3 = s3 + com.groundwater_fenqu_supply[year, time, i];

                        //工程分区界河水供水量
                        s4 = s4 + com.boundaryriver_fenqu_supply[year, time, i];

                        //工程分区河网水供水量
                        s5 = s5 + com.riverwater_fenqu_supply[year, time, i];

                        //工程分区水库水供水量
                        s6 = s6 + com.reservoirwater_fenqu_supply[year, time, i];
                    }
                    //工程分区再生水供水量
                    com.recycledwater_fenqu_supplyY[year, i] = s1;

                    //工程分区本地地表水供水量
                    com.locatedwater_fenqu_supplyY[year, i] = s2;

                    //工程分区地下水供水量
                    com.groundwater_fenqu_supplyY[year, i] = s3;

                    //工程分区界河水供水量
                    com.boundaryriver_fenqu_supplyY[year, i] = s4;

                    //工程分区河网水供水量
                    com.riverwater_fenqu_supplyY[year, i] = s5;

                    //工程分区水库水供水量
                    com.reservoirwater_fenqu_supplyY[year, i] = s6;

                    s7 = s7 + s1;
                    s8 = s8 + s2;
                    s9 = s9 + s3;
                    s10 = s10 + s4;
                    s11 = s11 + s5;
                    s12 = s12 + s6;

                    s1 = 0;
                    s2 = 0;
                    s3 = 0;
                    s4 = 0;
                    s5 = 0;
                    s6 = 0;
                }
                s1 = 0;
                s2 = 0;
                s3 = 0;
                s4 = 0;
                s5 = 0;
                s6 = 0;

                recycledwater_fenqu_supplyY_avg[i] = s7 / (com.Years - 1);
                locatedwater_fenqu_supplyY_avg[i] = s8 / (com.Years - 1);
                groundwater_fenqu_supplyY_avg[i] = s9 / (com.Years - 1);
                boundaryriver_fenqu_supplyY_avg[i] = s10 / (com.Years - 1);
                riverwater_fenqu_supplyY_avg[i] = s11 / (com.Years - 1);
                reservoirwater_fenqu_supplyY_avg[i] = s12 / (com.Years - 1);

                s7 = 0;
                s8 = 0;
                s9 = 0;
                s10 = 0;
                s11 = 0;
                s12 = 0;
            }
            //各工程分区 分用水户 年值统计
            double[,] fenqu_users_shortQY_avg = new double[com.Fenqus, com.Users + 1];         //各个工程分区各用户年缺水量的年平均
            double[,] fenqu_users_shortQOY_avg = new double[com.Fenqus, com.Users + 1];        //各个工程分区各用户原始需水量的年平均
            double[,] fenqu_users_shortRY_avg = new double[com.Fenqus, com.Users + 1];
            double[,] qs5 = new double[com.Fenqus, com.Users + 1];
            double[,] qs6 = new double[com.Fenqus, com.Users + 1];
            s1 = 0;
            s2 = 0;
            s3 = 0;
            s4 = 0;
            s5 = 0;
            s6 = 0;
            s7 = 0;
            s8 = 0;
            for (int i = 1; i < com.Fenqus; i++)
            {
                for (int ii = 1; ii < com.Users + 1; ii++)
                {
                    for (int year = 1; year < com.Years; year++)
                    {
                        for (int time = 1; time < com.Times; time++)
                        {
                            s1 = s1 + com.fenqu_shortQ[year, time, i, ii];
                            s2 = s2 + com.fenqu_shortQO[year, time, i, ii];   //需水量累加
                        }
                        com.fenqu_users_shortQY[year, i, ii] = s1;           //各个工程分区各用户年缺水量  ' + 1'为了存储所有工程分区统计量
                        com.fenqu_users_shortQOY[year, i, ii] = s2;          //各个工程分区各用户原始需水量
                        if (s2 > 0)
                        {
                            com.fenqu_users_shortRY[year, i, ii] = s1 / s2;          //各个工程分区各用户缺水率
                        }
                        else
                        {
                            com.fenqu_users_shortRY[year, i, ii] = 0;
                        }
                        qs5[i, ii] = qs5[i, ii] + s1;
                        qs6[i, ii] = qs6[i, ii] + s2;
                        s1 = 0;
                        s2 = 0;
                    }
                    fenqu_users_shortQY_avg[i, ii] = qs5[i, ii] / (com.Years - 1);
                    fenqu_users_shortQOY_avg[i, ii] = qs6[i, ii] / (com.Years - 1);
                    if (qs6[i, ii] > 0)
                    {
                        fenqu_users_shortRY_avg[i, ii] = qs5[i, ii] / qs6[i, ii];
                    }
                    else
                    {
                        fenqu_users_shortRY_avg[i, ii] = 0;
                    }
                    s1 = 0;
                    s2 = 0;
                    s3 = 0;
                    s4 = 0;
                    s5 = 0;
                    s6 = 0;
                    qs5[i, ii] = 0;
                    qs6[i, ii] = 0;
                }

            }
            //县级区 各水源 年值统计
            s1 = 0;
            s2 = 0;
            s3 = 0;
            s4 = 0;
            s5 = 0;
            s6 = 0;
            s7 = 0;
            s8 = 0;
            for (int i = 1; i < com.County_Numb; i++)
            {
                for (int year = 1; year < com.Years; year++)
                {
                    for (int time = 1; time < com.Times; time++)
                    {
                        s1 = s1 + com.recycledwater_county_supply[year, time, i];

                        s2 = s2 + com.locatedwater_county_supply[year, time, i];

                        s3 = s3 + com.groundwater_county_supply[year, time, i];

                        s4 = s4 + com.riverwater_county_supply[year, time, i];

                        s5 = s5 + com.reservoirwater_county_supply[year, time, i];

                        s6 = s6 + com.boundaryriver_county_supply[year, time, i];
                    }
                    com.recycledwater_county_supplyY[year, i] = s1;

                    com.locatedwater_county_supplyY[year, i] = s2;

                    com.groundwater_county_supplyY[year, i] = s3;

                    com.riverwater_county_supplyY[year, i] = s4;

                    com.reservoirwater_county_supplyY[year, i] = s5;

                    com.boundaryriver_county_supplyY[year, i] = s6;
                    s1 = 0;
                    s2 = 0;
                    s3 = 0;
                    s4 = 0;
                    s5 = 0;
                    s6 = 0;
                    s7 = 0;
                    s8 = 0;
                    s9 = 0;
                }
                s1 = 0;
                s2 = 0;
                s3 = 0;
                s4 = 0;
                s5 = 0;
                s6 = 0;
                s7 = 0;
                s8 = 0;
            }
            //县级区 各用水户 年值统计
            s1 = 0;
            s2 = 0;
            for (int i = 1; i < com.County_Numb; i++)
            {
                for (int iii = 1; iii < com.Users + 1; iii++)
                {
                    for (int year = 1; year < com.Years; year++)
                    {
                        for (int time = 1; time < com.Times; time++)
                        {
                            s1 = s1 + com.county_short_ture[year, time, i, iii];
                            s2 = s2 + com.county_needO_ture[year, time, i, iii];
                        }
                        com.county_short_tureY[year, i, iii] = s1;
                        com.county_needO_tureY[year, i, iii] = s2;
                        if (s2 > 0)
                        {
                            com.county_shortRY[year, i, iii] = s1 / s2;
                        }
                        else
                        {
                            com.county_shortRY[year, i, iii] = 0;
                        }
                        s1 = 0;
                        s2 = 0;
                    }
                    s1 = 0;
                    s2 = 0;
                }
            }
            //地级区 各水源 年值统计
            s1 = 0;
            s2 = 0;
            s3 = 0;
            s4 = 0;
            s5 = 0;
            s6 = 0;
            for (int i = 1; i < com.City_Numb; i++)
            {
                for (int year = 1; year < com.Years; year++)
                {
                    for (int time = 1; time < com.Times; time++)
                    {
                        s1 = s1 + com.recycledwater_city_supply[year, time, i];

                        s2 = s2 + com.locatedwater_city_supply[year, time, i];

                        s3 = s3 + com.groundwater_city_supply[year, time, i];

                        s4 = s4 + com.riverwater_city_supply[year, time, i];

                        s5 = s5 + com.reservoirwater_city_supply[year, time, i];

                        s6 = s6 + com.boundaryriver_city_supply[year, time, i];
                    }
                    com.recycledwater_city_supplyY[year, i] = s1;

                    com.locatedwater_city_supplyY[year, i] = s2;

                    com.groundwater_city_supplyY[year, i] = s3;

                    com.riverwater_city_supplyY[year, i] = s4;

                    com.reservoirwater_city_supplyY[year, i] = s5;

                    com.boundaryriver_city_supplyY[year, i] = s6;
                    s1 = 0;
                    s2 = 0;
                    s3 = 0;
                    s4 = 0;
                    s5 = 0;
                    s6 = 0;
                    s7 = 0;
                    s8 = 0;
                    s9 = 0;
                }
                s1 = 0;
                s2 = 0;
                s3 = 0;
                s4 = 0;
                s5 = 0;
                s6 = 0;
                s7 = 0;
                s8 = 0;
                s9 = 0;
            }
            //地级区 各用水户 年值统计
            s1 = 0;
            s2 = 0;
            for (int i = 1; i < com.City_Numb; i++)
            {
                for (int iii = 1; iii < com.Users + 1; iii++)
                {
                    for (int year = 1; year < com.Years; year++)
                    {
                        for (int time = 1; time < com.Times; time++)
                        {
                            s1 = s1 + com.city_short_ture[year, time, i, iii];
                            s2 = s2 + com.city_needO_ture[year, time, i, iii];
                        }
                        com.city_short_tureY[year, i, iii] = s1;
                        com.city_needO_tureY[year, i, iii] = s2;
                        if (s2 > 0)
                        {
                            com.city_shortRY[year, i, iii] = s1 / s2;
                        }
                        else
                        {
                            com.city_shortRY[year, i, iii] = 0;
                        }
                    }
                    s1 = 0;
                    s2 = 0;
                }
            }
            //省级区 各水源 年值统计
            s1 = 0;
            s2 = 0;
            s3 = 0;
            s4 = 0;
            s5 = 0;
            s6 = 0;
            for (int i = 1; i < com.Province_Numb; i++)
            {
                for (int year = 1; year < com.Years; year++)
                {
                    for (int time = 1; time < com.Times; time++)
                    {
                        s1 = s1 + com.recycledwater_province_supply[year, time, i];

                        s2 = s2 + com.locatedwater_province_supply[year, time, i];

                        s3 = s3 + com.groundwater_province_supply[year, time, i];

                        s4 = s4 + com.riverwater_province_supply[year, time, i];

                        s5 = s5 + com.reservoirwater_province_supply[year, time, i];

                        s6 = s6 + com.boundaryriver_province_supply[year, time, i];
                    }
                    com.recycledwater_province_supplyY[year, i] = s1;

                    com.locatedwater_province_supplyY[year, i] = s2;

                    com.groundwater_province_supplyY[year, i] = s3;

                    com.riverwater_province_supplyY[year, i] = s4;

                    com.reservoirwater_province_supplyY[year, i] = s5;

                    com.boundaryriver_province_supplyY[year, i] = s6;
                    s1 = 0;
                    s2 = 0;
                    s3 = 0;
                    s4 = 0;
                    s5 = 0;
                    s6 = 0;
                    s7 = 0;
                    s8 = 0;
                    s9 = 0;
                }
                s1 = 0;
                s2 = 0;
                s3 = 0;
                s4 = 0;
                s5 = 0;
                s6 = 0;
                s7 = 0;
                s8 = 0;
                s9 = 0;
            }
            //省级区 各用水户 年值统计
            s1 = 0;
            s2 = 0;
            s3 = 0;
            s4 = 0;
            s5 = 0;
            s6 = 0;
            s7 = 0;
            s8 = 0;
            s9 = 0;
            for (int i = 1; i < com.Province_Numb; i++)
            {
                for (int iii = 1; iii < com.Users + 1; iii++)
                {
                    for (int year = 1; year < com.Years; year++)
                    {
                        for (int time = 1; time < com.Times; time++)
                        {
                            s1 = s1 + com.province_short_ture[year, time, i, iii];
                            s2 = s2 + com.province_needO_ture[year, time, i, iii];
                        }
                        com.province_short_tureY[year, i, iii] = s1;
                        com.province_needO_tureY[year, i, iii] = s2;
                        if (s2 > 0)
                        {
                            com.province_shortRY[year, i, iii] = s1 / s2;
                        }
                        else
                        {
                            com.province_shortRY[year, i, iii] = 0;
                        }
                        s1 = 0;
                        s2 = 0;
                    }
                    s1 = 0;
                    s2 = 0;
                    s3 = 0;
                    s4 = 0;
                    s5 = 0;
                    s6 = 0;
                    s7 = 0;
                    s8 = 0;
                    s9 = 0;
                }
            }
            //统计 松花江 的航运历时保证率
            long[] qs7 = new long[com.River_Node[2] + 1];
            long[] qs8 = new long[com.River_Node[2] + 1];
            for (int i = 1; i <= com.River_Node[2]; i++)
            {
                for (int year = 1; year < com.Years; year++)
                {
                    for (int time = 5; time <= 16; time++) //只统计5-8月
                    {
                        if (com.RiverQ[year, time, 2, i] < com.SHJ_limitQ[year, time])
                        {
                            qs7[i] = qs7[i] + 1;
                        }
                    }
                    if (qs7[i] > 0)
                    {
                        qs8[i] = qs8[i] + 1;
                    }
                }
                com.hangyunPtime[i] = (12 * (com.Years - 1) - qs7[i] / (12 * com.Years));
                com.hangyunPyear[i] = ((com.Years - 1) - qs8[i] / (com.Years));
            }
            //直接写入航运保证率
            new ShippingFlowBLL().Add(com);
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //++++++++++++++++++++++ O2 各个水源分水结果 ++++++++++++++++++++++++++++++++++++++++++++++
            //-----------------------------------------------------------------------------------------------------------------------------------
            new WaterSupplyBLL().Add(com);
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //++++++++++++++++++++++ O 河流节点径流量、供水量统计输出++++++++++++++++++++++++++++++++++++++++++++ +
            new RiverNodeSupplyBLL().Add(com);
            //------------------------调用输出模块 (  O11计算单元结果统计  ) (原 四级区套行政区 )
            new UnitsStatisticBLL().Add(com);
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //------------------------调用输出模块 (   O12工程分区供需平衡结果表统计 )
            new FenqusStatisticBLL().Add(com);
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //------------------------调用输出模块 ( O13 县级区统计结果  )
            new CountyStatisticBLL().Add(com);
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //------------------------调用输出模块 ( O14 地级区统计结果  )
            new CityStatisticBLL().Add(com);
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //------------------------调用输出模块 ( O15 省级区统计结果  )
            new ProvinceStatisticBLL().Add(com);
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            //------------------------调用输出模块 ( O16 三江连通工程区统计结果  )
            new SJLTStatisticBLL().Add(com);
            //调用 O31  历时保证率 子模板
            new UnitsPtimeBLL().Add(com);
            //调用 O32  年保证率 子模板
            new UnitsPyearBLL().Add(com);
            //^---------------------------------------------------------------------------------------------------------------------------------
            //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&& 年值结果输出 &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
            //^---------------------------------------------------------------------------------------------------------------------------------
            //O11计算单元供需平衡结果表_年值  输出
            new ValueYearsOutputBLL().Add(com, units_groundwater_xuefengY, units_groundwater_shiyongY);
            //O12工程分区供需平衡结果表_年值  输出
            new FenqusYearsOutputBLL().Add(com, locatedwater_fenqu_supplyY_avg, fenqu_users_shortQOY_avg, fenqu_users_shortQY_avg, recycledwater_fenqu_supplyY_avg, riverwater_fenqu_supplyY_avg, groundwater_fenqu_supplyY_avg, boundaryriver_fenqu_supplyY_avg, fenqu_users_shortRY_avg);
            //O13县级区供需平衡结果表_年值
            new CountyYearsOutputBLL().Add(com);
            //O14地级区供需平衡结果表_年值
            new CityYearsOutputBLL().Add(com);
            //O15省级区供需平衡结果表_年值
            new ProvinceYearsOutputBLL().Add(com);
            //O16三江连通工程区供需平衡结果表_年值
            new SJLTYearsOutputBLL().Add(com);
            result = true;

            return result;
        }
        private Common groundwater_divisionLH(Common com, int units_numb, int year, int time, int users, int guanqu_numb, int unitscode)
        {
            //先把剩余的地表毛需水转为 地下毛需水
            //水田
            if (com.unit_groundwaterK[year, time, unitscode, 0] > 0)
            {
                com.Units_Waterneed[year, time, unitscode, 4] = com.Units_Waterneed[year, time, unitscode, 4] * com.unit_surfacewaterK[year, time, unitscode, 0] / com.unit_groundwaterK[year, time, unitscode, 0];
            }
            //本历时供不完的地下水给下一历时用
            if (time > 1 && time < 20)
            {
                com.Groundwater_Supply[year, time, unitscode] = com.Groundwater_Supply[year, time, unitscode] + com.Groundwater_Supply[year, time - 1, unitscode];
            }
            double[] water_userdividsion = new double[users]; //该水源比例各个用户分水量，并非实际供水量   中间变量

            for (int i = 1; i < users; i++)
            {
                //该水源比例各个用户可分水量
                water_userdividsion[i] = com.Groundwater_Supply[year, time, unitscode] * com.User_Proportion_Groundwater[year, time, unitscode, i];

                if (com.Units_Waterneed[year, time, unitscode, i] >= water_userdividsion[i])//判断可供水量是否满足单元该用户需水量
                {
                    com.Groundwater_Supply_Ture[year, time, unitscode, i] = water_userdividsion[i];//不满足则完全供水
                }
                else//满足则供水=需水
                {
                    com.Groundwater_Supply_Ture[year, time, unitscode, i] = com.Units_Waterneed[year, time, unitscode, i];
                }
                //初次按比例供水后的单元需水量变化
                com.Units_Waterneed[year, time, unitscode, i] = com.Units_Waterneed[year, time, unitscode, i] - com.Groundwater_Supply_Ture[year, time, unitscode, i];
            }
            for (int i = 1; i < users; i++)
            {
                //剩余可供水量
                com.Groundwater_Supply[year, time, unitscode] = com.Groundwater_Supply[year, time, unitscode] - com.Groundwater_Supply_Ture[year, time, unitscode, i];
            }

            //如果该水源还有剩余，且用户还有需求则 按照用户优先级 再次供水
            int orders = 1;//首先分给等级最高的用户
            if (com.Groundwater_Supply[year, time, unitscode] > 0)//判断是否有余水
            {
                do
                {
                    for (int i = 1; i < users; i++)
                    {
                        if (com.User_Orders_Groundwater[year, time, unitscode, i] == orders)//剩余水大于需水   需水满足  剩余水作为单元退水
                        {
                            com.Groundwater_Supply[year, time, unitscode] = com.Groundwater_Supply[year, time, unitscode] - com.Units_Waterneed[year, time, unitscode, i];
                            com.Groundwater_Supply_Ture[year, time, unitscode, i] = com.Groundwater_Supply_Ture[year, time, unitscode, i] + com.Units_Waterneed[year, time, unitscode, i];
                            com.Units_Waterneed[year, time, unitscode, i] = 0;
                        }
                        else if (com.Groundwater_Supply[year, time, unitscode] < com.Units_Waterneed[year, time, unitscode, i])//剩余水小于需水  无剩余水
                        {
                            com.Units_Waterneed[year, time, unitscode, i] = com.Units_Waterneed[year, time, unitscode, i] - com.Groundwater_Supply[year, time, unitscode];
                            com.Groundwater_Supply_Ture[year, time, unitscode, i] = com.Groundwater_Supply_Ture[year, time, unitscode, i] + com.Groundwater_Supply[year, time, unitscode];
                            com.Groundwater_Supply[year, time, unitscode] = 0;
                        }
                    }
                    orders++;
                } while (orders < users);
            }
            if (time == 20 && year < com.Years - 1)//年内最后一个历时剩余地下水下一年第一个历时用，最后一年最后一个历时保留
            {
                com.Groundwater_Supply[year + 1, 1, unitscode] = com.Groundwater_Supply[year + 1, 1, unitscode] + com.Groundwater_Supply[year, time, unitscode];
            }
            return com;
        }

        private Common riverwater_supply(Common com, int unitscode, int users, int River_Totalnoderank, int year, int time)
        {
            double[] water_userdividsion = new double[users];
            for (int ii = 1; ii < users; ii++)
            {
                //节点可供用户水量
                water_userdividsion[ii] = com.Riverwater_Supply[year, time, River_Totalnoderank, unitscode] * com.user_proportion_riverwater[year, time, unitscode, ii];
                if (com.Units_Waterneed[year, time, unitscode, ii] >= water_userdividsion[ii])//判断用户需水是否大于节点可供水量
                {
                    com.Riverwater_Supply_Ture[year, time, River_Totalnoderank, unitscode, ii] = water_userdividsion[ii];     //实际供水量 = 可供水量
                }
                else
                {
                    //否则 实际供水量 = 需水量
                    com.Riverwater_Supply_Ture[year, time, River_Totalnoderank, unitscode, ii] = com.Units_Waterneed[year, time, unitscode, ii];
                }
                //需水量 = 需水量 - 实际供水量（各个用户）
                com.Units_Waterneed[year, time, unitscode, ii] = com.Units_Waterneed[year, time, unitscode, ii] - com.Riverwater_Supply_Ture[year, time, River_Totalnoderank, unitscode, ii];
            }
            for (int ii = 1; ii < users; ii++)
            {
                //可供水量（单元总的） = 可供水量（单元总的）- 实际供水量（各个用户）
                com.Riverwater_Supply[year, time, River_Totalnoderank, unitscode] = com.Riverwater_Supply[year, time, River_Totalnoderank, unitscode] - com.Riverwater_Supply_Ture[year, time, River_Totalnoderank, unitscode, ii];
            }
            int orders = 1;//首先分给等级最高的用户
            if (com.Riverwater_Supply[year, time, River_Totalnoderank, unitscode] > 0)//把剩下的水按等级分给可供的各个用户
            {
                do
                {
                    for (int ii = 1; ii < users; ii++)
                    {
                        if (com.user_orders_riverwater[year, time, unitscode, ii] == orders)//剩余水大于需水   需水满足  剩余水作为单元退水
                        {
                            com.Riverwater_Supply[year, time, River_Totalnoderank, unitscode] = com.Riverwater_Supply[year, time, River_Totalnoderank, unitscode] - com.Units_Waterneed[year, time, unitscode, ii];

                            com.Riverwater_Supply_Ture[year, time, River_Totalnoderank, unitscode, ii] = com.Riverwater_Supply_Ture[year, time, River_Totalnoderank, unitscode, ii] + com.Units_Waterneed[year, time, unitscode, ii];

                            com.Units_Waterneed[year, time, unitscode, ii] = 0;
                        }
                        else if (com.Riverwater_Supply[year, time, River_Totalnoderank, unitscode] < com.Units_Waterneed[year, time, unitscode, ii])//剩余水小于需水  无剩余水
                        {
                            com.Units_Waterneed[year, time, unitscode, ii] = com.Units_Waterneed[year, time, unitscode, ii] - com.Riverwater_Supply[year, time, River_Totalnoderank, unitscode];
                            com.Riverwater_Supply_Ture[year, time, River_Totalnoderank, unitscode, ii] = com.Riverwater_Supply_Ture[year, time, River_Totalnoderank, unitscode, ii] + com.Riverwater_Supply[year, time, River_Totalnoderank, unitscode];
                        }
                    }
                    orders = orders + 1; //确定下一级别用户
                } while (orders < users);
            }
            return com;
        }
        /// <summary>
        /// 计算单元本地地表径流水分水计算子函数（所有单元）
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        private Common locatedwater_division(Common com)
        {
            double[] water_userdividsion = new double[com.Users];
            for (int year = 1; year < com.Years; year++)
            {
                for (int time = 1; time < com.Times; time++)
                {
                    for (int j = 1; j < com.Units_Numb; j++)
                    {
                        for (int i = 1; i < com.Users; i++)
                        {
                            //该水源比例各个用户可分水量
                            water_userdividsion[i] = com.Locatedwater_Supply[year, time, j] * com.User_Proportion_Locatedwater[year, time, j, i];
                            if (com.Units_Waterneed[year, time, j, i] >= water_userdividsion[i]) //判断可供水量是否满足单元该用户需水量
                            {
                                com.Locatedwater_Supply_Ture[year, time, j, i] = water_userdividsion[i];    //不满足则完全供水
                            }
                            else//满足则供水=需水
                            {
                                com.Locatedwater_Supply_Ture[year, time, j, i] = com.Units_Waterneed[year, time, j, i];
                            }
                            //初次按比例供水后的单元需水量变化
                            com.Units_Waterneed[year, time, j, i] = com.Units_Waterneed[year, time, j, i] - com.Locatedwater_Supply_Ture[year, time, j, i];
                        }
                        for (int i = 1; i < com.Users; i++)
                        {
                            //剩余可供水量
                            com.Locatedwater_Supply[year, time, j] = com.Locatedwater_Supply[year, time, j] - com.Locatedwater_Supply_Ture[year, time, j, i];
                        }
                        int orders = 1;         //首先分给等级最高的用户
                        if (com.Locatedwater_Supply[year, time, j] > 0)
                        {
                            do
                            {
                                for (int i = 1; i < com.Users; i++)
                                {
                                    if (com.User_Orders_Locatedwater[year, time, j, i] == orders)
                                    {
                                        if (com.Locatedwater_Supply[year, time, j] > com.Units_Waterneed[year, time, j, i])
                                        {
                                            com.Locatedwater_Supply[year, time, j] = com.Locatedwater_Supply[year, time, j] - com.Units_Waterneed[year, time, j, i];
                                            com.Locatedwater_Supply_Ture[year, time, j, i] = com.Locatedwater_Supply_Ture[year, time, j, i] + com.Units_Waterneed[year, time, j, i];
                                            com.Units_Waterneed[year, time, j, i] = 0;
                                        }
                                        else if (com.Locatedwater_Supply[year, time, j] < com.Units_Waterneed[year, time, j, i])
                                        {
                                            com.Units_Waterneed[year, time, j, i] = com.Units_Waterneed[year, time, j, i] - com.Locatedwater_Supply[year, time, j];
                                            com.Locatedwater_Supply_Ture[year, time, j, i] = com.Locatedwater_Supply_Ture[year, time, j, i] + com.Locatedwater_Supply[year, time, j];
                                            com.Locatedwater_Supply[year, time, j] = 0;
                                        }
                                    }
                                }
                                orders++;
                            } while (orders < com.Users);
                        }
                    }
                }
            }
            return com;
        }
    }
}
