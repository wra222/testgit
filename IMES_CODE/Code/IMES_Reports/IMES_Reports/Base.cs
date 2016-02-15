using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IMES_Reports
{
    public partial class Base : Form
    {

        Flash_Form Flash = new Flash_Form();
        Fly_Form fm = new Fly_Form();

        public Base()
        {
            InitializeComponent();
            this.Top = 10;
            this.Left = Screen.PrimaryScreen.Bounds.Width - 1000;
            //Flash_IsSHow = false;
            
        }
        void Base_Load(object sender, EventArgs e)
        {
           
        }

        void Base_MouseHover(object sender, EventArgs e)
        {
            if (Fly_Form.Check)
            {
               // fm.CloseFlash();
                
            }

        }
    }
}
