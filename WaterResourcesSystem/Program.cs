using BLL;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;

namespace WaterResourcesSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DateTime dtold = DateTime.Now;
            var com = new InputInformation().OneKeyInput("推荐方案（调水方案1111）");
            DateTime dtnew = DateTime.Now;
            TimeSpan dtspan = dtnew - dtold;
            string result = dtspan.Hours.ToString() + "时" + dtspan.Minutes.ToString() + "分" + dtspan.Seconds.ToString() + "秒";
            Console.WriteLine(result);
            dtold = DateTime.Now;
            new RunningTips().RunningTipsClick(com);
            dtnew = DateTime.Now;
            dtspan = dtnew - dtold;
            result = dtspan.Hours.ToString() + "时" + dtspan.Minutes.ToString() + "分" + dtspan.Seconds.ToString() + "秒";
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
