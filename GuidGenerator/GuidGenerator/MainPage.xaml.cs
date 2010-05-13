using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using GuidGenerator.Cryptography;

namespace GuidGenerator
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnGUID_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtInput.Text))
            {
                txtGUID.Text = Guid.NewGuid().ToString();
            }
            else
            {
                txtGUID.Text = GetDeterministicGuid(txtInput.Text).ToString();
            }
            Clipboard.SetText(txtGUID.Text);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Text = txtGUID.Text = string.Empty;
        }

        private Guid GetDeterministicGuid(string input)
        {
            var inputBytes = Encoding.Unicode.GetBytes(input);
            var hashBytes = MD5HashProvider.ComputeHash(inputBytes);
            Guid hashGuid = new Guid(hashBytes);
            return hashGuid;
        }
    }
}
