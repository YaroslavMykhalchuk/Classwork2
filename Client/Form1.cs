using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using ProductLibrary;

namespace Client
{
    public partial class Form1 : Form
    {
        private readonly string connectionIP;
        private readonly string[] addressparts;
        public Form1()
        {
            InitializeComponent();

            connectionIP = ConfigurationManager.AppSettings["ServerIP"];
            addressparts = connectionIP.Split(":");
        }

        private void buttonReceive_Click(object sender, EventArgs e)
        {
            ConnectServer();
        }

        private void ConnectServer()
        {
            IPAddress address = IPAddress.Parse(addressparts[0]);
            IPEndPoint endPoint = new IPEndPoint(address, Convert.ToInt32(addressparts[1]));

            using (Socket client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP))
            {
                try
                {
                    client_socket.Connect(endPoint);

                    if (client_socket.Connected)
                    {
                        byte[] buff = new byte[1024];
                        int receiveBytes;
                        string convertedFromBytes;
                        Product receivedProduct;
                        do
                        {
                            receiveBytes = client_socket.Receive(buff);
                            convertedFromBytes = Encoding.Default.GetString(buff, 0, receiveBytes);
                            receivedProduct = JsonSerializer.Deserialize<Product>(convertedFromBytes);
                            if (textBox.Text != "")
                                textBox.Text += Environment.NewLine;
                            textBox.Text += $"Answer from server {client_socket.RemoteEndPoint}{Environment.NewLine}" +
                                $"Name: {receivedProduct.Name}{Environment.NewLine}Price: {receivedProduct.Price}{Environment.NewLine}" +
                                $"Manufacturer: {receivedProduct.Manufacturer}{Environment.NewLine}";
                        } while (client_socket.Available > 0);
                    }
                    else
                    {
                        MessageBox.Show("Error connection!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (SocketException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally { client_socket.Close(); }
            }
        }
    }
}