using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SampleClient.SampleService;

namespace SampleClient
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnUnhandled_Click(object sender, EventArgs e)
        {
            using (ErroringServiceClient client = new ErroringServiceClient())
            {
                client.ThrowUnhandledException();
                client.Close();
            }
        }

        private void btnHandled_Click(object sender, EventArgs e)
        {
            using (ErroringServiceClient client = new ErroringServiceClient())
            {
                client.LogHandledException();
                client.Close();
            }
        }
    }
}