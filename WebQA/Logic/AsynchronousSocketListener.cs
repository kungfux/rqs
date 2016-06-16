/*   
 *  WebQA
 *  WebQA Server
 *  Copyright (C) Fuks Alexander 2013-2015
 *  
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *  
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *  
 *  You should have received a copy of the GNU General Public License along
 *  with this program; if not, write to the Free Software Foundation, Inc.,
 *  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
 *  
 *  Fuks Alexander, hereby disclaims all copyright
 *  interest in the program "WebQA"
 *  (which makes passes at compilers)
 *  written by Alexander Fuks.
 * 
 *  Alexander Fuks, 06 November 2013.
 */

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

namespace WebQA.Logic
{
    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder receivedData = new StringBuilder();
        // Short parsed url
        public string url = "";
    }

    public class AsynchronousSocketListener
    {
        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        // RQS instance
        RQS rqs = new RQS();

        public AsynchronousSocketListener()
        {
        }

        public void StartListening()
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(Program.WebQAaddress);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections
                    Trace.Instance.Add("Waiting...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Trace.Instance.Add("ERROR: " + e.ToString(), Trace.Color.Red);
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue
            allDone.Set();

            // Get the socket that handles the client request
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            string clientIp = handler.RemoteEndPoint.ToString();
            string clientName = Dns.GetHostEntry(
                        IPAddress.Parse(handler.RemoteEndPoint.ToString().Substring(0, handler.RemoteEndPoint.ToString().IndexOf(":")))).HostName;

            Trace.Instance.Add(
                string.Format("Client connected: {1} [{0}]", clientIp, clientName,
                Trace.Color.Yellow));

            // Read data from the client socket
            int bytesRead = handler.EndReceive(ar);

            // Store the data received
            state.receivedData.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

            int space1 = state.receivedData.ToString().IndexOf(" ");
            int space2 = 0;

            if (state.receivedData.Length > 1)
            {
                space2 = state.receivedData.ToString().IndexOf(" ", space1 + 1);
            }

            if (space1 < 0 ||
                state.receivedData.Length - space1 < 1 ||
                space2 - space1 - 2 < 0)
            {
                // if empty request or has no requested data
                Trace.Instance.Add("URL: Wrong request received", Trace.Color.Yellow);
                Send(handler, rqs.ProcessRequest(state.url).ToString());
                return;
            }

            state.url = state.receivedData.ToString().Substring(space1 + 2, space2 - space1 - 2);

            //// Store the data received
            //state.receivedData.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

            //string[] request = state.receivedData.ToString().Split(null);

            //if (request.Length < 2 || string.IsNullOrEmpty(request[1]))
            //{
            //    // if empty request or has no requested data
            //    Trace.Instance.Add("URL: Wrong or empty request received", Trace.Color.Yellow);
            //    Send(handler, rqs.ProcessRequest(state.url).ToString());
            //    return;
            //}

            //state.url = request[1];

            Trace.Instance.Add(
                string.Format("URL: {0}", state.url), Trace.Color.Yellow);

            // Call RQS instance to process request
            Send(handler, rqs.ProcessRequest(state.url).ToString());
        }

        private void Send(Socket handler, String data)
        {
            // Header
            string header = 
                    string.Concat(
                    "HTTP/1.1 200 OK\n",
                    "Content-type: text/html",
                    "\nContent-Encoding: 8bit",
                    "\nContent-Length:",
                    data.Length.ToString(),
                    "\n\n");
            // Convert the header and string data to byte
            byte[] byteData = Encoding.UTF8.GetBytes(header + data);
            // Begin sending the data to the remote device
            handler.BeginSend(byteData, 0, header.Length + data.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private byte[] StringToByte(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device
                int bytesSent = handler.EndSend(ar);
                Trace.Instance.Add(
                    string.Format("{0} bytes sent to client.", bytesSent));

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Trace.Instance.Add("EXCEPTION: " + e.ToString(), Trace.Color.Red);
            }
        }
    }
}