using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class Common
    {
        /// <summary>
        /// 河流名称
        /// </summary>
        public string[]? RiverName { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string[]? NodeName { get; set; }
        /// <summary>
        /// 存储计算单元名称
        /// </summary>
        public string[]? UnitsName { get; set; }
        /// <summary>
        /// 分区名称
        /// </summary>
        public string[]? FenquName { get; set; }
        /// <summary>
        /// 结果输出时，确定表格长度
        /// </summary>
        public int Outputnumb { get; set; }
        /// <summary>
        /// 工程分区个数
        /// </summary>
        public int Fenqus { get; set; }
        /// <summary>
        /// 县名
        /// </summary>
        public string[]? CountyName { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string[]? CityName { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string[]? ProvinceName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? SJLTname { get; set; }
        /// <summary>
        /// 月旬
        /// </summary>
        public string[]? YueXun { get; set; }
        /// <summary>
        /// 黑龙江丰平枯来水年份
        /// </summary>
        public int Hyear { get; set; }
        /// <summary>
        /// 松花江丰平枯来水年份
        /// </summary>
        public int Syear { get; set; }
        /// <summary>
        /// 本地降水丰平枯来水年份
        /// </summary>
        public int Lyear { get; set; }
        /// <summary>
        /// 挠力河丰平枯来水年份
        /// </summary>
        public int Nyear { get; set; }
        /// <summary>
        /// 定义松花江控制流量
        /// </summary>
        public double SHJ_limitQ1 { get; set; }
        /// <summary>
        /// 水量与流量之间的转换
        /// </summary>
        public double[]? Qt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double[,]? SHJ_limitQ { get; set; }
        /// <summary>
        /// 定义调水工程各节点的规模
        /// </summary>
        public double[,]? WaterTransferScale { get; set; }
        /// <summary>
        /// 定义调水工程各节点最大可调水水量 逐历时
        /// </summary>
        public double[,,,]? WaterTransferMAX { get; set; }
        /// <summary>
        /// 干渠各个节点的缺水量 削峰逆推法使用  决定规模
        /// </summary>
        public double[,,,]? Channel_Node_Shortage { get; set; }
        /// <summary>
        /// 干渠各个节点的缺水量
        /// </summary>
        public double[,,,]? Channel_Node_ShortageQ { get; set; }
        /// <summary>
        /// 引黑规模 水量
        /// </summary>
        public double Yinhei_Guimo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Yinsong_Guimo { get; set; }
        /// <summary>
        /// 本计算单元的水田 毛需水 年值
        /// </summary>
        public double[,]? ShuitianY { get; set; }
        /// <summary>
        /// 计算时间  hbd 修改
        /// </summary>
        public int Time { get; set; }
        /// <summary>
        /// 计算时间  hbd 修改
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 计算时间  hbd 修改
        /// </summary>
        public int First_Year { get; set; }
        /// <summary>
        /// 计算总历时数
        /// </summary>
        public int Times { get; set; }
        /// <summary>
        /// 年数
        /// </summary>
        public int Years { get; set; }
        /// <summary>
        /// 月旬数
        /// </summary>
        public int YueXuns { get; set; }
        /// <summary>
        /// 计算单元数目
        /// </summary>
        public int Units_Numb { get; set; }
        /// <summary>
        /// 计算单元编号
        /// </summary>
        public int[]? Units { get; set; }
        /// <summary>
        /// 计算单元各用户需水量，计算过程中变化
        /// </summary>
        public double[,,,]? Units_Waterneed { get; set; }
        /// <summary>
        /// 计算单元各用户原始需水量
        /// </summary>
        public double[,,,]? Units_WaterneedO { get; set; }
        /// <summary>
        /// 计算单元各用户原始需水量
        /// </summary>
        public double[,,,]? units_waterneed1 { get; set; }
        /// <summary>
        /// 单元用水户分类
        /// </summary>
        public int Users { get; set; }
        /// <summary>
        /// 计算单元用户名称
        /// </summary>
        public string[]? Users_Name { get; set; }
        /// <summary>
        /// 单元用水户分类
        /// </summary>
        public int[]? User_Kinds { get; set; }
        /// <summary>
        /// 各水源该计算单元可供水量
        /// </summary>
        public double[]? Water_Supply { get; set; }
        /// <summary>
        /// 计算单元各用户退水量
        /// </summary>
        public double[,,,]? Units_Waterreturn { get; set; }
        /// <summary>
        /// 计算单元各用户退水系数
        /// </summary>
        public double[,,,]? Units_WaterreturnC { get; set; }
        /// <summary>
        /// 计算单元总退水量
        /// </summary>
        public double[,,]? Units_Watereturn_Total { get; set; }
        /// <summary>
        /// 计算单元到相应河流的引水系数
        /// </summary>
        public double[]? River_UnitsK { get; set; }
        /// <summary>
        /// 河流节点向计算单元的引水量
        /// </summary>
        public double[,,,]? Riverwater_Supply { get; set; }
        /// <summary>
        /// 再生水各用户优先级
        /// </summary>
        public double[,,,]? User_Orders_Recycledwater { get; set; }
        /// <summary>
        /// 再生水各用户分水比
        /// </summary>
        public double[,,,]? User_Proportion_Recycledwater { get; set; }
        /// <summary>
        /// 各计算单元再生水供水能力
        /// </summary>
        public double[,,]? User_Capability_Recycledwater { get; set; }
        /// <summary>
        /// 各计算单元再生水理论可供水量
        /// </summary>
        public double[,,]? User_Availability_Recycledwater { get; set; }
        /// <summary>
        /// 各计算单元再生水实际可供水量
        /// </summary>
        public double[,,]? Recycledwater_Supply { get; set; }
        /// <summary>
        /// 各计算单元再生水实际供水量
        /// </summary>
        public double[,,,]? Recycledwater_Supply_Ture { get; set; }
        /// <summary>
        /// 本地地表水各用户优先级
        /// </summary>
        public double[,,,]? User_Orders_Locatedwater { get; set; }
        /// <summary>
        /// 本地地表水各用户优先级
        /// </summary>
        public double[,,,]? User_Proportion_Locatedwater { get; set; }
        /// <summary>
        /// 各计算单元本地地表水供水能力
        /// </summary>
        public double[,,]? User_Capability_Locatedwater { get; set; }
        /// <summary>
        /// 各计算单元本地地表水理论可供水量
        /// </summary>
        public double[,,]? User_Availability_Locatedwater { get; set; }
        /// <summary>
        /// 各计算单元本地地表水实际可供水量
        /// </summary>
        public double[,,]? Locatedwater_Supply { get; set; }
        /// <summary>
        /// 各计算单元本地地表水实际供水量
        /// </summary>
        public double[,,,]? Locatedwater_Supply_Ture { get; set; }
        /// <summary>
        /// 各计算单元本地地表产流（开发利用率之外的部分直接进入河道的产流量）
        /// </summary>
        public double[,,]? Locatedwater_Runoff { get; set; }
        /// <summary>
        /// 各计算单元本地地表径流开发利用系数
        /// </summary>
        public double[,,]? Locatedwater_RunoffK { get; set; }
        /// <summary>
        /// 地下水各用户优先级
        /// </summary>
        public double[,,,]? User_Orders_Groundwater { get; set; }
        /// <summary>
        /// 地下水各用户分水比
        /// </summary>
        public double[,,,]? User_Proportion_Groundwater { get; set; }
        /// <summary>
        /// 各计算单元地下水供水能力
        /// </summary>
        public double[,,]? User_Capability_Groundwater { get; set; }
        /// <summary>
        /// 各计算单元地下水理论可供水量
        /// </summary>
        public double[,,]? User_Availability_Groundwater { get; set; }
        /// <summary>
        /// 各计算单元地下水实际可供水量
        /// </summary>
        public double[,,]? Groundwater_Supply { get; set; }
        /// <summary>
        /// 各计算单元地下水实际供水量
        /// </summary>
        public double[,,,]? Groundwater_Supply_Ture { get; set; }
        /// <summary>
        /// 各计算单元地下水实际供水量
        /// </summary>
        public double[,,]? Units_Groundwater_Userate { get; set; }
        /// <summary>
        /// 各计算单元地下水实际可供水量  年可开采水量
        /// </summary>
        public double[,]? Groundwater_Ysupply { get; set; }
        /// <summary>
        /// 地下水的开发利用率
        /// </summary>
        public double[,]? Units_Groundwater_Yuserate { get; set; }
        /// <summary>
        /// 各计算单元地下水理论可供水量
        /// </summary>
        public double[,]? User_Availability_Ygroundwater { get; set; }
        /// <summary>
        /// 地下水各用户优先级
        /// </summary>
        public double[,,]? User_Orders_Ygroundwater { get; set; }
        /// <summary>
        /// 地下水各用户分水比
        /// </summary>
        public double[,,]? User_Proportion_Ygroundwater { get; set; }
        /// <summary>
        /// 河网提水点向各用户优先级
        /// </summary>
        public double[]? User_Orders_Pumpwater { get; set; }
        /// <summary>
        /// 河网提水点向各用户分水比
        /// </summary>
        public double[]? User_Proportion_Pumpwater { get; set; }
        /// <summary>
        /// 河道数目
        /// </summary>
        public int River_Numb { get; set; }
        /// <summary>
        /// 河道基本信息
        /// </summary>
        public int[,]? River_Info { get; set; }
        /// <summary>
        /// 河流信息数目
        /// </summary>
        public int River_Attribute { get; set; }
        /// <summary>
        /// 各河流节点数目（循环中使用，动态）
        /// </summary>
        public int[]? River_Node { get; set; }
        /// <summary>
        /// 节点属性信息表
        /// </summary>
        public int[,]? River_Node_Info { get; set; }
        /// <summary>
        /// 所有节点数目
        /// </summary>
        public int River_Allnode { get; set; }
        /// <summary>
        /// 河流节点水量        不是流量！！
        /// </summary>
        public double[,,,]? RiverQ { get; set; }
        /// <summary>
        /// 河流节点至上一个节点损失系数
        /// </summary>
        public double[,,,]? RiverK { get; set; }
        /// <summary>
        /// 河道节点实际供水量
        /// </summary>
        public double[,,,,]? Riverwater_Supply_Ture { get; set; }
        /// <summary>
        /// 节点流量限制
        /// </summary>
        public double[,,,]? RiverQ_Limit { get; set; }
        /// <summary>
        /// 用户总供水量
        /// </summary>
        public double[,,,]? Water_Supply_Ture { get; set; }
        /// <summary>
        /// 用户总退水量
        /// </summary>
        public double[,,,]? Waterreturn_Total { get; set; }
        /// <summary>
        /// 计算单元中最大缺水率
        /// </summary>
        public double Units_MaxwatershortR { get; set; }
        /// <summary>
        /// 计算单元中最大缺水率对应的计算单元编号，为调整参数准备
        /// </summary>
        public int Unitscode_MaxwatershortR { get; set; }
        /// <summary>
        /// 计算单元中最小缺水率
        /// </summary>
        public double Units_MinwatershortR { get; set; }
        /// <summary>
        /// 计算单元中最小缺水率对应的计算单元编号，为调整参数准备
        /// </summary>
        public double Unitscode_MinwatershortR { get; set; }
        /// <summary>
        /// 定义调水工程开关  0 表示不使用调水工程， 1 表示采用该规划调水工程（引逊工程）
        /// </summary>
        public int Waterschedule_Project1 { get; set; }
        /// <summary>
        /// (七台河、鸡西引水)
        /// </summary>
        public int Waterschedule_Project2 { get; set; }
        /// <summary>
        /// (引松补挠力河)
        /// </summary>
        public int Waterschedule_Project3 { get; set; }
        /// <summary>
        /// (呼玛河引水)
        /// </summary>
        public int Waterschedule_Project4 { get; set; }
        /// <summary>
        /// (引黑济松工程)
        /// </summary>
        public int Waterschedule_Project5 { get; set; }
        /// <summary>
        /// (诺敏河引水工程)
        /// </summary>
        public int Waterschedule_Project6 { get; set; }
        /// <summary>
        /// (绰尔河引水工程)
        /// </summary>
        public int Waterschedule_Project7 { get; set; }
        /// <summary>
        /// 县级区个数
        /// </summary>
        public int County_Numb { get; set; }
        /// <summary>
        /// 地级区个数
        /// </summary>
        public int City_Numb { get; set; }
        /// <summary>
        /// 省级区个数
        /// </summary>
        public int Province_Numb { get; set; }
        /// <summary>
        /// 节点总编号(带入子程序用)
        /// </summary>
        public int[,]? River_Totalnode { get; set; }
        /// <summary>
        /// 河道节点总编号
        /// </summary>
        public int River_Totalnoderank { get; set; }
        /// <summary>
        /// 节点总的可供水量
        /// </summary>
        public double[,,]? Riverwater_Totalsupply { get; set; }
        /// <summary>
        /// 节点实际供水量
        /// </summary>
        public double[,,]? Riverwater_Node_Supply { get; set; }
        /// <summary>
        /// 退水点数目
        /// </summary>
        public int Channel_Node { get; set; }
        /// <summary>
        /// 退水点信息
        /// </summary>
        public double[,,,]? Channel_Node_Info { get; set; }
        /// <summary>
        /// 引水点信息
        /// </summary>
        public double[]? channel_node_info { get; set; }
        /// <summary>
        /// 提水点数目
        /// </summary>
        public int pump_node { get; set; }
        /// <summary>
        /// 提水点信息
        /// </summary>
        public double[]? pump_node_info { get; set; }
        /// <summary>
        /// 退水点数目
        /// </summary>
        public int return_node { get; set; }
        /// <summary>
        /// 退水点信息
        /// </summary>
        public double[,,,]? return_node_info { get; set; }              
        /// <summary>
        /// 调出水点数目
        /// </summary>
        public int Transferin_Node { get; set; }
        /// <summary>
        /// 调出水点信息
        /// </summary>
        public double[,,,]? Transferin_Node_Info { get; set; }
        /// <summary>
        /// 水库数目
        /// </summary>
        public int Reservoir_Numb { get; set; }
        public int reservoir_numb { get; set; }

        /// <summary>
        /// 水库信息列
        /// </summary>
        public int Reservoir_Fields { get; set; }
        /// <summary>
        /// 水库信息
        /// </summary>
        public double[]? Reservoir_Info { get; set; }
        /// <summary>
        /// 水库供水信息  ？？？？需不需要考虑到供水类型
        /// </summary>
        public double[,,,]? Reservoirwater_Supply { get; set; }
        /// <summary>
        /// 水库可供水量【年，历时，水库编号】
        /// </summary>
        public double[]? Reservoirwater_Available { get; set; }
        /// <summary>
        /// 水库实际供水信息【年，历时，水库编号，计算单元】
        /// </summary>
        public double[,,,,]? Reservoirwater_Supply_Ture { get; set; }
        /// <summary>
        /// 水库对水库调水【年，历时，调出水库，供给水库】
        /// </summary>
        public double[]? Reservoir_To_Reservoir { get; set; }
        /// <summary>
        /// 确定界河水供水单元属性数组
        /// </summary>
        public int[]? Boundaryriver { get; set; }
        /// <summary>
        /// 确定界河水各个计算单元各个用户供水量
        /// </summary>
        public double[,,,]? Boundaryriver_Supply_Ture { get; set; }
        /// <summary>
        /// 计算单元最终实际供水量
        /// </summary>
        public double[,,]? Units_Water_Supply { get; set; }
        /// <summary>
        /// 计算单元最终缺水量
        /// </summary>
        public double[,,]? units_water_shortQ { get; set; }
        /// <summary>
        /// 计算单元最终缺水率
        /// </summary>
        public double[,,]? units_water_shortR { get; set; }
        /// <summary>
        /// 计算单元各用户最终缺水率
        /// </summary>
        public double[,,,]? units_water_usershortR { get; set; }
        /// <summary>
        /// 所有计算单元各用户总缺水量
        /// </summary>
        public double[,,]? water_usershort { get; set; }
        /// <summary>
        /// 所有计算单元各用户总缺水量原始值
        /// </summary>
        public double[,,]? water_usershortO { get; set; }
        /// <summary>
        /// 所有计算单元各用户总缺水率
        /// </summary>
        public double[,,]? water_usershortR { get; set; }
        /// <summary>
        /// 河网水向计算单元供水
        /// </summary>
        public double[,,]? riverwater_units_supply { get; set; }
        /// <summary>
        /// 河网水向计算单元供水
        /// </summary>
        public double[,,,]? riverwater_unitsuser_supply { get; set; }
        /// <summary>
        /// 河网水各用户优先级
        /// </summary>
        public double[,,,]? user_orders_riverwater { get; set; }
        /// <summary>
        /// 河网水各用户分水比
        /// </summary>
        public double[,,,]? user_proportion_riverwater { get; set; }
        /// <summary>
        /// 水库水向计算单元供水
        /// </summary>
        public double[,,]? reservoirwater_units_supply { get; set; }
        /// <summary>
        /// 水库水向计算单元供水
        /// </summary>
        public double[,,,]? reservoirwater_unitsuser_supply { get; set; }
        /// <summary>
        /// 每个水库向每个单元的供水
        /// </summary>
        public double[,,,]? reservoirwater_node_supply { get; set; }

        /// <summary>
        /// 水库水各用户优先级
        /// </summary>
        public double[,,,,]? user_orders_reservoirwater { get; set; }
        /// <summary>
        /// 水库水各用户供水能力限制
        /// </summary>
        public double[,,,,]? user_proportion_reservoirwater { get; set; }

        /// <summary>
        /// 计算单元对应工程分区数组
        /// </summary>
        public int[,]? fenqu_units { get; set; }
        /// <summary>
        /// 计算单元对应县级区数组
        /// </summary>
        public int[,]? county_units { get; set; }
        /// <summary>
        /// 计算单元对应地级区数组
        /// </summary>
        public int[,]? city_units { get; set; }
        /// <summary>
        /// 计算单元对应省级区数组
        /// </summary>
        public int[,]? province_units { get; set; }
        /// <summary>
        /// 各个工程分区各用户缺水量
        /// </summary>
        public double[,,,]? fenqu_shortQ { get; set; }
        /// <summary>
        /// 各个工程分区各用户需水量
        /// </summary>
        public double[,,,]? fenqu_shortQO { get; set; }
        /// <summary>
        /// 各个工程分区各用户缺水率
        /// </summary>
        public double[,,,]? fenqu_shortR { get; set; }
        /// <summary>
        /// 再生水工程分区供水量
        /// </summary>
        public double[,,]? recycledwater_fenqu_supply { get; set; }
        /// <summary>
        /// 本地地表水工程分区供水量
        /// </summary>
        public double[,,]? locatedwater_fenqu_supply { get; set; }
        /// <summary>
        /// 地下水工程分区供水量
        /// </summary>
        public double[,,]? groundwater_fenqu_supply { get; set; }
        /// <summary>
        /// 河网水工程分区供水量
        /// </summary>
        public double[,,]? riverwater_fenqu_supply { get; set; }
        /// <summary>
        /// 水库水工程分区供水量
        /// </summary>
        public double[,,]? reservoirwater_fenqu_supply { get; set; }
        /// <summary>
        /// 水库水工程分区供水量
        /// </summary>
        public double[,,]? boundaryriver_fenqu_supply { get; set; }
        /// <summary>
        /// 工程分区总的供水量
        /// </summary>
        public double[,,]? fenqu_supplyQ { get; set; }
        /// <summary>
        /// 本地水各个单元供水量
        /// </summary>
        public double[,,]? locatedwater_unit_supply { get; set; }
        /// <summary>
        /// 再生水各个单元供水量
        /// </summary>
        public double[,,]? recycledwater_unit_supply { get; set; }
        /// <summary>
        /// 地下水各个单元供水量
        /// </summary>
        public double[,,]? groundwater_unit_supply { get; set; }
        /// <summary>
        /// 河网（引提水）水各个单元供水量
        /// </summary>
        public double[,,]? riverwater_unit_supply { get; set; }
        /// <summary>
        /// 水库水各个单元供水量
        /// </summary>
        public double[,,]? reservoirwater_unit_supply { get; set; }
        /// <summary>
        /// 界河水各个单元供水量
        /// </summary>
        public double[,,]? boundaryriver_unit_supply { get; set; }
        /// <summary>
        /// 各个单元原始总的需水量
        /// </summary>
        public double[,,]? units_waterneedsum { get; set; }
        /// <summary>
        /// 三江连通工程区总需水量各历时
        /// </summary>
        public double[,]? SJLT_waterneed { get; set; }
        /// <summary>
        /// 三江连通工程区总供水量各历时
        /// </summary>
        public double[,]? SJLT_watersupply { get; set; }
        /// <summary>
        /// 三江连通工程区总缺水量各历时
        /// </summary>
        public double[,]? SJLT_watershortage { get; set; }
        /// <summary>
        /// 三江连通工程区本地水总供水量
        /// </summary>
        public double[,]? locatedwater_SJLT_supply { get; set; }
        /// <summary>
        /// 三江连通工程区再生水总供水量
        /// </summary>
        public double[,]? recycledwater_SJLT_supply { get; set; }
        /// <summary>
        /// 三江连通工程区地下水总供水量
        /// </summary>
        public double[,]? groundwater_SJLT_supply { get; set; }
        /// <summary>
        /// 三江连通工程区河网（引提水）总供水量
        /// </summary>
        public double[,]? riverwater_SJLT_supply { get; set; }
        /// <summary>
        /// 三江连通工程区水库水总供水量
        /// </summary>
        public double[,]? reservoirwater_SJLT_supply { get; set; }
        /// <summary>
        /// 三江连通工程区界河水总供水量
        /// </summary>
        public double[,]? boundaryriver_SJLT_supply { get; set; }
        /// <summary>
        /// 各个县级区向各个用户缺水量
        /// </summary>
        public double[,,,]? county_short_ture { get; set; }
        /// <summary>
        /// 各个县级区向各个用户缺水率
        /// </summary>
        public double[,,,]? county_shortR { get; set; }
        /// <summary>
        /// 各个县级区各个用户原始需水量
        /// </summary>
        public double[,,,]? county_needO_ture { get; set; }
        /// <summary>
        /// 本地水各个县级区供水量
        /// </summary>
        public double[,,]? locatedwater_county_supply { get; set; }
        /// <summary>
        /// 再生水各个县级区供水量
        /// </summary>
        public double[,,]? recycledwater_county_supply { get; set; }
        /// <summary>
        /// 地下水各个县级区供水量
        /// </summary>
        public double[,,]? groundwater_county_supply { get; set; }
        /// <summary>
        /// 河网（引提水）水各个县级区供水量
        /// </summary>
        public double[,,]? riverwater_county_supply { get; set; }
        /// <summary>
        /// 水库水各个县级区供水量
        /// </summary>
        public double[,,]? reservoirwater_county_supply { get; set; }
        /// <summary>
        /// 界河水各个县级区供水量
        /// </summary>
        public double[,,]? boundaryriver_county_supply { get; set; }
        /// <summary>
        /// 各个地级区向各个用户缺水量
        /// </summary>
        public double[,,,]? city_short_ture { get; set; }
        /// <summary>
        /// 各个地级区向各个用户缺水率
        /// </summary>
        public double[,,,]? city_shortR { get; set; }
        /// <summary>
        /// 各个地级区各个用户原始需水量
        /// </summary>
        public double[,,,]? city_needO_ture { get; set; }
        /// <summary>
        /// 本地水各个地级区供水量
        /// </summary>
        public double[,,]? locatedwater_city_supply { get; set; }
        /// <summary>
        /// 再生水各个地级区供水量
        /// </summary>
        public double[,,]? recycledwater_city_supply { get; set; }
        /// <summary>
        /// 地下水各个地级区供水量
        /// </summary>
        public double[,,]? groundwater_city_supply { get; set; }
        /// <summary>
        /// 河网（引提水）水各个地级区供水量
        /// </summary>
        public double[,,]? riverwater_city_supply { get; set; }
        /// <summary>
        /// 水库水各个地级区供水量
        /// </summary>
        public double[,,]? reservoirwater_city_supply { get; set; }
        /// <summary>
        /// 界河水各个地级区供水量
        /// </summary>
        public double[,,]? boundaryriver_city_supply { get; set; }
        /// <summary>
        /// 各个省级区向各个用户缺水量
        /// </summary>
        public double[,,,]? province_short_ture { get; set; }
        /// <summary>
        /// 各个省级区向各个用户缺水率
        /// </summary>
        public double[,,,]? province_shortR { get; set; }
        /// <summary>
        /// 各个省级区各个用户原始需水量
        /// </summary>
        public double[,,,]? province_needO_ture { get; set; }
        /// <summary>
        ///  本地水各个省级区供水量
        /// </summary>
        public double[,,]? locatedwater_province_supply { get; set; }
        /// <summary>
        ///  再生水各个省级区供水量
        /// </summary>
        public double[,,]? recycledwater_province_supply { get; set; }
        /// <summary>
        /// 地下水各个省级区供水量
        /// </summary>
        public double[,,]? groundwater_province_supply { get; set; }
        /// <summary>
        /// 河网（引提水）水各个省级区供水量
        /// </summary>
        public double[,,]? riverwater_province_supply { get; set; }
        /// <summary>
        /// 水库水各个省级区供水量
        /// </summary>
        public double[,,]? reservoirwater_province_supply { get; set; }
        /// <summary>
        /// 界河水各个省级区供水量
        /// </summary>
        public double[,,]? boundaryriver_province_supply { get; set; }
        /// <summary>
        /// 再生水供用户水量
        /// </summary>
        public double[,,]? users_recycledwatersupply { get; set; }
        /// <summary>
        /// 本地地表水水供用户水量
        /// </summary>
        public double[,,]? users_locatedwatersupply { get; set; }
        /// <summary>
        /// 地下水供用户水量
        /// </summary>
        public double[,,]? users_groundwatersupply { get; set; }
        /// <summary>
        /// 河网水供用户水量
        /// </summary>
        public double[,,]? users_riverwatersupply { get; set; }
        /// <summary>
        /// 水库水供用户水量
        /// </summary>
        public double[,,]? users_reservoirwatersupply { get; set; }
        /// <summary>
        /// 界河水供用户水量
        /// </summary>
        public double[,,]? users_boundaryriversupply { get; set; }


        // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        // ******************************** 定义调水工程参数
        /// <summary>
        /// 节点调水到计算单元
        /// </summary>
        public double[]? transfer_rivernode_units { get; set; }
        /// <summary>
        /// 节点调水到水库
        /// </summary>
        public double[,,,]? transfer_rivernode_reservoir { get; set; }
        /// <summary>
        /// 节点调水到节点
        /// </summary>
        public double[]? transfer_rivernode_rivernode { get; set; }
        /// <summary>
        /// 水库调水到计算单元
        /// </summary>
        public double[]? transfer_reservoir_units { get; set; }
        /// <summary>
        /// 水库调水到水库
        /// </summary>
        public double[]? transfer_reservoir_reservoir { get; set; }
        /// <summary>
        /// 水库调水到节点
        /// </summary>
        public double[]? transfer_reservoir_rivernode { get; set; }




        // '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // '''''''''''''''''''''''' hbd 新增 定义 ''''''''''''''''''''''''''''''''''''''''''''
        // '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        // '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // '''''''''''''''''''''''' 渠系水有效利用系数 定义 ''''''''''''''''''''''''''''''''
        /// <summary>
        /// 计算单元农业需水地下水渠系有效利用系数
        /// </summary>
        public double[,,,]? unit_groundwaterK { get; set; }
        /// <summary>
        /// 计算单元农业需水地下水渠系有效利用系数
        /// </summary>
        public double[,,,]? unit_surfacewaterK { get; set; }

        /// <summary>
        /// 定义各计算单元 河道引提供水 分水比 与 计算单元1分水比类似
        /// </summary>
        public double[,,]? unit_yintiK { get; set; }
        /// <summary>
        /// 节点提水总量，过程变量
        /// </summary>
        public double yintiQ { get; set; }

        // '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        // '''''''''''''''''''''''' 保证率 定义 ''''''''''''''''''''''''''''''''''''''''''''
        /// <summary>
        /// 历时保证率     计算单元
        /// </summary>
        public double[,]? unitsusers_waterPtime { get; set; }
        /// <summary>
        /// 年保证率
        /// </summary>
        public double[,]? unitsusers_waterPyear { get; set; }

        /// <summary>
        /// 综合平均历时保证率    整体结果
        /// </summary>
        public double[]? units_waterPtime { get; set; }
        /// <summary>
        /// 综合平均年保证率
        /// </summary>
        public double[]? units_waterPyear { get; set; }

        /// <summary>
        /// 航运历时保证率  每个节点都需要计算
        /// </summary>
        public double[]? hangyunPtime { get; set; }
        /// <summary>
        /// 航运 年 保证率
        /// </summary>
        public double[]? hangyunPyear { get; set; }      

        // '''''''''''统计 所有计算单元 加和 起来的 保证率   而不是 整个区域面上的保证率

        // 分子为各个单元不缺水的个数的总和，分母为 units_numb*times 和 units_numb * years
        // 数据库中 就是 计算单元的 历时和年保证率 最后多一行 “整个区域”

        public double unitsum_Ptime { get; set; }
        /// <summary>
        /// 整个区域的 综合平均保证率 （实际上没有意义！！）
        /// </summary>
        public double unitsum_Pyear { get; set; }                
        public double[]? unitsum_user_Ptime { get; set; }
        /// <summary>
        /// 整个区域的 不同用水户的 保证率
        /// </summary>
        public double[]? unitsum_user_Pyear { get; set; }              

        // '''''''''''''''''''三江连通工程区 面上 保证率
        public double[]? SJLT_users_Ptime { get; set; }
        public double[]? SJLT_users_Pyear { get; set; }


        public double SJLT_Ptime { get; set; }
        public double SJLT_Pyear { get; set; }

        // ''''''''''''''''''''''''''''''''''''''定义 所有灌区的 用水保证率

        public double guanqu_Ptime { get; set; }
        public double guanqu_Pyear { get; set; }

        public double[]? guanqu_user_Ptime { get; set; }
        public double[]? guanqu_user_Pyear { get; set; }



        // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
        // ****************************************************************************************************************************************************************
        // _____________________________________________________________________________________________________________________________________________________
        /// <summary>
        /// 所有用户的需水量年值
        /// </summary>
        public double[,]? SJLT_user_waterneedOY { get; set; }
        /// <summary>
        /// 所有用户的缺水量年值
        /// </summary>
        public double[,]? SJLT_user_watershortQY { get; set; }
        /// <summary>
        /// 所有用户的缺水率年值
        /// </summary>
        public double[,]? SJLT_user_watershortRY { get; set; }
        /// <summary>
        /// 三江连通工程区年总需水量年值
        /// </summary>
        public double[]? SJLT_waterneedY { get; set; }
        /// <summary>
        /// 三江连通工程区年总供水量年值
        /// </summary>
        public double[]? SJLT_watersupplyY { get; set; }
        /// <summary>
        /// 三江连通工程区年总缺水量年值
        /// </summary>
        public double[]? SJLT_watershortY { get; set; }
        /// <summary>
        /// 三江连通工程区年总缺水率年值
        /// </summary>
        public double[]? SJLT_watershortRY { get; set; }
        /// <summary>
        /// 三江连通工程区本地水总供水量
        /// </summary>
        public double[]? locatedwater_SJLT_supplyY { get; set; }
        /// <summary>
        /// 三江连通工程区再生水总供水量
        /// </summary>
        public double[]? recycledwater_SJLT_supplyY { get; set; }
        /// <summary>
        /// 三江连通工程区地下水总供水量
        /// </summary>
        public double[]? groundwater_SJLT_supplyY { get; set; }
        /// <summary>
        /// 三江连通工程区河网（引提水）总供水量
        /// </summary>
        public double[]? riverwater_SJLT_supplyY { get; set; }
        /// <summary>
        /// 三江连通工程区水库水总供水量
        /// </summary>
        public double[]? reservoirwater_SJLT_supplyY { get; set; }
        /// <summary>
        /// 三江连通工程区界河水总供水量
        /// </summary>
        public double[]? boundaryriver_SJLT_supplyY { get; set; }                    



        // ''''''''''''''''''''''''''''''''''''''' 定义年值的合计量  整个三江连通工程区 年值 再累加求和

        public double SJLT_waterneedYsum { get; set; }
        public double SJLT_watersupplyYsum { get; set; }
        public double SJLT_watershortYsum { get; set; }
        public double SJLT_watershortRYsum { get; set; }
        public double locatedwater_SJLT_supplyYsum { get; set; }
        public double recycledwater_SJLT_supplyYsum { get; set; }
        public double groundwater_SJLT_supplyYsum { get; set; }
        public double riverwater_SJLT_supplyYsum { get; set; }
        public double reservoirwater_SJLT_supplyYsum { get; set; }
        public double boundaryriver_SJLT_supplyYsum { get; set; }

        // '''''''''''''''''''''''''''''''''''''''''''''定义整个三江连通区分用水户  年值的合计值
        public double[]? SJLT_user_watershortQYsum { get; set; }
        public double[]? SJLT_user_waterneedOYsum { get; set; }
        public double[]? SJLT_user_watershortRYsum { get; set; }


        // ^------------------------------------------------- 计算单元（原四级区套地市）年值结果输出
        /// <summary>
        /// 各个计算单元年总供水量
        /// </summary>
        public double[,]? units_watersupplyY { get; set; }
        /// <summary>
        /// 各个计算单元年总缺水量
        /// </summary>
        public double[,]? units_watershortQY { get; set; }
        /// <summary>
        /// 各个计算单元年总缺水率
        /// </summary>
        public double[,]? units_watershortRY { get; set; }
        /// <summary>
        /// 各个计算单元年总原始需水量
        /// </summary>
        public double[,]? units_waterneedOY { get; set; }
        /// <summary>
        /// 各个计算单元年本地地表水供水量
        /// </summary>
        public double[,]? units_locatedwater_supplyY { get; set; }
        /// <summary>
        /// 各个计算单元年再生水供水量
        /// </summary>
        public double[,]? units_recycledwater_supplyY { get; set; }
        /// <summary>
        /// 各个计算单元年地下水供水量
        /// </summary>
        public double[,]? units_groundwater_supplyY { get; set; }
        /// <summary>
        /// 各个计算单元年河网水供水量
        /// </summary>
        public double[,]? units_riverwater_supplyY { get; set; }
        /// <summary>
        /// 各个计算单元年水库水供水量
        /// </summary>
        public double[,]? units_reservoirdwater_supplyY { get; set; }
        /// <summary>
        /// 各个计算单元年界河供水量
        /// </summary>
        public double[,]? units_boundaryriver_supplyY { get; set; }
        /// <summary>
        /// 各个计算单元各个用户年缺水量
        /// </summary>
        public double[,,]? unitsusers_watershortQY { get; set; }
        /// <summary>
        /// 各个计算单元各个用户年缺水率
        /// </summary>
        public double[,,]? unitsusers_watershortRY { get; set; }
        /// <summary>
        /// 各个计算单元各个用户年原需水量
        /// </summary>
        public double[,,]? unitsusers_waterneedOY { get; set; }

        // ------------------------------ 工程分区结果汇总
        /// <summary>
        /// 各个工程分区各用户缺水量  '+1'为了存储所有工程分区统计量
        /// </summary>
        public double[,,]? fenqu_users_shortQY { get; set; }
        /// <summary>
        /// 各个工程分区各用户原始需水量
        /// </summary>
        public double[,,]? fenqu_users_shortQOY { get; set; }
        /// <summary>
        /// 各个工程分区各用户缺水
        /// </summary>
        public double[,,]? fenqu_users_shortRY { get; set; }
        /// <summary>
        /// 再生水工程分区供水量
        /// </summary>
        public double[,]? recycledwater_fenqu_supplyY { get; set; }
        /// <summary>
        /// 本地地表水工程分区供水量
        /// </summary>
        public double[,]? locatedwater_fenqu_supplyY { get; set; }
        /// <summary>
        /// 地下水工程分区供水量
        /// </summary>
        public double[,]? groundwater_fenqu_supplyY { get; set; }
        /// <summary>
        /// 河网水工程分区供水量
        /// </summary>
        public double[,]? riverwater_fenqu_supplyY { get; set; }
        /// <summary>
        /// 水库水工程分区供水量
        /// </summary>
        public double[,]? reservoirwater_fenqu_supplyY { get; set; }
        /// <summary>
        /// 水库水工程分区供水量
        /// </summary>
        public double[,]? boundaryriver_fenqu_supplyY { get; set; }



        // ---------------- 县级区结果年值统计
        /// <summary>
        /// 各个县级区向各个用户缺水率  users + 1 表示对所用户
        /// </summary>
        public double[,,]? county_short_tureY { get; set; }
        /// <summary>
        /// 各个县级区向各个用户原始需水量
        /// </summary>
        public double[,,]? county_needO_tureY { get; set; }
        /// <summary>
        /// 各个县级区向各个用户缺水率  users + 1 表示对所用户
        /// </summary>
        public double[,,]? county_shortRY { get; set; }
        /// <summary>
        /// 本地水各个县级区供水量
        /// </summary>
        public double[,]? locatedwater_county_supplyY { get; set; }
        /// <summary>
        /// 再生水各个县级区供水量
        /// </summary>
        public double[,]? recycledwater_county_supplyY { get; set; }
        /// <summary>
        /// 地下水各个县级区供水量
        /// </summary>
        public double[,]? groundwater_county_supplyY { get; set; }
        /// <summary>
        /// 河网（引提水）水各个县级区供水量
        /// </summary>
        public double[,]? riverwater_county_supplyY { get; set; }
        /// <summary>
        /// 水库水各个县级区供水量
        /// </summary>
        public double[,]? reservoirwater_county_supplyY { get; set; }
        /// <summary>
        /// 界河水各个县级区供水量
        /// </summary>
        public double[,]? boundaryriver_county_supplyY { get; set; }


        // ---------------- 地级区结果年值统计
        /// <summary>
        /// 各个地级区向各个用户缺水率  users + 1 表示对所用户
        /// </summary>
        public double[,,]? city_short_tureY { get; set; }
        /// <summary>
        /// 各个地级区向各个用户原始需水量
        /// </summary>
        public double[,,]? city_needO_tureY { get; set; }
        /// <summary>
        /// 各个地级区向各个用户缺水率  users + 1 表示对所用户
        /// </summary>
        public double[,,]? city_shortRY { get; set; }

        /// <summary>
        /// 本地水各个地级区供水量
        /// </summary>
        public double[,]? locatedwater_city_supplyY { get; set; }
        /// <summary>
        /// 再生水各个地级区供水量
        /// </summary>
        public double[,]? recycledwater_city_supplyY { get; set; }
        /// <summary>
        /// 地下水各个地级区供水量
        /// </summary>
        public double[,]? groundwater_city_supplyY { get; set; }
        /// <summary>
        ///  河网（引提水）水各个地级区供水量
        /// </summary>
        public double[,]? riverwater_city_supplyY { get; set; }
        /// <summary>
        /// 水库水各个地级区供水量
        /// </summary>
        public double[,]? reservoirwater_city_supplyY { get; set; }
        /// <summary>
        /// 界河水各个地级区供水量
        /// </summary>
        public double[,]? boundaryriver_city_supplyY { get; set; }


        // ---------------- 省级区结果年值统计
        /// <summary>
        /// 各个省级区向各个用户缺水率  users + 1 表示对所用户
        /// </summary>
        public double[,,]? province_short_tureY { get; set; }
        /// <summary>
        /// 各个省级区向各个用户原始需水量
        /// </summary>
        public double[,,]? province_needO_tureY { get; set; }
        /// <summary>
        /// 各个省级区向各个用户缺水率  users + 1 表示对所用户
        /// </summary>
        public double[,,]? province_shortRY { get; set; }
        /// <summary>
        /// 本地水各个省级区供水量
        /// </summary>
        public double[,]? locatedwater_province_supplyY { get; set; }
        /// <summary>
        /// 再生水各个省级区供水量
        /// </summary>
        public double[,]? recycledwater_province_supplyY { get; set; }
        /// <summary>
        /// 地下水各个省级区供水量
        /// </summary>
        public double[,]? groundwater_province_supplyY { get; set; }
        /// <summary>
        /// 河网（引提水）水各个省级区供水量
        /// </summary>
        public double[,]? riverwater_province_supplyY { get; set; }
        /// <summary>
        /// 水库水各个省级区供水量
        /// </summary>
        public double[,]? reservoirwater_province_supplyY { get; set; }
        /// <summary>
        /// 界河水各个省级区供水量
        /// </summary>
        public double[,]? boundaryriver_province_supplyY { get; set; }


        // '''''' 所有灌区 的个数
        /// <summary>
        /// 并非实际灌区个数，而是拆分后的总数
        /// </summary>
        public int guanqu_numb { get; set; }
        /// <summary>
        /// 保护区起始编号
        /// </summary>
        public int baohuqu_Fnum { get; set; }
        /// <summary>
        /// 年调水规模-设计
        /// </summary>
        public double[,]? transferDsnY { get; set; }
        /// <summary>
        /// 年调水规模-实际
        /// </summary>
        public double[,]? transferRelY { get; set; }                            
        public double[,]? transferChazhiY { get; set; }
        /// <summary>
        /// 历时 实际调水量
        /// </summary>
        public double[,,]? transferinRel { get; set; }                            
        public double[,,]? transferChazhi { get; set; }
        /// <summary>
        /// 调水工程名称
        /// </summary>
        public string[]? transferName { get; set; }                       
        public int transferininfo_numb { get; set; }
        /// <summary>
        /// 存储水库供水成果数据
        /// </summary>
        public double[]? shuiku_info { get; set; }
        /// <summary>
        /// 存数引提水点对应的计算单元编号 其实两者数量上是相等的
        /// </summary>
        public int[]? channel_unit_numb { get; set; }               

        public int channelinfo_numb { get; set; }
        public int returninfo_numb { get; set; }
        public int riverinfo_numb { get; set; }
        /// <summary>
        /// 存储计算单元调水过程  水量
        /// </summary>
        public double[,,]? unit_transfer { get; set; }
        /// <summary>
        /// 存储计算单元调水过程 流量
        /// </summary>
        public double[,,]? unit_transferQ { get; set; }
        /// <summary>
        /// 存储计算单元调水规模
        /// </summary>
        public double[]? unit_transferScale { get; set; }
        /// <summary>
        /// 引松区和沿江区 75%的流量值
        /// </summary>
        public double[]? yinsongScale { get; set; }
        /// <summary>
        /// 试验站有效降雨量
        /// </summary>
        public double[]? eP { get; set; }
        /// <summary>
        /// 试验站个数
        /// </summary>
        public int eP_numb { get; set; }

        /// <summary>
        /// 定义 计算单元所采用的灌溉制度编号
        /// </summary>
        public int[]? units_ISnumb { get; set; }
        /// <summary>
        /// 定义 计算单元灌溉面积
        /// </summary>
        public double[]? units_ISarea { get; set; }
        /// <summary>
        ///  定义 计算单元 地表水利用系数
        /// </summary>
        public double[]? units_ISdbxishu { get; set; }
        /// <summary>
        /// 定义 计算单元 地下水利用系数
        /// </summary>
        public double[]? units_ISdxxishu { get; set; }
        /// <summary>
        /// 灌区渠首设计分水流量
        /// </summary>
        public double[]? fenshuiQ { get; set; }
        /// <summary>
        /// 灌区渠首设计分水水量
        /// </summary>
        public double[,,]? fenshuiW { get; set; }            
    }
}

