using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using IMES_Reports.App_Code;
namespace IMES_Reports
{
    public partial class Flash_Form : Form
    {
        public const Int32 AW_HOR_POSITIVE = 0x00000001;
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;
        public const Int32 AW_VER_POSITIVE = 0x00000004;
        public const Int32 AW_VER_NEGATIVE = 0x00000008;
        public const Int32 AW_CENTER = 0x00000010;
        public const Int32 AW_HIDE = 0x00010000;
        public const Int32 AW_ACTIVATE = 0x00020000;
        public const Int32 AW_SLIDE = 0x00040000;
        public const Int32 AW_BLEND = 0x00080000;
        public static bool IsShow;
        public int Flash_Left
        { get; set; }
        public int Flash_Top
        { get; set; }
       
        public Flash_Form()
        {
            InitializeComponent();
            this.Load += new EventHandler(Flash_From_Load);
            this.MouseLeave += new EventHandler(Flash_From_MouseLeave);
            this.MouseHover += new EventHandler(Flash_From_MouseHover);
            this.MouseMove += new MouseEventHandler(Flash_From_MouseMove);
            //this.Left = Flash_Left;
            //this.Top = Flash_Top;
            //SetWindowRegion();
        }

        void Flash_From_MouseMove(object sender, MouseEventArgs e)
        {
            IsShow = false;
        }

        void Flash_From_MouseHover(object sender, EventArgs e)
        {
            IsShow = false;
            Fly_Form.Check = true;
        }



        void Flash_From_MouseLeave(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 300, AW_SLIDE + AW_VER_NEGATIVE + AW_HIDE);
            this.Hide();
            Fly_Form.Check = false;
            IsShow = false;
        }

        void Flash_From_Load(object sender, EventArgs e)
        {
            Init_FlashForm();
        }
        void Init_FlashForm()
        {
            this.button1.Visible = false;
            this.BT_QC.Visible = false;
            this.button3.Visible = false;
            this.button4.Visible = false;
            AppHelper AH = new AppHelper();
            if (AH.Buttun_Check("Btn_For_Delivery"))
            {
                this.button1.Visible = true;
            }
            else
            {
                this.button1.Visible = false;
            }
            if (AH.Buttun_Check("Btn_For_QC"))
            {
                this.BT_QC.Visible = true;
                if (!AH.Buttun_Check("Btn_For_Delivery"))
                {
                    this.BT_QC.Location = new System.Drawing.Point(36, 25);
                    this.button3.Location = new System.Drawing.Point(133, 25);
                    this.button4.Location = new System.Drawing.Point(238, 25);
                }
                else
                {
                    this.button1.Location = new System.Drawing.Point(36, 25);
                    this.BT_QC.Location = new System.Drawing.Point(133, 25);
                    this.button3.Location = new System.Drawing.Point(238, 25);
                    this.button4.Location = new System.Drawing.Point(347, 25);
                }
            }
            else
            {
                this.BT_QC.Visible = false;
            }
            if (AH.Buttun_Check("Btn_For_Product"))
            {
                this.button3.Visible = true;
                if (!AH.Buttun_Check("Btn_For_Delivery") && !AH.Buttun_Check("Btn_For_QC"))
                {
                    this.button3.Location = new System.Drawing.Point(36, 25);
                   // this.button4.Location = new System.Drawing.Point(133, 25);
                    //this.button4.Location = new System.Drawing.Point(238, 25);
                }
                else if (!AH.Buttun_Check("Btn_For_Delivery"))
                {
                   // this.BT_QC.Location = new System.Drawing.Point(36, 25);
                    this.button3.Location = new System.Drawing.Point(133, 25);
                    //this.button4.Location = new System.Drawing.Point(238, 25);
                }
                else if (!AH.Buttun_Check("Btn_For_QC"))
                {
                    //this.button3.Location = new System.Drawing.Point(36, 25);
                    this.button3.Location = new System.Drawing.Point(133, 25);
                   // this.button4.Location = new System.Drawing.Point(238, 25);
                }
                //else
                //{
                //    this.button1.Location = new System.Drawing.Point(36, 25);
                //    this.BT_QC.Location = new System.Drawing.Point(133, 25);
                //    this.button3.Location = new System.Drawing.Point(238, 25);
                //    this.button4.Location = new System.Drawing.Point(347, 25);
                //}
            }
            else
            {
                this.button3.Visible = false;
            }
            if (AH.Buttun_Check("Btn_For_WIP"))
            {
                this.button4.Visible = true;
                if (!AH.Buttun_Check("Btn_For_Delivery") && !AH.Buttun_Check("Btn_For_QC") && !AH.Buttun_Check("Btn_For_Product"))
                {
                    this.button4.Location = new System.Drawing.Point(36, 25);
                    //this.button4.Location = new System.Drawing.Point(133, 25);
                    //this.button4.Location = new System.Drawing.Point(238, 25);
                }
                else if (!AH.Buttun_Check("Btn_For_Delivery") && !AH.Buttun_Check("Btn_For_QC"))
                {
                    //this.button3.Location = new System.Drawing.Point(36, 25);
                    this.button4.Location = new System.Drawing.Point(133, 25);
                    //this.button4.Location = new System.Drawing.Point(238, 25);
                }
                else if (!AH.Buttun_Check("Btn_For_Delivery") && !AH.Buttun_Check("Btn_For_Product"))
                {
                   // this.button4.Location = new System.Drawing.Point(36, 25);
                    this.button4.Location = new System.Drawing.Point(133, 25);
                    //this.button4.Location = new System.Drawing.Point(238, 25);
                }
                else if (!AH.Buttun_Check("Btn_For_QC") && !AH.Buttun_Check("Btn_For_Product"))
                {
                   // this.button4.Location = new System.Drawing.Point(36, 25);
                     this.button4.Location = new System.Drawing.Point(133, 25);
                   // this.button4.Location = new System.Drawing.Point(238, 25);
                }
                else if (!AH.Buttun_Check("Btn_For_QC") || !AH.Buttun_Check("Btn_For_Product") || !AH.Buttun_Check("Btn_For_Delivery"))
                {
                    // this.button4.Location = new System.Drawing.Point(36, 25);
                    //this.button4.Location = new System.Drawing.Point(133, 25);
                     this.button4.Location = new System.Drawing.Point(238, 25);
                }
                //else
                //{
                //    this.button1.Location = new System.Drawing.Point(36, 25);
                //    this.BT_QC.Location = new System.Drawing.Point(133, 25);
                //    this.button3.Location = new System.Drawing.Point(238, 25);
                //    this.button4.Location = new System.Drawing.Point(347, 25);
                //}
            }
            else
            {
                this.button4.Visible = false;
            }
        }
        /*
        public void Change_Status(String Tp)
        {

           // this.Show();
            if (!Fly_From.Check && Tp=="Over")
            {
                this.Left = Flash_Left;
                this.Top = Flash_Top;
                AnimateWindow(this.Handle, 300, AW_SLIDE + AW_VER_NEGATIVE);
                this.Show();
                Fly_From.Check = true;
                IsShow = true;
            }

            else
            {
                AnimateWindow(this.Handle, 300, AW_SLIDE + AW_VER_NEGATIVE + AW_HIDE);
                this.Hide();
                Fly_From.Check = false;
                IsShow = false;
            }
        }
         */
        public void Change_Status_T(String Tp)
        {
            //Init_FlashForm();
            // this.Show();
            if (!Fly_Form.Check && Tp == "Over")
            {
                
                //InitializeComponent();
                this.Left = Flash_Left;
                this.Top = Flash_Top;
                Flash_Form.AnimateWindow(this.Handle, 300, Flash_Form.AW_SLIDE + Flash_Form.AW_VER_NEGATIVE);
                this.Show();
              
                Fly_Form.Check = true;
                Flash_Form.IsShow = true;
                this.TopMost = false;
                this.TopMost = true;
            }

            else
            {
                Flash_Form.AnimateWindow(this.Handle, 300, Flash_Form.AW_SLIDE + Flash_Form.AW_VER_NEGATIVE + Flash_Form.AW_HIDE);
                this.Hide();
                Fly_Form.Check = false;
                Flash_Form.IsShow = false;
                
            }
        
          //  Flash_From_Load(object sender, EventArgs e);
        }
        public void Change_Status(String Tp)
        {
            //Init_FlashForm();
            try
            {
            // this.Show();
            if (!Fly_Form.Check && Tp == "Over")
            {
                Init_FlashForm();
                //InitializeComponent();
                //this.Left = this.Left - 284;
                //this.Top = this.Top + 30;
                this.Left = Flash_Left;
                this.Top = Flash_Top;
                Flash_Form.AnimateWindow(this.Handle, 300, Flash_Form.AW_SLIDE + Flash_Form.AW_VER_NEGATIVE);
                this.Show();
                
                Fly_Form.Check = true;
                Flash_Form.IsShow = true;
                this.TopMost = false;
                this.TopMost = true;
                this.ShowInTaskbar = false;
            }

            else
            {
                Flash_Form.AnimateWindow(this.Handle, 300, Flash_Form.AW_SLIDE + Flash_Form.AW_VER_NEGATIVE + Flash_Form.AW_HIDE);
                this.Hide();
                Fly_Form.Check = false;
                Flash_Form.IsShow = false;
            }
        }
    catch(Exception ex)
        {}
          
        }

        [DllImportAttribute("user32.dll")]
        //private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            AnimateWindow(this.Handle, 300, AW_SLIDE + AW_VER_NEGATIVE + AW_HIDE);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main_From pParent = new Main_From();
            //pParent.RestoreWindow();
            pParent.RestoreWindow();
        }
        private void SetWindowRegion()
        {
            GraphicsPath FormPath = new GraphicsPath();
            Rectangle rect = new Rectangle(347, 175, 50, 50);
            FormPath = GetRoundedRectPath(rect, 45);
            this.Region = new Region(FormPath);
        }
        private GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius;
            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();
            //GraphicsPath path2 = new GraphicsPath();
            //左上角
            path.AddArc(arcRect, 180, 90);
            //右上角
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);
            //右下角
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);
            //左下角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);

            //Point origin = new Point(20, 20);

           

           // path.AddEllipse(0, 0, 55, 55);
            //path2.AddPie(25, 18, 40, 30, -45, 70);

           // path.AddRectangle(rect1);
           
            path.CloseFigure();
            path.CloseFigure();
            return path;
        }

        private void BT_AddButtun_Click(object sender, EventArgs e)
        {
            Add_new AN = new Add_new();
            AN.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            QC_Reports pParent = new QC_Reports();
            //pParent.RestoreWindow();
            pParent.RestoreWindow();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Input_Output pParent = new Input_Output();
            //pParent.RestoreWindow();
            pParent.RestoreWindow();
        }

    }
}
