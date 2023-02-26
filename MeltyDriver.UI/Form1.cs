using MeltyDriver.Service;
using System.Windows.Forms.VisualStyles;

namespace MeltyDriver.UI
{
    public partial class Form1 : Form
    {
        private string[] _availablePorts;

        public Form1()
        {
            InitializeComponent();

            // get available ports
            _availablePorts = SerialPortService.GetAvailablePorts();
            lbAvailablePorts.Items.AddRange(_availablePorts);
        }

        private void lbAvailablePorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lbAvailablePorts.SelectedIndex != -1) { 
                btnConnect.Enabled = true;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Melty not connected";
            lblStatus.BackColor = Color.Red;

            var result = SerialPortService.ConnectToPort(lbAvailablePorts.SelectedItem.ToString());

            if(result)
            {
                lblStatus.Text = "Connected!";
                lblStatus.BackColor = Color.Green;
            }
            else
            {
                lblStatus.Text = "Melty not connected";
                lblStatus.BackColor = Color.Red;
            }
        }
    }
}