using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;


namespace HappyTwitchBot
{
    public class LED
    {
        public TcpClient tcpClient;         //tcpClient for TCP connection

        public string logfilepath;
        internal static string port = "80";
        internal static string ip = "127.0.0.1";
        internal static string postURL = "/body";
        internal static string R = "0";
        internal static string G = "0";
        internal static string B = "0";
        internal static string H = "127";
        internal static string led = "null";


        public LED()
        {

        }

        public LED(string ip, string port)
        {
            
            LED.ip = ip;
            LED.port = port;
            tcpClient = new TcpClient(ip, Int32.Parse(port));                       // create TCP Client

            
            logfilepath = Path.GetTempPath() + "_HappyTwitchBotLED.log";            //default log file path
        }


        async public void sendSettings()
        {
            string body = R + ',' + G + ',' + B + ',' + H + ',' + led;
            string postData = "POST " + postURL + " HTTP/1.0" + Environment.NewLine + "Host: " + LED.ip + Environment.NewLine + "Content-Type: text/plain" + Environment.NewLine + "Content-Length: " + body.Length + Environment.NewLine + Environment.NewLine + R + ',' + G + ',' + B + ',' + H + ',' + led + Environment.NewLine;
            byte[] postDataBinary = System.Text.Encoding.UTF8.GetBytes(postData);
                tcpClient.Client.Send(postDataBinary);

            byte[] bytes = new byte[1024];
            int lengthOfResponse = tcpClient.Client.Receive(bytes);

            string resp = System.Text.Encoding.UTF8.GetString(bytes, 0, lengthOfResponse);
        }
    }
}
