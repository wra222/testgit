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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;
using IMES.FisObject.FA.Product;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using System.ComponentModel;

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
        ///  NoCheckItemThrowError
        /// </summary>
        public static DependencyProperty NoCheckItemThrowErrorProperty = DependencyProperty.Register("NoCheckItemThrowError", typeof(bool), typeof(ITCNDCheckItem), new PropertyMetadata(true));

        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("NoCheckItemThrowError")]
        [CategoryAttribute("InArguments Of ITCNDCheckItem")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool  NoCheckItemThrowError
        {
            get
            {
                return ((bool)(base.GetValue(NoCheckItemThrowErrorProperty)));
            }
            set
            {
                base.SetValue(NoCheckItemThrowErrorProperty, value);
            }
        }

        /// <summary>
        ///  NoCheckItemThrowError
        /// </summary>
        public static DependencyProperty NotMatchThrowErrorProperty = DependencyProperty.Register("NotMatchThrowError", typeof(bool), typeof(ITCNDCheckItem), new PropertyMetadata(false));

        /// <summary>
        /// 
        /// </summary>
        [DescriptionAttribute("NotMatchCheckItemThrowError")]
        [CategoryAttribute("InArguments Of ITCNDCheckItem")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NotMatchThrowError
        {
            get
            {
                return ((bool)(base.GetValue(NotMatchThrowErrorProperty)));
            }
            set
            {
                base.SetValue(NotMatchThrowErrorProperty, value);
            }
        }

        /// <summary> 
        /// </summary>        
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Session session = CurrentSession;         
            var product = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
            if (product == null)
            {
                throw new NullReferenceException("Product in session is null");
            }

            if (string.IsNullOrEmpty(product.Model) || string.IsNullOrEmpty(product.Family) || string.IsNullOrEmpty(this.Line))
            {
                throw new Exception("Either Mode or Family or Line value has empty or null");
            }
            IList<ITCNDCheckSettingDef> checkItemList= getITCNDCheckSetting(this.Station, product.Model, product.Family, this.Line.Substring(0, 1));

            if (checkItemList==null  || checkItemList.Count == 0)
            {
                if (this.NoCheckItemThrowError)
                {
                    throw new FisException("CHK831", new List<string>());
                }
                else
                {
                    session.AddValue(Session.SessionKeys.ValueToCheck, "PASS");
                    CurrentSession.AddValue("ITCNDCheckItems", new List<string>());
                    CurrentSession.AddValue("ITCNDCheckValues", new List<string>());                  
                     return base.DoExecute(executionContext);
                }
            }

            session.AddValue(Session.SessionKeys.ValueToCheck, "PASS");
            IList<IMES.FisObject.FA.Product.ProductInfo> productInfoList = product.ProductInfoes;
            var notMatchItem = checkItemList.FirstOrDefault(x=> !checkItemResult(product.ProId,x,productInfoList));  

            if (notMatchItem != null)
            {
                session.AddValue(Session.SessionKeys.ValueToCheck, "FAIL");
                if (this.NotMatchThrowError)
                {
                    if (notMatchItem.checkType == "Exist")
                    {
                       throw new FisException("CHK832", new List<string>{notMatchItem.checkItem} );                        
                    }
                    else
                    {
                        throw new FisException("CHK833", new List<string> { notMatchItem.checkItem });                       
                    }
                }
                else
                {
                    CurrentSession.AddValue("ITCNDCheckItems", new List<string> { notMatchItem.checkItem.ToUpper() });
                    CurrentSession.AddValue("ITCNDCheckValues", new List<string> { notMatchItem.checkType.ToUpper() });                  
                }
            }
            else
            {
                var ckeckOKItemList = checkItemList.Where(y => y.checkCondition == "1");
                CurrentSession.AddValue("ITCNDCheckItems", ckeckOKItemList.Select(x => x.checkItem).ToList());
                CurrentSession.AddValue("ITCNDCheckValues", ckeckOKItemList.Select(x => x.checkType).ToList());
            }

            #region Vincent 2014-07-29 disable code 
            //var productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            //IList<string[]> needItems = new List<string[]>();
            //needItems = productRepository.GetCheckItemListFromITCNDCheckSetting(product.Model);
            //if(needItems == null || needItems.Count == 0)
            //{
            //    Model modelObj = product.ModelObj;
            //    if(modelObj != null)
            //    {
            //        needItems = productRepository.GetCheckItemListFromITCNDCheckSetting(modelObj.FamilyName);
            //        if(needItems == null || needItems.Count == 0)
            //        {
            //            needItems = productRepository.GetCheckItemListFromITCNDCheckSetting(this.Line.Substring(0, 1));
            //            if(needItems == null || needItems.Count == 0)
            //            {
            //                List<string> errpara = new List<string>();
            //                FisException e = new FisException("CHK831", errpara);
            //                throw e;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        throw new NullReferenceException("Model in session is null");
            //    }
            //}

            //bool bExist = false;
            //bool bUnique = false;
            //bool bFlag = true;
            //string value = string.Empty;
            //IList<string> array1 = new List<string>();
            //IList<string> array2 = new List<string>();
            //foreach(string[] temp in needItems)
            //{
            //    //CheckCondition:
            //    if (String.IsNullOrEmpty(temp[2]))
            //    {
            //        if (temp[1] == "Exist")
            //        {
            //            bExist = productRepository.CheckExistProductInfo(product.ProId, temp[0]);
            //            if (bExist == false)
            //            {
            //                array1.Clear();
            //                array2.Clear();
            //                array1.Add(temp[0]);
            //                array2.Add("EXIST");
            //                bFlag = false;
            //                break;
            //            }

            //            value = (string)product.GetExtendedProperty(temp[0]);
            //            if (String.IsNullOrEmpty(value))
            //            {
            //                array1.Clear();
            //                array2.Clear();
            //                array1.Add(temp[0]);
            //                array2.Add("EXIST");
            //                bFlag = false;
            //                break;
            //            }
            //            array1.Add(temp[0]);
            //            array2.Add(value);
            //        }
            //        else if (temp[1] == "Unique")
            //        {
            //            bExist = productRepository.CheckExistProductInfo(product.ProId, temp[0]);
            //            if (bExist == true)
            //            {
            //                value = (string)product.GetExtendedProperty(temp[0]);
            //                if (String.IsNullOrEmpty(value))
            //                {
            //                    array1.Clear();
            //                    array2.Clear();
            //                    array1.Add(temp[0]);
            //                    array2.Add("EXIST");
            //                    bFlag = false;
            //                    break;
            //                }
            //                bUnique = productRepository.CheckExistProductInfoWhoseInfoValueBeenOccupiedByAnotherProduct(product.ProId, temp[0]);
            //                if (bUnique == true)
            //                {
            //                    array1.Clear();
            //                    array2.Clear();
            //                    array1.Add(temp[0]);
            //                    array2.Add("UNIQUE");
            //                    bFlag = false;
            //                    break;
            //                }
            //            }
            //            else
            //            {
            //                array1.Clear();
            //                array2.Clear();
            //                array1.Add(temp[0]);
            //                array2.Add("EXIST");
            //                bFlag = false;
            //                break;
            //            }
            //            array1.Add(temp[0]);
            //            value = (string)product.GetExtendedProperty(temp[0]);
            //            array2.Add(value);
            //        }
            //    }
            //    else
            //    {
            //        int bspReturnValue = 0;
            //        IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            //        bspReturnValue = partRep.CheckBySP(temp[2], product.ProId, temp[0]); 
            //        if (bspReturnValue == 1)//sp return true;
            //        {
            //            if (temp[1] == "Exist")
            //            {
            //                bExist = productRepository.CheckExistProductInfo(product.ProId, temp[0]);
            //                if (bExist == false)
            //                {
            //                    array1.Clear();
            //                    array2.Clear();
            //                    array1.Add(temp[0]);
            //                    array2.Add("EXIST");
            //                    bFlag = false;
            //                    break;
            //                }

            //                value = (string)product.GetExtendedProperty(temp[0]);
            //                if (String.IsNullOrEmpty(value))
            //                {
            //                    array1.Clear();
            //                    array2.Clear();
            //                    array1.Add(temp[0]);
            //                    array2.Add("EXIST");
            //                    bFlag = false;
            //                    break;
            //                }
            //                array1.Add(temp[0]);
            //                array2.Add(value);
            //            }
            //            else if (temp[1] == "Unique")
            //            {
            //                bExist = productRepository.CheckExistProductInfo(product.ProId, temp[0]);
            //                if (bExist == true)
            //                {
            //                    value = (string)product.GetExtendedProperty(temp[0]);
            //                    if (String.IsNullOrEmpty(value))
            //                    {
            //                        array1.Clear();
            //                        array2.Clear();
            //                        array1.Add(temp[0]);
            //                        array2.Add("EXIST");
            //                        bFlag = false;
            //                        break;
            //                    }
            //                    bUnique = productRepository.CheckExistProductInfoWhoseInfoValueBeenOccupiedByAnotherProduct(product.ProId, temp[0]);
            //                    if (bUnique == true)
            //                    {
            //                        array1.Clear();
            //                        array2.Clear();
            //                        array1.Add(temp[0]);
            //                        array2.Add("UNIQUE");
            //                        bFlag = false;
            //                        break;
            //                    }
            //                }
            //                else
            //                {
            //                    array1.Clear();
            //                    array2.Clear();
            //                    array1.Add(temp[0]);
            //                    array2.Add("EXIST");
            //                    bFlag = false;
            //                    break;
            //                }
            //                array1.Add(temp[0]);
            //                value = (string)product.GetExtendedProperty(temp[0]);
            //                array2.Add(value);
            //            }
            //        }
            //        else//sp return false!
            //        {
            //        }
            //    }
            //}

            //if (bFlag)
            //    CurrentSession.AddValue(Session.SessionKeys.ValueToCheck, "PASS");
            //else
            //    CurrentSession.AddValue(Session.SessionKeys.ValueToCheck, "FAIL");

            //CurrentSession.AddValue("ITCNDCheckItems", array1);
            //CurrentSession.AddValue("ITCNDCheckValues", array2);

            #endregion
            return base.DoExecute(executionContext);
        }

        private IList<ITCNDCheckSettingDef> getITCNDCheckSetting(string station, string model ,string family,string line)
        {
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IList<ITCNDCheckSettingDef> stationCheckItemList = prodRep.GetITCNDCheckSettingByStationAndLine(station,new List<string> {model,family,line});
            if (stationCheckItemList.Count == 0)
            {
                return null;
            }
            var checkItemList = stationCheckItemList.Where(x => x.line == model);
            if (checkItemList.Count() == 0)
            {
                checkItemList = stationCheckItemList.Where(x => x.line == family);
                if (checkItemList.Count() == 0)
                {
                    checkItemList = stationCheckItemList.Where(x => x.line == line);
                }
            }
            return checkItemList.ToList();
        }

        private bool checkItemResult(string productId, ITCNDCheckSettingDef checkItem, IList<IMES.FisObject.FA.Product.ProductInfo> infoList)
        {
            bool ret = true;
            bool needCheck = true;
            if (!string.IsNullOrEmpty(checkItem.checkCondition))
            {
                IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
                if (partRep.CheckBySP(checkItem.checkCondition, productId, checkItem.checkItem) != 1)
                {
                    needCheck = false;
                } 
            }
            if (needCheck)
            {
                if (checkItem.checkType == "Exist")
                {
                    var infoValue= checkProductInfoExists(infoList, checkItem.checkItem);
                    if (infoValue != null)
                    {
                        checkItem.checkType = infoValue.InfoValue;
                        checkItem.checkCondition = "1";
                        return true;
                    }
                    return false;
                }
                else if (checkItem.checkType == "Unique")
                {
                    var infoValue = checkProductInfoExists(infoList, checkItem.checkItem);
                    if (infoValue!=null)
                    {
                        if (checkProductInfoUnique(productId, infoList, checkItem.checkItem))
                        {
                            checkItem.checkType = infoValue.InfoValue;
                            checkItem.checkCondition = "1";
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        checkItem.checkType = "Exist";
                        return false;
                    }
                }
            }
            return ret;
        }

        private IMES.FisObject.FA.Product.ProductInfo checkProductInfoExists(IList<IMES.FisObject.FA.Product.ProductInfo> infoList, string name)
        {
            return infoList.FirstOrDefault(x => x.InfoType == name && !string.IsNullOrEmpty(x.InfoValue));

        }
        private bool checkProductInfoUnique(string productId,IList<IMES.FisObject.FA.Product.ProductInfo> infoList, string name)
        {
            IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            if (!prodRep.CheckExistProductInfoWhoseInfoValueBeenOccupiedByAnotherProduct(productId, name))
            {
                return true;
            }

            return false;
        }

    }
}