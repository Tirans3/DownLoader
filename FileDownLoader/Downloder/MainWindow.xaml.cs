using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace Downloder
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        string str;

        HttpClient client = new HttpClient();

        public bool flag = true;

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
                    using (var stream = await client.GetStreamAsync(str))
                    {

                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line;
                            StringBuilder outtext = new StringBuilder();
                            while ((line = await reader.ReadLineAsync()) != null)
                            {
                                if (flag == false) return;
                                if (Bar.Value == 100) Bar.Value = 0;

                                Bar.Value += 0.001;

                                outtext.Insert(outtext.Length, line);
                            }

                            Bar.Value = 100;

                            outbox.Text = outtext.ToString();

                        }

                    }
                }
                catch (HttpRequestException ex)
                {
                    outbox.Text = ex.Message;
                }
                catch (ArgumentException ex)
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            flag = false;

        }
    }
}
