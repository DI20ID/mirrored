using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace HappyTwitchBot
{

    
    public class XCOMTcpClient
    {

        private string message;
        
        public XCOMTcpClient(string messagegot)
        {
            message = messagegot;
        }
        
        public async Task<string> send_message_to_XCOM_Thread( )
        {
            String responseData = String.Empty;

            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 3742;
                TcpClient xclient = new TcpClient("localhost", port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = xclient.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Debug.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Debug.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                xclient.Close();
            }
            catch (ArgumentNullException e)
            {
                Debug.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Debug.WriteLine("SocketException: {0}", e);

                return "";

            }
            return responseData;
        }
    }
}
