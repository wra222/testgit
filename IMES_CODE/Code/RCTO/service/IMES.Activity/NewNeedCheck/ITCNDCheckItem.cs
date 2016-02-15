/*
 * INVENTEC corporation ©2011 all rights reserved. 
 * Description:Activity/ITCNDCheckItem
 * UI:CI-MES12-SPEC-FA-UI ITCND Check.docx –2011/10/10 
 * UC:CI-MES12-SPEC-FA-UC ITCND Check.docx –2011/10/10            
 * Update: 
 * Date        Name                  Reason 
 * ==========  ===================== =====================================
* 2011-11-11   zhanghe               (Reference Ebook SourceCode) Create
* Known issues:
* TODO：
* 1.//以下属性尚未提供
* 5.	Check D/L Pass
* A.	Image资料检查 (目前只有此业务，需要灵活配置。根据CheckItem的定义作此处理)
* 异常情况：
* a.	若设定的Item没有上传，，则提示”相关信息还未上传 ”
* XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.Common.Part;

namespace IMES.Activity
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///         应用于ITCND Check
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
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
    /// 数据更新:
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    /// </para> 
    /// </remarks>
    public partial class ITCNDCheckItem : BaseActivity
    {
        /// <summary> 
        /// </summary>
        public ITCNDCheckItem()
        {
            InitializeComponent();
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {                        
            var product = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
            if (product == null)
            {
                throw new NullReferenceException("Product in session is null");
            }
            var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<string[]> needItems = new List<string[]>();
            needItems = productRepository.GetCheckItemListFromITCNDCheckSetting(product.Model);
            if(needItems == null || needItems.Count == 0)
            {
                Model modelObj = product.ModelObj;
                if(modelObj != null)
                {
                    needItems = productRepository.GetCheckItemListFromITCNDCheckSetting(modelObj.FamilyName);
                    if(needItems == null || needItems.Count == 0)
                    {
                        needItems = productRepository.GetCheckItemListFromITCNDCheckSetting(this.Line.Substring(0, 1));
                        if(needItems == null || needItems.Count == 0)
                        {
                            List<string> errpara = new List<string>();
                            FisException e = new FisException("CHK831", errpara);
                            throw e;
                        }
                    }
                }
                else
                {
                    throw new NullReferenceException("Model in session is null");
                }
            }

            bool bExist = false;
            bool bUnique = false;
            bool bFlag = true;
            string value = string.Empty;
            IList<string> array1 = new List<string>();
            IList<string> array2 = new List<string>();
            foreach(string[] temp in needItems)
            {
                //CheckCondition:
                if (String.IsNullOrEmpty(temp[2]))
                {
                    if (temp[1] == "Exist")
                    {
                        bExist = productRepository.CheckExistProductInfo(product.ProId, temp[0]);
                        if (bExist == false)
                        {
                            array1.Clear();
                            array2.Clear();
                            array1.Add(temp[0]);
                            array2.Add("EXIST");
                            bFlag = false;
                            break;
                        }

                        value = (string)product.GetExtendedProperty(temp[0]);
                        if (String.IsNullOrEmpty(value))
                        {
                            array1.Clear();
                            array2.Clear();
                            array1.Add(temp[0]);
                            array2.Add("EXIST");
                            bFlag = false;
                            break;
                        }
                        array1.Add(temp[0]);
                        array2.Add(value);
                    }
                    else if (temp[1] == "Unique")
                    {
                        bExist = productRepository.CheckExistProductInfo(product.ProId, temp[0]);
                        if (bExist == true)
                        {
                            value = (string)product.GetExtendedProperty(temp[0]);
                            if (String.IsNullOrEmpty(value))
                            {
                                array1.Clear();
                                array2.Clear();
                                array1.Add(temp[0]);
                                array2.Add("EXIST");
                                bFlag = false;
                                break;
                            }
                            bUnique = productRepository.CheckExistProductInfoWhoseInfoValueBeenOccupiedByAnotherProduct(product.ProId, temp[0]);
                            if (bUnique == true)
                            {
                                array1.Clear();
                                array2.Clear();
                                array1.Add(temp[0]);
                                array2.Add("UNIQUE");
                                bFlag = false;
                                break;
                            }
                        }
                        else
                        {
                            array1.Clear();
                            array2.Clear();
                            array1.Add(temp[0]);
                            array2.Add("EXIST");
                            bFlag = false;
                            break;
                        }
                        array1.Add(temp[0]);
                        value = (string)product.GetExtendedProperty(temp[0]);
                        array2.Add(value);
                    }
                }
                else
                {
                    int bspReturnValue = 0;
                    IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                    bspReturnValue = partRep.CheckBySP(temp[2], product.ProId, temp[0]); 
                    if (bspReturnValue == 1)//sp return true;
                    {
                        if (temp[1] == "Exist")
                        {
                            bExist = productRepository.CheckExistProductInfo(product.ProId, temp[0]);
                            if (bExist == false)
                            {
                                array1.Clear();
                                array2.Clear();
                                array1.Add(temp[0]);
                                array2.Add("EXIST");
                                bFlag = false;
                                break;
                            }

                            value = (string)product.GetExtendedProperty(temp[0]);
                            if (String.IsNullOrEmpty(value))
                            {
                                array1.Clear();
                                array2.Clear();
                                array1.Add(temp[0]);
                                array2.Add("EXIST");
                                bFlag = false;
                                break;
                            }
                            array1.Add(temp[0]);
                            array2.Add(value);
                        }
                        else if (temp[1] == "Unique")
                        {
                            bExist = productRepository.CheckExistProductInfo(product.ProId, temp[0]);
                            if (bExist == true)
                            {
                                value = (string)product.GetExtendedProperty(temp[0]);
                                if (String.IsNullOrEmpty(value))
                                {
                                    array1.Clear();
                                    array2.Clear();
                                    array1.Add(temp[0]);
                                    array2.Add("EXIST");
                                    bFlag = false;
                                    break;
                                }
                                bUnique = productRepository.CheckExistProductInfoWhoseInfoValueBeenOccupiedByAnotherProduct(product.ProId, temp[0]);
                                if (bUnique == true)
                                {
                                    array1.Clear();
                                    array2.Clear();
                                    array1.Add(temp[0]);
                                    array2.Add("UNIQUE");
                                    bFlag = false;
                                    break;
                                }
                            }
                            else
                            {
                                array1.Clear();
                                array2.Clear();
                                array1.Add(temp[0]);
                                array2.Add("EXIST");
                                bFlag = false;
                                break;
                            }
                            array1.Add(temp[0]);
                            value = (string)product.GetExtendedProperty(temp[0]);
                            array2.Add(value);
                        }
                    }
                    else//sp return false!
                    {
                    }
                }
            }

            if (bFlag)
                CurrentSession.AddValue(Session.SessionKeys.ValueToCheck, "PASS");
            else
                CurrentSession.AddValue(Session.SessionKeys.ValueToCheck, "FAIL");

            CurrentSession.AddValue("ITCNDCheckItems", array1);
            CurrentSession.AddValue("ITCNDCheckValues", array2);
            

            return base.DoExecute(executionContext);
        }
    }
}