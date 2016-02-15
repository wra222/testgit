using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
using IMES_Reports.App_Code;
using System.Diagnostics;
using Microsoft.Win32;
namespace IMES_Reports
{
    public partial class Fly_Form : Form
    {
         
        #region 全局變量聲明
        public static bool Check;
        private Point ptMouseCurrentPos, ptMouseNewPos, ptFormPos, ptFormNewPos;
        private bool blnMouseDown = false;
        Flash_Form Flash = new Flash_Form();
        Thread t1;
        Thread t2;
        Thread t3;
        GraphicsPath path;
        GraphicsPath path2;
        Graphics gp;
        GraphicsPath path3;
        GraphicsPath path4;
        public bool IsfromRegister = false;
        public bool Isasyn = false;
        private delegate void FlushClient();//代理
        AppHelper AH = new AppHelper();
        StringSprit StrSp = new StringSprit();
        string Flag = "";
        #endregion
        public Fly_Form()
        {
            InitializeComponent();
    
        }
        public void Fly_From_Load(object sender, EventArgs e)
        {

            if (Process_Check("IMES_Reports"))
            {
                MessageBox.Show("當前程式已開啓");
                Application.Exit();
            }
            else
            {
                this.Top = 100;
                this.Left = Screen.PrimaryScreen.Bounds.Width - 200;
                DataSet ds = new DataSet();
                #region 添加右鍵菜單
                ContextMenuStrip docMenu = new ContextMenuStrip();
                //ToolStripMenuItem OpenLable = new ToolStripMenuItem();
                //OpenLable.Text = "打开";
                //OpenLable.Click += new EventHandler(OpenLable_Click);
                if (!AH.IsActive_Check())
                {
                    ToolStripMenuItem Register = new ToolStripMenuItem();
                    Register.Text = "注册";
                    Register.Click += new EventHandler(Register_Click);
                    docMenu.Items.AddRange(new ToolStripMenuItem[] { Register });
                    WorkDone callBack = new WorkDone(WorkDoneHandler_forActiveCheck);
                    Working_2(callBack);
                    /*      if (AH.ReadConfig("appSettings", "TryOut") == "")
                          {
                              if (MessageBox.Show("您現在使用的版本未激活是否試用？", "系統提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                              {
                                  AH.WriteConfig("TryOut", "30");
                              }
                              else
                              {
                                  Application.Exit();
                              }
                          }
                          else 
                          {
                              MessageBox.Show("您還可試用：" + AH.ReadConfig("appSettings", "TryOut") + "次！");
                              int j;
                              int.TryParse(AH.ReadConfig("appSettings", "TryOut"), out j );
                              j -= 1;
                              AH.WriteConfig("TryOut", j.ToString());
                          }
                          */
                }

                ToolStripMenuItem ExitLable = new ToolStripMenuItem();
                ExitLable.Text = "退出";
                ExitLable.Click += new EventHandler(ExitLable_Click);
                //docMenu.Items.AddRange(new ToolStripMenuItem[] { OpenLable, ExitLable });
                //set Autostart
                if (!AH.IsAuto_Check())
                {
                    ToolStripMenuItem Autostart = new ToolStripMenuItem();
                    Autostart.Text = "自动启动";
                    Autostart.Click += new EventHandler(AutoStart_Click);
                    docMenu.Items.AddRange(new ToolStripMenuItem[] { Autostart });
                }
                else
                {
                    ToolStripMenuItem DisAutostart = new ToolStripMenuItem();
                    DisAutostart.Text = "关闭自动启动";
                    DisAutostart.Click += new EventHandler(DisAutoStart_Click);
                    docMenu.Items.AddRange(new ToolStripMenuItem[] { DisAutostart });
                }
                //docMenu.Items.AddRange(new ToolStripMenuItem[] { OpenLable, ExitLable });
                docMenu.Items.AddRange(new ToolStripMenuItem[] { ExitLable });
                this.ContextMenuStrip = docMenu;
                #endregion
                this.MouseHover += new EventHandler(Fly_From_MouseHover);
                this.MouseLeave += new EventHandler(Fly_From_MouseLeave);
                Flag = "1";
                Change_Message();
                AH.WriteConfig("IsOpen", "YES");
            }
            
        }
        public void ActiveCheckFunction()
        {
            if (!AH.IsActive_Check())
            {
                if (AH.ReadTryMessage("Tryout", "Tryout") == "")
                {
                    if (MessageBox.Show("您現在使用的版本未激活是否試用？", "系統提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        AH.WriteTryMessage("Tryout", "30", "Tryout");
                        MessageBox.Show("您還可試用：" + AH.ReadTryMessage("Tryout", "Tryout") + "次！");
                        if (this.t2.IsAlive)
                        {
                            if (t2.ThreadState.ToString().IndexOf("Suspended") == -1)
                                this.t2.Suspend();
                        }
                    }
                    else
                    {
                        AH.WriteConfig("IsOpen", "NO");
                        Application.Exit();
                    }
                }
                else
                {
                    int i;
                    int.TryParse(AH.ReadTryMessage("Tryout", "Tryout"), out i);
                    if (i == 0 || AH.IsTryOut())
                    {
                        try
                        {
                            try
                            {
                                RegistryKey UseTime_retkey = Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Inventec").OpenSubKey("IMES.INI").OpenSubKey("Tryout");

                                UseTime_retkey.SetValue("Tryout Times", "Run Out");

                            }
                            catch
                            {
                                RegistryKey retkey = Registry.CurrentUser.OpenSubKey("Software", true).CreateSubKey("Inventec").CreateSubKey("IMES.INI").CreateSubKey("Tryout");
                                retkey.SetValue("Tryout Times", "Run Out");
                            }
                            this.MouseHover -= new EventHandler(Fly_From_MouseHover);
                            this.MouseLeave -= new EventHandler(Fly_From_MouseLeave);
                            if (MessageBox.Show("您的試用次數已結束請聯繫IMES開發人員,或者您已有激活碼？", "系統警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {

                                ChangeForm Change_ToRester;
                                Change_ToRester = SwitchToReg;
                                this.BeginInvoke(Change_ToRester);
                                //Chang_ToRester();
                                if (!IsfromRegister)
                                {
                                    if (this.t2.IsAlive)
                                    {
                                        if (t2.ThreadState.ToString().IndexOf("Suspended") == -1)
                                            this.t2.Suspend();
                                    }
                                }
                            }
                            else if (!AH.IsActive_Check())
                            {
                                AH.WriteConfig("IsOpen", "NO");
                                Application.Exit();
                            }
                        }
                        catch (Exception e)
                        {
                            AH.WriteConfig("IsOpen", "NO");
                            Application.Exit();
                        }
                    }
                    else
                    {

                        if (MessageBox.Show("您還可試用：" + AH.ReadTryMessage("Tryout", "Tryout") + "次,是否激活？", "系統提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //子線程調用主線程的Form
                            ChangeForm Chang_ToRester;
                            Chang_ToRester = SwitchToReg;
                            this.BeginInvoke(Chang_ToRester);
                            //Chang_ToRester();
                            if (!IsfromRegister)
                            {
                                if (this.t2.IsAlive)
                                {
                                    if (t2.ThreadState.ToString().IndexOf("Suspended") == -1)
                                        this.t2.Suspend();
                                }
                            }

                        }
                        else
                        {
                            int j;
                            int.TryParse(AH.ReadTryMessage("Tryout", "Tryout"), out j);
                            j -= 1;
                            AH.WriteTryMessage("Tryout", j.ToString(), "Tryout");
                            //AH.WriteConfig("TryOut", j.ToString());
                            if (!IsfromRegister)
                            {
                                if (this.t2.IsAlive)
                                {
                                    if (t2.ThreadState.ToString().IndexOf("Suspended") ==-1)
                                       this.t2.Suspend();
                                }

                            }
                        }
                    }
       
                }
                
            }
        }
        void Fly_From_MouseLeave(object sender, EventArgs e)
        {
            //Flash.Hide();
            for (int i = 0; i < 1; i++)
            {
                delayTime(1);
                Application.DoEvents();
            }
             if (Flash_Form.IsShow)
            {
                Flash.Change_Status("Leave");
            }
        }
        private void frmTopMost_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (blnMouseDown)
            {
                //Get the current postion of the mouse in the screen.
                ptMouseNewPos = Control.MousePosition;

                //Set window postion.
                ptFormNewPos.X = ptMouseNewPos.X - ptMouseCurrentPos.X + ptFormPos.X;
                ptFormNewPos.Y = ptMouseNewPos.Y - ptMouseCurrentPos.Y + ptFormPos.Y;

                //Save window postion.
                Location = ptFormNewPos;
                ptFormPos = ptFormNewPos;

                //Save mouse pontion.
                ptMouseCurrentPos = ptMouseNewPos;
            }
        }
        private void frmTopMost_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                blnMouseDown = true;

                //Save window postion and mouse postion.
                ptMouseCurrentPos = Control.MousePosition;
                ptFormPos = Location;
            }
        }
        private void frmTopMost_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                blnMouseDown = false;
            }
        }
        private void OpenLable_Click(object sender, System.EventArgs e)
        {
            SwitchToMain();
        }
        private void Register_Click(object sender, System.EventArgs e)
        {
            SwitchToReg();
        }
        private void ExitLable_Click(object sender, System.EventArgs e)
        {
            //t1.Abort(this.t1);
            //t1.Abort(ApartmentState.STA);
            //t1 = null;

            //foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
            //{
            //    if (Thread.CurrentThread.ManagedThreadId == thread.Id) continue;//避免别的线程没结束，自己先被结束了
            //    TerminateThread(thread.Id, 0);
            //}
            AH.WriteConfig("IsOpen", "NO");
            Application.Exit();

        }
        private void AutoStart_Click(object sender, System.EventArgs e)
        {
            AH.SetAutoRun(Application.ExecutablePath, true);
            AH.WriteConfig("IsAuto", "YES");
            this.Invalidate();
            AH.WriteConfig("IsOpen", "NO");
            AH.WriteTryMessage("IsAuto", "YES", "IsAuto");
            Fly_From_Load( sender,  e);
        }
        private void DisAutoStart_Click(object sender, System.EventArgs e)
        {
            AH.SetAutoRun(Application.ExecutablePath, false);
            AH.WriteConfig("IsAuto", "");
            AH.WriteConfig("IsOpen", "NO");
            AH.WriteTryMessage("IsAuto", "NO", "IsAuto");
            Fly_From_Load(sender, e);
        }
        private void Fly_From_MouseHover(object sender, System.EventArgs e)
        {
            if (!Flash_Form.IsShow)
            {
                SwitchToFlash("Over");
            }
        }      
        private void SwitchToMain()
        {
            Main_From pParent = new Main_From();
            //pParent.RestoreWindow();
            pParent.RestoreWindow();
            this.Hide();
        }
        private void SwitchToReg()
        {
            Register_Form pParent = new Register_Form();
            //pParent.RestoreWindow();
            pParent.RestoreWindow();
            //this.Hide();
        }
        private void SwitchToFlash(string Tp)
        {
            if (Tp == "Over" && !Check)
            {
                Flash.Flash_Left = this.Left - 492;
                Flash.Flash_Top = this.Top + 30;
                Flash.Change_Status("Over");
             
            }
            else
            {
                Flash.Change_Status("Leave");
       
            }

            //Flash.Show();
           
            
        }
        //public void CloseFlash()
        //{
        //    Flash.Change_Status(); 
        //}
         private void SetWindowRegion()
        {         
            GraphicsPath FormPath = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width-83, this.Height+5 );
            FormPath = GetRoundedRectPath(rect, 45);
            this.Region = new Region(FormPath);      
        }
         private void GetRoundedRectPath_asyn( )
         {
              path3 = new GraphicsPath();
              path4 = new GraphicsPath();
             path3.AddEllipse(3, 3, 60, 60);
             path2.AddPath(path3, true);
             Rectangle rect2 = new Rectangle(60, 17, 120, 30);
             path4.AddRectangle(rect2);
             //右上角
             rect2.X = 50;
             path4.AddArc(rect2, 270, 0);
             ////右下角
             rect2.Y = 12;
             path4.AddArc(rect2, 0, 0);

             path2.AddPath(path4, true);
             path3.CloseFigure();
             Isasyn = true;
         }
        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {   
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            //GraphicsPath path = new GraphicsPath();
            path = new GraphicsPath();
            path2 = new GraphicsPath();
            ////左上角
            //path.AddArc(arcRect, 180, 90);
            ////右上角
            //arcRect.X = rect.Right - diameter;
            //path.AddArc(arcRect,270,90);
            ////右下角
            //arcRect.Y = rect.Bottom - diameter;
            //path.AddArc(arcRect,0,90);
            ////左下角
            //arcRect.X = rect.Left;
            //path.AddArc(arcRect,90,90);
            #region 添加扇形Example
            /*  
          private void AddPieExample(PaintEventArgs e)
            {
             
                    // Create a pie slice of a circle using the AddPie method.
                    GraphicsPath myPath = new GraphicsPath();
                    myPath.AddPie(20, 20, 70, 70, -45, 90);
                             
                    // Draw the path to the screen.
                    Pen myPen = new Pen(Color.Black, 2);
                    e.Graphics.DrawPath(myPen, myPath);
             }
        */
#endregion
            #region 添加string Example
            Rectangle rect1 = new Rectangle(60, 17, 115, 30);
              string stringText = "Sample Text";
              FontFamily family = new FontFamily("Arial");
              int fontStyle = (int)FontStyle.Italic;
              int emSize = 9;
              // path2.AddString(stringText, family, fontStyle, emSize, rect1, format);
              #endregion
              //Point origin = new Point(20, 20);

             StringFormat format = StringFormat.GenericDefault;
        
            path.AddEllipse( 0, 0, 65,65);
            //path2.AddPie(25, 18, 40, 30, -45, 70);
         
            path2.AddRectangle(rect1);
            path2.AddPath(path, true);
            path.CloseFigure();
            path2.CloseFigure();
            return path2;
        }
        private void Change_Show(string Title, string Rate)
        {
            ThreadPool.QueueUserWorkItem(h =>
            {
                delayTime(2);
                    L_Title.SafeCall((data) =>
                    {
                        L_Title.Text = data.ToString();
                    }, Title);
                    Thread.Sleep(100);
                           });
            ThreadPool.QueueUserWorkItem(h =>
            {
                delayTime(2);
                L_Rate.SafeCall((data) =>
                {
                    L_Rate.Text = data.ToString();
                }, Rate);
                Thread.Sleep(100);
            });
            WorkDone callBack = new WorkDone(WorkDoneHandler);
            Working(callBack);
            GetRoundedRectPath_asyn();

        }
      
       void Change_Message()
        {

                //Change_Show("出货达成率", "100%");
                delayTime(1);
                try
                {
   
                              IsInOANet IsOANet = new IsInOANet();
                            string Url = AH.ReadConfig("appSettings", "NetCheck");
                            if (IsOANet.IsCanConnect(Url))
                            {
                                string ComStr = "";
                                string ConStr = AH.ReadConfig("appSettings", "connectionstring");
                                if (Flag == "1")
                                {
                                    ComStr = AH.ReadConfig("appSettings", "Data_for_Delivery");
                                    if (AH.ReadConfig("appSettings", "Data_for_QC") != "")
                                    {
                                        Flag = "2";
                                    }
                                    else if (AH.ReadConfig("appSettings", "Data_for_WIP") != "")
                                    {
                                        Flag = "3";
                                    }
                                    else if (AH.ReadConfig("appSettings", "Data_for_Input") != "")
                                    {
                                        Flag = "4";
                                    }
                                }
                                else if (Flag == "2")
                                {
                                    ComStr = AH.ReadConfig("appSettings", "Data_for_QC");
                                    if (AH.ReadConfig("appSettings", "Data_for_WIP") != "")
                                    {
                                        Flag = "3";
                                    }
                                    else if (AH.ReadConfig("appSettings", "Data_for_Input") != "")
                                    {
                                        Flag = "4";
                                    }
                                    else
                                    {
                                        Flag = "1";
                                    }

                                }
                                else if (Flag == "3")
                                {
                                    ComStr = AH.ReadConfig("appSettings", "Data_for_WIP");

                                    if (AH.ReadConfig("appSettings", "Data_for_Input") != "")
                                    {
                                        Flag = "4";
                                    }
                                    else { Flag = "1"; }
                                }
                                else if (Flag == "4")
                                {
                                    ComStr = AH.ReadConfig("appSettings", "Data_for_Input");
                                    Flag = "1";
                                }

                                string[] resultStrSp = StrSp.StringSprint(ConStr, ComStr, "^");
                                Application.DoEvents();
                                string Rate = "";
                                string Title = "";
                                //Rate = (float.Parse(resultStrSp[0]) * 10).ToString() + "%";

                                Rate = resultStrSp[0].ToString() + "%";
                                if (resultStrSp[0].ToString() == "No Data")
                                {
                                    //MessageBox.Show(resultStrSp[0].ToString());
                                    Change_Show("No Data", "    0");
                                }
                                //Rate = (1 * 10).ToString() + "%";
                                else
                                {
                                    Title = resultStrSp[1];
                                    Change_Show(Title, Rate);
                                }
                            }
                            else
                            {
                                Change_Show("未連到IMESDB", "0");
                                //this.L_Title.Text = "未連到IMESDB";
                                //this.L_Rate.Text = "0";
                                MessageBox.Show("無法連接到OA 網絡請檢查網絡連接", "系統提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                if (t1 != null)
                                {
                                    if (t3 != null)
                                    {
                                        if (!t3.IsAlive)
                                            t3.Start();
                                        if (t3.ThreadState.ToString().IndexOf("Suspended") > 1)
                                            t3.Resume();
                                    }
                                    else
                                    {
                                        WorkDone callBack = new WorkDone(WorkDoneHandler_forNet);
                                        Working_3(callBack);
                                    }
                                    t1.Suspend();
                                }
                                else
                                {
                                    if (t3 != null)
                                    {
                                        if (!t3.IsAlive)
                                            t3.Start();
                                        if (t3.ThreadState.ToString().IndexOf("Suspended") > 1)
                                            t3.Resume();
                                    }
                                    else
                                    {
                                        WorkDone callBack = new WorkDone(WorkDoneHandler_forNet);
                                        Working_3(callBack);
                                    }
                                }

                                    
                              
                            }

                }
                catch(Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
               
               // Change_Show("出货达成率", "90%");


        }
protected override void OnResize(System.EventArgs e)
        {
            this.Region = null;
            SetWindowRegion();

        }
//private void CrossThreadFlush()
//{

//    FlushClient FlushClient = new FlushClient(Show);

//    FlushClient.BeginInvoke(null, null);
//}
//private void CrossThreadFlush()
//{
//    while (true)
//    {
//        //将sleep和无限循环放在等待异步的外面
//        Thread.Sleep(4000);
//        ThreadFunction();
//    }
//}
//private void ThreadFunction()
//{
//    //if (this.textBox1.InvokeRequired)//等待异步
//    //{
//        //FlushClient fc = new FlushClient(ThreadFunction);
//        //this.Invoke(fc);//通过代理调用刷新方法
//    //}
//    //else
//    //{
//    //    this.textBox1.Text = DateTime.Now.ToString();
//    //}
//    M_Show();
//}
//void M_Show()
//{
//    Change_Show("1234", "20%");
//}
#region 委託Example
delegate void ChangeForm();

delegate void WorkDone();
void Working(WorkDone callBack)
{
    //Working code.
    //当工作完成的时候执行这个委托.
    if (t1 != null)
    {
       
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
            Thread.Sleep(15000);
            try
            {
                Change_Message();
                //WorkDone callBack = new WorkDone(WorkDoneHandler);
                //Working(callBack);
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
void Working_2(WorkDone callBack)
{
    //Working code.
    //当工作完成的时候执行这个委托.
    if (t2 != null)
    {

        Thread.Sleep(5000);
    }
    else
    {
        t2 = new Thread(new ThreadStart(callBack));
        t2.IsBackground = true;
        t2.Start();
    }

}
void WorkDoneHandler_forActiveCheck()
{
    //Do something other.
    //Change_Message();
    while (true)
    {
        try
        {
            Thread.Sleep(5000);
            try
            {
                ActiveCheckFunction();
                //WorkDone callBack = new WorkDone(WorkDoneHandler);
                //Working(callBack);
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
void Working_3(WorkDone callBack)
{
    //Working code.
    //当工作完成的时候执行这个委托.
    if (t3 != null)
    {

        Thread.Sleep(5000);
    }
    else
    {
        t3 = new Thread(new ThreadStart(callBack));
        t3.IsBackground = true;
        t3.Start();
    }

}
void WorkDoneHandler_forNet()
{
    //Do something other.
    //Change_Message();
    while (true)
    {
        try
        {
            Thread.Sleep(5000);
            try
            {
               IsInOANet IsOANet = new IsInOANet();
                            string Url = AH.ReadConfig("appSettings", "NetCheck");
                            if (IsOANet.IsCanConnect(Url))
                            {
                               
                                if (t1 != null)
                                {
                                    //MessageBox.Show(t1.ThreadState.ToString());
                                    if (t1.ThreadState.ToString().IndexOf("Suspended")>1)
                                       t1.Resume();
                                }
                                else
                                {
                                    Change_Message();
                                }
                                t3.Suspend();
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
#endregion
/*        public void Change_Status(String Tp)
{

    // this.Show();
    if (!Fly_From.Check && Tp == "Over")
    {

        Flash.Left = this.Left - 284;
        Flash.Top = this.Top + 30;
        Flash_From.AnimateWindow(Flash.Handle, 300, Flash_From.AW_SLIDE + Flash_From.AW_VER_NEGATIVE);
        Flash.Show();
        Fly_From.Check = true;
        Flash_From.IsShow = true;
        this.TopMost = false;
        Flash.TopMost = true;
    }

    else
    {
        Flash_From.AnimateWindow(Flash.Handle, 300, Flash_From.AW_SLIDE + Flash_From.AW_VER_NEGATIVE + Flash_From.AW_HIDE);
        Flash.Hide();
        Fly_From.Check = false;
        Flash_From.IsShow = false;
    }
}
        */
/*
public void Change_Status_T(String Tp)
{

    // this.Show();
    if (!Fly_From.Check && Tp == "Over")
    {

        Flash.Left = this.Left - 284;
        Flash.Top = this.Top + 30;
        Flash_From.AnimateWindow(Flash.Handle, 300, Flash_From.AW_SLIDE + Flash_From.AW_VER_NEGATIVE);
        Flash.Show();
        Fly_From.Check = true;
        Flash_From.IsShow = true;
        this.TopMost = false;
        Flash.TopMost = true;
    }

    else
    {
        Flash_From.AnimateWindow(Flash.Handle, 300, Flash_From.AW_SLIDE + Flash_From.AW_VER_NEGATIVE + Flash_From.AW_HIDE);
        Flash.Hide();
        Fly_From.Check = false;
        Flash_From.IsShow = false;
    }
}
 */
#region
             /*
               private void CrossThreadFlush()
               {

                   FlushClient FlushClient = new FlushClient(Check_Flash_Mouse);

                   FlushClient.BeginInvoke(null, null);
               }

                delegate void WorkDone();
        void Working(WorkDone callBack)
        {
            //Working code.
            //当工作完成的时候执行这个委托.
            if (t1 != null)
            {
                t1.Start();
            }
            else
            {
                t1 = new Thread(new ThreadStart(callBack));
                t1.Start();
            }

        }
        void WorkDoneHandler()
        {
            //Do something other.
            object a = new object();
            CrossThreadFlush();
            //Check_Flash_Mouse();
        }
        public void Check_Flash_Mouse()
        {
            if (Flash_From.IsShow)
            {
                delayTime(2);
                //SwitchToFlash("Leave");
                Flash.Change_Status_T("Leave");
                //t1.Suspend();
                //t1.Abort();
                //t1 = null;
            }
            else
            {
               // t1.Suspend();
                //t1 = null;
                //t1.Abort();
              
            }
        }
        //void Do()
        //{
        //}
              */
        #endregion
        private void delayTime(double secend)
        {
            DateTime tempTime = DateTime.Now;

            while (tempTime.AddSeconds(secend).CompareTo(DateTime.Now) > 0)
            {
                ;
                //Application.DoEvents();

            }

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                Pen reds = new Pen(Color.Red, 2);
                Brush dsds = Brushes.Green;
                Brush dsds2 = Brushes.White;
                Brush dsds3 = Brushes.SeaGreen;
                base.OnPaint(e);
                gp = e.Graphics;
                gp.Clear(Color.Coral);

                gp.DrawPath(reds, path);
                gp.FillPath(dsds, path);
                gp.FillPath(dsds2, path2);
                if (Isasyn)
                {
                    gp.FillPath(dsds3, path3);
                    gp.FillPath(dsds, path4);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        public bool Process_Check(string Target)
        {
            bool result = false;
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName == Target && AH.ReadConfig("appSettings", "IsOpen") == "YES")
                {
                    result = true;
                    break;
                 
                }
                else
                {
                    result = false;
                    
                }
              
            }
            return result;
        }
        }
    
    
}
