// INVENTEC corporation (c)2009 all rights reserved. 
// Description: 根据传入的PrintItem填充相应的值
//                         
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2009-11-05   Yuan XiaoWei                 create
// 2010-03-05   itc206010                    Modify ITC-1122-0179
// 2010-03-05   itc206010                    Modify ITC-1122-0181
// 2010-03-05   itc206010                    Modify ITC-1103-0253
// 2010-03-15   itc206010                    Modify ITC-1122-0214
// Known issues:
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using IMES.Infrastructure.Utility.RuleSets;
using IMES.DataModel;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.PrintItem;
using IMES.FisObject.Common.MO;
using IMES.FisObject.PCA.MB;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PAK.DN;
using IMES.FisObject.Common.PrintLog;
using IMES.FisObject.Common.ReprintLog;
using IMES.Common;
using IMES.Resolve.Common;

namespace IMES.Activity
{

    /// <summary>
    /// 业务功能：
    ///      确定指定打印类型当前需要使用的Template, 收集Template需要的参数等信息
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///    BaseActivity
    /// </para>
    /// <para>
    /// 应用场景：
    ///      所有有打印項的Station的Workflow
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///       参考UC
    /// </para>
    /// <para>
    /// 异常类型：
    ///      1.系统异常：
    ///             无
    ///      2.业务异常：
    /// </para>
    /// <para>
    /// 输入：
    ///      MOFrom (模板打印时，如果RuleMode是MO,MO从Session的那个对象获取)
    ///      DnFrom (模板打印时，如果RuleMode是PO,PO从Session的那个对象获取)
    ///      Session.PrintItems
    /// </para>
    /// <para>
    /// 输出：
    ///      Session.PrintItems
    /// </para>
    /// <para>
    /// 是否进行输入等待：
    ///      否
    /// </para>
    /// </remarks>
    public partial class Print : BaseActivity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Print()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 执行逻辑
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            IList<PrintItem> resultPrintItemList = new List<PrintItem>();
            Session session =CurrentSession;
            var printItemList = (IList<PrintItem>)session.GetValue(Session.SessionKeys.PrintItems);

            ILabelTypeRepository lblTypeRepository = RepositoryFactory.GetInstance().GetRepository<ILabelTypeRepository, LabelType>();
            PrintUtility resolvePrintTemplate = null;
            if (printItemList != null)
            {
                for (int i = 0; i < printItemList.Count; i++)
                {
                    PrintItem tempPrintItem = printItemList[i];
                    if (tempPrintItem.PrintMode == Bat)
                    {
                        PrintTemplate newTemplate = lblTypeRepository.GetPrintTemplate(tempPrintItem.TemplateName);
                        if (newTemplate != null)
                        {
                            //if (IsReprint)
                            //{
                            //    tempPrintItem.Piece = 1;
                            //}
                            //else
                            //{
                            tempPrintItem.Piece = newTemplate.Piece;
                            //}
                            tempPrintItem.SpName = newTemplate.SpName;
                            resultPrintItemList.Add(tempPrintItem);
                        }
                        else if (NotExistException)
                        {
                            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(tempPrintItem.TemplateName);
                            ex = new FisException("CHK064", erpara);
                            logger.Error(ex.mErrmsg, ex);
                            throw ex;
                        }
                       

                    }
                    else if (tempPrintItem.PrintMode == Template)
                    {
                        if (resolvePrintTemplate == null)
                        {
                            resolvePrintTemplate = new PrintUtility(session, ModelName(session), MONO(session), Dn(session), PartNo(session), Customer);
                        }
                        PrintTemplate newTemplate = resolvePrintTemplate.GetPrintTemplate(tempPrintItem.LabelType, tempPrintItem.RuleMode, tempPrintItem.PrintMode);
                        //PrintTemplate newTemplate = lblTypeRepository.GetPrintTemplate(tempPrintItem.LabelType, ModelName, MONO, Dn, PartNo, tempPrintItem.RuleMode,Customer);
                        if (newTemplate != null)
                        {
                            //if (IsReprint)
                            //{
                            //    tempPrintItem.Piece = 1;
                            //}
                            //else
                            //{
                            tempPrintItem.Piece = newTemplate.Piece;
                            //}
                            tempPrintItem.TemplateName = newTemplate.TemplateName;
                            tempPrintItem.Layout = newTemplate.Layout;
                            resultPrintItemList.Add(tempPrintItem);
                        }
                        else if (NotExistException)
                        {
                            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(tempPrintItem.LabelType);
                            ex = new FisException("CHK065", erpara);
                            logger.Error(ex.mErrmsg, ex);
                            throw ex;
                        }                       

                    }
                    else if (tempPrintItem.PrintMode == Bartender ||
                              tempPrintItem.PrintMode == BartenderSrv)
                    {
                        //PrintTemplate newTemplate = lblTypeRepository.GetPrintTemplate(tempPrintItem.TemplateName);
                        if (resolvePrintTemplate == null)
                        {
                            resolvePrintTemplate = new PrintUtility(session, ModelName(session), MONO(session), Dn(session), PartNo(session), Customer);
                        }
                        PrintTemplate newTemplate = resolvePrintTemplate.GetPrintTemplate(tempPrintItem.LabelType, tempPrintItem.RuleMode, tempPrintItem.PrintMode);
                        if (newTemplate != null)
                        {
                            //if (IsReprint)
                            //{
                            //    tempPrintItem.Piece = 1;
                            //}
                            //else
                            //{
                            tempPrintItem.Piece = newTemplate.Piece;
                            //}
                            tempPrintItem.TemplateName = newTemplate.TemplateName;
                            tempPrintItem.SpName = newTemplate.SpName;
                            resultPrintItemList.Add(tempPrintItem);
                        }
                        else if (NotExistException)
                        {
                           // var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                            FisException ex;
                            List<string> erpara = new List<string>();
                            erpara.Add(tempPrintItem.TemplateName);
                            ex = new FisException("CHK064", erpara);
                            //logger.Error(ex.mErrmsg, ex);
                            throw ex;
                        }
                    }
                }

                if (resultPrintItemList.Count == 0 & !NotExistException)
                {
                    //var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                    FisException ex;
                    List<string> erpara = new List<string>();
                    ex = new FisException("CHK065",new string[]{""});
                    //logger.Error(ex.mErrmsg, ex);
                    throw ex;
                }

                session.AddValue(Session.SessionKeys.PrintItems, resultPrintItemList);

                #region write print log
                if (this.IsWritePrintLog && resultPrintItemList != null && resultPrintItemList.Count > 0)
                {
                    ActivityCommonImpl utl = ActivityCommonImpl.Instance;
                    var rep = RepositoryFactory.GetInstance().GetRepository<IPrintLogRepository, PrintLog>();
                    var reprintRep = RepositoryFactory.GetInstance().GetRepository<IReprintLogRepository, ReprintLog>();
                    string descrMsg= "Line:{0} PrintMode:{1} SPName:{2} Pieces:{3}";
                    string reasonMsg = "Template:{0}";
                    string begNo = utl.GetPrintLogBegNoValue(session, this.PrintLogBegNo, this.SessionBegNoName);
                    string endNo = utl.GetPrintLogEndNoValue(session,this.SessionEndNoName);
                    begNo = begNo ?? session.Key;
                    endNo = endNo ?? session.Key;
                    foreach (PrintItem item in resultPrintItemList)
                    {
                        if (this.IsReprint)
                        {
                            var log = new ReprintLog
                            {
                                LabelName = item.LabelType,
                                BegNo = begNo,
                                EndNo =endNo,
                                Descr = string.Format(descrMsg,
                                                                    this.Line?? "",
                                                                    item.PrintMode.ToString(),
                                                                    item.SpName,
                                                                    item.Piece.ToString()),
                                Reason = session.GetValue(Session.SessionKeys.Reason) as string ,
                                Editor = this.Editor
                            };
                            if (string.IsNullOrEmpty(log.Reason))
                            {
                                log.Reason = string.Format(reasonMsg, item.TemplateName);
                            }
                            else
                            {
                                log.Reason = log.Reason +" "+ string.Format(reasonMsg, item.TemplateName);
                            }
                            reprintRep.Add(log, session.UnitOfWork);
                        }
                        else
                        {
                            var log = new PrintLog
                            {
                                LabelTemplate = item.TemplateName,
                                Station = this.Station,
                                Name = item.LabelType,
                                BeginNo = begNo,
                                EndNo = endNo,
                                Descr = string.Format(descrMsg,
                                                                    this.Line ?? "",
                                                                    item.PrintMode.ToString(),
                                                                    item.SpName,
                                                                    item.Piece.ToString()),
                                Editor = this.Editor
                            };

                            rep.Add(log, session.UnitOfWork);
                        }
                    }
                }
                #endregion
            }
            //Vincent disable code don't Change printitem object or not
            //else
            //{
            //   // var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //    FisException ex;
            //    List<string> erpara = new List<string>();
            //    ex = new FisException("CHK066", erpara);
            //   // logger.Error(ex.mErrmsg, ex);
            //    throw ex;
            //}
            return base.DoExecute(executionContext);
        }

        /// <summary>
        /// IsReprint
        /// </summary>
        public static DependencyProperty IsReprintProperty = DependencyProperty.Register("IsReprint", typeof(bool), typeof(Print),new PropertyMetadata(false));

        /// <summary>
        /// IsReprint:True Or False
        /// </summary>
        [DescriptionAttribute("IsReprint")]
        [CategoryAttribute("InArguments Of Print")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]      
        public bool IsReprint
        {
            get
            {
                return ((bool)(base.GetValue(Print.IsReprintProperty)));
            }
            set
            {
                base.SetValue(Print.IsReprintProperty, value);
            }
        }

        /// <summary>
        /// MO的来源，共有四种MONO，ProdMO，Product,MB
        /// </summary>
        public static DependencyProperty MOFromProperty = DependencyProperty.Register("MOFrom", typeof(MOFromEnum), typeof(Print));

        /// <summary>
        /// MO的来源，共有四种MONO，ProdMO，Product,MB
        /// </summary>
        [DescriptionAttribute("MOFrom")]
        [CategoryAttribute("InArguments Of Print")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(MOFromEnum.MONO)]
        public MOFromEnum MOFrom
        {
            get
            {
                return ((MOFromEnum)(base.GetValue(Print.MOFromProperty)));
            }
            set
            {
                base.SetValue(Print.MOFromProperty, value);
            }
        }


        /// <summary>
        /// MO的来源，共有四种MONO，ProdMO，Product,MB
        /// </summary>
        public enum MOFromEnum
        {
            /// <summary>
            /// 从Session.MONO中获取
            /// </summary>
            MONO = 1,

            /// <summary>
            /// 从Session.ProdMO中获取
            /// </summary>
            ProdMO = 2,

            /// <summary>
            /// 从Session.Product中获取
            /// </summary>
            Product = 4,

            /// <summary>
            /// 从Session.MB中获取
            /// </summary>
            MB = 8,

            /// <summary>
            /// 流程中没有MO
            /// </summary>
            No = 16
        }

        /// <summary>
        ///  Model的来源，共有两种ModelName，ModelObj
        ///  当是模板打印，PrintMode=1，RuleModel为1或0，却没有MO和DN时，可能需要改参数。
        ///  比如TravelCardPrint 没有ProductId的类型时。
        /// </summary>
        public static DependencyProperty ModelFromProperty = DependencyProperty.Register("ModelFrom", typeof(ModelFromEnum), typeof(Print));

        /// <summary>
        ///  Model的来源，共有两种ModelName，ModelObj
        /// </summary>
        [DescriptionAttribute("ModelFrom")]
        [CategoryAttribute("InArguments Of Print")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(ModelFromEnum.ModelName)]
        public ModelFromEnum ModelFrom
        {
            get
            {
                return ((ModelFromEnum)(base.GetValue(Print.ModelFromProperty)));
            }
            set
            {
                base.SetValue(Print.ModelFromProperty, value);
            }
        }


        /// <summary>
        /// Model的来源，共有两种ModelName，ModelObj
        /// </summary>
        public enum ModelFromEnum
        {
            /// <summary>
            /// 从Session.ModelName中获取
            /// </summary>
            ModelName = 1,

            /// <summary>
            /// 從Product抓Model
            /// </summary>
            Product_Model=2,
            /// <summary>
            /// 從MB抓Model
            /// </summary>
            MB_Model=3,
            /// <summary>
            /// 從Delivery抓Model
            /// </summary>
            Delivery_Model=4,
            /// <summary>
            /// 流程中没有Model
            /// </summary>
            No = 16
        }



        /// <summary>
        /// Dn的来源(Dn就是PO)，共有三种Delivery，DeliveryNo，Product
        /// </summary>
        public static DependencyProperty DnFromProperty = DependencyProperty.Register("DnFrom", typeof(DnFromEnum), typeof(Print));

        /// <summary>
        /// Dn的来源(Dn就是PO)，共有三种Delivery，DeliveryNo，Product
        /// </summary>
        [DescriptionAttribute("DnFrom")]
        [CategoryAttribute("InArguments Of Print")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(DnFromEnum.DeliveryNo)]
        public DnFromEnum DnFrom
        {
            get
            {
                return ((DnFromEnum)(base.GetValue(Print.DnFromProperty)));
            }
            set
            {
                base.SetValue(Print.DnFromProperty, value);
            }
        }

        /// <summary>
        /// Dn的来源(Dn就是PO)，共有三种Delivery，DeliveryNo，Product
        /// </summary>
        public enum DnFromEnum
        {
            /// <summary>
            /// 从Session.DeliveryNo中获取
            /// </summary>
            DeliveryNo = 1,

            /// <summary>
            /// 从Session.Delivery中获取
            /// </summary>
            Delivery = 2,

            /// <summary>
            /// 从Session.Product中获取
            /// </summary>
            Product = 4,

            /// <summary>
            /// 流程中没有Dn
            /// </summary>
            No = 8
        }



        /// <summary>
        /// PartNo的来源，共有一种PartNo
        /// </summary>
        public static DependencyProperty PartNoFromProperty = DependencyProperty.Register("PartNoFrom", typeof(PartNoFromEnum), typeof(Print));

        /// <summary>
        /// PartNo的来源，共有一种PartNo
        /// </summary>
        [DescriptionAttribute("PartNoFrom")]
        [CategoryAttribute("InArguments Of Print")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        [DefaultValue(PartNoFromEnum.PartNo)]
        public PartNoFromEnum PartNoFrom
        {
            get
            {
                return ((PartNoFromEnum)(base.GetValue(Print.PartNoFromProperty)));
            }
            set
            {
                base.SetValue(Print.PartNoFromProperty, value);
            }
        }

        /// <summary>
        /// PartNo的来源，共有一种PartNo
        /// </summary>
        public enum PartNoFromEnum
        {
            /// <summary>
            /// 从Session.PartNo中获取
            /// </summary>
            PartNo = 1,

            /// <summary>
            /// 流程中没有PartNo信息
            /// </summary>
            No = 2,

            /// <summary>
            /// MB.Model 當作PartNo  來抓Part and PartInfo
            /// </summary>
            MB_Model =3 
        }

        /// <summary>
        /// 找不到模板要報錯
        /// </summary>
        public static DependencyProperty NotExistExceptionProperty = DependencyProperty.Register("NotExistException", typeof(bool), typeof(Print),new PropertyMetadata(true));

        /// <summary>
        /// 找不到模板要報錯
        /// </summary>
        [DescriptionAttribute("NotExistException")]
        [CategoryAttribute("InArguments Of Print")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool NotExistException
        {
            get
            {
                return ((bool)(base.GetValue(Print.NotExistExceptionProperty)));
            }
            set
            {
                base.SetValue(Print.NotExistExceptionProperty, value);
            }
        }

        /// <summary>
        /// write Print log
        /// </summary>
        public static DependencyProperty IsWritePrintLogProperty = DependencyProperty.Register("IsWritePrintLog", typeof(bool), typeof(Print),new PropertyMetadata(false));

        /// <summary>
        /// write Print log
        /// </summary>
        [DescriptionAttribute("Is Write PrintLog")]
        [CategoryAttribute("Print log")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsWritePrintLog
        {
            get
            {
                return ((bool)(base.GetValue(IsWritePrintLogProperty)));
            }
            set
            {
                base.SetValue(IsWritePrintLogProperty, value);
            }
        }


        /// <summary>
        /// Print log BegNo
        /// </summary>
        public static DependencyProperty PrintLogBegNoProperty = DependencyProperty.Register("PrintLogBegNo", typeof(PrintLogBegNoEnum), typeof(Print), new PropertyMetadata(PrintLogBegNoEnum.ID));

        /// <summary>
        /// Print log BegNo
        /// </summary>
        [DescriptionAttribute("Print log BegNo")]
        [CategoryAttribute("Print log")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]       
        public PrintLogBegNoEnum PrintLogBegNo
        {
            get
            {
                return ((PrintLogBegNoEnum)(base.GetValue(PrintLogBegNoProperty)));
            }
            set
            {
                base.SetValue(PrintLogBegNoProperty, value);
            }
        }

        /// <summary>
        /// Print log BegNo
        /// </summary>
        public static DependencyProperty SessionBegNoNameProperty = DependencyProperty.Register("SessionBegNoName", typeof(string), typeof(Print), new PropertyMetadata(""));

        /// <summary>
        /// Print log BegNo
        /// </summary>
        [DescriptionAttribute("SessionBegNoName")]
        [CategoryAttribute("Print log")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]      
        public string SessionBegNoName
        {
            get
            {
                return ((string)(base.GetValue(SessionBegNoNameProperty)));
            }
            set
            {
                base.SetValue(SessionBegNoNameProperty, value);
            }
        }

        /// <summary>
        /// Print log EndNo
        /// </summary>
        public static DependencyProperty SessionEndNoNameProperty = DependencyProperty.Register("SessionEndNoName", typeof(string), typeof(Print), new PropertyMetadata(""));

        /// <summary>
        /// Print log EndNo
        /// </summary>
        [DescriptionAttribute("SessionEndNoName")]
        [CategoryAttribute("Print log")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]        
        public string SessionEndNoName
        {
            get
            {
                return ((string)(base.GetValue(SessionEndNoNameProperty)));
            }
            set
            {
                base.SetValue(SessionEndNoNameProperty, value);
            }
        }

        /// <summary>
        /// bat方式
        /// </summary>
        private const int Bat = 0;


        /// <summary>
        /// 模板方式
        /// </summary>
        private const int Template = 1;


        /// <summary>
        /// bartender方式
        /// </summary>
        private const int Bartender = 3;

        /// <summary>
        /// bartenderSrv方式
        /// </summary>
        private const int BartenderSrv = 4;

        /// <summary>
        /// MO
        /// </summary>
        private const int MO = 0;


        /// <summary>
        /// PO
        /// </summary>
        private const int PO = 1;

        /// <summary>
        /// Part
        /// </summary>
        private const int Part = 2;


        /// <summary>
        /// MONO
        /// </summary>
        private string MONO(Session session)
        {           
            switch (MOFrom)
            {
                case MOFromEnum.MONO:
                    return session.GetValue(Session.SessionKeys.MONO) as string;

                case MOFromEnum.ProdMO:
                    MO currentMO = session.GetValue(Session.SessionKeys.ProdMO) as MO;
                    if (currentMO == null)
                    {
                        return "";
                    }
                    else
                    {
                        return currentMO.MONO;
                    }

                case MOFromEnum.MB:
                    MB currentMB = session.GetValue(Session.SessionKeys.MB) as MB;
                    if (currentMB == null)
                    {
                        return "";
                    }
                    else
                    {
                        return currentMB.SMTMO;
                    }

                case MOFromEnum.Product:
                    Product currentProduct = session.GetValue(Session.SessionKeys.Product) as Product;
                    if (currentProduct == null)
                    {
                        return "";
                    }
                    else
                    {
                        return currentProduct.MO;
                    }

                default:
                    return "";
            }          
        }



        /// <summary>
        /// Dn
        /// </summary>
        private string Dn(Session session)
        {           
            switch (DnFrom)
            {
                case DnFromEnum.DeliveryNo:
                    return session.GetValue(Session.SessionKeys.DeliveryNo) as string;

                case DnFromEnum.Delivery:
                    Delivery currentDelivery = session.GetValue(Session.SessionKeys.Delivery) as Delivery;
                    if (currentDelivery == null)
                    {
                        return "";
                    }
                    else {
                        return currentDelivery.DeliveryNo;
                    }
                    //return CurrentSession.GetValue(Session.SessionKeys.Delivery) == null ? 
                    //    "" : (CurrentSession.GetValue(Session.SessionKeys.Delivery) as Delivery).DeliveryNo;
                case DnFromEnum.Product:
                    Product currentProduct = session.GetValue(Session.SessionKeys.Product) as Product;
                    if (currentProduct == null)
                    {
                        return "";
                    }
                    else
                    {
                        return currentProduct.DeliveryNo;
                    }

                default:
                    return "";
            }           
        }

        /// <summary>
        /// PartNo
        /// </summary>
        private string PartNo(Session session)
        {           
            switch (PartNoFrom)
            {
                case PartNoFromEnum.PartNo:
                    return session.GetValue(Session.SessionKeys.PartNo) as string;
                case PartNoFromEnum.MB_Model:
                    MB currentMB = session.GetValue(Session.SessionKeys.MB) as MB;
                    if (currentMB == null)
                    {
                        return "";
                    }
                    else
                    {
                        return currentMB.Model;
                    }
                default:
                    return "";
            }            
        }



        /// <summary>
        /// ModelName
        /// </summary>
        private string ModelName(Session session)
        {
            switch (ModelFrom)
            {
                case ModelFromEnum.ModelName:
                    var modelname = session.GetValue(Session.SessionKeys.ModelName) as string;
                    if (string.IsNullOrEmpty(modelname))
                    {
                        return "";
                    }
                    else
                    {
                        return modelname;
                    }
                case ModelFromEnum.Product_Model:
                    Product currentProduct = session.GetValue(Session.SessionKeys.Product) as Product;
                    if (currentProduct == null)
                    {
                        return "";
                    }
                    else
                    {
                        return currentProduct.Model;
                    }
                case ModelFromEnum.MB_Model:
                    MB currentMB = session.GetValue(Session.SessionKeys.MB) as MB;
                    if (currentMB == null)
                    {
                        return "";
                    }
                    else
                    {
                        return currentMB.Model;
                    }
                case ModelFromEnum.Delivery_Model:
                    Delivery currentDelivery = session.GetValue(Session.SessionKeys.Delivery) as Delivery;
                    if (currentDelivery == null)
                    {
                        return "";
                    }
                    else
                    {
                        return currentDelivery.ModelName;
                    }
                default:
                    return "";
            }            
        }


       
        
    }
}
