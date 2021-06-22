using SagaBNS.Core;
using SagaBNS.Core.Cryptography;
using SagaBNS.Core.Handling;
using SagaBNS.Core.Serializing;
using SagaBNS.DTO;
using SagaBNS.Enums;
using SagaBNS.Networking;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace SagaBNS.GameObject.Networking
{
    public class ClientSession
    {
        #region Members

        private readonly NetworkManager _manager;

        private readonly SocketSession _socket;

        #endregion

        #region Instantiation

        public ClientSession(SocketSession socket, NetworkManager networkManager)
        {
            _socket = socket;
            _manager = networkManager;
        }

        #endregion

        #region Properties

        // this should be a game object
        public AccountDTO Account { get; set; }

        // this should be internal id not connected to socket in any matter
        public int ClientId { get; private set; }

        public RC4 CryptIn { get; set; }

        public RC4 CryptOut { get; set; }

        public int LastRequestId { get; set; }

        /// <summary>
        /// Gets or sets the complete command name that is currently awaiting for a body packet.
        /// </summary>
        public string PendingCommand { get; set; }

        public string IPAddress => (_socket.Socket.RemoteEndPoint as IPEndPoint)?.Address.ToString();

        public bool Ready { get; set; }

        public long SessionId => _socket.SessionId;

        public SessionState SessionState { get; set; }

        public SRP6 SRP { get; set; }

        #endregion

        #region Methods

        public void Destroy()
        {
            // dispose here. we should remove any unnecessary data for client etc for proper disposal.
            _manager.DeleteSession(SessionId);
        }

        public void HandlePacket(MemoryStream packet)
        {
            CryptIn?.EncryptBuffer(packet.GetBuffer(), 0, packet.Length);

            // temporary logging
            //if (Ready)
            //{
            //    byte[] buf = packet.GetBuffer();
            //    string handshake = Encoding.UTF8.GetString(buf);
            //    Logger.Debug(handshake);
            //}

            /* The protocol is basically HTTP and respects the rules of HTTP/1.1
             * I'm trying to make this as simple as possible and the code is trying to detect errors.
             * instead of attempting to handle invalid or corrupted data.
             * I am not absolutely certain of the encoding used.
             */
            using (StreamReader reader = new StreamReader(packet, Encoding.UTF8))
            {
                if (PendingCommand == null)
                {
                    while (reader.Peek() > 0)
                    {
                        string[] requestLine = reader.ReadLine().Split(' ');
                        if (requestLine.Length == 3)
                        {
                            // we will somehow need to decrypt the header for game and lobby,
                            // unless what i noticed is true that packets seem to get corrupted, or are held in a stream
                            // that corrupts itself when next packets get added to it soo we shouldnt be really bothered by it.
                            string method = requestLine[0];

                            if (method == "POST")
                            {
                                string command = requestLine[1];
                                string type = requestLine[2];
                                string log = $"Recieve: {method} {command} {type}";

                                // get headers, there may be more.
                                int l = 0;
                                int s = 0; // "status" index of message

                                string line = reader.ReadLine();
                                while (!string.IsNullOrEmpty(line) && !reader.EndOfStream)
                                {
                                    string[] header = line.Split(new char[] { ':' }, 2);

                                    // we can log additional data about lines and status of packet
                                    //log += "\n" + header[0] + ":" + header[1];

                                    switch (header[0])
                                    {
                                        case "l":
                                            l = int.Parse(header[1]);
                                            break;

                                        case "s":

                                            // NOTE: Could this be more than just a number?
                                            //       Server sends #R.
                                            s = int.Parse(header[1]);
                                            break;

                                        default:
                                            Logger.Warn($"Unknown header in message: {line}");
                                            break;
                                    }

                                    line = reader.ReadLine();
                                }

                                Logger.Debug(log);

                                LastRequestId = s;

                                char[] buffer = new char[l];
                                reader.Read(buffer, 0, l);
                                StreamReader content = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(buffer)), Encoding.UTF8);
                                DispatchCommand(command, content);
                            }
                            else
                            {
                                Logger.Warn($"Server received unhandled request method: {method}");
                            }
                        }
                        else
                        {
                            Logger.Warn($"Server received invalid request line: {requestLine}");
                        }
                    }
                }
                else
                {
                    DispatchCommand(PendingCommand, reader);
                    PendingCommand = null;
                }
            }
        }

        public void SendErrorReply(string errorType, string errorData)
        {
            using (MemoryStream packetStream = new MemoryStream())
            using (TextWriter writer = new StreamWriter(packetStream))
            {
                writer.WriteLine("STS/1.0 400 {0}", errorType);
                writer.WriteLine("l:{0}", errorData.Length + 1);
                writer.WriteLine("s:{0}", LastRequestId); // we may change this system eventually.
                writer.WriteLine();
                writer.Write(errorData);
                writer.Write('\n');
                writer.Flush();

                // test output
                //Logger.Debug("Packet Sent: " + Encoding.UTF8.GetString(packetStream.GetBuffer()));

                _manager.DistributePacket(SessionId, packetStream);
            }
        }

        public void SendFormattedReply(string header, string data)
        {
            using (MemoryStream packetStream = new MemoryStream())
            using (TextWriter writer = new StreamWriter(packetStream))
            {
                writer.WriteLine(header);
                writer.WriteLine("l:{0}", data.Length + 1);
                writer.WriteLine("s:{0}", LastRequestId); // we may change this system eventually.
                writer.WriteLine();
                writer.Write(data);
                writer.Write('\n');
                writer.Flush();

                // test output
                //Logger.Debug("Packet Sent: " + Encoding.UTF8.GetString(packetStream.GetBuffer()));

                _manager.DistributePacket(SessionId, packetStream);
            }
        }

        public void SendFormattedReply(string header, MemoryStream dataStream)
        {
            using (MemoryStream packetStream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(packetStream))
            {
                // write headers.
                writer.WriteLine(header);
                writer.WriteLine("l:{0}", dataStream.Length + 1);
                writer.WriteLine("s:{0}R", LastRequestId);
                writer.WriteLine();
                writer.Flush();

                // copy xml data to stream.
                dataStream.WriteTo(packetStream);

                // HTTP content ends with a single \n
                writer.Write('\n');
                writer.Flush();

                // apply encryption if active.
                CryptOut?.EncryptBuffer(packetStream.GetBuffer(), 0, packetStream.Length);

                // test output
                //Logger.Debug("Packet Sent: " + Encoding.UTF8.GetString(packetStream.GetBuffer()));

                _manager.DistributePacket(SessionId, packetStream);
            }
        }

        public void SendOkReply()
        {
            /* With the current implementation it is necessary to make two streams if we want the xml data length.
             *  (the client absolutely requires that the 'l' and 's' headers be exact)
             */

            // todo: make this static?
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = false,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true
            };

            using (MemoryStream dataStream = new MemoryStream())
            using (XmlWriter xmlWriter = XmlWriter.Create(dataStream, settings))
            {
                using (MemoryStream packetStream = new MemoryStream())
                using (StreamWriter writer = new StreamWriter(packetStream))
                {
                    // write headers.
                    writer.WriteLine("STS/1.0 200 OK");
                    writer.WriteLine("l:{0}", dataStream.Length + 1); // this was commented out
                    writer.WriteLine("s:{0}R", LastRequestId); // this was commented out
                    writer.WriteLine();
                    writer.Flush();

                    // test output
                    //Logger.Debug("Packet Sent: " + Encoding.UTF8.GetString(packetStream.GetBuffer()));

                    _manager.DistributePacket(SessionId, packetStream);
                }
            }
        }

        public void SendOkReplyStream(MemoryStream dataStream)
        {
            using (MemoryStream packetStream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(packetStream))
            {
                // write headers.
                writer.WriteLine("STS/1.0 200 OK");
                writer.WriteLine("l:{0}", dataStream.Length + 1);
                writer.WriteLine("s:{0}R", LastRequestId);
                writer.WriteLine();
                writer.Flush();

                // copy xml data to stream.
                dataStream.WriteTo(packetStream);

                // HTTP content ends with a single \n
                writer.Write('\n');
                writer.Flush();

                // apply encryption if active.
                CryptOut?.EncryptBuffer(packetStream.GetBuffer(), 0, packetStream.Length);

                // test output
                //Logger.Debug("Packet Sent: " + Encoding.UTF8.GetString(packetStream.GetBuffer()));

                _manager.DistributePacket(SessionId, packetStream);
            }
        }

        private void DispatchCommand(string command, StreamReader reader)
        {
            // debug logs
            //Logger.Debug($"Dispatching '{command}'");
            string content = reader.ReadToEnd();
            //if (!string.IsNullOrWhiteSpace(content))
            //{
            //    Logger.Debug("Content: " + content);
            //}
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            string[] commandSplit = command.Split('/');
            HandlerMethodReference methodReference = PacketFacility.GetHandlerMethodReference(commandSplit[2]);
            if (methodReference != null)
            {
                try
                {
                    PacketFacility.HandlePacket(this, commandSplit[2], reader);
                }
                catch (Exception ex)
                {
                    Logger.Error("Handler Error", ex);

                    // TODO: KICK THE CLIENT, or tell it that there was error handling packets
                }
            }
            else
            {
                Logger.Warn("Handler for packet: " + command + " not found, implement it using this data: " + content);
            }
        }

        #endregion
    }
}