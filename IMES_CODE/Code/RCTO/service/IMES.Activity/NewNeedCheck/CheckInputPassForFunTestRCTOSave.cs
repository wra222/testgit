// INVENTEC corporation (c)2009 all rights reserved. 
// Description: PCA Test Station 检查保存前检查，处理15种异常情况
//                    
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2012-01-13   Kaisheng                     create
// Known issues:
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.FisObject.PCA.MB;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Repair;
using IMES.DataModel;
using IMES.FisObject.Common.TestLog;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Defect;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Part;
namespace IMES.Activity
{
    /// <summary>
    /// PCA Test Station 当前检查站为SMD_A或SMD_B，则检查是否已经通过此站，若通过此战，则报告错误,不良品,良品保存前数据库校验
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于PCA Test Station 
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         不良品,良品保存前数据库校验
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.MB
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         无
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///         IMBRepository
    ///         IMB
    /// </para> 
    /// </remarks>
    public partial class CheckInputPassForFunTestRCTOSave : BaseActivity
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckInputPassForFunTestRCTOSave()
        {
            InitializeComponent();
        }

        /// <summary>
        /// PCA Test Station 检查MBSNO，处理15种异常情况
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IMBRepository currentMBRepository = RepositoryFactory.GetInstance().GetRepository<IMBRepository, IMB>();
            IProductRepository productRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            Product currentProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IBOMRepository bomRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();

            bool mbHave = false;
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
            IHierarchicalBOM curBOM = bomRep.GetHierarchicalBOMByModel(currentProduct.Model);
            IList<IBOMNode> bomNodeLst = curBOM.FirstLevelNodes;

            if (bomNodeLst != null && bomNodeLst.Count > 0)
            {
                foreach (IBOMNode item in bomNodeLst)
                {
                    if (!string.IsNullOrEmpty(item.Part.BOMNodeType) &&
                          item.Part.BOMNodeType.ToUpper() == "MB")
                    {
                        mbHave = true;
                        break;
                    }
                }
            }

            if (!mbHave)
            {
                return base.DoExecute(executionContext);
            }

            if (String.IsNullOrEmpty(currentProduct.PCBID) )
            {
                FisException ex;
                List<string> erpara = new List<string>();
                erpara.Add(currentProduct.ProId);
                ex = new FisException("CHK400", erpara);
                throw ex;
            }
            string MBSN = currentProduct.PCBID;
            string strMBCode = MBSN.Substring(0, 2);
            //Vincent Mark this logical 
           // string strMBCode = "";  
            //if (MBSN.Substring(5, 1) == "M")
            //{
            //    strMBCode = MBSN.Substring(0, 3);
            //    if (MBSN.Substring(4, 1) == "M" && MBSN.Length == 10)
            //    {
            //        strMBCode = MBSN.Substring(0, 2);
            //    }
            //}
            //else if (MBSN.Substring(4, 1) == "M")
            //{
            //    strMBCode = MBSN.Substring(0, 2);
            //}
            //else
            //{
            //    strMBCode = MBSN.Substring(0, 2);
            //}

            //Get latest move out datetime
            DateTime lastMoveOutTime = currentProduct.Status.Udt;

            FaItCnDefectCheckInfo con = new FaItCnDefectCheckInfo();
            con.code = strMBCode;
            IList<FaItCnDefectCheckInfo> listDefectCheck = productRep.GetFaItCnDefectCheckInfoList(con);
            foreach (FaItCnDefectCheckInfo temp in listDefectCheck)
            {
                string MAC = temp.mac;
                string MBCT = temp.mbct;
                string HDDV = temp.hddv;
                string BIOS = temp.bios;


                TestLog eqCondition = new TestLog();
                TestLog notNullCondition = new TestLog();
                IList<TestLog> listTestLog = null;
                string mac15 = "";
                string mac32 = "";
                string mbct15 = "";
                string mbct32 = "";

                eqCondition.Status = TestLog.TestLogStatus.Pass;
                eqCondition.Sn = MBSN;//add
                eqCondition.Station = "15";
                notNullCondition.Remark = "";
                listTestLog = currentMBRepository.GetPCBTestLogInfo(eqCondition, notNullCondition);               
                //MAC:E4115B3D46FB~MBCT:PCCKB001X2A4X7~V:CT11-IU2.03~BIOS:68SCF Ver. F.22~ 
                foreach (TestLog tmp in listTestLog)
                {
                    if (tmp.Sn != currentProduct.PCBID)
                    {
                        continue;
                    }
                    string strgetremark = tmp.Remark.ToUpper();
                    if (MAC == "Y")
                    {
                        var strMACremark = "";
                        var ilocMAC = strgetremark.IndexOf("MAC");
                        if (ilocMAC != -1)
                        {
                            ilocMAC = ilocMAC + 4;
                            string strremarkMAC = strgetremark.Substring(ilocMAC);
                            var ilenMAC = strremarkMAC.IndexOf("~");
                            if (ilenMAC == -1)
                                strMACremark = strremarkMAC;
                            else
                                strMACremark = strremarkMAC.Substring(0, ilenMAC);
                        }
                        if ("" == strMACremark)
                        {
                            //SA未上传MAC
                            throw new FisException("CHK550", new string[] { });
                        }
                        else
                        {
                            if (mac15 == "")
                            {
                                mac15 = strMACremark;
                            }
                        }
                    }
                    if (MBCT == "Y")
                    {
                        var strMBCTremark = "";
                        var ilocMBCT = strgetremark.IndexOf("MBCT");
                        if (ilocMBCT != -1)
                        {
                            ilocMBCT = ilocMBCT + 5;
                            string strremarkMBCT = strgetremark.Substring(ilocMBCT);
                            var ilenMBCT = strremarkMBCT.IndexOf("~");

                            if (ilenMBCT == -1)
                            {
                                strMBCTremark = strremarkMBCT;
                            }
                            else
                            {
                                strMBCTremark = strremarkMBCT.Substring(0, ilenMBCT);
                            }
                        }
                        if ("" == strMBCTremark)
                        {
                            //SA未上传MBCT
                            throw new FisException("CHK551", new string[] { });
                        }
                        else
                        {
                            if (mbct15 == "")
                            {
                                mbct15 = strMBCTremark;
                            }
                        }
                    }
                    break;
                }
                if (MAC == "Y")
                {
                    if (mac15 == "")
                    {
                        //SA未上传MAC
                        throw new FisException("CHK550", new string[] { });
                    }
                }
                if (MBCT == "Y")
                {
                    if (mbct15 == "")
                    {
                        //SA未上传MBCT
                        throw new FisException("CHK551", new string[] { });
                    }
                }

                //eqCondition.Status = TestLog.TestLogStatus.Pass;
                //eqCondition.Sn = MBSN;//add
                //eqCondition.Station = "32";
                //notNullCondition.Remark = "";
                //listTestLog = currentMBRepository.GetPCBTestLogInfo(eqCondition, notNullCondition);

                //mantis 1902: RCTO..Function Test For RCTO页面check 测试记录逻辑修改
                listTestLog = currentMBRepository.GetPCBTestLogListFromPCBTestLog(MBSN, 1, "32", lastMoveOutTime);
                bool no1 = true;
                bool no2 = true;
                foreach (TestLog tmp in listTestLog)
                {
                    if (tmp.Sn != currentProduct.PCBID)
                    {
                        continue;
                    }
                    string strgetremark = tmp.Remark.ToUpper();
                    var strMACremark = "";
                    if (MAC == "Y")
                    {
                        var ilocMAC = strgetremark.IndexOf("MAC");
                        if (ilocMAC != -1)
                        {
                            ilocMAC = ilocMAC + 4;
                            string strremarkMAC = strgetremark.Substring(ilocMAC);
                            var ilenMAC = strremarkMAC.IndexOf("~");
                            if (ilenMAC == -1)
                                strMACremark = strremarkMAC;
                            else
                                strMACremark = strremarkMAC.Substring(0, ilenMAC);
                        }
                        if ("" == strMACremark)
                        {
                            //FA未上传MAC
                            throw new FisException("CHK552", new string[] { });
                        }
                        else
                        {
                            if (mac32 == "")
                            {
                                mac32 = strMACremark;
                            }
                        }
                    }
                    if (MBCT == "Y")
                    {
                        var strMBCTremark = "";
                        var ilocMBCT = strgetremark.IndexOf("MBCT");
                        if (ilocMBCT != -1)
                        {
                            ilocMBCT = ilocMBCT + 5;
                            string strremarkMBCT = strgetremark.Substring(ilocMBCT);
                            var ilenMBCT = strremarkMBCT.IndexOf("~");

                            if (ilenMBCT == -1)
                            {
                                strMBCTremark = strremarkMBCT;
                            }
                            else
                            {
                                strMBCTremark = strremarkMBCT.Substring(0, ilenMBCT);
                            }
                        }
                        if ("" == strMBCTremark)
                        {
                            //FA未上传MBCT
                            throw new FisException("CHK553", new string[] { });
                        }
                        else
                        {
                            if (mbct32 == "")
                            {
                                mbct32 = strMBCTremark;
                            }
                        }
                    }
                    if (HDDV != "N" && HDDV != "")
                    {
                        var strHDDVremark = "";
                        var ilocHDDV = strgetremark.IndexOf("HDD");
                        if (ilocHDDV != -1)
                        {
                            ilocHDDV = ilocHDDV + 4;
                            string strremarkHDDV = strgetremark.Substring(ilocHDDV);
                            var ilenHDDV = strremarkHDDV.IndexOf("~");

                            if (ilenHDDV == -1)
                            {
                                strHDDVremark = strremarkHDDV;
                            }
                            else
                            {
                                strHDDVremark = strremarkHDDV.Substring(0, ilenHDDV);
                            }
                        }
                        if ("" == strHDDVremark)
                        {
                            //FA未上传V
                            throw new FisException("CHK554", new string[] { });
                        }
                        no1 = false;
                        if (strHDDVremark != HDDV)
                        {
                            //FA上传的V与Maintain值不一致
                            throw new FisException("CHK555", new string[] { });
                        }
                    }
                    if (BIOS != "N" && BIOS != "")
                    {
                        var strBIOSremark = "";
                        var ilocBIOS = strgetremark.IndexOf("BIOS");
                        if (ilocBIOS != -1)
                        {
                            ilocBIOS = ilocBIOS + 5;
                            string strremarkBIOS = strgetremark.Substring(ilocBIOS);
                            var ilenBIOS = strremarkBIOS.IndexOf("~");

                            if (ilenBIOS == -1)
                            {
                                strBIOSremark = strremarkBIOS;
                            }
                            else
                            {
                                strBIOSremark = strremarkBIOS.Substring(0, ilenBIOS);
                            }
                        }
                        if ("" == strBIOSremark)
                        {
                            //FA未上传BIOS
                            throw new FisException("CHK556", new string[] { });
                        }
                        no2 = false;
                        if (strBIOSremark != BIOS)
                        {
                            //FA上传的BIOS与Maintain值不一致
                            throw new FisException("CHK557", new string[] { });
                        }
                    }
                    break;
                }
                if (MAC == "Y")
                {
                    if (mac32 == "")
                    {
                        //FA未上传MAC
                        throw new FisException("CHK552", new string[] { });
                    }
                }
                if (MBCT == "Y")
                {
                    if (mbct32 == "")
                    {
                        //FA未上传MBCT
                        throw new FisException("CHK553", new string[] { });
                    }
                }
                if (HDDV != "N" && HDDV != "")
                {
                    if (no1)
                    {
                        //FA未上传V
                        throw new FisException("CHK554", new string[] { });
                    }
                }
                if (BIOS != "N" && BIOS != "")
                {
                    if (no2)
                    {
                        //FA未上传BIOS
                        throw new FisException("CHK556", new string[] { });
                    }
                }
                if (MAC == "Y")
                {
                    if (mac15 != mac32)
                    {
                        //SA和FA上传的MAC不一致
                        throw new FisException("CHK558", new string[] { });
                    }
                }
                if (MBCT == "Y")
                {
                    if (mbct15 != mbct32)
                    {
                        //SA和FA上传的MBCT不一致
                        throw new FisException("CHK559", new string[] { });
                    }
                }
            }

           

            return base.DoExecute(executionContext);
        }
    }
}
