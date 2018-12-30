using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSUI.Dialogs
{
    public partial class frmConfirm : Form
    {

        public frmConfirm(string title, string message)
        {
            InitializeComponent();
            lblControl.Text = message;
            lblTitle.Text = title;
        }

     
        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }
    }
}
