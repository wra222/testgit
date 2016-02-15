// INVENTEC corporation (c)2011 all rights reserved. 
// Description:  根据输入的productID,RMN 检查RMN，并放到Session中
// UI:CI-MES12-SPEC-PAK-UI PD PA Label 1.docx
// UC:CI-MES12-SPEC-PAK-UC PD PA Label 1.docx                           
// Update: 
// Date         Name                         Reason 
// ==========   =======================      ============
// 2011-12-02   Du Xuan (itc98066)          create
// ITC-1360-0828 针对无法找到part的情况，不报错直接返回
// ITC-1360-0834 同828
// ITC-1360-1432 修改GetProductPart过滤条件增加productID条件
// Known issues:
using System;
using System.Data;
using System.Workflow.ComponentModel;
using IMES.FisObject.Common.FisBOM;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.Part;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.PAK.Pallet;
using IMES.DataModel;
using System.Collections.Generic;

namespace IMES.Activity
{
    /// <summary>
    /// 根据输入的productID,RMN 检查RMN，并放到Session中
    /// </summary>
    /// <remarks>
    /// <para>
    /// 基类：
    ///         <see cref="IMES.Activity.BaseActivity">BaseActivity</see>  
    /// </para>
    /// <para>
    /// 应用场景：
    ///      应用于所有以Pallet为主线的站
    /// </para>
    /// <para>
    /// 实现逻辑：
    ///         1.根据输入的productID,RMN 检查RMN
    ///</para> 
    /// <para> 
    /// 异常类型：
    ///         1.系统异常：
    ///         2.业务异常：
    /// 
    /// </para> 
    /// <para>    
    /// 输入：
    ///         Session.Product
    /// </para> 
    /// <para>    
    /// 中间变量：
    ///         无
    /// </para> 
    ///<para> 
    /// 输出：
    ///         
    /// </para> 
    ///<para> 
    /// 数据更新:
    ///          
    /// </para> 
    ///<para> 
    /// 相关FisObject:
    ///              IProductRepository
    ///              productId
    /// </para> 
    /// </remarks>
    public partial class GetWipbuffer : BaseActivity
	{
        /// <summary>
        /// constructor
        /// </summary>
        public GetWipbuffer()
		{
			InitializeComponent();
		}

        /// <summary>
        /// Check RMN
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            Product curProduct = (Product)CurrentSession.GetValue(Session.SessionKeys.Product);
            IList<IBOMNode> bomNodeList = (IList<IBOMNode>)CurrentSession.GetValue(Session.SessionKeys.SessionBom);
            String curCode = (String)CurrentSession.GetValue(Session.SessionKeys.MBCode);
            
            string ID = (string)CurrentSession.GetValue(Session.SessionKeys.RMN);
            string spno = string.Empty;
            string model = string.Empty;
            string family = string.Empty;
            string family2 = string.Empty;
            string family3 = string.Empty;
            string size = string.Empty;
            string pno = string.Empty;

            //string pattern = "SS09";

            IList<WipBufferDef> wipBufferList = new List<WipBufferDef>();
            IProductRepository productRep =RepositoryFactory.GetInstance().GetRepository<IProductRepository, IProduct>();
            IPalletRepository palletRep = RepositoryFactory.GetInstance().GetRepository<IPalletRepository, Pallet>();
            IModelRepository modelRep = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository, IPart>();
            IBOMRepository bomRep = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
                        
            pno = curProduct.Model.Trim();
            family = modelRep.Find(pno).FamilyName;
            
             //4.Get #wipbuffer
             //CREATE TABLE #wipbuffer (partno nvarchar(20),Tp nvarchar(50),Qty char(2))
            //a)	当@Code 为'PD PA label-1' / 'PA A Label-1' / 'PA B Label-1' / 'PA C Label-1' /
            //'PA D Label-1' / 'PA E Label-1' / 'PA F Label-1' / 'PA G Label-1' / 'PA H Label-1' / 
            //'PA J Label-1' / 'PA K Label-1' / 'PA L Label-1' / 'PA M Label-1' / 'SS11PA-1' / 'SS11PA-2'值时，
            string[] codeArray = { "PD PA label-1", "PA A Label-1", "PA B Label-1", "PA C Label-1", 
                                     "PA D Label-1", "PA E Label-1", "PA F Label-1", "PA G Label-1", "PA H Label-1", 
                                     "PA J Label-1", "PA K Label-1", "PA L Label-1", "PA M Label-1", "SS11PA-1", "SS11PA-2"};

            Boolean codeflag = false;
            for (int i = 0; i < codeArray.Length; i++)
            {
                if (curCode.Contains(codeArray[i]))
                {
                    codeflag = true;
                    break;
                }
            }

            Boolean bomTypeFlag = false;
            List<string> PnList = new List<string>();
            foreach (BOMNode bomNode in bomNodeList)
            {
                PnList.Add(bomNode.Part.PN);
                if (bomNode.Part.BOMNodeType == "PL")
                {
                    bomTypeFlag = true;
                }
            }

            IList<WipBuffer> defList = (IList<WipBuffer>)productRep.GetWipBufferByPnoListAndCode(PnList.ToArray(), curCode);

            if (codeflag)
            {
                //按照如下方法获取#wipbuffer 
                //i.取@family 第一个空格前的字串赋值给变量@family3
                string[] splitpattern = family.Split(' ');
                if (splitpattern.Length < 1)
                {
                    FisException fe = new FisException("PAK069", new string[] { });   //此机器Family不符合条件！
                    throw fe;
                }
                family3 = splitpattern[0].Trim();

                //ii.如果使用Model LIKE @family3 + '%' 查询IMES_FA..PAK_CHN_TW_Light 表存在记录，
                //并且ModelBOM 中Model 直接下阶中存在在IMES_FA..PAK_CHN_TW_Light 存在的Part 
                //（IMES_FA..PAK_CHN_TW_Light.PartNo）时，则按照如下方法继续：

                IList<PakChnTwLightInfo> infoList = palletRep.GetPakChnTwLightInfoListByLikeModel(family3);
                bool bomflag = bomRep.CheckExistMaterialByPno(pno);
                /*if (infoList.Count == 0)
                {
                    FisException fe = new FisException("PAK069", new string[] { });   //此机器Family不符合条件！
                    throw fe;
                    //......需要歸納一下SQL
                }*/
                if (infoList.Count > 0 && bomflag)
                {

                    //1.首先在IMES_FA..Product_Part 表中取PartSn 以字符'C' 开头BomNodeType 为'KP' 的记录Part
                    IList<ProductPart> partList = partRep.GetProductPart("C", "KP", curProduct.ProId);
                    if (partList.Count == 0)
                    {
                        CurrentSession.AddValue("WipBuffer", wipBufferList);
                        return base.DoExecute(executionContext);
                    }
                    
                    //2.查询IMES_GetData..Part表，得到该Part 的IMES_GetData..Part.Descr 字段值，并将Descr第5，6位字符串赋值给变量@size
                    IPart parttmp = partRep.Find(partList[0].PartID);
                    size = parttmp.Descr.Substring(4, 2);

                    //3.当@family 中含有'SS09' 字符串的时候，参考如下方法Insert #wipbuffer
                    //参考方法：
                    string[] condition = { "DIESEL", "TIMEX", "VILLEMONTD", "VILLEMONTU", "DD", "TT", "PP", "ZZ","VV",
                                         "VULCAIND",  "VOLNAU", "VULCAINU","VOLNAD", "VULCAIND", "NEUTRON",
                                        "PPD", "PPU","ZI09","ZZ09"};
                    if (family.Contains("SS09"))
                    {
                        //SET @family2=LEFT(@family,CHARINDEX(' ',@family)-1)+' '
                        //SET @family = SUBSTRING(@family,CHARINDEX(' ',@family)+1,LEN(@family))
                        //SET @family2=@family2+LEFT(@family,CHARINDEX(' ',@family)-1)+' '+@size
                        //INSERT #wipbuffer
                        //SELECT a.PartNo, a.Descr, 1
                        //FROM PAK_CHN_TW_Light a (nolock), ModelBOM b (nolock)
                        //WHERE a.Model = @family2 AND b.Material = @Pno AND b.Component = a.PartNo
                        family2 = splitpattern[0] + " " + splitpattern[1] + " " + size;
                    }
                    else
                    {
                        //4. 当@family 中不含有'SS09' 字符串的时候，但满足如下条件时，参考如下方法Insert #wipbuffer
                        //.....
                        Boolean flag = false;
                        for (int i = 0; i < condition.Length; i++)
                        {
                            if (family.Contains(condition[i]))
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (flag)
                        {
                            //参考方法：
                            //SELECT @family2=LEFT(@family,CHARINDEX(' ',@family)-1)+' '+@size
                            //INSERT #wipbuffer
                            //SELECT a.PartNo, a.Descr, 1
                            //FROM PAK_CHN_TW_Light a (nolock), ModelBOM b (nolock)
                            //WHERE a.Model = @family2 AND b.Material = @Pno AND b.Component = a.PartNo
                            family2 = splitpattern[0] + " " + size;
                        }
                        else
                        {
                            //5.	当不满足上面的条件时，参考如下方法Insert #wipbuffer
                            //INSERT #wipbuffer
                            //SELECT a.PartNo, a.Descr, 1
                            //FROM PAK_CHN_TW_Light a (nolock), ModelBOM b (nolock)
                            //WHERE a.Model = LEFT(@family,CHARINDEX(' ',@family)-1) AND b.Material = @Pno AND b.Component = a.PartNo
                            family2 = splitpattern[0];
                        }
                    }
                
                    IList<PakChnTwLightInfo> pakCTLInfoLst = new List<PakChnTwLightInfo>();
                    pakCTLInfoLst = palletRep.GetPakChnTwLightInfoListByModelAndPno(family2, pno);

                
                    foreach (PakChnTwLightInfo infoNode in pakCTLInfoLst)
                    {
                        WipBufferDef wipItem = new WipBufferDef();
                        wipItem.PartNo = infoNode.partNo;
                        wipItem.LightNo = infoNode.lightNo;
                        wipItem.Tp = infoNode.descr;
                        wipItem.Qty = "1";
                        wipBufferList.Add(wipItem);

                    }
                }

                if (defList.Count > 0)
                {
                    //iii.	使用PartNo in (SELECT SPno FROM #Bom) AND Code=RTRIM(@Code)
                    //查询IMES_FA..WipBuffer 表存在记录的时候，
                    //执行如下操作后，退出，#wipbuffer 中的数据即Label List：
                    //参考方法：
                    //INSERT #wipbuffer
                    //SELECT PartNo,Tp,Qty FROM WipBuffer (nolock) 
                    //WHERE PartNo in (SELECT SPno FROM #Bom) AND Code=RTRIM(@Code)
                    //SELECT * FROM #wipbuffer
                    //#wipbuffer 此时没有记录也是正常的，表示此站没有可以粘贴的Label，不需要报错。
                    foreach (WipBuffer defNode in defList)
                    {
                        WipBufferDef wipItem = new WipBufferDef();
                        wipItem.PartNo = defNode.PartNo;
                        wipItem.Tp = defNode.Tp;
                        wipItem.Qty = Convert.ToString(defNode.Qty);
                        wipItem.LightNo = defNode.LightNo;
                        wipBufferList.Add(wipItem);
                    }
                }
                else
                {
                    //iv.	使用PartNo in (SELECT SPno FROM #Bom) AND Code=RTRIM(@Code) 
                    //查询IMES_FA..WipBuffer 表不存在记录的时候，
                    //如果ModelBOM 直接下阶中没有BomNodeType 为'PL' 的Part 存在时，则报告错误：“Maintain 错误！”后，退出；
                    //如果ModelBOM 直接下阶中有BomNodeType 为'PL' 的Part 存在时，则，执行如下操作后，退出，
                    //#wipbuffer 中的数据即Label List：
                    //参考方法：
                    //SELECT * FROM #wipbuffer
                    if (!bomTypeFlag)
                    {
                        FisException fe = new FisException("PAK070", new string[] { });   //Maintain 错误
                        throw fe;
                    }
                }
            }
            else
            {
                //b)当@Code 不为上述值时，按照如下方法获取#wipbuffer
                if (defList.Count > 0)
                {
                    //i.使用PartNo in (SELECT SPno FROM #Bom) AND Code=RTRIM(@Code) 
                    //查询IMES_FA..WipBuffer 表存在记录的时候，执行如下操作后，退出，#wipbuffer 中的数据即Label List：

                    //参考方法：
                    //IF EXISTS (SELECT SPno FROM #Bom WHERE SPno='6060B0483201')
                    //BEGIN
                    //INSERT #wipbuffer SELECT '6060B0483201','HITACH Label',1
                    //END 
                    //INSERT #wipbuffer
                    //SELECT PartNo,Tp,Qty FROM WipBuffer (nolock) 
                    //WHERE PartNo in (SELECT SPno FROM #Bom) AND Code=RTRIM(@Code)
                    //SELECT * FROM #wipbuffer
                    //#wipbuffer 此时没有记录也是正常的，表示此站没有可以粘贴的Label，不需要报错。
                    foreach (BOMNode bomNode in bomNodeList)
                    {
                        if (bomNode.Part.PN == "6060B0483201")
                        {
                            WipBufferDef wipItem = new WipBufferDef();
                            wipItem.PartNo = "6060B0483201";
                            wipItem.Tp = "HITACH Label";
                            wipItem.Qty = "1";
                            wipBufferList.Add(wipItem);
                        }
                    }

                    foreach (WipBuffer defNode in defList)
                    {
                        WipBufferDef wipItem = new WipBufferDef();
                        wipItem.PartNo = defNode.PartNo;
                        wipItem.Tp = defNode.Tp;
                        wipItem.Qty = Convert.ToString(defNode.Qty);
                        wipItem.LightNo = defNode.LightNo;
                        wipBufferList.Add(wipItem);
                    }
                    
                }
                else
                {
                    //ii.使用PartNo in (SELECT SPno FROM #Bom) AND Code=RTRIM(@Code) 
                    //查询IMES_FA..WipBuffer 表不存在记录的时候，如果ModelBOM 直接下阶中没有BomNodeType 为'PL' 的Part 存在时，
                    //则报告错误：“Maintain 错误！”后，退出；
                    if (!bomTypeFlag)
                    {
                        FisException fe = new FisException("PAK070", new string[] { });   //Maintain 错误
                        throw fe;
                    }
                }

              }
            
            CurrentSession.AddValue("WipBuffer",wipBufferList);

	        return base.DoExecute(executionContext);
        }
	}
}
