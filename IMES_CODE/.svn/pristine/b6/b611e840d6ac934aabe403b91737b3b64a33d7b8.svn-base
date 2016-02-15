using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.FisObject.PCA.MBModel;
using IMES.FisObject.Common.Line;
using IMES.FisObject.PCA.MBMO;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectBase;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.PAK.Pallet;
using IMES.FisObject.Common.CheckItem;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.Common.Repair;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.Common.MO;
using IMES.DataModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Warranty;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.Infrastructure.UnitOfWork;
using IMES.FisObject.Common.NumControl;
using System.Data;

namespace IMES.Maintain.Implementation
{
    public class UniteMBManager : MarshalByRefObject, IUniteMB
    {
        #region IUniteMB Members
        IMBModelRepository MBrepository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<MBCodeDef> getAllUniteMB()
        {

            List<MBCodeDef> result = new List<MBCodeDef>();
            try
            {
                IList<MBCode> getData = MBrepository.GetAllUniteMB();
                if (getData != null && getData.Count > 0)
                {
                    foreach (MBCode mbc in getData)
                    {
                        MBCodeDef mbcDef = new MBCodeDef();
                        mbcDef = convertToMbCodeDef(mbc);
                        result.Add(mbcDef);
                    }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MBCode"></param>
        /// <returns></returns>
        public IList<MBCodeDef> getLstByMB(string MBCode)
        {
            List<MBCodeDef> result = new List<MBCodeDef>();
            try
            {

                IList<MBCode> getData = MBrepository.GetLstByMB(MBCode);
                if (getData != null && getData.Count > 0)
                {
                    foreach (MBCode mbc in getData)
                    {
                        MBCodeDef mbcDef = new MBCodeDef();
                        mbcDef = convertToMbCodeDef(mbc);
                        result.Add(mbcDef);
                    }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void addUniteMB(MBCodeDef obj)
        {
            try
            {
                IList<MBCode> mbLst = MBrepository.GetLstByMB(obj.MBCode);
                if (mbLst.Count > 0)
                {
                    //已经存在具有相同MBCode的MBCode记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT120", erpara);
                    throw ex;
                }
                else
                {
                    MBCode mbc = convertToMBcode(obj);
                    MBrepository.AddUniteMB(mbc);
                }
            }

            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MBCode"></param>
        public void deleteUniteMB(string MBCode)
        {
            try
            {
                MBrepository.DeleteUniteMB(MBCode);
            }
            catch (FisException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="MBCode"></param>
        public void updateUniteMB(MBCodeDef obj, string MBCode)
        {
            IList<MBCode> mbcLst = MBrepository.GetLstByMB(MBCode);
            MBCode mbc = new MBCode();
            MBCode mbcUpdate = new MBCode();
            try
            {
                if (mbcLst != null && mbcLst.Count > 0)
                {
                    mbc = mbcLst.First();
                    obj.cdt = mbc.Cdt;
                    mbcUpdate = convertToMBcode(obj);
                }
                if (!MBCode.Equals(obj.MBCode))
                {
                    IList<MBCode> lstMBC = MBrepository.GetLstByMB(obj.MBCode);
                    if (lstMBC != null && lstMBC.Count > 0)
                    {
                        //已经存在具有相同MBCode的MBCode记录
                        List<string> erpara = new List<string>();
                        FisException ex;
                        ex = new FisException("DMT120", erpara);
                        throw ex;
                    }
                    else
                    {
                        MBrepository.UpdateUniteMB(mbcUpdate, MBCode);
                    }

                }
                else
                {
                    MBrepository.UpdateUniteMB(mbcUpdate,MBCode);
                }
            }
            catch (FisException ex)
            { throw ex; }
            catch (Exception ex)
            { throw ex; }

        }

        #endregion

        public MBCode convertToMBcode(MBCodeDef mbcDef)
        {
            MBCode mbc = new MBCode(mbcDef.MBCode, mbcDef.Description, (short)mbcDef.Qty, mbcDef.editor, mbcDef.cdt, mbcDef.udt, mbcDef.Type);
            return mbc;

        }

        public MBCodeDef convertToMbCodeDef(MBCode mbc)
        {
            MBCodeDef mbcDef = new MBCodeDef();
            mbcDef.MBCode = mbc.MbCode;
            mbcDef.Description = mbc.Description;
            mbcDef.Type = mbc.Type;
            mbcDef.Qty = (int)mbc.MultQty;
            mbcDef.editor = mbc.Editor;
            mbcDef.cdt = mbc.Cdt;
            mbcDef.udt = mbc.Udt;
            return mbcDef;
        }

    }
}
