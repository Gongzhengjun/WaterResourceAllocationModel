using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Util
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class ToolHelper
    {
        /// <summary>
        /// 转换Int类型
        /// </summary>
        /// <param name="str">想转换的字符串</param>
        /// <param name="num">为空默认值(默认是0)</param>
        /// <returns></returns>
        public static int ToInt(this object str, int num = 0)
        {
            var Str = str.ConvertString();
            if (Str.IsNullOrEmpty())
            {
                return num;
            }
            try
            {
                if (Str.IndexOf('.') <= 0)
                {
                    return int.Parse(Str);
                }
                else
                {
                    var number = (int)Str.ToDecimal();
                    return number;
                }

            }
            catch (Exception)
            {
                throw new Exception("转换Int类型出错！");
            }

        }
        /// <summary>
        /// 转换Decimal类型
        /// </summary>
        /// <param name="str">想转换的字符串</param>
        /// <param name="num">为空默认值(默认是0)</param>
        /// <returns></returns>
        public static decimal ToDecimal(this object str, decimal num = 0)
        {
            var Str = str.ConvertString();
            if (Str.IsNullOrEmpty())
            {
                return num;
            }
            try
            {
                return decimal.Parse(Str);
            }
            catch (Exception)
            {

                throw new Exception("转换Decimal类型出错！");
            }

        }
        /// <summary>
        /// 转换Double类型
        /// </summary>
        /// <param name="str">想转换的字符串</param>
        /// <param name="num">为空默认值(默认是0)</param>
        /// <returns></returns>
        public static double ToDouble(this object str, double num = 0)
        {
            var Str = str.ConvertString();
            if (Str.IsNullOrEmpty())
            {
                return num;
            }
            try
            {
                return double.Parse(Str);
            }
            catch (Exception)
            {

                throw new Exception("转换Decimal类型出错！");
            }

        }
        /// <summary>
        /// 转换DateTime类型
        /// </summary>
        /// <param name="str">想转换的字符串</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object str)
        {
            var Str = str.ConvertString();
            if (Str.IsNullOrEmpty())
            {
                return DateTime.MinValue;
            }
            try
            {
                return DateTime.Parse(Str);
            }
            catch (Exception)
            {
                throw new Exception("转换DateTime类型出错！");
            }

        }
        /// <summary>
        /// 转换Long类型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static long ToLong(this object str, long num = 0)
        {
            var Str = str.ConvertString();
            if (Str.IsNullOrEmpty())
            {
                return num;
            }
            try
            {
                return long.Parse(Str);
            }
            catch (Exception)
            {
                throw new Exception("转换DateTime类型出错！");
            }
        }
        /// <summary>
        /// 转换String类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertString<T>(this T str)
        {
            return str == null ? "" : str.ToString();
        }
        /// <summary>
        /// 表为空时显示列名
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="DisplayColumns"></param>
        /// <returns></returns>
        public static DataTable ShowColumnName(this DataTable dt, string DisplayColumns)
        {
            if (dt.IsNull())
            {
                dt = new DataTable();
            }
            if (dt.Rows.Count == 0)
            {
                string[] ColumnNames = DisplayColumns.Split(',');
                foreach (var item in ColumnNames)
                {
                    dt.Columns.Add(new DataColumn(item));
                }
            }
            return dt;
        }

        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt)
        {
            var list = new List<T>();
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            if (dt.IsNull() || dt.Rows.Count == 0)
            {
                return null;
            }

            foreach (DataRow item in dt.Rows)
            {
                T s = Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name.ToLower() == dt.Columns[i].ColumnName.ToLower());
                    if (info != null)
                    {
                        try
                        {
                            if (!Convert.IsDBNull(item[i]))
                            {
                                var v = Convert.ChangeType(item[i], info.PropertyType.GetDefaultType());
                                info.SetValue(s, v, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("字段[" + info.Name + "]转换出错," + ex.Message);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }

        /// <summary>
        /// DataTable转成实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this DataTable dt)
        {
            T s = Activator.CreateInstance<T>();
            if (dt == null || dt.Rows.Count == 0)
            {
                return default(T);
            }
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                if (info != null)
                {
                    try
                    {
                        if (!Convert.IsDBNull(dt.Rows[0][i]))
                        {
                            var v = Convert.ChangeType(dt.Rows[0][i], info.PropertyType.GetDefaultType());
                            info.SetValue(s, v, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("字段[" + info.Name + "]转换出错," + ex.Message);
                    }
                }
            }
            return s;
        }
        /// <summary>
        /// 为空类型返回默认类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetDefaultType(this Type type)
        {
            var DefaultType = Nullable.GetUnderlyingType(type);
            return DefaultType.IsNull() ? type : DefaultType;
        }
        /// <summary>
        /// 布尔类型转Int
        /// </summary>
        /// <param name="Isbool"></param>
        /// <returns></returns>
        public static int ToInt(this bool Isbool)
        {
            if (Isbool)
                return 1;
            return 0;
        }
        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this T obj) where T : class
        {
            return obj == null;
        }
        /// <summary>
        /// List转Table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static DataTable ToTable<T>(this List<T> array)
        {
            var ret = new DataTable();
            foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                ret.Columns.Add(dp.Name.ToUpper());
            foreach (T item in array)
            {
                var Row = ret.NewRow();
                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                    Row[dp.Name.ToUpper()] = dp.GetValue(item);
                ret.Rows.Add(Row);
            }
            return ret;
        }
        /// <summary>
        /// 启用程序
        /// </summary>
        /// <param name="FileName">程序名称(加后缀名)</param>
        /// <param name="FilePath">文件路径</param>
        public static void StartProgram(string FileName, string FilePath)
        {
            //声明一个程序类
            System.Diagnostics.Process Proc = new System.Diagnostics.Process(); ;

            try
            {
                Proc.StartInfo.WorkingDirectory = FilePath; // 初始化可执行文件的文件夹信息
                Proc.StartInfo.FileName = FileName; // 初始化可执行文件名
                Proc.StartInfo.UseShellExecute = true;
                //
                //启动外部程序
                //
                Proc.Start();
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                //Console.WriteLine("系统找不到指定的程序文件。\r{0}", e);
                return;
            }
            #region " 设定时间关闭程序 "
            //打印出外部程序的开始执行时间
            //Console.WriteLine("外部程序的开始执行时间：{0}", Proc.StartTime);

            ////等待3秒钟
            //Proc.WaitForExit(3000);

            ////如果这个外部程序没有结束运行则对其强行终止
            //if (Proc.HasExited == false)
            //{
            //    Console.WriteLine("由主程序强行终止外部程序的运行！");
            //    Proc.Kill();
            //}
            //else
            //{
            //    Console.WriteLine("由外部程序正常退出！");
            //}
            //Console.WriteLine("外部程序的结束运行时间：{0}", Proc.ExitTime);
            //Console.WriteLine("外部程序在结束运行时的返回值：{0}", Proc.ExitCode);
            #endregion
        }
        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="Num"></param>
        /// <param name="Digits">留的位数</param>
        /// <returns></returns>
        public static decimal Rounding(this decimal Num, int Digits = 2)
        {
            return Math.Round(Num, Digits, MidpointRounding.AwayFromZero);
        }

        #region POST
        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string HttpPost(string url, string param = null)
        {
            HttpWebRequest request;

            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;



            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }

            return responseStr;
        }


        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
        public static string BuildRequest(string strUrl, Dictionary<string, string> dicPara, string fileName)
        {
            string contentType = "image/jpeg";
            //待请求参数数组
            FileStream Pic = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] PicByte = new byte[Pic.Length];
            Pic.Read(PicByte, 0, PicByte.Length);
            int lengthFile = PicByte.Length;

            //构造请求地址

            //设置HttpWebRequest基本信息
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            //设置请求方式：get、post
            request.Method = "POST";
            //设置boundaryValue
            string boundaryValue = DateTime.Now.Ticks.ToString("x");
            string boundary = "--" + boundaryValue;
            request.ContentType = "\r\nmultipart/form-data; boundary=" + boundaryValue;
            //设置KeepAlive
            request.KeepAlive = true;
            //设置请求数据，拼接成字符串
            StringBuilder sbHtml = new StringBuilder();
            foreach (KeyValuePair<string, string> key in dicPara)
            {
                sbHtml.Append(boundary + "\r\nContent-Disposition: form-data; name=\"" + key.Key + "\"\r\n\r\n" + key.Value + "\r\n");
            }
            sbHtml.Append(boundary + "\r\nContent-Disposition: form-data; name=\"pic\"; filename=\"");
            sbHtml.Append(fileName);
            sbHtml.Append("\"\r\nContent-Type: " + contentType + "\r\n\r\n");
            string postHeader = sbHtml.ToString();
            //将请求数据字符串类型根据编码格式转换成字节流
            Encoding code = Encoding.GetEncoding("UTF-8");
            byte[] postHeaderBytes = code.GetBytes(postHeader);
            byte[] boundayBytes = Encoding.ASCII.GetBytes("\r\n" + boundary + "--\r\n");
            //设置长度
            long length = postHeaderBytes.Length + lengthFile + boundayBytes.Length;
            request.ContentLength = length;

            //请求远程HTTP
            Stream requestStream = request.GetRequestStream();
            Stream myStream = null;
            try
            {
                //发送数据请求服务器
                requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                requestStream.Write(PicByte, 0, lengthFile);
                requestStream.Write(boundayBytes, 0, boundayBytes.Length);
                HttpWebResponse HttpWResp = (HttpWebResponse)request.GetResponse();
                myStream = HttpWResp.GetResponseStream();
            }
            catch (WebException e)
            {
                //LogResult(e.Message);
                return "";
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                }
            }

            //读取处理结果
            StreamReader reader = new StreamReader(myStream, code);
            StringBuilder responseData = new StringBuilder();

            String line;
            while ((line = reader.ReadLine()) != null)
            {
                responseData.Append(line);
            }
            myStream.Close();
            Pic.Close();

            return responseData.ToString();
        }
        #endregion

        #region 将json转换为DataTable

        /// <summary>
        /// 将json转换为DataTable
        /// </summary>
        /// <param name="strJson">得到的json</param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string strJson)
        {
            //转换json格式
            strJson = strJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
            //取出表名   
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名   
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));
            //获取数据   
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split('*');
                //创建表   
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        string[] strCell = str.Split('#');
                        if (strCell[0].Substring(0, 1) == "\"")
                        {
                            int a = strCell[0].Length;
                            dc.ColumnName = strCell[0].Substring(1, a - 2);
                        }
                        else
                        {
                            dc.ColumnName = strCell[0];
                        }
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }
                //增加内容   
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }
            return tb;
        }

        #endregion

        #region " 读取XML节点内容 "
        /// <summary>
        /// 读取XML节点内容
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="NodeName"></param>
        /// <returns></returns>
        public static string GetNodeContent(string FileName, string NodeName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(FileName);//加载xml文件
            XmlNode node = doc.SelectSingleNode("//" + NodeName);
            return node.InnerText;
        }
        #endregion

       
        /// <summary>
        /// 源码密码加密方式
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ComputeHash(string source, string key)
        {
            if (source == null)
            {
                return "";
            }
            string text = "abcdefghjklmnopqrstuvwxyz";
            if (source.Length < 0x1a)
            {
                source = source + text.Substring(source.Length);
            }

            byte[] inArray = Encoding.Unicode.GetBytes(source);
            int length = inArray.Length;
            if ((key == null) || (key.Length == 0))
            {
                key = "Encrypthejinhua";
            }

            byte[] bytes = Encoding.Unicode.GetBytes(key);
            byte num2 = Convert.ToByte(bytes.Length);
            byte num3 = 2;
            byte index = 0;
            for (int i = 0; i < length; i++)
            {
                byte[] buffer3;
                IntPtr ptr;
                byte num5 = (byte)(bytes[index] | num2);
                num5 = (byte)(num5 & num3);
                (buffer3 = inArray)[(int)(ptr = (IntPtr)i)] = (byte)(buffer3[(int)ptr] ^ num5);
                num3 = (byte)(num3 + 1);
                if (num3 > 0xfd)
                {
                    num3 = 2;
                }
                index = (byte)(index + 1);
                if (index >= num2)
                {
                    index = 0;
                }
            }
            return Convert.ToBase64String(inArray, 0, inArray.Length);
        }
    }
}
