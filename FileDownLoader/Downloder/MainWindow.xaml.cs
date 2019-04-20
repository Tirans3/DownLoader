using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Downloder
{
   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        string str;
        HttpClient client;
        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            Download.IsEnabled = false;
            str = Inputurl.Text;
            if (str == string.Empty || !Uri.IsWellFormedUriString(str, UriKind.RelativeOrAbsolute))
            {
                MessageBox.Show("Enter valid url");

                Download.IsEnabled = true;

            }
            else
            {
                try
                {
                    HttpClient client = new HttpClient();
                    using (Stream stream = await client.GetStreamAsync(str))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line;
                            while ((line = await reader.ReadLineAsync()) != null)
                            {
                                if (Bar.Value == 100) Bar.Value = 0;
                                
                                Bar.Value += 0.001;

                            }

                            Bar.Value = 100;

                        }

                    }
                }
                catch(HttpRequestException ex )
                {
                    outbox.Text = ex.Message;
                }
                catch(ArgumentException ex)
                {
                    outbox.Text = ex.Message;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Download.IsEnabled = true;
                }
            }
        }
    }
}
