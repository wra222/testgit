
/*
 * INVENTEC corporation (c)2009 all rights reserved. 
 * Description: Light Guide Impl
 * Update: 
 * Date         Name              Reason 
 * ==========   ================= =====================================
 * 2010-04-15   LuycLiu     Create 
 * 该实现文件不需要编写工作流，直接掉Repositroy就可以
 * 
 * Known issues:Any restrictions about this file 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.PartSn;
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Station.Interface.StationIntf;
using log4net;


namespace IMES.Station.Implementation
{

    public class LightGuide : MarshalByRefObject, ILightGuide
    {

        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region ILightGuide Members



        /// <summary>
        /// 根据KittingCode获取料位信息
        /// </summary>
        /// <param name="code">Kitting code</param>
        /// <returns>料位信息列表</returns>
        public IList<LightBomInfo> getBomByCode(string code)
        {
            logger.Debug("(LightGuide)getBomByCode Start, "
                      + " [code]:" + code);
            try
            {

                IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                IList<LightBomInfo> retLst = productRepository.GetWipBufferInfoListByKittingCode(code);
                return retLst;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(LightGuide)getBomByCode End, "
                   + " [code]:" + code);
            }
        }


        /// <summary>
        /// 获取到料位信息是根据model和code一块查询出来的
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="code">code</param>
        /// <returns>料位信息列表</returns>
        public IList<LightBomInfo> getBomByModel(string model, out string code)
        {
            logger.Debug("(LightGuide)getBomByModel Start, "
                               + " [model]:" + model);
                               
            try
            {
                IList<LightBomInfo> retLst = new List<LightBomInfo>();
                
                //判断系统中是否存在该Model
                IModelRepository modelRepository = RepositoryFactory.GetInstance().GetRepository<IModelRepository, Model>();
                Model modelObject = modelRepository.Find(model);
                if (modelObject == null)
                {
                    throw new FisException("CHK038", new string[] { model });
                }
                else
                {
                    //判断该Model是否维护Kitting Code
                    string ret = modelObject.GetAttribute("DM2");

                    logger.Debug("(LightGuide)Maintain Kitting Code, "
                                       + " [ret]:" + ret);
                    if (ret == null || ret.Trim() == string.Empty)
                    {
                        throw new FisException("CHK113", new string[] { model });
                    }
                    else
                    {
                        //根据model获取Kitting Code
                        code = modelRepository.GetKittingCodeByModel(model);
                        if (code == null || code.Trim() == string.Empty)
                        {
                            throw new FisException("CHK113", new string[] { model });
                        }
                        else
                        {
                            //判断查询得到的Kitting Code 是否在Kitting 表中存在
                            IProductRepository productRepository = RepositoryFactory.GetInstance().GetRepository<IProductRepository, IMES.FisObject.FA.Product.IProduct>();
                            int count = productRepository.GetCountOfKittingCodeByCode(code);
                            if (count == 0)
                            {
                                throw new FisException("CHK114", new string[] { model });
                            }
                            else
                            {
                                //获取bom
                                retLst = productRepository.GetWipBufferInfoListByKittingCodeAndModel(code, model);
                            }
                        }
                    }
                }
              
                return retLst;


            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(LightGuide)getBomByModel End, "
                               + " [model]:" + model);
                              
            }
        }

        /// <summary>
        /// 对输入的ctNo进行partMatch,返回partNo
        /// </summary>
        /// <param name="ctNo">ct no</param>
        /// <returns>part no</returns>
        public string checkCTNo(string ctNo)
        {
            logger.Debug("(LightGuide)checkCTNo Start, "
                      + " [ctNo]:" + ctNo);
            try
            {
                IPartSnRepository partSnRepository = RepositoryFactory.GetInstance().GetRepository<IPartSnRepository, PartSn>();
                PartSn partSn = partSnRepository.Find(ctNo);

                if (partSn == null)
                {
                    string convertCt = ctNo.Substring(0,13);
                    partSn = partSnRepository.Find(convertCt);
                    if (partSn == null)
                    {
                        throw new FisException("CHK027", new string[] { ctNo });
                    }
                }
                string partNo = partSn.IecPn;             
                return partNo;
            }
            catch (FisException e)
            {
                logger.Error(e.mErrmsg);
                throw e;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw new SystemException(e.Message);
            }
            finally
            {
                logger.Debug("(LightGuide)checkCTNo End, "
                     + " [ctNo]:" + ctNo);
            }
        }


        #endregion
    }
}