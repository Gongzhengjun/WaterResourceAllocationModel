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
    public class ShippingFlowBLL
    {
        private ShippingFlowDAL dal = new ShippingFlowDAL();
        /// <summary>
        /// 直接写入航运保证率
        /// </summary>
        /// <returns></returns>
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
                        for (int i = 1; i <= com.River_Node[2]; i++)
                        {
                            result = dal.Increase(2, com.RiverName[2], i, com.NodeName[com.River_Totalnode[2, i]], string.Format("{0:P}", com.hangyunPtime[i]), string.Format("{0:P}", com.hangyunPyear[i]), trans);
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
