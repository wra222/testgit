using System;
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
using IMES.FisObject.FA.Product;
using IMES.Infrastructure;
using IMES.Infrastructure.FisObjectRepositoryFramework;
using System.Collections.Generic;
using IMES.FisObject.Common.Part;
using IMES.DataModel;
using MySql.Data.MySqlClient;
using IMES.FisObject.Common.Model;
using IMES.FisObject.PCA.MB;

namespace IMES.Activity
{
    /// <summary>
    /// Tablet Check Secure On
    /// </summary>
    public partial class CheckTabletSecureOn : BaseActivity
	{
        /// <summary>
        /// Construct class
        /// </summary>
		public CheckTabletSecureOn()
		{
			InitializeComponent();
		}

        //private static string connectstr = "SERVER=192.168.130.31;DATABASE=db_se_os_hp slate 7 plus;UID=admin;PASSWORD=birch;Compress=false;Pooling=true;";
        private static string connectstr = "SERVER={0};DATABASE={1};UID={2};PASSWORD={3};Compress=false;Pooling=true;";
        /// <summary>
        /// MySQL Connection String
        /// </summary>
        public static DependencyProperty MySQLConnectStrProperty = DependencyProperty.Register("MySQLConnectStr", typeof(string), typeof(CheckTabletSecureOn), new PropertyMetadata(connectstr));
        /// <summary>
        ///  MySQL Connection String
        /// </summary>
        [DescriptionAttribute("MySQLConnectStr")]
        [CategoryAttribute("MySQL")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public string MySQLConnectStr
        {
            get
            {
                return ((string)(base.GetValue(CheckTabletSecureOn.MySQLConnectStrProperty)));
            }
            set
            {
                base.SetValue(CheckTabletSecureOn.MySQLConnectStrProperty, value);
            }
        }

        /// <summary>
        /// SQL Comand Time Out
        /// </summary>
        public static DependencyProperty CmdTimeOutProperty = DependencyProperty.Register("CmdTimeOut", typeof(int), typeof(CheckTabletSecureOn), new PropertyMetadata(15));
        /// <summary>
        ///  MySQL Connection String
        /// </summary>
        [DescriptionAttribute("SQL Comand Time Out")]
        [CategoryAttribute("MySQL")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public int CmdTimeOut
        {
            get
            {
                return ((int)(base.GetValue(CheckTabletSecureOn.CmdTimeOutProperty)));
            }
            set
            {
                base.SetValue(CheckTabletSecureOn.CmdTimeOutProperty, value);
            }
        }

        /// <summary>
        /// ThrowException
        /// </summary>
        public static DependencyProperty IsThrowExceptionProperty = DependencyProperty.Register("IsThrowException", typeof(bool), typeof(CheckTabletSecureOn), new PropertyMetadata(true));
        /// <summary>
        ///  SecureOn
        /// </summary>
        [DescriptionAttribute("IsThrowException")]
        [CategoryAttribute("SecureOn")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsThrowException
        {
            get
            {
                return ((bool)(base.GetValue(CheckTabletSecureOn.IsThrowExceptionProperty)));
            }
            set
            {
                base.SetValue(CheckTabletSecureOn.IsThrowExceptionProperty, value);
            }
        }

        ///<summary>
        /// IsMB
        /// </summary>
        public static DependencyProperty IsMBCPUIDProperty = DependencyProperty.Register("IsMBCPUID", typeof(bool), typeof(CheckTabletSecureOn), new PropertyMetadata(false));
        /// <summary>
        ///  SecureOn
        /// </summary>
        [DescriptionAttribute("IsMBCPUID")]
        [CategoryAttribute("SecureOn")]
        [BrowsableAttribute(true)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)]
        public bool IsMBCPUID
        {
            get
            {
                return ((bool)(base.GetValue(CheckTabletSecureOn.IsMBCPUIDProperty)));
            }
            set
            {
                base.SetValue(CheckTabletSecureOn.IsMBCPUIDProperty, value);
            }
        }

        private string mySqlIp = "";
        private string mySqlDb = "";
        private string mySqlUser = "";
        private string mySqlPwd = "";
        private string secureSql = "";
        /// <summary>
        /// Tablet Check Secure On
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {         
            if(IsMBCPUID)
            {
                 checkMBSecureOn();
            }
            else
            {
                checkProductSecureOn();
            }
           
            //for unit test
            //IProductRepository prodRep = RepositoryFactory.GetInstance().GetRepository<IProductRepository>();
            //IMES.Infrastructure.UnitOfWork.IUnitOfWork uof = new IMES.Infrastructure.UnitOfWork.UnitOfWork();
            //prodRep.Update(currentProd, uof);
            //uof.Commit();

            return base.DoExecute(executionContext);
        }

        private bool getSecureFromMySQL(string dbCnStr, string sqlStr, string cpuUid ,string sn)
        {
            bool ret = false;
//            string strSQL = @"select  id, uid, create_time as cdt, verified, user, ip
//                                            from `model_hp slate 7 plus`
//                                            where serialNo=?sn                                           
//                                            order by create_time desc
//                                             limit 1 ";

            string strSQL = sqlStr;
            bool dbConnected = false;
            using (MySqlConnection conn = new MySqlConnection(dbCnStr))
             {
                try
                {                  
                    conn.Open();
                    dbConnected = true;
                    using (MySqlCommand command = conn.CreateCommand())
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = strSQL;
                        command.CommandTimeout = this.CmdTimeOut;

                        command.Parameters.Add("?sn", MySqlDbType.VarChar);
                        command.Parameters["?sn"].Value = sn;

                        command.Parameters.Add("?uid", MySqlDbType.VarChar);
                        command.Parameters["?uid"].Value = cpuUid;


                        using (MySqlDataReader sqlR = command.ExecuteReader())
                        {
                            if (sqlR != null )
                            {
                                ret = sqlR.HasRows;
                            }                          
                            sqlR.Close();
                        }
                    }                                      
                    return ret;
                }
                catch (Exception e)
                {
                    throw new FisException("CHK1037", new string[]{e.Message + " " + e.StackTrace});
                }
                finally 
                {
                    if (dbConnected) conn.Close();
                }
            }
        }

        private void getMySqlConnectionInfo(string family)
        {
            IFamilyRepository familyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();

            IList<FamilyInfoDef> familyInfoList = familyRep.GetExistFamilyInfo(new FamilyInfoDef { family = family });


            foreach (FamilyInfoDef item in familyInfoList)
            {
                switch (item.name.Trim())
                {
                    case "MySQLIP":
                        mySqlIp = item.value;
                        break;
                    case "MySQLDB":
                        mySqlDb = item.value;
                        break;
                    case "MySQLUser":
                        mySqlUser = item.value;
                        break;
                    case "MySQLPwd":
                        mySqlPwd = item.value;
                        break;
                    case "SecureSQL":
                        secureSql = item.value;
                        break;
                }
            }

            if (string.IsNullOrEmpty(mySqlIp))
            {
                throw new FisException("CHK1036", new string[] { family, "MySQLIP" });
            }

            if (string.IsNullOrEmpty(mySqlDb))
            {
                throw new FisException("CHK1036", new string[] { family, "MySQLDB" });
            }

            if (string.IsNullOrEmpty(mySqlUser))
            {
                throw new FisException("CHK1036", new string[] { family, "MySQLUser" });
            }

            if (string.IsNullOrEmpty(mySqlPwd))
            {
                throw new FisException("CHK1036", new string[] { family, "MySQLPwd" });
            }

            if (string.IsNullOrEmpty(secureSql))
            {
                throw new FisException("CHK1036", new string[] { family, "SecureSQL" });
            }
        }

        private void  checkProductSecureOn()
        {

            IPartRepository partRep = RepositoryFactory.GetInstance().GetRepository<IPartRepository>();
            IList<ConstValueTypeInfo> checkFamilyList = partRep.GetConstValueTypeList("CheckTabletSecureFamily");
            if (checkFamilyList.Count == 0)
            {
                return;
            }

            IProduct currentProd = CurrentSession.GetValue(Session.SessionKeys.Product) as IProduct;
            if (currentProd == null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(this.Key);
                throw new FisException("SFC002", errpara);
            }

            var checkFamily = (from item in checkFamilyList
                               where item.value.Equals(currentProd.Family)
                               select item).ToList();

            if (checkFamily.Count == 0)
            {
                return;
            }

            string cpuUid = (string)currentProd.GetExtendedProperty("CPUID");
            if (string.IsNullOrEmpty(cpuUid))
            {
                throw new FisException("CHK1035", new string[] { currentProd.ProId, "CPUID" });
            }

            getMySqlConnectionInfo(currentProd.Family);


            string dbCnStr = string.Format(this.MySQLConnectStr, mySqlIp, mySqlDb, mySqlUser, mySqlPwd);
            CurrentSession.AddValue("TabletSecureOn","Y");
            if (!getSecureFromMySQL(dbCnStr, secureSql, cpuUid, currentProd.CUSTSN))
            {
                if (this.IsThrowException)
                {
                    throw new FisException("CHK1029", new string[] { currentProd.CUSTSN, cpuUid });
                }
                else
                {
                    CurrentSession.AddValue("TabletSecureOn", "N");
                }
            }
           
        }


        private void checkMBSecureOn()
        {
            IMB currentMB = CurrentSession.GetValue(Session.SessionKeys.MB) as IMB;
            if (currentMB == null)
            {
                List<string> errpara = new List<string>();
                errpara.Add(this.Key);
                throw new FisException("SFC002", errpara);
            }
            
            string cpuUid = (string)currentMB.GetExtendedProperty("CPUID");
            if (string.IsNullOrEmpty(cpuUid))
            {
                if (this.IsThrowException)
                {
                    throw new FisException("CHK1035", new string[] { currentMB.Sn, "CPUID" });
                }
                return;
            }

            IFamilyRepository familyRep = RepositoryFactory.GetInstance().GetRepository<IFamilyRepository>();
          
            IList<FamilyInfoDef> FamilyInfoList= familyRep.GetExistFamilyInfo(new FamilyInfoDef() {
                                                                                                                             name="Category",
                                                                                                                              value="Tablet"
                                                                                                                        });

            CurrentSession.AddValue("TabletSecureOn", "Y");
            bool isSecureOn = false;
             foreach (FamilyInfoDef item in FamilyInfoList)
            {
                getMySqlConnectionInfo(item.family);                
                string dbCnStr = string.Format(this.MySQLConnectStr, mySqlIp, mySqlDb, mySqlUser, mySqlPwd);
                
                if (getSecureFromMySQL(dbCnStr, secureSql, cpuUid, ""))
                {
                    isSecureOn = true;
                    break;
                }
            }

             if (!isSecureOn)
             {
                 if (this.IsThrowException)
                 {
                     throw new FisException("CHK1029", new string[] { "", cpuUid });
                 }
                 else
                 {
                     CurrentSession.AddValue("TabletSecureOn", "N");
                 }
             }            

        }

	}
}
