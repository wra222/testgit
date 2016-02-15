using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using IMES_Reports.App_Code;
using System.Threading;
namespace IMES_Reports
{
    
    public partial class Register_Form : Form
    {
        Thread t1;
        AppHelper AH = new AppHelper();
        public Register_Form()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(Register_From_FormClosed);
        }
        public static bool state = true;  //软件是否为可用状态
        SoftReg softReg = new SoftReg();
        private void btnClose_Click(object sender, EventArgs e)
        {
            //Fly_From fly = new Fly_From();
            //fly.Show();
            this.Close();
            //if (state == true)
            //{
            //    this.Close();
            //}
            //else
            //{
            //    Application.Exit();
            //}
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtRNum.Text == softReg.GetRNum())
                {
                    //string Result=softReg.GetCheckCode(txtRNum.Text);
                    //MessageBox.Show(Result);
                    MessageBox.Show("注册成功！重启软件后生效！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    try
                    {
                        RegistryKey retkey = Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Inventec").OpenSubKey("IMES.INI").OpenSubKey("IsActive");
                        retkey.SetValue("UserName", txtRNum.Text);
                    }
                    catch
                    {
                        RegistryKey retkey = Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("Inventec").CreateSubKey("IMES.INI").CreateSubKey("IsActive");
                        retkey.SetValue("UserName", txtRNum.Text);
                    }
                    //string check = AH.WriteConfig("IsActive", "YES");
                    AH.WriteTryMessage("IsActive", "YES", "IsActive");
                    this.Close();
                    Application.Exit();
                }
                else
                {
                    //MessageBox.Show(softReg.GetRNum());
                    MessageBox.Show("注册码错误！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRNum.SelectAll();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void RestoreWindow()
        {
            //this.WindowState = ;
            this.Show();
            this.ShowInTaskbar = true;
        }
        private void frmRegisterForm_Load(object sender, EventArgs e)
        {
            //this.txtMNum.Text = softReg.GetMNum();
        }
        private void Register_From_FormClosed(object sender, EventArgs e)
        {

            WorkDone callBack = new WorkDone(WorkDoneHandler);
            Working(callBack);
        }
        delegate void WorkDone();
        void Working(WorkDone callBack)
        {
            //Working code.
            //当工作完成的时候执行这个委托.
            if (t1 != null)
            {
               // t1.Abort();
                ;
            }
            else
            {
                t1 = new Thread(new ThreadStart(callBack));
                t1.IsBackground = true;
                t1.Start();
            }

        }
        void WorkDoneHandler()
        {
            //Do something other.
            //Change_Message();
            while (true)
            {
                try
                {
                    
                    try
                    {
                        Fly_Form fly = new Fly_Form();
                        //fly.Show();
                        fly.IsfromRegister = true;
                        fly.ActiveCheckFunction();
                        //WorkDone callBack = new WorkDone(WorkDoneHandler);
                        //Working(callBack);
                        if (t1.IsAlive)
                        {
                            t1.Suspend();
                        }
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        }

    }
}
