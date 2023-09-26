using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace TCPExample
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private NetworkStream stream;
        private bool isConnected = false;

        public MainWindow() => this.InitializeComponent();

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (this.isConnected)
            {
                Disconnect();
                ((Button)sender).Content = "Connect";
                return;
            }

            ((Button)sender).Content = "Disconnect";
            var serverIP = this.txtServerIP.Text;

            if (!int.TryParse(this.txtPort.Text, out var port) || port < 0 || port > 65535)
            {
                MessageBox.Show("Invalid port number.");
                return;
            }

            try
            {
                this.client = new TcpClient();
                this.client.Connect(serverIP, port);
                this.stream = this.client.GetStream();
                this.isConnected = true;

                // Start receiving data in a background thread
                this.StartReceiving();
                MessageBox.Show("Connected to the server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}");
            }
        }

        private void Disconnect()
        {
            if (this.client != null)
            {
                this.client.Close();
                this.client = null;
                this.isConnected = false;
                MessageBox.Show("Disconnected from the server.");
            }
        }

        private void StartReceiving()
        {
            var buffer = new byte[4096];
            var receivedData = new StringBuilder();
            var thread = new Thread(() =>
            {
                while (this.isConnected)
                {
                    try
                    {
                        var bytesRead = this.stream.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            // The server disconnected
                            this.Disconnect();
                            break;
                        }

                        // Process received data
                        var receivedText = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            // If in hex mode, convert bytes to hex
                            if (this.chkHex.IsChecked ?? false)
                            {
                                var hexData = new StringBuilder();
                                foreach (var b in buffer.Take(bytesRead))
                                {
                                    hexData.AppendFormat("{0:X2} ", b);
                                }
                                this.txtReceived.AppendText(hexData.ToString());
                            }
                            else
                            {
                                // Remove trailing null characters and append to receivedData
                                receivedData.Append(receivedText.TrimEnd('\0'));
                                this.txtReceived.AppendText(receivedText.TrimEnd('\0'));
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error while receiving data: {ex.Message}");
                        this.Disconnect();
                        break;
                    }
                }
            });
            thread.Start();
        }


        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (!this.isConnected)
            {
                MessageBox.Show("Not connected to the server.");
                return;
            }

            var sendData = this.txtSend.Text;

            if (string.IsNullOrWhiteSpace(sendData))
            {
                MessageBox.Show("Please enter data to send.");
                return;
            }

            try
            {
                var bytesToSend = this.chkHex.IsChecked ?? false ? this.HexStringToBytes(sendData) : Encoding.ASCII.GetBytes(sendData);
                this.stream.Write(bytesToSend, 0, bytesToSend.Length);
                this.stream.Flush();
                this.txtSend.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while sending data: {ex.Message}");
                this.Disconnect();
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e) => this.txtReceived.Clear();

        private byte[] HexStringToBytes(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            var bytes = new byte[hexString.Length / 2];

            for (var i = 0; i < hexString.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return bytes;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.Disconnect();
            base.OnClosing(e);
        }
    }
}
