using libplctag;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tag_Browser
{
    public partial class SettingsDialog : Form
    {
        public string GateWayIpAddress
        {
            get { return this.textBoxGwIp.Text; }
            set { this.textBoxGwIp.Text = value; }
        }

        public string CpuPath
        {
            get { return this.maskedTextBoxCpuPath.Text; }
            set { this.maskedTextBoxCpuPath.Text = value; }
        }

        public PlcType PlcType
        {
            get { return (PlcType)this.comboBoxPlcType.SelectedItem; }
            set { this.comboBoxPlcType.SelectedItem = value; }
        }

        public SettingsDialog()
        {
            InitializeComponent();

            foreach (var val in Enum.GetValues(typeof(PlcType)))
            {
                this.comboBoxPlcType.Items.Add(val);
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
