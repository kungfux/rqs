/*   
 *  WebQA
 *  WebQA Server
 *  Copyright (C) Fuks Alexander 2013
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
                    Trace.Add("Waiting...", Trace.Color.Green);
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Trace.Add("ERROR: " + e.ToString(), Trace.Color.Red);
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

            Trace.Add(
                string.Format("Client connected: {1} [{0}]", 
                    handler.RemoteEndPoint.ToString(),
                    Dns.GetHostByAddress(
                        IPAddress.Parse(handler.RemoteEndPoint.ToString().Substring(0,handler.RemoteEndPoint.ToString().IndexOf(":")))).HostName), 
                Trace.Color.Yellow);

            // Read data from the client socket
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far
                state.receivedData.Append(Encoding.UTF8.GetString(
                    state.buffer, 0, bytesRead));

                int space1 = state.receivedData.ToString().IndexOf(" ");
                int space2 = state.receivedData.ToString().IndexOf(" ", space1 + 1);
                state.url = state.receivedData.ToString().Substring(space1 + 2, space2 - space1 - 2);

                Trace.Add(
                    string.Format("URL: {0}", state.url), Trace.Color.Yellow);

                if (state.url.StartsWith("favicon.ico"))
                {

                }
                else
                {
                    // Call RQS instance to process request
                    Send(handler, rqs.ProcessRequest(state.url).ToString());
                }

                //if (content.IndexOf("<EOF>") > -1)
                //{
                //    // All the data has been read from the 
                //    // client. Display it on the console.
                //    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                //        content.Length, content);
                //    // Echo the data back to the client.
                //    Send(handler, content);
                //}
                //else
                //{
                //    // Not all data received. Get more.
                //    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                //    new AsyncCallback(ReadCallback), state);
                //}
            }
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

            //int sent = 0;
            //while (sent < data.Length)
            //{
            //    handler.Send(StringToByte(data.Substring(sent, sent < data.Length ? 1024 : data.Length - sent)), SocketFlags.Truncated);
            //    sent += 1024;
            //}
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
                Trace.Add(
                    string.Format("{0} bytes sent to client.", bytesSent), Trace.Color.Green);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Trace.Add("EXCEPTION: " + e.ToString(), Trace.Color.Red);
            }
        }

        //public string GetContent(string file_path)
        //{
        //    string ext = "";
        //    int dot = file_path.LastIndexOf(".");
        //    if (dot >= 0)
        //        ext = file_path.Substring(dot, file_path.Length - dot).ToUpper();
        //    if (Contents[ext] == null)
        //        return "application/" + ext;
        //    else
        //        return (string)Contents[ext];
        //}

        //public void WriteHeaderToClient(string content_type, long length)
        //{
        //    string str = "HTTP/1.1 200 OK\nContent-type: " + content_type
        //           + "\nContent-Encoding: 8bit\nContent-Length:" + length.ToString()
        //           + "\n\n";
        //    Client.GetStream().Write(Encoding.ASCII.GetBytes(str), 0, str.Length);
        //}

        //public void WriteToClient(string request)
        //{
        //    string file_path = GetPath(request);
        //    if (file_path.IndexOf("..") >= 0 || !File.Exists(file_path))
        //    {
        //        Error_Message = "Source file not found on server side.\r\nStack trace: " + file_path + " not found.";
        //        WriteHeaderToClient("text/plain", Error_Message.Length);
        //        Client.GetStream().Write(Encoding.ASCII.GetBytes(Error_Message), 0, Error_Message.Length);
        //        return;
        //    }
        //    FileStream file = File.Open(file_path, FileMode.Open);
        //    WriteHeaderToClient(GetContent(file_path), file.Length);
        //    byte[] buf = new byte[1024];
        //    int len;
        //    while ((len = file.Read(buf, 0, 1024)) != 0)
        //        Client.GetStream().Write(buf, 0, len);
        //    file.Close();
        //    if (file_path.Contains("~tmp"))
        //    {
        //        File.Delete(file_path);
        //    }
        //}
    }
}