using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using IMES.Infrastructure.Repository.PCA;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject;
using log4net;
using IMES.FisObject.PCA.MB;

namespace IMES.Maintain.Implementation
{
    public class TestMBManager : MarshalByRefObject, ITestMB
    {
        protected IMBRepository itestMB = RepositoryFactory.GetInstance().GetRepository<IMBRepository>();
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region ITestMB Members

        public IList<string> getCode(string family)
        {
            throw new NotImplementedException();
        }

        public void addMB_Test(IMES.DataModel.MB_TestDef mbTest)
        {
            IList<MBTestDef> lstMBtestDef = new List<MBTestDef>();
            lstMBtestDef = itestMB.GetMBTestByCodeFamilyAndType(mbTest.Code, mbTest.Family, mbTest.Type);
            try
            {

                MBTestDef mbTestDef = new MBTestDef();
                mbTestDef.family = mbTest.Family;
                mbTestDef.type = mbTest.Type;
                mbTestDef.code = mbTest.Code;
                mbTestDef.remark = mbTest.Remark;
                mbTestDef.editor = mbTest.editor;
                mbTestDef.cdt = mbTest.cdt;
                mbTestDef.udt = mbTest.udt;

                if (lstMBtestDef != null && lstMBtestDef.Count > 0)
                {
                    //已经存在具有相同testMB的记录
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT113", erpara);
                    throw ex;

                }
                else
                {
                    itestMB.AddMBTest(mbTestDef);
                }
            }

            catch (Exception e)
            {

                throw e;
            }
        }


        public void deleteMB_Test(string code, string family, bool type)
        {
            IList<MBTestDef> lstMBtestDef = new List<MBTestDef>();
            lstMBtestDef = itestMB.GetMBTestByCodeFamilyAndType(code, family, type);
            try
            {

                if (lstMBtestDef == null || lstMBtestDef.Count <= 0)
                {   //已经不存在此testMb记录，不能删除
                    List<string> erpara = new List<string>();
                    FisException ex;
                    ex = new FisException("DMT114", erpara);
                    throw ex;
                }
                else
                {
                    itestMB.DeleteMBTest(code, family, type);
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }


        public IList<IMES.DataModel.MB_TestDef> getMBTestbyFamily(string family)
        {
            IList<MB_TestDef> lstMB_TestDef = new List<MB_TestDef>();
            IList<MBTestDef> lstMbTestDef = new List<MBTestDef>();
            try
            {
                lstMbTestDef = itestMB.GetMBTestbyFamily(family);
                if (lstMbTestDef != null && lstMbTestDef.Count > 0)
                {
                    lstMB_TestDef = convertToMB_TestDef(lstMbTestDef);
                    return lstMB_TestDef;
                }
                else
                {
                    return null;
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }


        protected IList<MB_TestDef> convertToMB_TestDef(IList<MBTestDef> lstMBTestDef)
        {
            IList<MB_TestDef> lstMB_TestDef = new List<MB_TestDef>();
            foreach (MBTestDef mbtestDef in lstMBTestDef)
            {
                MB_TestDef mb_testDef = new MB_TestDef();
                mb_testDef.id = mbtestDef.id;
                mb_testDef.Family = mbtestDef.family;
                mb_testDef.Code = mbtestDef.code;
                mb_testDef.Remark = mbtestDef.remark;
                mb_testDef.Type = mbtestDef.type;
                mb_testDef.editor = mbtestDef.editor;
                mb_testDef.cdt = mbtestDef.cdt;
                mb_testDef.udt = mbtestDef.udt;
                lstMB_TestDef.Add(mb_testDef);
            }
            return lstMB_TestDef;
        }


        protected IList<MBTestDef> convertTombTestDef(IList<MB_TestDef> lstMB_TestDef)
        {
            IList<MBTestDef> lstMBTestDef = new List<MBTestDef>();
            foreach (MB_TestDef mbtestDef in lstMB_TestDef)
            {
                MBTestDef mbTestDef = new MBTestDef();
                mbTestDef.id = mbtestDef.id;
                mbTestDef.family = mbtestDef.Family;
                mbTestDef.code = mbtestDef.Code;
                mbTestDef.remark = mbtestDef.Remark;
                mbTestDef.type = mbtestDef.Type;
                mbTestDef.editor = mbtestDef.editor;
                mbTestDef.cdt = mbtestDef.cdt;
                mbTestDef.udt = mbtestDef.udt;
                lstMBTestDef.Add(mbTestDef);
            }
            return lstMBTestDef;
        }

        public IList<MB_TestDef> GetMBTestByCodeFamilyAndType(string code, string family, bool type)
        {
            IList<MB_TestDef> lstMB_TestDef = new List<MB_TestDef>();
            IList<MBTestDef> lstMbTestDef = new List<MBTestDef>();
            try
            {
                lstMbTestDef = itestMB.GetMBTestByCodeFamilyAndType(code, family, type);
                if (lstMbTestDef != null && lstMbTestDef.Count > 0)
                {
                    lstMB_TestDef = convertToMB_TestDef(lstMbTestDef);
                    return lstMB_TestDef;
                }
                else
                {
                    return null;
                }
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw;
            }
        }

        #endregion
    }
}
