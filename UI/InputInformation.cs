using BLL;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace UI
{
    public class InputInformation
    {
        /// <summary>
        /// 一键输入所有数据
        /// </summary>
        /// <returns></returns>
        public Common OneKeyInput(string CmbDvlpPrgm)
        {
            ////////////////////////////////////////////////// 读取数据库，输入系统参数
            ////^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            ////******************************** 读取系统信息表  *************************************************
            ////__________________________________________________________________________________________________
            Common com = new Common();

            //读取数据库，输入系统参数
            //读取系统信息表
            SystemInfo System = new SystemBLL().GetEntity();
            com.Units_Numb = System.CalculationtNumber.ToInt() + 1; //工程分区个数
            com.Fenqus = System.ProjectNumber.ToInt() + 1;
            com.County_Numb = System.CountyNumber.ToInt() + 1;
            com.City_Numb = System.PrefectureNumber.ToInt() + 1;
            com.Province_Numb = System.ProvincialNumber.ToInt() + 1;
            com.Years = System.Years.ToInt() + 1;
            com.First_Year = System.StartYear.ToInt();
            com.YueXuns = System.MonthlyNumber.ToInt();
            com.Users = System.UserNumber.ToInt() + 1;
            com.Times = com.YueXuns + 1;
            com.Years = 2;
            //////////方案1  "黑平松平挠平本地平"
            ////Hyear = 1981    //黑龙江 25 %：1998，50 %：1981，75 %：1965，90 %：1996
            ////Syear = 1972    //松花江 25 %：1961，50 %：1972，75 %：1967，90 %：2004
            ////Lyear = 1989    //本地降水  25 %：1972，50 %：1989，75 %：1974，90 %：1978
            ////Nyear = 1958    //挠力河  25 %：1997，50 %：1958，75 %：2000，90 %：1982


            //////////方案2   "黑丰松枯挠枯本地枯"
            ////Hyear = 1998    //黑龙江 25 %：1998，50 %：1981，75 %：1965，90 %：1996
            ////Syear = 1967    //松花江 25 %：1961，50 %：1972，75 %：1967，90 %：2004
            ////Lyear = 1974    //本地降水  25 %：1972，50 %：1989，75 %：1974，90 %：1978
            ////Nyear = 2000    //挠力河  25 %：1997，50 %：1958，75 %：2000，90 %：1982


            //////////方案3   "黑枯松丰挠枯本地枯"
            ////Hyear = 1965    //黑龙江 25 %：1998，50 %：1981，75 %：1965，90 %：1996
            ////Syear = 1961    //松花江 25 %：1961，50 %：1972，75 %：1967，90 %：2004
            ////Lyear = 1974    //本地降水  25 %：1972，50 %：1989，75 %：1974，90 %：1978
            ////Nyear = 2000    //挠力河  25 %：1997，50 %：1958，75 %：2000，90 %：1982


            //////////方案4   "黑枯松枯挠丰本地枯"
            ////Hyear = 1965    //黑龙江 25 %：1998，50 %：1981，75 %：1965，90 %：1996
            ////Syear = 1967    //松花江 25 %：1961，50 %：1972，75 %：1967，90 %：2004
            ////Lyear = 1974    //本地降水  25 %：1972，50 %：1989，75 %：1974，90 %：1978
            ////Nyear = 1997    //挠力河  25 %：1997，50 %：1958，75 %：2000，90 %：1982

            //方案5  黑枯松枯挠枯本地枯
            com.Hyear = 1965;//黑龙江 25%：1998，50%：1981，75%：1965，90%：1996
            com.Syear = 1967;//松花江 25%：1961，50%：1972，75%：1967，90%：2004
            com.Lyear = 1974;//本地降水  25%：1972，50%：1989，75%：1974，90%：1978
            com.Nyear = 2000;//挠力河  25%：1997，50%：1958，75%：2000，90%：1982

            com.YueXun = new string[com.Times];
            com.UnitsName = new string[com.Units_Numb];//存储计算单元名称
            com.FenquName = new string[com.Fenqus];//分区名称
            com.CountyName = new string[com.County_Numb];
            com.CityName = new string[com.City_Numb];
            com.ProvinceName = new string[com.Province_Numb];

            com.fenqu_units = new int[com.Units_Numb, 3];//计算单元对应工程分区【计算单元编号，工程分区编号】
            com.county_units = new int[com.Units_Numb, 3];//计算单元对应县级区数组
            com.city_units = new int[com.Units_Numb, 3];//计算单元对应地级区数组
            com.province_units = new int[com.Units_Numb, 3];//计算单元对应省级区数组
            //松花江航运流量
            com.SHJ_limitQ = new double[com.Years, com.Times];
            com.SHJ_limitQ1 = 850;     //按850个流量进行控制（水量单位万方）5-8月逐旬控制，按照旬控制，10或11天
            for (int i = 1; i < com.Years; i++)
            {
                for (int j = 1; j < com.Times; j++)
                {
                    switch (j)
                    {
                        case 7:
                        case 13:
                        case 16:
                            com.SHJ_limitQ[i, j] = com.SHJ_limitQ1 * 0.36 * 24 * 11;
                            break;
                        default:
                            com.SHJ_limitQ[i, j] = com.SHJ_limitQ1 * 0.36 * 24 * 10;
                            break;
                    }
                }
            }
            com.Users_Name = new string[7] { "", System.UserName1, System.UserName2, System.UserName3, System.UserName4, System.UserName5, System.UserName6 };

            ////^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            ////************************************** 读取计算单元所属信息  ******************************************
            ////_______________________________________________________________________________________________________
            var Data = new ComputingUnitBLL().GetTable();
            for (int i = 1; i < com.Units_Numb; i++)
            {
                com.fenqu_units[i, 1] = i;
                com.UnitsName[i] = Data.Rows[i - 1]["计算单元名称"].ToString();
                com.fenqu_units[i, 2] = Data.Rows[i - 1]["所属工程分区编号"].ToInt();
                com.county_units[i, 2] = Data.Rows[i - 1]["所属地级区编号"].ToInt();
                com.city_units[i, 2] = Data.Rows[i - 1]["所属地级区编号"].ToInt();
                com.province_units[i, 2] = Data.Rows[i - 1]["所属省级区编号"].ToInt();
            }
            //写入工程分区的名称
            for (int i = 1; i < com.Fenqus; i++)
            {
                for (int ii = 1; ii < com.Units_Numb; ii++)
                {
                    if (com.fenqu_units[ii, 2] == i)
                    {
                        com.FenquName[i] = Data.Rows[ii - 1]["所属工程分区"].ToString();
                    }
                }
            }
            //写入县级区的名称
            for (int i = 1; i < com.County_Numb; i++)
            {
                for (int ii = 1; ii < com.Units_Numb; ii++)
                {
                    if (com.county_units[ii, 2] == i)
                    {
                        com.CountyName[i] = Data.Rows[i - 1]["所属灌区名称"].ToString();
                    }
                }
            }
            //写入地级区的名称
            for (int i = 1; i < com.City_Numb; i++)
            {
                for (int ii = 1; ii < com.Units_Numb; ii++)
                {
                    if (com.city_units[ii, 2] == i)
                    {
                        com.CityName[i] = Data.Rows[ii - 1]["所属地级区名称"].ToString();
                    }
                }
            }
            //写入省级区的名称
            for (int i = 1; i < com.Province_Numb; i++)
            {
                for (int ii = 1; ii < com.Units_Numb; ii++)
                {
                    if (com.province_units[ii, 2] == i)
                    {
                        com.ProvinceName[i] = Data.Rows[ii - 1]["所属省级区名称"].ToString();
                    }
                }
            }

            //数组最后一维区别水田和旱田： 0为水田 1为旱田
            com.unit_groundwaterK = new double[com.Years, com.Times, com.Units_Numb, 3];//计算单元农业需水地下水渠系有效利用系数
            com.unit_surfacewaterK = new double[com.Years, com.Times, com.Units_Numb, 3];//计算单元农业需水地下水渠系有效利用系数

            com.unit_yintiK = new double[com.Years, com.Times, com.Units_Numb + 1];//定义各计算单元 河道引提供水 分水比 与 计算单元1分水比类似

            com.unitsusers_waterPtime = new double[com.Units_Numb, com.Users];//历时保证率
            com.unitsusers_waterPyear = new double[com.Units_Numb, com.Users];//年保证率

            com.units_waterPtime = new double[com.Units_Numb];//综合平均历时保证率
            com.units_waterPyear = new double[com.Units_Numb];//综合平均年保证率

            com.unitsum_user_Ptime = new double[com.Users];
            com.unitsum_user_Pyear = new double[com.Users];
            com.SJLT_users_Ptime = new double[com.Users];
            com.SJLT_users_Pyear = new double[com.Users];

            com.guanqu_user_Ptime = new double[com.Users];
            com.guanqu_user_Pyear = new double[com.Users];
            com.SJLT_user_waterneedOY = new double[com.Years, com.Users];               //三江连通工程区所有用户的需水量年值
            com.SJLT_user_watershortQY = new double[com.Years, com.Users];              //三江连通工程区所有用户的缺水量
            com.SJLT_user_watershortRY = new double[com.Years, com.Users];              //三江连通工程区所有用户的缺水率

            com.SJLT_waterneedY = new double[com.Years];                   //三江连通工程区年总需水量年值
            com.SJLT_watersupplyY = new double[com.Years];                 //三江连通工程区年总供水量年值
            com.SJLT_watershortY = new double[com.Years];                  //三江连通工程区年总缺水量年值
            com.SJLT_watershortRY = new double[com.Years];                 //三江连通工程区年总缺水率年值

            com.SJLT_user_watershortQYsum = new double[com.Users];
            com.SJLT_user_waterneedOYsum = new double[com.Users];
            com.SJLT_user_watershortRYsum = new double[com.Users];


            com.locatedwater_SJLT_supplyY = new double[com.Years];                      //三江连通工程区本地水总供水量
            com.recycledwater_SJLT_supplyY = new double[com.Years];                     //三江连通工程区再生水总供水量
            com.groundwater_SJLT_supplyY = new double[com.Years];                       //三江连通工程区地下水总供水量
            com.riverwater_SJLT_supplyY = new double[com.Years];                        //三江连通工程区河网（引提水）总供水量
            com.reservoirwater_SJLT_supplyY = new double[com.Years];                    //三江连通工程区水库水总供水量
            com.boundaryriver_SJLT_supplyY = new double[com.Years];                     //三江连通工程区界河水总供水量

            com.fenqu_users_shortQY = new double[com.Years, com.Fenqus, com.Users + 1];             //各个工程分区各用户缺水量 // + 1//为了存储所有工程分区统计量
            com.fenqu_users_shortQOY = new double[com.Years, com.Fenqus, com.Users + 1];            //各个工程分区各用户原始需水量
            com.fenqu_users_shortRY = new double[com.Years, com.Fenqus, com.Users + 1];             //各个工程分区各用户缺水率

            com.recycledwater_fenqu_supplyY = new double[com.Years, com.Fenqus];              //再生水工程分区供水量
            com.locatedwater_fenqu_supplyY = new double[com.Years, com.Fenqus];               //本地地表水工程分区供水量
            com.groundwater_fenqu_supplyY = new double[com.Years, com.Fenqus];                //地下水工程分区供水量
            com.riverwater_fenqu_supplyY = new double[com.Years, com.Fenqus];                 //河网水工程分区供水量
            com.reservoirwater_fenqu_supplyY = new double[com.Years, com.Fenqus];             //水库水工程分区供水量
            com.boundaryriver_fenqu_supplyY = new double[com.Years, com.Fenqus];              //界河水工程分区供水量

            com.county_short_tureY = new double[com.Years, com.County_Numb, com.Users + 1];            //各个县级区向各个用户缺水率  users + 1 表示对所用户
            com.county_needO_tureY = new double[com.Years, com.County_Numb, com.Users + 1];            //各个县级区向各个用户原始需水量
            com.county_shortRY = new double[com.Years, com.County_Numb, com.Users + 1];                //各个县级区向各个用户缺水率  users + 1 表示对所用户

            com.locatedwater_county_supplyY = new double[com.Years, com.County_Numb];                      //本地水各个县级区供水量
            com.recycledwater_county_supplyY = new double[com.Years, com.County_Numb];                     //再生水各个县级区供水量
            com.groundwater_county_supplyY = new double[com.Years, com.County_Numb];                       //地下水各个县级区供水量
            com.riverwater_county_supplyY = new double[com.Years, com.County_Numb];                        //河网（引提水）水各个县级区供水量
            com.reservoirwater_county_supplyY = new double[com.Years, com.County_Numb];                     //水库水各个县级区供水量
            com.boundaryriver_county_supplyY = new double[com.Years, com.County_Numb];                      //界河水各个县级区供水量

            com.city_short_tureY = new double[com.Years, com.City_Numb, com.Users + 1];             //各个地级区向各个用户缺水率  users + 1 表示对所用户
            com.city_needO_tureY = new double[com.Years, com.City_Numb, com.Users + 1];             //各个地级区向各个用户原始需水量
            com.city_shortRY = new double[com.Years, com.City_Numb, com.Users + 1];                 //各个地级区向各个用户缺水率  users + 1 表示对所用户

            com.locatedwater_city_supplyY = new double[com.Years, com.City_Numb];                      //本地水各个地级区供水量
            com.recycledwater_city_supplyY = new double[com.Years, com.City_Numb];                     //再生水各个地级区供水量
            com.groundwater_city_supplyY = new double[com.Years, com.City_Numb];                       //地下水各个地级区供水量
            com.riverwater_city_supplyY = new double[com.Years, com.City_Numb];                        //河网（引提水）水各个地级区供水量
            com.reservoirwater_city_supplyY = new double[com.Years, com.City_Numb];                     //水库水各个地级区供水量
            com.boundaryriver_city_supplyY = new double[com.Years, com.City_Numb];                      //界河水各个地级区供水量

            com.province_short_tureY = new double[com.Years, com.Province_Numb, com.Users + 1];             //各个省级区向各个用户缺水率  users + 1 表示对所用户
            com.province_needO_tureY = new double[com.Years, com.Province_Numb, com.Users + 1];             //各个省级区向各个用户原始需水量
            com.province_shortRY = new double[com.Years, com.Province_Numb, com.Users + 1];                 //各个省级区向各个用户缺水率  users + 1 表示对所用户

            com.locatedwater_province_supplyY = new double[com.Years, com.Province_Numb];                      //本地水各个省级区供水量
            com.recycledwater_province_supplyY = new double[com.Years, com.Province_Numb];                     //再生水各个省级区供水量
            com.groundwater_province_supplyY = new double[com.Years, com.Province_Numb];                       //地下水各个省级区供水量
            com.riverwater_province_supplyY = new double[com.Years, com.Province_Numb];                        //河网（引提水）水各个省级区供水量
            com.reservoirwater_province_supplyY = new double[com.Years, com.Province_Numb];                     //水库水各个省级区供水量
            com.boundaryriver_province_supplyY = new double[com.Years, com.Province_Numb];                      //界河水各个省级区供水量

            ////------------------------------------------------------------------------------------------
            ////******************************* 历时值结果统计时在定义变量  ******************************
            ////------------------------------------------------------------------------------------------

            com.Units_Water_Supply = new double[com.Years, com.Times, com.Units_Numb];                //计算单元最终实际供水量
            com.units_water_shortQ = new double[com.Years, com.Times, com.Units_Numb];                //计算单元最终缺水量
            com.units_water_shortR = new double[com.Years, com.Times, com.Units_Numb];                //计算单元最终缺水率
            com.units_water_usershortR = new double[com.Years, com.Times, com.Units_Numb, com.Users];     //计算单元各用户最终缺水率
            com.water_usershort = new double[com.Years, com.Times, com.Users + 1];                    //所有计算单元各用户总缺水量 users + 1表示所有用户合计
            com.water_usershortO = new double[com.Years, com.Times, com.Users + 1];                   //所有计算单元各用户总缺水量原始值
            com.water_usershortR = new double[com.Years, com.Times, com.Users + 1];                   //所有计算单元各用户总缺水率

            //----------------------------------河网水向计算单元供水统计
            com.riverwater_units_supply = new double[com.Years, com.Times, com.Units_Numb];                  //河网水向计算单元供水
            com.riverwater_unitsuser_supply = new double[com.Years, com.Times, com.Units_Numb, com.Users];       //河网水向计算单元各用户供水




            com.fenqu_shortQ = new double[com.Years, com.Times, com.Fenqus + 1, com.Users + 1];            //各个工程分区各用户缺水量 // + 1//为了存储所有工程分区统计量
            com.fenqu_shortQO = new double[com.Years, com.Times, com.Fenqus + 1, com.Users + 1];           //各个工程分区各用户原始需水量
            com.fenqu_shortR = new double[com.Years, com.Times, com.Fenqus + 1, com.Users + 1];            //各个工程分区各用户缺水率

            com.recycledwater_fenqu_supply = new double[com.Years, com.Times, com.Fenqus];            //再生水工程分区供水量
            com.locatedwater_fenqu_supply = new double[com.Years, com.Times, com.Fenqus];             //本地地表水工程分区供水量
            com.groundwater_fenqu_supply = new double[com.Years, com.Times, com.Fenqus];              //地下水工程分区供水量
            com.riverwater_fenqu_supply = new double[com.Years, com.Times, com.Fenqus];               //河网水工程分区供水量
            com.reservoirwater_fenqu_supply = new double[com.Years, com.Times, com.Fenqus];           //水库水工程分区供水量
            com.boundaryriver_fenqu_supply = new double[com.Years, com.Times, com.Fenqus];            //水库水工程分区供水量
            com.fenqu_supplyQ = new double[com.Years, com.Times, com.Fenqus];                         //工程分区总的供水量

            com.locatedwater_unit_supply = new double[com.Years, com.Times, com.Units_Numb];                       //本地水各个单元供水量
            com.recycledwater_unit_supply = new double[com.Years, com.Times, com.Units_Numb];                      //再生水各个单元供水量
            com.groundwater_unit_supply = new double[com.Years, com.Times, com.Units_Numb];                        //地下水各个单元供水量
            com.riverwater_unit_supply = new double[com.Years, com.Times, com.Units_Numb];                         //河网（引提水）水各个单元供水量
            com.reservoirwater_unit_supply = new double[com.Years, com.Times, com.Units_Numb];                     //水库水各个单元供水量
            com.boundaryriver_unit_supply = new double[com.Years, com.Times, com.Units_Numb];                      //界河水各个单元供水量


            com.units_waterneedsum = new double[com.Years, com.Times, com.Units_Numb];                             //各个单元原始总的需水量

            com.SJLT_waterneed = new double[com.Years, com.Times];                                             //三江连通工程区总需水量各历时
            com.SJLT_watersupply = new double[com.Years, com.Times];                                           //三江连通工程区总供水量各历时
            com.SJLT_watershortage = new double[com.Years, com.Times];                                         //三江连通工程区总缺水量各历时

            com.locatedwater_SJLT_supply = new double[com.Years, com.Times];                      //三江连通工程区本地水总供水量
            com.recycledwater_SJLT_supply = new double[com.Years, com.Times];                     //三江连通工程区再生水总供水量
            com.groundwater_SJLT_supply = new double[com.Years, com.Times];                       //三江连通工程区地下水总供水量
            com.riverwater_SJLT_supply = new double[com.Years, com.Times];                        //三江连通工程区河网（引提水）总供水量
            com.reservoirwater_SJLT_supply = new double[com.Years, com.Times];                    //三江连通工程区水库水总供水量
            com.boundaryriver_SJLT_supply = new double[com.Years, com.Times];                     //三江连通工程区界河水总供水量

            com.county_short_ture = new double[com.Years, com.Times, com.County_Numb, com.Users + 1];              //各个县级区向各个用户缺水率  users + 1 表示对所用户
            com.county_needO_ture = new double[com.Years, com.Times, com.County_Numb, com.Users + 1];              //各个县级区向各个用户原始需水量
            com.county_shortR = new double[com.Years, com.Times, com.County_Numb, com.Users + 1];                  //各个县级区向各个用户缺水率  users + 1 表示对所用户

            com.locatedwater_county_supply = new double[com.Years, com.Times, com.County_Numb];                       //本地水各个县级区供水量
            com.recycledwater_county_supply = new double[com.Years, com.Times, com.County_Numb];                      //再生水各个县级区供水量
            com.groundwater_county_supply = new double[com.Years, com.Times, com.County_Numb];                       //地下水各个县级区供水量
            com.riverwater_county_supply = new double[com.Years, com.Times, com.County_Numb];                         //河网（引提水）水各个县级区供水量
            com.reservoirwater_county_supply = new double[com.Years, com.Times, com.County_Numb];                     //水库水各个县级区供水量
            com.boundaryriver_county_supply = new double[com.Years, com.Times, com.County_Numb];                     //界河水各个县级区供水量

            com.city_short_ture = new double[com.Years, com.Times, com.City_Numb, com.Users + 1];              //各个地级区向各个用户缺水率  users + 1 表示对所用户
            com.city_needO_ture = new double[com.Years, com.Times, com.City_Numb, com.Users + 1];              //各个地级区向各个用户原始需水量
            com.city_shortR = new double[com.Years, com.Times, com.City_Numb, com.Users + 1];                  //各个地级区向各个用户缺水率  users + 1 表示对所用户


            com.locatedwater_city_supply = new double[com.Years, com.Times, com.City_Numb];                       //本地水各个地级区供水量
            com.recycledwater_city_supply = new double[com.Years, com.Times, com.City_Numb];                      //再生水各个地级区供水量
            com.groundwater_city_supply = new double[com.Years, com.Times, com.City_Numb];                       //地下水各个地级区供水量
            com.riverwater_city_supply = new double[com.Years, com.Times, com.City_Numb];                         //河网（引提水）水各个地级区供水量
            com.reservoirwater_city_supply = new double[com.Years, com.Times, com.City_Numb];                     //水库水各个地级区供水量
            com.boundaryriver_city_supply = new double[com.Years, com.Times, com.City_Numb];                     //界河水各个地级区供水量

            com.province_short_ture = new double[com.Years, com.Times, com.Province_Numb, com.Users + 1];               //各个省级区向各个用户缺水率  users + 1 表示对所用户
            com.province_needO_ture = new double[com.Years, com.Times, com.Province_Numb, com.Users + 1];               //各个省级区向各个用户原始需水量
            com.province_shortR = new double[com.Years, com.Times, com.Province_Numb, com.Users + 1];                   //各个省级区向各个用户缺水率  users + 1 表示对所用户

            com.locatedwater_province_supply = new double[com.Years, com.Times, com.Province_Numb];                       //本地水各个省级区供水量
            com.recycledwater_province_supply = new double[com.Years, com.Times, com.Province_Numb];                      //再生水各个省级区供水量
            com.groundwater_province_supply = new double[com.Years, com.Times, com.Province_Numb];                        //地下水各个省级区供水量
            com.riverwater_province_supply = new double[com.Years, com.Times, com.Province_Numb];                         //河网（引提水）水各个省级区供水量
            com.reservoirwater_province_supply = new double[com.Years, com.Times, com.Province_Numb];                     //水库水各个省级区供水量
            com.boundaryriver_province_supply = new double[com.Years, com.Times, com.Province_Numb];                      //界河水各个省级区供水量

            com.users_recycledwatersupply = new double[com.Years, com.Times, com.Users];             //再生水供用户水量
            com.users_locatedwatersupply = new double[com.Years, com.Times, com.Users];              //本地地表水水供用户水量
            com.users_groundwatersupply = new double[com.Years, com.Times, com.Users];               //地下水供用户水量
            com.users_riverwatersupply = new double[com.Years, com.Times, com.Users];                //河网水供用户水量
            com.users_reservoirwatersupply = new double[com.Years, com.Times, com.Users];            //水库水供用户水量
            com.users_boundaryriversupply = new double[com.Years, com.Times, com.Users];             //界河水供用户水量

            if (CmbDvlpPrgm == "推荐方案（调水方案1111）")//方案1111 数据库SJLT203010004
            {
                com.guanqu_numb = 116;  //不同方案，灌区的个数不一样,方案1111为43个灌区(部分灌区一拆为二，下同)
                com.baohuqu_Fnum = 117;
            }
            if (CmbDvlpPrgm == "调水方案1121")//方案1121 数据库SJLT203010005
            {
                com.guanqu_numb = 43;  //不同方案，灌区的个数不一样,方案1121为43个灌区
                com.baohuqu_Fnum = 58;
            }
            if (CmbDvlpPrgm == "调水方案1122")//方案1122 数据库SJLT203010006
            {
                com.guanqu_numb = 43;  //不同方案，灌区的个数不一样,方案1122为43个灌区
                com.baohuqu_Fnum = 58;
            }
            if (CmbDvlpPrgm == "调水方案1123")//方案1123 数据库SJLT203010007
            {
                com.guanqu_numb = 40;  //不同方案，灌区的个数不一样,方案1123为39个灌区
                com.baohuqu_Fnum = 58;
            }
            if (CmbDvlpPrgm == "调水方案2122")//方案2122 数据库SJLT203010006
            {
                com.guanqu_numb = 43; //不同方案，灌区的个数不一样,方案1122为42个灌区
                com.baohuqu_Fnum = 58;
            }
            if (CmbDvlpPrgm == "调水方案2123")//方案2123 数据库SJLT203010007
            {
                com.guanqu_numb = 40; //不同方案，灌区的个数不一样,方案2123为39个灌区
                com.baohuqu_Fnum = 58;
            }
            if (CmbDvlpPrgm == "调水方案2222")//方案2222 数据库SJLT203010006
            {
                com.guanqu_numb = 43; //不同方案，灌区的个数不一样,方案2222为42个灌区
                com.baohuqu_Fnum = 58;
            }
            if (CmbDvlpPrgm == "调水方案2223")//方案2222 数据库SJLT203010006
            {
                com.guanqu_numb = 40; //不同方案，灌区的个数不一样,方案2222为39个灌区
                com.baohuqu_Fnum = 58;
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            ////--------------------------------------------- 读入R 基本信息
            ////______________________________________________________________________________________
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            ////--------------------------------------------- 读入 河流信息
            ////______________________________________________________________________________________

            Data = new RiverInformationBLL().GetTable();
            com.River_Numb = Data.Rows.Count + 1;
            com.River_Attribute = Data.Columns.Count + 1;
            com.River_Info = new int[com.River_Numb, com.River_Attribute];
            com.RiverName = new string[com.River_Numb];
            for (int i = 1; i < com.River_Numb; i++)
            {
                com.River_Info[i, 1] = Data.Rows[i - 1]["编号"].ToInt();
                com.River_Info[i, 2] = Data.Rows[i - 1]["河道等级"].ToInt();
                com.River_Info[i, 3] = Data.Rows[i - 1]["汇入河流编号"].ToInt();
                com.River_Info[i, 4] = Data.Rows[i - 1]["节点数目"].ToInt();
                com.River_Info[i, 5] = Data.Rows[i - 1]["汇入河流节点编号"].ToInt();
                com.RiverName[com.River_Info[i, 1]] = Data.Rows[i - 1]["河流名称"].ToString();
            }
            ////---------------------------------------------------------------------------------------------------
            ////---------------------------------------------- 读入 各河流所有 节点信息
            ////---------------------------------------------------------------------------------------------------
            Data = new NodeInformationBLL().GetTable();
            com.River_Allnode = Data.Rows.Count + 1;
            com.riverinfo_numb = Data.Columns.Count + 1;
            com.NodeName = new string[com.River_Allnode];
            for (int i = 1; i < com.River_Allnode; i++)
            {
                com.NodeName[i] = Data.Rows[i - 1]["节点名称"].ToString();
            }
            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            ////&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&   定义河网水数据变量
            ////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            com.user_orders_riverwater = new double[com.Years, com.Times, com.Units_Numb, com.Users];                             //河网水各用户优先级
            com.user_proportion_riverwater = new double[com.Years, com.Times, com.Units_Numb, com.Users];                         //河网水各用户分水比
            com.Riverwater_Supply_Ture = new double[com.Years, com.Times, com.River_Allnode, com.Units_Numb, com.Users];               //各计算单元河网水实际供水量
            com.Riverwater_Supply = new double[com.Years, com.Times, com.River_Allnode, com.Units_Numb];                             //河流节点向计算单元的引水量


            com.Channel_Node_Shortage = new double[com.Years, com.Times, com.riverinfo_numb, com.River_Allnode];            //////////干渠各个节点的缺水量 削峰逆推法使用  决定规模
            com.Channel_Node_ShortageQ = new double[com.Years, com.Times, com.riverinfo_numb, com.River_Allnode];            //////////干渠各个节点的缺水量 削峰逆推法使用  决定规模

            com.River_Node = new int[com.River_Numb];                                     //各个河流的节点
            com.River_Totalnode = new int[com.River_Numb, com.River_Allnode];                    //所有节点总编号
            com.RiverQ = new double[com.Years, com.Times, com.River_Numb, com.River_Allnode + 1];   //+1 0为初始水量  //河流节点流量【河流，所有节点】
            com.RiverK = new double[com.Years, com.Times, com.River_Numb, com.River_Allnode];                     //河流节点损失【河流，所有节点】
            com.River_Node_Info = new int[com.River_Allnode, com.River_Numb];                  //节点属性信息
            com.RiverQ_Limit = new double[com.Years, com.Times, com.River_Numb, com.River_Allnode];               //节点流量限制
            com.Riverwater_Totalsupply = new double[com.Years, com.Times, com.River_Allnode];                 //节点总的可供水量
            com.Riverwater_Node_Supply = new double[com.Years, com.Times, com.River_Allnode];                 //节点实际供水量

            com.Units = new int[com.Units_Numb];   //计算单元编号各
            com.Units_Waterneed = new double[com.Years, com.Times, com.Units_Numb, com.Users];                 //计算单元用户需水量【年，历时，计算单元，用户类型】
            com.Units_WaterneedO = new double[com.Years, com.Times, com.Units_Numb, com.Users];                //计算单元用户原始需水量【年，历时，计算单元，用户类型】
            com.units_waterneed1 = new double[com.Years, com.Times, com.Units_Numb, com.Users];                //计算单元用户原始需水量【年，历时，计算单元，用户类型】

            com.Units_Waterreturn = new double[com.Years, com.Times, com.Units_Numb, com.Users];               //计算单元各用户退水量
            com.Units_WaterreturnC = new double[com.Years, com.Times, com.Units_Numb, com.Users];              //计算单元各用户退水系数
            com.Units_Watereturn_Total = new double[com.Years, com.Times, com.Units_Numb];                 //计算单元总退水量
            com.Water_Supply_Ture = new double[com.Years, com.Times, com.Units_Numb, com.Users];               //各个单元各个用户供水量

            //------------------------------- 再定义数组  计算单元 （原四级区套地市）年值结果输出

            com.units_watersupplyY = new double[com.Years, com.Units_Numb];                  //各个计算单元年总供水量
            com.units_watershortQY = new double[com.Years, com.Units_Numb];                  //各个计算单元年总缺水量
            com.units_watershortRY = new double[com.Years, com.Units_Numb];                  //各个计算单元年总缺水率
            com.units_waterneedOY = new double[com.Years, com.Units_Numb];                   //各个计算单元年总原始需水量

            com.units_locatedwater_supplyY = new double[com.Years, com.Units_Numb];                 //各个计算单元年本地地表水供水量
            com.units_recycledwater_supplyY = new double[com.Years, com.Units_Numb];               //各个计算单元年再生水供水量
            com.units_groundwater_supplyY = new double[com.Years, com.Units_Numb];                  //各个计算单元年地下水供水量
            com.units_riverwater_supplyY = new double[com.Years, com.Units_Numb];                   //各个计算单元年河网水供水量
            com.units_reservoirdwater_supplyY = new double[com.Years, com.Units_Numb];              //各个计算单元年水库水供水量
            com.units_boundaryriver_supplyY = new double[com.Years, com.Units_Numb];                //各个计算单元年界河供水量

            com.unitsusers_watershortQY = new double[com.Years, com.Units_Numb, com.Users];             //各个计算单元各个用户缺水量
            com.unitsusers_watershortRY = new double[com.Years, com.Units_Numb, com.Users];             //各个计算单元各个用户年缺水率
            com.unitsusers_waterneedOY = new double[com.Years, com.Units_Numb, com.Users];              //各个计算单元各个用户年原需水量



            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&   定义地下水数据变量

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            com.User_Orders_Groundwater = new double[com.Years, com.Times, com.Units_Numb, com.Users];               //地下水各用户优先级
            com.User_Proportion_Groundwater = new double[com.Years, com.Times, com.Units_Numb, com.Users];           //地下水各用户分水比
            com.User_Capability_Groundwater = new double[com.Years, com.Times, com.Units_Numb];                  //各计算单元地下水供水能力
            com.User_Availability_Groundwater = new double[com.Years, com.Times, com.Units_Numb];                //各计算单元地下水理论可供水量
            com.Groundwater_Supply = new double[com.Years, com.Times, com.Units_Numb];                           //各计算单元地下水实际可供水量
            com.Groundwater_Supply_Ture = new double[com.Years, com.Times, com.Units_Numb, com.Users];               //各计算单元地下水实际供水量
            com.Units_Groundwater_Userate = new double[com.Years, com.Times, com.Units_Numb];                    //地下水的开发利用率

            com.User_Orders_Ygroundwater = new double[com.Years, com.Units_Numb, com.Users];                //地下水各用户优先级
            com.User_Proportion_Ygroundwater = new double[com.Years, com.Units_Numb, com.Users];            //地下水各用户分水比

            com.Groundwater_Ysupply = new double[com.Years, com.Units_Numb];                            //各计算单元地下水实际可供水量
            com.Units_Groundwater_Yuserate = new double[com.Years, com.Units_Numb];                     //地下水的开发利用率
            com.User_Availability_Ygroundwater = new double[com.Years, com.Units_Numb];                //各计算单元地下水理论可供水量

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&   定义本地地表水数据变量

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            com.User_Orders_Locatedwater = new double[com.Years, com.Times, com.Units_Numb, com.Users];               //本地地表水各用户优先级
            com.User_Proportion_Locatedwater = new double[com.Years, com.Times, com.Units_Numb, com.Users];           //本地地表水各用户分水比
            com.User_Capability_Locatedwater = new double[com.Years, com.Times, com.Units_Numb];                  //各计算单元本地地表水供水能力
            com.User_Availability_Locatedwater = new double[com.Years, com.Times, com.Units_Numb];                //各计算单元本地地表水理论可供水量
            com.Locatedwater_Supply = new double[com.Years, com.Times, com.Units_Numb];                           //各计算单元本地地表水实际可供水量
            com.Locatedwater_Supply_Ture = new double[com.Years, com.Times, com.Units_Numb, com.Users];               //各计算单元本地地表水实际供水量
            com.Locatedwater_Runoff = new double[com.Years, com.Times, com.Units_Numb];                           //各计算单元本地地表产流（开发利用率之外的部分直接进入河道的产流量）
            com.Locatedwater_RunoffK = new double[com.Years, com.Times, com.Units_Numb];                          //各计算单元本地地表径流开发利用系数

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&   定义再生水数据变量

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


            com.User_Orders_Recycledwater = new double[com.Years, com.Times, com.Units_Numb, com.Users];               //再生水各用户优先级
            com.User_Proportion_Recycledwater = new double[com.Years, com.Times, com.Units_Numb, com.Users];           //再生水各用户分水比
            com.User_Capability_Recycledwater = new double[com.Years, com.Times, com.Units_Numb];                  //各计算单元再生水供水能力
            com.User_Availability_Recycledwater = new double[com.Years, com.Times, com.Units_Numb];                //各计算单元再生水理论可供水量
            com.Recycledwater_Supply = new double[com.Years, com.Times, com.Units_Numb];                           //各计算单元再生水实际可供水量
            com.Recycledwater_Supply_Ture = new double[com.Years, com.Times, com.Units_Numb, com.Users];               //各计算单元再生水实际供水量

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&   定义水库水数据变量

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            com.user_orders_reservoirwater = new double[com.Years, com.Times, com.riverinfo_numb, com.Units_Numb, com.Users];                //水库水各用户优先级
            com.user_proportion_reservoirwater = new double[com.Years, com.Times, com.riverinfo_numb, com.Units_Numb, com.Users];            //水库水各用户供水能力限制
            com.Reservoirwater_Supply = new double[com.Years, com.Times, com.riverinfo_numb, com.Units_Numb];                             //各计算单元水库水实际可供水量
            com.Reservoirwater_Supply_Ture = new double[com.Years, com.Times, com.riverinfo_numb, com.Units_Numb, com.Users];                 //各计算单元水库水实际供水量

            com.reservoirwater_units_supply = new double[com.Years, com.Times, com.Units_Numb];                              //水库水向计算单元供水
            com.reservoirwater_unitsuser_supply = new double[com.Years, com.Times, com.Units_Numb, com.Users];                   //水库水向计算单元供水
            com.reservoirwater_node_supply = new double[com.Years, com.Times, com.riverinfo_numb, com.Units_Numb];               //每个水库向每个单元的供水


            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&   定义界河水数据变量

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            com.Boundaryriver = new int[com.Units_Numb];                                          //确定界河水供水单元属性数组 2中属性 供或不供
            com.Boundaryriver_Supply_Ture = new double[com.Years, com.Times, com.Units_Numb, com.Users];                    //确定界河水各个计算单元各个用户供水量

            //---------------------------------------------------------------------------------------------------
            //--------------------------------------------- 读入退水系统信息表 定义 退水数组
            //---------------------------------------------------------------------------------------------------
            Data = new BackwaterSystemBLL().GetTable();
            com.return_node = Data.Rows.Count + 1;
            com.returninfo_numb = Data.Columns.Count + 1;

            com.return_node_info = new double[com.Years, com.Times, com.return_node, com.returninfo_numb];            //退水点信息
            com.Waterreturn_Total = new double[com.Years, com.Times, com.River_Numb, com.River_Allnode];                  //节点退水总量

            //---------------------------------------------------------------------------------------------------
            //-------------------------------------- 读入引水系统表，获取引水数组的上限值
            //---------------------------------------------------------------------------------------------------
            Data = new RiverDiversionRatioBLL().GetTable();
            com.Channel_Node = Data.Rows.Count + 1;
            com.channelinfo_numb = Data.Columns.Count + 1;
            com.Channel_Node_Info = new double[com.Years, com.Times, com.Channel_Node + 1, com.channelinfo_numb];
            com.channel_unit_numb = new int[com.Channel_Node];
            //逐历时读入
            Data = new RiverDiversionRatioBLL().GetTableByYear(com.Lyear);
            for (int i = 1; i < com.Channel_Node; i++)
            {
                for (int y = 1; y < com.Years; y++)
                {
                    for (int t = 1; t < com.Times; t++)
                    {
                        com.Channel_Node_Info[y, t, i, 1] = Data.Rows[t - 1]["引提水点编号"].ToDouble();
                        com.Channel_Node_Info[y, t, i, 2] = Data.Rows[t - 1]["引提水单元数目"].ToDouble();
                        com.Channel_Node_Info[y, t, i, 3] = Data.Rows[t - 1]["计算单元1编号"].ToDouble();//一个引提水点只对应一个计算单元
                        com.Channel_Node_Info[y, t, i, 6] = Data.Rows[t - 1]["计算单元1分水比"].ToDouble();//人为设定比例    灌区引水干渠规模/总干渠沿程规模
                    }
                }
                com.channel_unit_numb[i] = Data.Rows[i - 1]["计算单元1编号"].ToInt();//再次写入计算单元的编号，后面用
            }

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&   读入地下水数据 年数据

            //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&& （供水量，供水优先级，供用户比例)
            Data = new GroundWaterBLL().GetTableByYear(com.Lyear);
            for (int i = 1; i < com.guanqu_numb; i++)
            {
                for (int y = 1; y < com.Years; y++)
                {
                    com.User_Availability_Ygroundwater[y, i] = Data.Rows[y - 1]["地下水可开采量（农业）"].ToDouble();
                    com.Units_Groundwater_Yuserate[y, i] = Data.Rows[y - 1]["地下水开发利用率"].ToDouble() * 1.2;
                    //地下水可利用量
                    com.Groundwater_Ysupply[y, i] = com.User_Availability_Ygroundwater[y, i] * com.Units_Groundwater_Yuserate[y, i];
                    com.Groundwater_Supply[y, 1, i] = com.Groundwater_Ysupply[y, i];
                    com.User_Availability_Groundwater[y, 1, i] = com.User_Availability_Ygroundwater[y, i];
                    for (int u = 1; u < com.Users; u++)
                    {
                        //各个用户的地下水供水优先级
                        com.User_Orders_Ygroundwater[y, i, u] = Data.Rows[y - 1][com.Users_Name[u] + "优先序"].ToDouble();
                        //各个用户的地下水供水优先级
                        com.User_Proportion_Ygroundwater[y, i, u] = Data.Rows[y - 1][com.Users_Name[u] + "分水比例"].ToDouble();
                        for (int t = 1; t < com.Times; t++)
                        {
                            //年值变月值
                            com.User_Orders_Groundwater[y, t, i, u] = com.User_Orders_Ygroundwater[y, i, u];
                            com.User_Proportion_Groundwater[y, t, i, u] = com.User_Proportion_Ygroundwater[y, i, u];
                        }
                    }
                }
            }

            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            //******************************** 河流初始水量初值赋值      黑龙江初始水量
            //_________________________________________
            var RiverBll = new RiverInitialWaterBLL();
            Data = RiverBll.GetTableByYear(com.Hyear);
            for (int i = 1; i < com.River_Numb; i++)
            {
                for (int y = 1; y < com.Years; y++)
                {
                    for (int t = 1; t < com.Times; t++)
                    {
                        if (Data.Rows[t - 1]["初始水量"].ConvertString() != "Null")
                        {
                            if (i == 1) //黑龙江编号为1
                            {
                                com.RiverQ[y, t, i, 0] = Data.Rows[t - 1]["初始水量"].ToDouble();
                            }
                        }
                        else
                        {
                            com.RiverQ[y, t, i, 1] = 0;
                        }
                        com.YueXun[t] = Data.Rows[t - 1]["月旬"].ConvertString();
                    }
                }
            }

            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            //******************************** 河流初始水量初值赋值      松花江初始水量
            //_________________________________________
            Data = RiverBll.GetTableByYear(com.Syear);
            for (int i = 1; i < com.River_Numb; i++)
            {
                for (int y = 1; y < com.Years; y++)
                {
                    for (int t = 1; t < com.Times; t++)
                    {
                        if (Data.Rows[t - 1]["初始水量"].ConvertString() != "Null")
                        {
                            if (i == 2) //松花江编号为2 
                            {
                                com.RiverQ[y, t, i, 0] = Data.Rows[t - 1]["初始水量"].ToDouble();
                            }
                        }
                    }
                }
            }
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            //******************************** 河流初始水量初值赋值      万金山渠首交叉处 挠力河进入挠力河一干渠水量20210806
            //_________________________________________
            Data = RiverBll.GetTableByYear(com.Nyear);
            for (int i = 1; i < com.River_Numb; i++)
            {
                for (int y = 1; y < com.Years; y++)
                {
                    for (int t = 1; t < com.Times; t++)
                    {
                        if (Data.Rows[t - 1]["初始水量"].ConvertString() != "Null")
                        {
                            if (i == 20) //万金山虚拟河流 编号为20
                            {
                                com.RiverQ[y, t, i, 0] = Data.Rows[t - 1]["初始水量"].ToDouble();
                            }
                        }
                    }
                }
            }
            //---------------------------------------------------------------------------------------------------
            //------------------------------------------------------ 读入退水系统表
            //---------------------------------------------------------------------------------------------------
            Data = new BackwaterSystemBLL().GetTableSortNumber();
            for (int y = 1; y < com.Years; y++)
            {
                for (int t = 1; t < com.Times; t++)
                {
                    for (int i = 1; i < com.return_node; i++)
                    {
                        com.return_node_info[y, t, i, 1] = Data.Rows[i - 1]["退水点编号"].ToDouble();
                        com.return_node_info[y, t, i, 2] = Data.Rows[i - 1]["单元数目"].ToDouble();
                        com.return_node_info[y, t, i, 3] = Data.Rows[i - 1]["计算单元1"].ToDouble();
                    }
                }
            }
            //---------------------------------------------------------------------------------------------------
            //------------------------------------ 读入 调水工程表
            //---------------------------------------------------------------------------------------------------
            Data = new WaterTransferProjectBLL().GetTableByYear(com.Hyear);
            com.Transferin_Node = Data.Rows.Count / (com.Times * (com.Years - 1)) + 1;
            com.transferininfo_numb = Data.Columns.Count + 1;
            com.Transferin_Node_Info = new double[com.Years, com.Times, com.Transferin_Node, com.transferininfo_numb];
            com.transferName = new string[com.Transferin_Node];
            for (int i = 1; i < com.Transferin_Node; i++)
            {
                com.transferName[i] = Data.Rows[i - 1]["调水工程名称"].ConvertString();
                for (int y = 1; y < com.Years; y++)
                {
                    for (int t = 1; t < com.Times; t++)
                    {
                        com.Transferin_Node_Info[y, t, i, 1] = Data.Rows[t - 1]["调入水点编号"].ToDouble();
                        com.Transferin_Node_Info[y, t, i, 2] = Data.Rows[t - 1]["设计调水量"].ToDouble() * 1.05;
                        com.Transferin_Node_Info[y, t, i, 3] = Data.Rows[t - 1]["对应调出点编号"].ToDouble();
                        if (Data.Rows[t - 1]["实际调水量"].ConvertString().IsNullOrEmpty())
                        {
                            com.Transferin_Node_Info[y, t, i, 4] = Data.Rows[t - 1]["实际调水量"].ToDouble();
                        }
                        else
                        {
                            com.Transferin_Node_Info[y, t, i, 4] = 0;
                        }

                        if (t == 7 || t == 13 || t == 16)
                        {
                            com.Transferin_Node_Info[y, t, i, 2] = com.Transferin_Node_Info[y, t, i, 1] * 0.36 * 24 * 11;
                        }
                        else if (t < 5 || t > 16)
                        {
                            com.Transferin_Node_Info[y, t, i, 2] = 0;
                        }
                        else
                        {
                            com.Transferin_Node_Info[y, t, i, 2] = com.Transferin_Node_Info[y, t, i, 1] * 0.36 * 24 * 10;
                        }
                    }
                }
            }
            com.transferDsnY = new double[com.Transferin_Node, com.Years];
            com.transferRelY = new double[com.Transferin_Node, com.Years];
            com.transferChazhiY = new double[com.Transferin_Node, com.Years];
            com.transferinRel = new double[com.Transferin_Node, com.Years, com.Times];
            com.transferChazhi = new double[com.Transferin_Node, com.Years, com.Times];
            //  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
            // ------------------------------------------------  灌溉面积 乘以 灌溉制度 得到需水数据
            //  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
            int ISnumb = 17;
            double[,,] IrrigationSchedule = new double[ISnumb, com.Years, com.Times];//存储 灌溉制度（用水定额）数值
            Data = new IrrigationSystemBLL().GetTableByYear(com.Lyear);
            for (int i = 1; i < ISnumb; i++)
            {
                for (int y = 1; y < com.Years; y++)
                {
                    for (int t = 1; t < com.Times; t++)
                    {
                        IrrigationSchedule[i, y, t] = Data.Rows[t - 1]["用水定额"].ToDouble();
                    }
                }
            }
            com.units_ISnumb = new int[com.Units_Numb];//定义 计算单元所采用的灌溉制度编号
            com.units_ISarea = new double[com.Units_Numb];//定义 计算单元灌溉面积
            com.units_ISdbxishu = new double[com.Units_Numb];//定义 计算单元 地表水利用系数
            com.units_ISdxxishu = new double[com.Units_Numb];// 定义 计算单元 地下水利用系数
            com.fenshuiQ = new double[com.Units_Numb];//灌区渠首设计分水流量
            com.fenshuiW = new double[com.Years, com.Times, com.Units_Numb];//灌区渠首设计分水水量
            //读取 计算单元灌溉面积、利用系数等数据   +分水流量
            Data = new ComputingUnitBLL().GetTable();
            for (int i = 1; i < com.Units_Numb; i++)
            {
                com.units_ISnumb[i] = Data.Rows[i - 1]["灌溉制度编号"].ToInt();
                com.units_ISarea[i] = Data.Rows[i - 1]["灌溉面积"].ToDouble();
                com.units_ISdbxishu[i] = Data.Rows[i - 1]["地表水利用系数"].ToDouble();
                com.units_ISdxxishu[i] = Data.Rows[i - 1]["地下水利用系数"].ToDouble();
                com.fenshuiQ[i] = Data.Rows[i - 1]["分水流量"].ToDouble() * 1;
            }
            //计算  计算单元需水量
            for (int i = 1; i < com.Units_Numb; i++)
            {
                for (int y = 1; y < com.Years; y++)
                {
                    for (int t = 1; t < com.Times; t++)
                    {
                        for (int ii = 1; ii < com.Users; ii++)
                        {
                            //各个计算单元各个用户的需水量，该值在计算过程中此需水不断被供水满足，是实时的单元用户需水量  先全部写为0
                            com.Units_Waterneed[y, t, i, ii] = 0;


                            //各个计算单元各个用户的需水量，该值计算最后用以统计原始需水量
                            com.Units_WaterneedO[y, t, i, ii] = 0;
                        }
                        //判断计算单元属于哪一个灌溉制度  计算 水田需水量 编号4
                        com.Units_Waterneed[y, t, i, 4] = com.units_ISarea[i] * IrrigationSchedule[com.units_ISnumb[i], y, t];  //面积乘以对应灌溉制度的用水定额
                        com.Units_WaterneedO[y, t, i, 4] = com.Units_Waterneed[y, t, i, 4];
                        //直接把净需水转为 地表毛需水
                        //水田
                        if (com.unit_surfacewaterK[y, t, i, 0] > 0)
                        {
                            com.Units_Waterneed[y, t, i, 4] = com.Units_Waterneed[y, t, i, 4] / com.unit_surfacewaterK[y, t, i, 0];
                        }
                        com.Units_WaterneedO[y, t, i, 4] = com.Units_Waterneed[y, t, i, 4];
                    }
                }
            }
            //计算灌区设计分水水量
            for (int i = 1; i < com.Units_Numb; i++)
            {
                for (int y = 1; y < com.Years; y++)
                {
                    for (int t = 1; t < com.Times; t++)
                    {
                        switch (t)
                        {
                            case 7:
                            case 13:
                            case 16:
                                com.fenshuiW[y, t, i] = com.fenshuiQ[i] * 0.36 * 24 * 11;
                                break;
                            default:
                                com.fenshuiW[y, t, i] = com.fenshuiQ[i] * 0.36 * 24 * 10;
                                break;
                        }
                    }
                }
            }
            //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
            //------------------------------------  读入单元退水系数
            //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
            Data = new RegressionCoefficientBLL().GetTableByYear(com.Lyear);
            for (int i = 1; i < com.Units_Numb; i++)
            {
                for (int year = 1; year < com.Years; year++)
                {
                    for (int time = 1; time < com.Times; time++)
                    {
                        for (int ii = 1; ii < com.Users; ii++)
                        {
                            //各个用户的退水系数
                            com.Units_WaterreturnC[year, time, i, ii] = Data.Rows[time - 1][com.Users_Name[ii] + "退水系数"].ToDouble();
                        }
                    }
                }
            }
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&      读入再生水数据
            //
            //
            //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&(供水量，供水优先级，供用户比例)

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            Data = new ReclaimedWaterRatioBLL().GetTableByYear(com.Lyear);
            for (int i = 1; i < com.Units_Numb; i++)
            {
                for (int year = 1; year < com.Years; year++)
                {
                    for (int time = 1; time < com.Times; time++)
                    {
                        com.User_Capability_Recycledwater[year, time, i] = Data.Rows[time - 1]["再生水供水能力"].ToDouble();
                        com.User_Availability_Recycledwater[year, time, i] = Data.Rows[time - 1]["再生水可利用量"].ToDouble();
                        if (com.User_Capability_Recycledwater[year, time, i] >= com.User_Availability_Recycledwater[year, time, i])
                        {
                            com.Recycledwater_Supply[year, time, i] = com.User_Availability_Recycledwater[year, time, i];
                        }
                        else
                        {
                            com.Recycledwater_Supply[year, time, i] = com.User_Capability_Recycledwater[year, time, i];
                        }
                        for (int ii = 1; ii < com.Users; ii++)
                        {
                            //各个用户的再生水供水优先级
                            com.User_Orders_Recycledwater[year, time, i, ii] = Data.Rows[time - 1][com.Users_Name[ii] + "优先序"].ToDouble();
                            //各个用户的再生水的分水比
                            com.User_Proportion_Recycledwater[year, time, i, ii] = Data.Rows[time - 1][com.Users_Name[ii] + "分水比例"].ToDouble();
                        }
                    }
                }
            }
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&    读入本地地表水数据    暂时不用20210806


            //&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&(供水量，供水优先级，供用户比例)
            Data = new LocalSurfaceRatioBLL().GetTableByYear(com.Lyear);
            for (int i = 1; i < com.Units_Numb; i++)
            {
                for (int year = 1; year < com.Years; year++)
                {
                    for (int time = 1; time < com.Times; time++)
                    {
                        com.Locatedwater_RunoffK[year, time, i] = Data.Rows[time - 1]["本地地表径流开发利用系数"].ToDouble();
                        com.User_Capability_Locatedwater[year, time, i] = Data.Rows[time - 1]["本地地表径流量"].ToDouble() * com.Locatedwater_RunoffK[year, time, i];
                        com.User_Availability_Locatedwater[year, time, i] = Data.Rows[time - 1]["本地地表径流供水能力"].ToDouble();
                        com.Locatedwater_Runoff[year, time, i] = Data.Rows[time - 1]["本地地表径流量"].ToDouble() * (1 - com.Locatedwater_RunoffK[year, time, i]);//开发利用之外径流和退水直接进入河道
                        if (com.User_Capability_Locatedwater[year, time, i] >= com.User_Availability_Locatedwater[year, time, i])
                        {
                            com.Locatedwater_Supply[year, time, i] = com.User_Availability_Locatedwater[year, time, i];
                        }
                        else
                        {
                            com.Locatedwater_Supply[year, time, i] = com.User_Capability_Locatedwater[year, time, i];
                        }
                        for (int ii = 1; ii < com.Users; ii++)
                        {
                            //各个用户的本地地表水供水优先级
                            com.User_Orders_Locatedwater[year, time, i, ii] = Data.Rows[time - 1][com.Users_Name[ii] + "优先序"].ToDouble();
                            //各个用户的本地地表水分水比
                            com.User_Proportion_Locatedwater[year, time, i, ii] = Data.Rows[time - 1][com.Users_Name[ii] + "分水比例"].ToDouble();
                        }
                    }
                }
            }
            Data = new RiverDiversionOrderRatioBLL().GetTableByYear(com.Lyear);
            for (int i = 1; i < com.Units_Numb; i++)
            {
                for (int year = 1; year < com.Years; year++)
                {
                    for (int time = 1; time < com.Times; time++)
                    {
                        if (com.User_Capability_Locatedwater[year, time, i] >= com.User_Availability_Locatedwater[year, time, i])
                        {
                            for (int ii = 1; ii < com.Users; ii++)
                            {
                                //各个用户的河网水供水优先级
                                com.user_orders_riverwater[year, time, i, ii] = Data.Rows[time - 1][com.Users_Name[ii] + "优先序"].ToDouble();
                                //各个用户的河网水分水比
                                com.user_proportion_riverwater[year, time, i, ii] = Data.Rows[time - 1][com.Users_Name[ii] + "分水比例"].ToDouble();
                            }
                        }
                        else
                        {
                            for (int ii = 1; ii < com.Users; ii++)
                            {
                                //各个用户的河网水供水优先级
                                com.user_orders_riverwater[year, time, i, ii] = 0;
                                //各个用户的河网水分水比
                                com.user_proportion_riverwater[year, time, i, ii] = 0;
                            }
                        }

                    }
                }
            }
            com.WaterTransferScale = new double[com.Transferin_Node, com.River_Allnode];//存储调水工程各个节点的规模   节点1即为渠首规模
            com.WaterTransferMAX = new double[com.Years, com.Times, com.Transferin_Node, com.River_Allnode];//存储调水工程各个节点的逐时段最大可调水量
            com.ShuitianY = new double[com.Units_Numb, com.Years];//计算单元水田年需水
            com.Qt = new double[com.Times];

            for (int time = 1; time < com.Times; time++)
            {
                switch (time)
                {
                    case 7:
                    case 13:
                    case 16:
                        com.Qt[time] = 0.0001 * 11 * 24 * 3600;    //1旬 按11天
                        break;
                    default:
                        com.Qt[time] = 0.0001 * 10 * 24 * 3600;   //1旬 按10天
                        break;
                }
            }
            com.unit_transfer = new double[com.Years, com.Times, com.Units_Numb];
            com.unit_transferQ = new double[com.Years, com.Times, com.Units_Numb];
            com.unit_transferScale = new double[com.Units_Numb];
            com.yinsongScale = new double[4];
            return com;
        }
    }
}
