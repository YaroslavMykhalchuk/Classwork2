using System.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ProductLibrary;

string serverIP = ConfigurationManager.AppSettings["ServerIP"];
string[] addressParts = serverIP.Split(":");
IPAddress address = IPAddress.Parse(addressParts[0]);
int port = Convert.ToInt32(addressParts[1]);

IPEndPoint endPoint = new IPEndPoint(address, port);

using (Socket server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP))
{
    server_socket.Bind(endPoint);
    server_socket.Listen(10);

    Console.WriteLine($"Server was started on {port} port.");

	try
	{
		while (true)
		{
			Socket client_socket = server_socket.Accept();
			Console.WriteLine(client_socket.RemoteEndPoint.ToString() + " has connected");
			byte[] buff = Encoding.Default.GetBytes(DataPackaging());
			client_socket.Send(buff);
			Console.WriteLine($"Data was sent to {client_socket.RemoteEndPoint}");
			client_socket.Shutdown(SocketShutdown.Both);
			client_socket.Close();
		}
	}
	catch (SocketException ex)
	{
		Console.WriteLine(ex.Message);
	}
}


string DataPackaging()
{
	return JsonSerializer.Serialize(new Product("Xiaomi Remdi Note 10 Pro", 8999, "Xiaomi"));
}