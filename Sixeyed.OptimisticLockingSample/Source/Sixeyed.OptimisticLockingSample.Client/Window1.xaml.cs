using System;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Sixeyed.OptimisticLockingSample.Client.CustomerService;
using Sixeyed.OptimisticLockingSample.ServiceModel.FaultDetail;
using Sixeyed.OptimisticLockingSample.ServiceModel.Helpers;

namespace Sixeyed.OptimisticLockingSample.Client
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private Customer CurrentCustomer { get; set; }

        public Window1()
        {
            InitializeComponent();
        }

        private BitmapImage ImageFromBuffer(Byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        private byte[] BufferFromImage(BitmapImage imageSource)
        {
            Stream stream = imageSource.StreamSource;
            if (stream == null && imageSource.UriSource != null && imageSource.UriSource.Scheme == "file")
            {
                stream = new FileStream(imageSource.UriSource.LocalPath, FileMode.Open, FileAccess.Read);
            }
            byte[] buffer = null;
            if (stream != null && stream.Length > 0)
            {
                stream.Position = 0;
                using (BinaryReader br = new BinaryReader(stream))
                {
                    buffer = br.ReadBytes((Int32)stream.Length);
                }
            }
            return buffer;
        }


        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            using (CustomerServiceClient serviceClient = new CustomerServiceClient())
            {
                serviceClient.Open();

                Customer customer = serviceClient.GetCustomer(int.Parse(this.txtId.Text));
                this.txtName.Text = customer.Name;
                this.txtStartDate.Text = customer.StartDate.ToShortDateString();
                this.txtCreditLimit.Text = customer.CreditLimit.ToString();
                this.imgLogo.Source = ImageFromBuffer(customer.Logo);

                IExtensibleDataObject extendedCustomer = customer as IExtensibleDataObject;
                if (extendedCustomer != null)
                {
                    //ES - ordinarily the data signature is hidden from the client, 
                    //and cannot be modified; access with reflection to display it:
                    object dataSiganture = ExtensionDataHelper.GetExtensionDataMemberValue(extendedCustomer, "dataSignature");
                    this.lblDataSignature.Content = string.Format("Data Signature: {0}", dataSiganture);
                }

                this.CurrentCustomer = customer;

                serviceClient.Close();
            }
        }

        private void btnLogo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.DefaultExt = ".png";
            openDialog.Filter = "Portable Network Graphics (.png)|*.png";
            bool? result = openDialog.ShowDialog();
            if (result == true)
            {
                this.imgLogo.Source = new BitmapImage(new Uri(openDialog.FileName));
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //use the loaded customer object to preserve data signature:
            Customer customer = this.CurrentCustomer;
            //update fields:
            customer.Id = int.Parse(this.txtId.Text);
            customer.Name = this.txtName.Text;
            customer.StartDate = Convert.ToDateTime(this.txtStartDate.Text);
            customer.CreditLimit = Convert.ToSingle(this.txtCreditLimit.Text);
            BitmapImage image = this.imgLogo.Source as BitmapImage;
            customer.Logo = BufferFromImage(image);

            using (CustomerServiceClient serviceClient = new CustomerServiceClient())
            {
                serviceClient.Open();
                try
                {
                    serviceClient.UpdateCustomer(customer);
                    MessageBox.Show("Changes saved");
                    //reload:
                    this.btnLoad_Click(null, null);
                }
                catch (FaultException<NoDataSignature> noSignatureException)
                {
                    MessageBox.Show("Data signature not found. Changes not saved", "Concurrency Violation");
                }
                catch (FaultException<ConcurrencyViolation> concurrencyViolationException)
                {
                    MessageBox.Show("Record has been updated by another user. Changes not saved", "Concurrency Violation");
                }                
                serviceClient.Close();
            }
        }
    }
}
