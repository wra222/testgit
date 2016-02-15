using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IMES.Maintain.Interface.MaintainIntf;
using IMES.DataModel;
using log4net;

using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using IMES.FisObject.Common.Model;
using IMES.FisObject.Common.FisBOM;
namespace IMES.Maintain.Implementation
{
    public class GradeManager : MarshalByRefObject,IGrade
    {

        #region IGrade Members
        public const string PUB = "<PUB>";
        public const string HP = "HP";
        private ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IList<GradeInfo> gradeLst = new List<GradeInfo>();
        IBOMRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IBOMRepository>();
        public IList<GradeInfo> GetAllGrades()
        {
            //if (gradeLst.Count == 0)
            //{
            //    int i=0;
            //    for (; i < 10; i++)
            //    {
            //        GradeInfo item = new GradeInfo();
            //        item.id=i;
            //        item.family = "Family" + i;
            //        item.series = "Series" + i;
            //        item.grade = "Consumer";
            //        item.energia = "Energia" + i;
            //        item.espera = "Espera" + i;
            //        item.editor = "Editor" + i;
            //        item.cdt = DateTime.Now;
            //        gradeLst.Add(item);
            //    }
            //    return gradeLst;
            //}
            //else
            //{
            //    return gradeLst;
            //}
            IList<GradeInfo> gradeList = new List<GradeInfo>();
            try 
            {
                gradeLst = (IList<GradeInfo>)itemRepository.GetAllGrades();
            }
            catch(FisException fe)
            {
                logger.Error(fe.Message);
                throw fe;
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return gradeLst;
        }

        public IList<GradeInfo> GetGradesByFamily(string family)
        {
            IList<GradeInfo> gradeLst2 = new List<GradeInfo>();
            //foreach (GradeInfo grade in gradeLst)
            //{
            //    if(grade.family.ToUpper()==family.ToUpper())
            //    {
            //        gradeLst2.Add(grade);
            //    }
            //}
            try 
            {
                if (family == "All")
                {
                    gradeLst2 = itemRepository.GetAllGrades();
                }
                else 
                {
                    gradeLst2 = itemRepository.GetGradesByFamily(family);
                }
                
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return gradeLst2;
        }

      

        public void DeleteSelectedGrade(int id)
        {

            try 
            {
                itemRepository.DeleteSelectedGrade(id);
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
        }

        public void UpdateSelectedGrade(GradeInfo newItem)
        {
            try 
            {
                
                 gradeLst = (IList<GradeInfo>)itemRepository.GetAllGrades();
                 Boolean duplicateFlag = false, notExistFlag = true;
                 FisException ex=null;
                    
                  foreach (GradeInfo info in gradeLst)
                  {
                      if(info.id==newItem.id)
                      {
                        notExistFlag=false;
                      }
                      if (info.id!=newItem.id&&info.family == newItem.family && info.series == newItem.series && info.grade == newItem.grade)
                      {
                           
                           duplicateFlag = true;
                           
                       }
                  }
                  if (notExistFlag)
                  {
                      List<string> param = new List<string>();
                      ex = new FisException("DMT083", param);
                      throw ex;
                  }
                  else if (duplicateFlag)
                  {
                    List<string> param = new List<string>();
                    ex = new FisException("DMT074", param);
                    throw ex;
                  }

                  newItem.udt = DateTime.Now;
                  itemRepository.UpdateSelectedGrade(newItem);    
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }

        }

        #endregion

        #region IGrade Members


        public string AddSelectedGrade(GradeInfo grade)
        {
            string id = null;
            IList<GradeInfo> gradeLst = new List<GradeInfo>();
            try 
            {
                if (grade == null)
                {
                    //throw exception;
                }
                else
                {
                    gradeLst = (IList<GradeInfo>)itemRepository.GetAllGrades();
                    foreach (GradeInfo info in gradeLst)
                    {
                        FisException ex;
                        if (info.family == grade.family && info.series == grade.series && info.grade == grade.grade)
                        {
                            List<string> param = new List<string>();
                            ex = new FisException("DMT074", param);
                            throw ex;
                        }
                    }
                    itemRepository.AddSelectedGrade(grade);
                    id=Convert.ToString(grade.id);
                }
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return id;
        }

        #endregion

        #region IGrade 成员


        public IList<string> GetAllFamilys()
        {
            IList<string> familyLst = new List<string>();
            IFamilyRepository itemRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
            try 
            {
                IList<Family> familyObjs = itemRepository.GetFamilyObjList();
                if(familyObjs!=null&&familyObjs.Count!=0)
                {
                    foreach(Family f in familyObjs)
                    {
                        familyLst.Add(f.FamilyName);
                    }
                }
            }
            catch(Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return familyLst;
        }

        #endregion

        #region IGrade 成员


        public IList<string> GetHPFamilys()
        {
            IList<string> hpfamily = new List<string>();
            string[] args = new string[2];
            IFamilyRepository familyRepository = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
            try
            {
                args[0] = PUB;
                args[1] = HP;
                hpfamily = familyRepository.GetFamilysByCustomer(args);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                throw e;
            }
            return hpfamily;
        }

        #endregion
    }
}
