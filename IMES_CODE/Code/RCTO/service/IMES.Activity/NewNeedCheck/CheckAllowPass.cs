using System.Workflow.ComponentModel;
using IMES.Infrastructure;
using IMES.FisObject.FA.Product;
using IMES.FisObject.PCA.MB;
using System.Data.SqlClient;
using System.Data;
using IMES.Infrastructure.Repository._Schema;
using IMES.Infrastructure.Extend;
using IMES.FisObject.Common.Station;

namespace IMES.Activity
{
    public partial class CheckAllowPass : BaseActivity
    {
        /// <summary>
        /// InitializeComponent
        /// </summary>
        public CheckAllowPass()
        {
            InitializeComponent();
        }

        /// <summary>
        /// DoExecute
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        protected internal override ActivityExecutionStatus DoExecute(ActivityExecutionContext executionContext)
        {
            //MB
            IMB mb = (IMB)CurrentSession.GetValue(Session.SessionKeys.MB);
            IProduct product = (IProduct)CurrentSession.GetValue(Session.SessionKeys.Product);
            string StatusStation = ""; //PCBStatus.Station


            if (mb == null && product == null)
            {
                FisException e1 = new FisException("CHK002", new string[] { });
                e1.stopWF = false;
                throw e1;
            }



            if (mb != null)
            {
                StatusStation = mb.MBStatus.Station;
                MBStatusEnum Status = mb.MBStatus.Status;

                string strSQL = "SELECT Station FROM StationAttr WHERE Station=@Station AND AttrName='AutoTest' AND AttrValue='Y' ";
                SqlParameter paraName = new SqlParameter("@Station", SqlDbType.VarChar, 32);
                paraName.Direction = ParameterDirection.Input;
                paraName.Value = this.Station;
                DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                        System.Data.CommandType.Text,
                                      strSQL, paraName);

                if (tb.Rows.Count > 0)
                {
                    CurrentSession.AddValue(ExtendSession.SessionKeys.AllowPass, "N");//不可直接輸入9999，要輸入DefectCode
                    CurrentSession.AddValue(ExtendSession.SessionKeys.DefectStation, this.Station);
                }
                else
                {
                    CurrentSession.AddValue(ExtendSession.SessionKeys.AllowPass, "Y");

                    if (Status == MBStatusEnum.Fail)// Check  PCBStatus.Status ==fail
                    {
                        CurrentSession.AddValue(ExtendSession.SessionKeys.DefectStation, StatusStation);
                    }
                    else
                    {
                        CurrentSession.AddValue(ExtendSession.SessionKeys.DefectStation, this.Station);
                    }
                }

                return base.DoExecute(executionContext);
            }


            //Product
            if (product != null)
            {
                StatusStation = product.Status.StationId;
                StationStatus Status = product.Status.Status;

                string strSQL = "SELECT Station FROM StationAttr WHERE Station=@Station AND AttrName='AutoTest' AND AttrValue='Y' ";
                SqlParameter paraName = new SqlParameter("@Station", SqlDbType.VarChar, 32);
                paraName.Direction = ParameterDirection.Input;
                paraName.Value = this.Station;
                DataTable tb = SqlHelper.ExecuteDataFill(SqlHelper.ConnectionString_GetData,
                                        System.Data.CommandType.Text,
                                      strSQL, paraName);

                if (tb.Rows.Count > 0)
                {
                    CurrentSession.AddValue(ExtendSession.SessionKeys.AllowPass, "N"); //不可直接輸入9999，要輸入DefectCode
                    CurrentSession.AddValue(ExtendSession.SessionKeys.DefectStation, this.Station);
                }
                else
                {
                    CurrentSession.AddValue(ExtendSession.SessionKeys.AllowPass, "Y");

                    if (Status == StationStatus.Fail)// Check  ProjectStatus.Status ==fail
                    {
                        CurrentSession.AddValue(ExtendSession.SessionKeys.DefectStation, StatusStation);
                    }
                    else
                    {
                        CurrentSession.AddValue(ExtendSession.SessionKeys.DefectStation, this.Station);
                    }
                }

            }
            return base.DoExecute(executionContext);
        }
    }

}
