/*
 * INVENTEC corporation (c)2010 all rights reserved. 
 * Description:  ICT input interface implement
 *             
 * Update: 
 * Date       Name                  Reason 
 * ========== ===================== =====================================
 * 2010-1-20  liu xiaoling          Create 
 * Known issues:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.DataModel;
using IMES.FisObject.Common.Model;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.Infrastructure.UnitOfWork;
using IMES.Maintain.Interface.MaintainIntf;
using log4net;
using IMES.FisObject.Common.Process;
using IMES.FisObject.Common.Station;
using IMES.FisObject.Common.Misc;
using IMES.FisObject.PCA.MBModel;
using System.Data;
using IMES.FisObject.Common.Part;
namespace IMES.Maintain.Implementation
{

    class ProcessManager :MarshalByRefObject, IProcessManager
    {
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IProcessRepository processRepository = RepositoryFactory.GetInstance().GetRepository<IProcessRepository, Process>();
        public IStationRepository stationRepository = RepositoryFactory.GetInstance().GetRepository<IStationRepository, IStation>();
        public IMiscRepository miscRepository = RepositoryFactory.GetInstance().GetRepository<IMiscRepository>();
        public IMBModelRepository mbModelRepository = RepositoryFactory.GetInstance().GetRepository<IMBModelRepository>();
        public IPartRepository iPartRepository = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();

        #region IProcessManager 成员

        public string ProcessSaveAs(ProcessMaintainInfo processInfo, string oldProcess)
        {
            string processName = processInfo.Process;
            string result = processName;

            try
            {
                Process processObjOld = processRepository.Find(oldProcess);
                if (processObjOld == null)
                {
                    //Copy的process源已经不存在
                    FisException ex;
                    List<string> paraError = new List<string>();
                    ex = new FisException("DMT141", paraError);
                    throw ex;
                }

                processInfo.Cdt = DateTime.Now;
                processInfo.Udt = DateTime.Now;

                //检查是否已存在相同的Process
                if (processRepository.CheckExistedProcess(processInfo.Process) > 0)
                {
                    FisException ex;
                    List<string> paraError = new List<string>();
                    ex = new FisException("DMT037", paraError);
                    throw ex;

                }

                Process processObj = new Process();
                processObj = convertToObjFromMaintainInfo(processObj, processInfo);
                List<ProcessStation> tmpProcessStationList = (List<ProcessStation>)processRepository.GetProcessStationList(oldProcess);

                List<ProcessStation> processStationList = new List<ProcessStation>();
                for (int i = 0; i < tmpProcessStationList.Count; i++)
                {
                    //!!!拷贝station时状态也拷贝，是吧？
                    ProcessStation item = new ProcessStation();
                    item.Cdt = DateTime.Now;
                    item.Udt = DateTime.Now;
                    item.Editor = processInfo.Editor;
                    item.PreStation = tmpProcessStationList[i].PreStation;
                    item.ProcessID = processName;
                    item.StationID = tmpProcessStationList[i].StationID;
                    item.Status = tmpProcessStationList[i].Status;
                    processStationList.Add(item);
                    
                }

                IUnitOfWork work = new UnitOfWork();
                processRepository.Add(processObj, work);
                processRepository.AddProcessStationsDefered(work, processStationList);
                work.Commit();


            }
            catch (Exception)
            {
                throw;
            }

            return result;

        }


        /// <summary>
        /// 上传一个excel中的内容， 以添加的形式添加process和processStation
        /// </summary>
        /// <returns></returns>
        public string UploadProcess(ProcessMaintainInfo processInfo, List<ProcessStationMaintainInfo> processStationList)
        {
            string processName = processInfo.Process;

            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                //检查是否已存在相同的Process
                if (processRepository.CheckExistedProcess(processInfo.Process) > 0)
                {
                    ex = new FisException("DMT037", paraError);
                    throw ex;

                }

                Process processObj = new Process();
                processObj = convertToObjFromMaintainInfo(processObj, processInfo);

                List<ProcessStation> stationList = new List<ProcessStation>();
                for (int i = 0; i < processStationList.Count; i++)
                {
                    ProcessStation item = new ProcessStation();
                    item.PreStation = processStationList[i].PreStation;
                    item.Status = processStationList[i].Status;
                    item.StationID = processStationList[i].Station;
                    item.Editor = processStationList[i].Editor;
                    item.Cdt = processStationList[i].Cdt;
                    item.Udt = processStationList[i].Udt;
                    item.ProcessID = processName;
                    stationList.Add(item);
                }

                IUnitOfWork work = new UnitOfWork();
                processRepository.Add(processObj, work);
                processRepository.AddProcessStationsDefered(work, stationList);
                work.Commit();


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

            return processName;

        }

        public ProcessInfoDef ExportProcess(string process)
        {
            ProcessInfoDef item = new ProcessInfoDef();
            ProcessMaintainInfo processInfo= getProcess(process);
            if (processInfo.Process == null || processInfo.Process == "")
            {
                //Copy的process源已经不存在
                FisException ex;
                List<string> paraError = new List<string>();
                ex = new FisException("DMT142", paraError);
                throw ex;
            }

            IList<ProcessStationMaintainInfo> processStationList = getProcessStationList(process);

            item.ProcessInfo = processInfo;
            item.ProcessStationList = processStationList;
            return item;

        }


        //!!!换一个函数
        public IList<ProcessMaintainInfo> getProcessList()
        {
            IList<ProcessMaintainInfo> processList = new List<ProcessMaintainInfo>();
            try
            {
                processList = processRepository.GetProcessList();
                //IList<Process> tmpProcessList = processRepository.FindAll();

                //foreach (Process temp in tmpProcessList)
                //{
                //    ProcessMaintainInfo process = new ProcessMaintainInfo();
                    
                //    process = convertToMaintainInfoFromObj(temp);

                //    processList.Add(process);
                //}

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

            return processList;
        }



        public IList<ProcessMaintainInfo> getProcessList(string process)
        {
            IList<ProcessMaintainInfo> processList = new List<ProcessMaintainInfo>();
            try
            {
                IList<Process> tmpProcessList = processRepository.getProcessList(process);

                foreach (Process temp in tmpProcessList)
                {
                    ProcessMaintainInfo tmpProcess = new ProcessMaintainInfo();

                    tmpProcess = convertToMaintainInfoFromObj(temp);

                    processList.Add(tmpProcess);
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

            return processList;
        }

        public IList<ModelProcessMaintainInfo> getModelProcessListByModel(string model)
        {
            IList<ModelProcessMaintainInfo> modelProcessList = new List<ModelProcessMaintainInfo>();
            try
            {
                IList<ModelProcess> tmpModelProcessList = processRepository.GetModelProcessListByModel(model);

                foreach (ModelProcess temp in tmpModelProcessList)
                {
                    ModelProcessMaintainInfo modelProcess = new ModelProcessMaintainInfo();

                    modelProcess = convertToMaintainInfoFromObj(temp);

                    modelProcessList.Add(modelProcess);
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

            return modelProcessList;
        }





        public IList<PalletProcessMaintainInfo> getPalletProcessListByCustomer(string customer)
        {
            IList<PalletProcessMaintainInfo> palletProcessList = new List<PalletProcessMaintainInfo>();
            try
            {
                IList<PalletProcess> tmpPalletProcessList = processRepository.GetPalletProcessListByCustomer(customer);

                foreach (PalletProcess temp in tmpPalletProcessList)
                {
                    PalletProcessMaintainInfo palletProcess = new PalletProcessMaintainInfo();

                    palletProcess = convertToMaintainInfoFromObj(temp);

                    palletProcessList.Add(palletProcess);
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

            return palletProcessList;
        }




        public IList<PartProcessMaintainInfo> getPartProcessListByMBFamily(string mbFamily)
        {
            IList<PartProcessMaintainInfo> partProcessList = new List<PartProcessMaintainInfo>();
            try
            {
                IList<PartProcess> tmpPartProcessList = processRepository.GetPartProcessListByMBFamily(mbFamily);

                foreach (PartProcess temp in tmpPartProcessList)
                {
                    PartProcessMaintainInfo partProcess = new PartProcessMaintainInfo();

                    partProcess = convertToMaintainInfoFromObj(temp);

                    partProcessList.Add(partProcess);
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

            return partProcessList;
        }



        public IList<ReworkProcessMaintainInfo> getReworkProcessListByReworkCode(string reworkCode)
        {
            IList<ReworkProcessMaintainInfo> reworkProcessList = new List<ReworkProcessMaintainInfo>();
            try
            {
                IList<ReworkProcess> tmpReworkProcessList = processRepository.GetReworkProcessListByReworkCode(reworkCode);

                foreach (ReworkProcess temp in tmpReworkProcessList)
                {
                    ReworkProcessMaintainInfo reworkProcess = new ReworkProcessMaintainInfo();

                    reworkProcess = convertToMaintainInfoFromObj(temp);

                    reworkProcessList.Add(reworkProcess);
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

            return reworkProcessList;
        }

        public IList<PalletProcessMaintainInfo> getPalletProcessListByProcess(string process)
        {
            IList<PalletProcessMaintainInfo> palletProcessList = new List<PalletProcessMaintainInfo>();
            try
            {
                IList<PalletProcess> tmpPalletProcessList = processRepository.GetPalletProcessListByProcess(process);

                foreach (PalletProcess temp in tmpPalletProcessList)
                {
                    PalletProcessMaintainInfo palletProcess = new PalletProcessMaintainInfo();

                    palletProcess = convertToMaintainInfoFromObj(temp);

                    palletProcessList.Add(palletProcess);
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

            return palletProcessList;
        }

        public IList<PartProcessMaintainInfo> getPCBProcessListByProcess(string process)
        {
            IList<PartProcessMaintainInfo> partProcessList = new List<PartProcessMaintainInfo>();
            try
            {
                IList<PartProcess> tmpPartProcessList = processRepository.GetPartProcessListByProcess(process);

                foreach (PartProcess temp in tmpPartProcessList)
                {
                    PartProcessMaintainInfo partProcess = new PartProcessMaintainInfo();

                    partProcess = convertToMaintainInfoFromObj(temp);

                    partProcessList.Add(partProcess);
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

            return partProcessList;
        }

        public IList<ReworkProcessMaintainInfo> getReworkProcessListByProcess(string process)
        {
            IList<ReworkProcessMaintainInfo> reworkProcessList = new List<ReworkProcessMaintainInfo>();
            try
            {
                IList<ReworkProcess> tmpReworkProcessList = processRepository.GetReworkProcessListByProcess(process);

                foreach (ReworkProcess temp in tmpReworkProcessList)
                {
                    ReworkProcessMaintainInfo reworkProcess = new ReworkProcessMaintainInfo();

                    reworkProcess = convertToMaintainInfoFromObj(temp);

                    reworkProcessList.Add(reworkProcess);
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

            return reworkProcessList;
        }

        public IList<ReworkReleaseTypeMaintainInfo> getReworkReleaseTypeListByProcess(string process)
        {
            IList<ReworkReleaseTypeMaintainInfo> reworkReleaseTypeList = new List<ReworkReleaseTypeMaintainInfo>();
            try
            {
                IList<ReworkReleaseType> tmpReworkReleaseTypeList = processRepository.GetReworkReleaseTypeListByProcess(process);

                foreach (ReworkReleaseType temp in tmpReworkReleaseTypeList)
                {
                    ReworkReleaseTypeMaintainInfo reworkReleaseType = new ReworkReleaseTypeMaintainInfo();

                    reworkReleaseType = convertToMaintainInfoFromObj(temp);

                    reworkReleaseTypeList.Add(reworkReleaseType);
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

            return reworkReleaseTypeList;
        }

        public IList<PalletProcessMaintainInfo> getPalletProcessList()
        {
            throw new NotImplementedException();
        }

        public IList<PartProcessMaintainInfo> getPCBProcessList()
        {
            IList<PartProcessMaintainInfo> PCBList = new List<PartProcessMaintainInfo>();
            try
            {
                IList<CustomerInfo> tmpCustomerList = miscRepository.GetCustomerList();

                foreach (CustomerInfo temp in tmpCustomerList)
                {
                    PartProcessMaintainInfo pcb = new PartProcessMaintainInfo();

                    pcb = convertToPartProcessMaintainInfoFromObj(temp);

                    PCBList.Add(pcb);
                }

                IList<string> tmpPCBFamilyList = processRepository.GetPCBFamilyList();

                foreach (string temp in tmpPCBFamilyList)
                {
                    PartProcessMaintainInfo pcb = new PartProcessMaintainInfo();

                    pcb = convertToPartProcessMaintainInfoFromObj(temp);

                    PCBList.Add(pcb);
                }

                //IList<string> tmpPCBModelList = processRepository.GetPCBModelList();

                //foreach (string temp in tmpPCBModelList)
                //{
                //    PartProcessMaintainInfo pcb = new PartProcessMaintainInfo();

                //    pcb = convertToPartProcessMaintainInfoFromObj(temp);

                //    PCBList.Add(pcb);
                //}
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

            return PCBList;
        }

        public IList<ReworkProcessMaintainInfo> getReworkProcessList()
        {
            IList<ReworkProcessMaintainInfo> reworkProcessList = new List<ReworkProcessMaintainInfo>();
            try
            {
                IList<string> tmpReworkProcessList = processRepository.GetReworkReleaseTypeList();

                foreach (string temp in tmpReworkProcessList)
                {
                    ReworkProcessMaintainInfo reworkProcess = new ReworkProcessMaintainInfo();

                    reworkProcess = convertToReworkProcessMaintainInfoFromObj(temp);

                    reworkProcessList.Add(reworkProcess);
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

            return reworkProcessList;
        }

        public IList<ReworkReleaseTypeMaintainInfo> getReworkReleaseTypeList()
        {
            IList<ReworkReleaseTypeMaintainInfo> reworkReleaseTypeList = new List<ReworkReleaseTypeMaintainInfo>();
            try
            {
                IList<string> tmpReworkReleaseTypeList = processRepository.GetReworkReleaseTypeList();

                foreach (string temp in tmpReworkReleaseTypeList)
                {
                    ReworkReleaseTypeMaintainInfo reworkReleaseType = new ReworkReleaseTypeMaintainInfo();

                    reworkReleaseType = convertToReworkReleaseTypeMaintainInfoFromObj(temp);

                    reworkReleaseTypeList.Add(reworkReleaseType);
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

            return reworkReleaseTypeList;
        }

        public ProcessMaintainInfo getProcess(string process)
        {
            ProcessMaintainInfo tmpProcess = new ProcessMaintainInfo();

            try
            {
                Process processObj = processRepository.Find(process);
                if (processObj == null)
                {
                    return new ProcessMaintainInfo();
                }
                tmpProcess = convertToMaintainInfoFromObj(processObj);
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

            return tmpProcess;
        }

        public void addProcess(ProcessMaintainInfo processInfo)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                //检查是否已存在相同的Process
                if (processRepository.CheckExistedProcess(processInfo.Process) > 0)
                {
                    ex = new FisException("DMT037", paraError);
                    throw ex;

                }
                else 
                {
                    Process processObj = new Process();
                    processObj = convertToObjFromMaintainInfo(processObj, processInfo);

                    IUnitOfWork work = new UnitOfWork();

                    processRepository.Add(processObj, work);

                    work.Commit();
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

        public void saveProcess(string strOldProcessName, ProcessMaintainInfo processInfo)
        {

            try
            {
                Process processObj = processRepository.Find(strOldProcessName);
                if (processObj == null)
                {
                    FisException ex;
                    List<string> paraError = new List<string>();
                    ex = new FisException("DMT141", paraError);
                    throw ex;
                }

                if (processObj.Type != processInfo.Type)
                {
                    FisException ex;
                    List<string> paraError = new List<string>();
                    ex = new FisException("DMT145", paraError);
                    throw ex;
                }
                
                processObj = convertToObjFromMaintainInfo(processObj, processInfo);

                IUnitOfWork work = new UnitOfWork();

                processRepository.SaveProcessDefered(work, strOldProcessName, processObj);

                work.Commit();

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

        public void deleteProcess(string process)
        {
            try
            {

                if (processRepository.ExistProcessRule(process) == true)
                {
                    //!!! Process已经被使用,不能
                    FisException ex;
                    List<string> paraError = new List<string>();
                    ex = new FisException("DMT140", paraError);
                    throw ex;
                }

                IList<ProcessStation> tmpProcessStationList = processRepository.GetProcessStationList(process);
                if (tmpProcessStationList.Count > 0)
                {
                    FisException ex;
                    List<string> paraError = new List<string>();
                    ex = new FisException("DMT140", paraError);
                    throw ex;
                }

                Process processObj = processRepository.Find(process);
                if (processObj == null)
                {
                    return;
                }

                IUnitOfWork work = new UnitOfWork();

                processRepository.Remove(processObj, work);

                work.Commit();
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

        public IList<ProcessStationMaintainInfo> getProcessStationList(string process)
        {
            IList<ProcessStationMaintainInfo> processStationList = new List<ProcessStationMaintainInfo>();
            try
            {
                IList<ProcessStation> tmpProcessStationList = processRepository.GetProcessStationList(process);

                foreach (ProcessStation temp in tmpProcessStationList)
                {
                    ProcessStationMaintainInfo processStation = new ProcessStationMaintainInfo();

                    processStation = convertToMaintainInfoFromObj(temp);

                    processStationList.Add(processStation);
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

            return processStationList;
        }

        public ProcessStationMaintainInfo getProcessStation(int id)
        {
            ProcessStationMaintainInfo processStation = new ProcessStationMaintainInfo();

            try
            {
                ProcessStation processStationObj = processRepository.GetProcessStation(id);

                processStation = convertToMaintainInfoFromObj(processStationObj);
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

            return processStation;
        }

        public void deleteProcessStation(int id)
        {
            try
            {
                IUnitOfWork work = new UnitOfWork();
                processRepository.DeleteProcessStationDefered(work, id);
                work.Commit();
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

        public int saveProcessStation(ProcessStationMaintainInfo processStationInfo)
        {
            FisException ex;
            List<string> paraError = new List<string>();
            try
            {
                
                ProcessStation processStationObj = null;
                if (processStationInfo.Id != 0)
                {
                    processStationObj = processRepository.GetProcessStation(processStationInfo.Id);
                }
                if (processStationObj == null)
                {
                    IList<ProcessStation> processStationList = processRepository.GetProcessStationList(processStationInfo.Process);
                    processStationObj = new ProcessStation();
                    processStationObj = convertToObjFromMaintainInfo(processStationObj, processStationInfo);
                    IList<ProcessStation> checkExistProcessStationList = (from q in processStationList
                                                                          where q.PreStation == processStationObj.PreStation &&
                                                                          q.StationID == processStationObj.StationID &&
                                                                          q.Status == processStationObj.Status
                                                                          select q).ToList<ProcessStation>();
                    if (checkExistProcessStationList != null && checkExistProcessStationList.Count > 0)
                    {
                        ex = new FisException("DMT038", paraError);
                        throw ex;
                    }
                    IUnitOfWork work = new UnitOfWork();
                    processRepository.AddProcessStationDefered(work, processStationObj);
                    work.Commit();
                    //int count = processRepository.CheckExistedProcessStation(processStationInfo.Process, processStationInfo.Station, processStationInfo.PreStation);
                    //if (count > 0)
                    //{
                    //    ex = new FisException("DMT038", paraError);
                    //    throw ex;
                    //}

                    //processStationObj = new ProcessStation();
                    //processStationObj = convertToObjFromMaintainInfo(processStationObj, processStationInfo);

                    //IUnitOfWork work = new UnitOfWork();
                    //processRepository.AddProcessStationDefered(work, processStationObj);
                    //work.Commit();

                }
                else
                {

                    processStationObj = convertToObjFromMaintainInfo(processStationObj, processStationInfo);

                    IUnitOfWork work = new UnitOfWork();

                    processRepository.SaveProcessStationDefered(work, processStationObj);

                    work.Commit();
                }

                return processStationObj.ID;

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

        public IList<StationMaintainInfo> getStationList()
        {
            IList<StationMaintainInfo> stationList = new List<StationMaintainInfo>();
            try
            {
                IList<IStation> tmpStationList = stationRepository.FindAll();

                foreach (IStation temp in tmpStationList)
                {
                    StationMaintainInfo station = new StationMaintainInfo();

                    station = convertToMaintainInfoFromObj(temp);

                    stationList.Add(station);
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

            return stationList;
        }

        public void addPartProcesses(IList<PartProcessMaintainInfo> arrCheckedMBFamily, PartProcessMaintainInfo partProcessInfo)
        {
            try
            {

                IUnitOfWork work = new UnitOfWork();

                processRepository.DeletePartProcessByProcessDefered(work, partProcessInfo.Process);

                for (int i = 0; i < arrCheckedMBFamily.Count(); i++)
                {
                    PartProcess partProcessObj = new PartProcess();

                    partProcessInfo.MBFamily = arrCheckedMBFamily[i].MBFamily;
                    partProcessInfo.PilotRun = arrCheckedMBFamily[i].PilotRun;

                    partProcessObj = convertToObjFromMaintainInfo(partProcessObj, partProcessInfo);


                    DataTable exists = processRepository.GetExistPartProcess(partProcessInfo.MBFamily);
                    if (exists != null && exists.Rows.Count > 0)
                    {
                        string process = Null2String(exists.Rows[0][1]);
                        String curProcess = Null2String(partProcessInfo.Process);
                        if (process != curProcess)
                        {
                            //具有相同MBFamily 1%的PartProcess已经存在，不能保存!
                            List<string> erpara = new List<string>();
                            erpara.Add(partProcessInfo.MBFamily);
                            FisException ex;
                            ex = new FisException("DMT143", erpara);
                            throw ex;
                        }
                    }

                    processRepository.AddPartProcessDefered(work, partProcessObj);

                }

                work.Commit();

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

        private static String Null2String(Object _input)
        {
            if (_input == null)
            {
                return "";
            }
            return _input.ToString().Trim();
        }

        public void addReworkProcesses(IList<string> arrCheckedReworkCode, ReworkProcessMaintainInfo reworkProcessInfo)
        {
            try
            {

                IUnitOfWork work = new UnitOfWork();

                processRepository.DeleteReworkProcessByProcessDefered(work, reworkProcessInfo.Process);

                for (int i = 0; i < arrCheckedReworkCode.Count(); i++)
                {
                    ReworkProcess reworkProcessObj = new ReworkProcess();

                    reworkProcessInfo.ReworkCode = arrCheckedReworkCode[i];

                    reworkProcessObj = convertToObjFromMaintainInfo(reworkProcessObj, reworkProcessInfo);

                    processRepository.AddReworkProcessDefered(work, reworkProcessObj);

                }

                work.Commit();

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

        public void addReworkReleaseType(IList<string> arrCheckedReworkReleaseType, ReworkReleaseTypeMaintainInfo reworkReleaseTypeProcessInfo)
        {
            try
            {

                IUnitOfWork work = new UnitOfWork();

                processRepository.DeleteReworkReleaseTypeByProcessDefered(work, reworkReleaseTypeProcessInfo.Process);

                for (int i = 0; i < arrCheckedReworkReleaseType.Count(); i++)
                {
                    ReworkReleaseType reworkReleaseTypeObj = new ReworkReleaseType();

                    reworkReleaseTypeObj.ReleaseType = arrCheckedReworkReleaseType[i];

                    reworkReleaseTypeObj.Process = reworkReleaseTypeProcessInfo.Process;

                    processRepository.AddReworkReleaseTypeDefered(work, reworkReleaseTypeObj);

                }

                work.Commit();

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
        

        public void addPalletProcesses(IList<string> arrCheckedCustomer, PalletProcessMaintainInfo palletProcessInfo)
        {
            try
            {

                IUnitOfWork work = new UnitOfWork();

                processRepository.DeletePalletProcessByProcessDefered(work, palletProcessInfo.Process);

                for (int i = 0; i < arrCheckedCustomer.Count(); i++)
                {
                    PalletProcess palletProcessObj = new PalletProcess();

                    palletProcessInfo.Customer = arrCheckedCustomer[i];

                    palletProcessObj = convertToObjFromMaintainInfo(palletProcessObj, palletProcessInfo);

                    processRepository.AddPalletProcessDefered(work, palletProcessObj);

                }

                work.Commit();

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


        public IList<ReworkMaintainInfo> getReworkList()
        {
            IList<ReworkMaintainInfo> reworkList = new List<ReworkMaintainInfo>();
            try
            {
                IList<Rework> tmpReworkList = miscRepository.GetReworkList();

                foreach (Rework temp in tmpReworkList)
                {
                    ReworkMaintainInfo rework = new ReworkMaintainInfo();

                    rework = convertToMaintainInfoFromObj(temp);

                    reworkList.Add(rework);
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

            return reworkList;
        }

        public IList<CustomerMaintainInfo> GetMyCustomerList()
        {
            IList<CustomerMaintainInfo> customerList = new List<CustomerMaintainInfo>();
            try
            {
                IList<CustomerInfo> tmpCustomerList = miscRepository.GetCustomerList();

                foreach (CustomerInfo temp in tmpCustomerList)
                {
                    CustomerMaintainInfo customer = new CustomerMaintainInfo();

                    customer = convertToCustomerMaintainInfoFromObj(temp);

                    customerList.Add(customer);
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

            return customerList;
        }


        public IList<string> getMBFamilyAndMBModelList()
        {
            IList<string> mbFamilyAndmbModelList = new List<string>();
            try
            {
                IList<string> tmpMbFamilyList = mbModelRepository.GetMBFamilyList();

                foreach (string temp in tmpMbFamilyList)
                {
                    mbFamilyAndmbModelList.Add(temp);
                }

                IList<string> tmpMbModelList = mbModelRepository.GetMBModelList();

                foreach (string temp in tmpMbModelList)
                {
                    mbFamilyAndmbModelList.Add(temp);
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

            return mbFamilyAndmbModelList;
        }


        private PalletProcess convertToObjFromMaintainInfo(PalletProcess obj, PalletProcessMaintainInfo temp)
        {

            obj.Customer = temp.Customer;
            obj.Process = temp.Process;
            obj.Udt = temp.Udt;
            obj.Cdt = temp.Cdt;
            obj.Editor = temp.Editor;

            return obj;
        }

        private ReworkProcess convertToObjFromMaintainInfo(ReworkProcess obj, ReworkProcessMaintainInfo temp)
        {

            obj.ReworkCode = temp.ReworkCode;
            obj.Process = temp.Process;
            obj.Udt = temp.Udt;
            obj.Cdt = temp.Cdt;
            obj.Editor = temp.Editor;

            return obj;
        }

        private PartProcess convertToObjFromMaintainInfo(PartProcess obj, PartProcessMaintainInfo temp)
        {

            obj.MBFamily = temp.MBFamily;
            obj.Process = temp.Process;
            obj.PilotRun = temp.PilotRun;
            obj.Udt = temp.Udt;
            obj.Cdt = temp.Cdt;
            obj.Editor = temp.Editor;

            return obj;
        }


        private Process convertToObjFromMaintainInfo(Process obj, ProcessMaintainInfo temp)
        {
            obj.Type = temp.Type;
            obj.process = temp.Process;
            obj.Descr = temp.Description;
            obj.Udt = temp.Udt;
            obj.Cdt = temp.Cdt;
            obj.Editor = temp.Editor;

            return obj;
        }

        private ProcessStation convertToObjFromMaintainInfo(ProcessStation obj, ProcessStationMaintainInfo temp)
        {

            obj.ID = temp.Id;
            obj.PreStation = temp.PreStation;
            obj.ProcessID = temp.Process;
            obj.StationID = temp.Station;
            obj.Status = temp.Status;
            obj.Editor = temp.Editor;

            return obj;
        }

        private ReworkReleaseTypeMaintainInfo convertToReworkReleaseTypeMaintainInfoFromObj(string temp)
        {
            ReworkReleaseTypeMaintainInfo reworkReleaseType = new ReworkReleaseTypeMaintainInfo();

            reworkReleaseType.ReleaseType = temp;

            return reworkReleaseType;
        }


        private ReworkProcessMaintainInfo convertToReworkProcessMaintainInfoFromObj(string temp)
        {
            ReworkProcessMaintainInfo reworkProcess = new ReworkProcessMaintainInfo();

            reworkProcess.ReworkCode = temp;

            return reworkProcess;
        }


        private PartProcessMaintainInfo convertToPartProcessMaintainInfoFromObj(string temp)
        {
            PartProcessMaintainInfo pcb = new PartProcessMaintainInfo();

            pcb.Process = temp;

            return pcb;
        }

        
        private PartProcessMaintainInfo convertToPartProcessMaintainInfoFromObj(CustomerInfo temp)
        {
            PartProcessMaintainInfo pcb = new PartProcessMaintainInfo();

            pcb.Process = temp.customer;

            return pcb;
        }


        private CustomerMaintainInfo convertToCustomerMaintainInfoFromObj(CustomerInfo temp)
        {
            CustomerMaintainInfo customer = new CustomerMaintainInfo();

            customer.Code = temp.Code;
            customer.Customer = temp.customer;
            customer.Description = temp.Description;

            return customer;
        }

        private ReworkMaintainInfo convertToMaintainInfoFromObj(Rework temp)
        {
            ReworkMaintainInfo rework = new ReworkMaintainInfo();

            rework.ReworkCode = temp.ReworkCode;
            rework.Status = temp.Status;
            rework.Editor = temp.Editor;
            rework.Cdt = temp.Cdt;
            rework.Udt = temp.Udt;

            return rework;
        }


        private StationMaintainInfo convertToMaintainInfoFromObj(IStation temp)
        {
            StationMaintainInfo station = new StationMaintainInfo();

            station.Station = temp.StationId;
            //station.StationType = temp.StationType;
            station.OperationObject = temp.OperationObject;
            station.Cdt = temp.Cdt;
            station.Descr = temp.Descr;
            station.Editor = temp.Editor;
            station.Udt = temp.Udt;

            return station;
        }
        
        private ProcessStationMaintainInfo convertToMaintainInfoFromObj(ProcessStation temp)
        {
            ProcessStationMaintainInfo processStation = new ProcessStationMaintainInfo();

            processStation.Id = temp.ID;
            processStation.PreStation = temp.PreStation;
            processStation.Station = temp.StationID;
            processStation.Process = temp.ProcessID;
            processStation.Status = temp.Status;
            processStation.Editor = temp.Editor;
            processStation.Cdt = temp.Cdt;
            processStation.Udt = temp.Udt;

            return processStation;
        }

        private ProcessMaintainInfo convertToMaintainInfoFromObj(Process temp)
        {
            ProcessMaintainInfo process = new ProcessMaintainInfo();
            process.Type = temp.Type;
            process.Process = temp.process;
            process.Description = temp.Descr;
            process.Editor = temp.Editor;
            process.Cdt = temp.Cdt;
            process.Udt = temp.Udt;

            return process;
        }

        private ModelProcessMaintainInfo convertToMaintainInfoFromObj(ModelProcess temp)
        {
            ModelProcessMaintainInfo modelProcess = new ModelProcessMaintainInfo();

            modelProcess.Model = temp.Model;
            modelProcess.Process = temp.Process;
            modelProcess.Id = temp.ID;
            //modelProcess.RuleType = temp.RuleType;
            modelProcess.Editor = temp.Editor;
            modelProcess.Cdt = temp.Cdt;
            modelProcess.Udt = temp.Udt;

            return modelProcess;
        }

        private PalletProcessMaintainInfo convertToMaintainInfoFromObj(PalletProcess temp)
        {
            PalletProcessMaintainInfo palletProcess = new PalletProcessMaintainInfo();

            palletProcess.Customer = temp.Customer;
            palletProcess.Process = temp.Process;
            palletProcess.Editor = temp.Editor;
            palletProcess.Cdt = temp.Cdt;
            palletProcess.Udt = temp.Udt;

            return palletProcess;
        }

        private PartProcessMaintainInfo convertToMaintainInfoFromObj(PartProcess temp)
        {
            PartProcessMaintainInfo partProcess = new PartProcessMaintainInfo();

            partProcess.MBFamily = temp.MBFamily;
            partProcess.Process = temp.Process;
            partProcess.PilotRun = temp.PilotRun;
            partProcess.Editor = temp.Editor;
            partProcess.Cdt = temp.Cdt;
            partProcess.Udt = temp.Udt;

            return partProcess;
        }

        private ReworkProcessMaintainInfo convertToMaintainInfoFromObj(ReworkProcess temp)
        {
            ReworkProcessMaintainInfo reworkProcess = new ReworkProcessMaintainInfo();

            reworkProcess.ReworkCode = temp.ReworkCode;
            reworkProcess.Process = temp.Process;
            reworkProcess.Editor = temp.Editor;
            reworkProcess.Cdt = temp.Cdt;
            reworkProcess.Udt = temp.Udt;

            return reworkProcess;
        }

        private ReworkReleaseTypeMaintainInfo convertToMaintainInfoFromObj(ReworkReleaseType temp)
        {
            ReworkReleaseTypeMaintainInfo reworkReleaseType = new ReworkReleaseTypeMaintainInfo();

            reworkReleaseType.ReleaseType = temp.ReleaseType;
            reworkReleaseType.Process = temp.Process;
            reworkReleaseType.Cdt = temp.Cdt;

            return reworkReleaseType;
        }


        //IList<ConstValueTypeInfo> getallProcessbyMaterial(string type);
        public IList<ConstValueTypeInfo> getallProcessbyMaterial(string type)
        {
            try
            {
                return iPartRepository.GetConstValueTypeList(type);
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

        public IList<MaterialProcess> GetMaterialProcessByProcess(string process)
        {
            try
            {
                return processRepository.GetMaterialProcessByProcess(process);
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

        public void AddMaterialProcess(string materialType, string process, string editor)
        {
            try
            {
                processRepository.AddMaterialProcess(materialType,process,editor);
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

        public void RemoveMaterialProcessByType(string materialType)
        {
            try
            {
                processRepository.RemoveMaterialProcessByType(materialType);
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
