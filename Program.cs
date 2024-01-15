using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using PumpkinMC.Packets;
using PumpkinMC.Packets.Handshake.Client;
using PumpkinMC.Packets.Login.Client;
using PumpkinMC.Packets.Login.Server;
using PumpkinMC.Packets.Play.Client;
using PumpkinMC.Packets.Play.Server;
using PumpkinMC.Util;
using PumpkinMC.Util.Conversion;

namespace PumpkinMC
{
    public class KAthread
    {
        public NetworkStream stream;
        public GameClient gameClient;

        public KAthread(NetworkStream stream, GameClient gameClient)
        {
            this.stream = stream;
            this.gameClient = gameClient;
        }

        public void sendKA()
        {
            try
            {
                while (stream != null)
                {
                    if (gameClient.state == GameState.PLAY && gameClient.exists)
                    {
                        if (DateTimeOffset.FromUnixTimeSeconds(gameClient.lastKA).AddSeconds(10) < DateTimeOffset.UtcNow)
                        {
                            stream.WriteByte(0x09);
                            stream.WriteByte(0x1F); // Keepalive
                            stream.Write(Program.KA);
                            gameClient.lastKA = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                            Console.WriteLine("KA S->C");
                        }
                    }
                } 
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
    }

    class Program
    {
        public bool connected;

        public static int PROTOCOL_VERSION = 340; // 1.12.2, hardcoded for now

        public static byte[] KA = new byte[8] { 0xDE, 0xAD, 0xBE, 0xEF, 0xDE, 0xAD, 0xBE, 0xEF };

        public static void kickClient(Stream stream, GameClient client, MCJsonString reason)
        {
            byte[] json = MCString.New(reason.ToString());

            stream.WriteByte((byte)(json.Length + 1));
            switch (client.state)
            {
                case GameState.LOGIN:
                    stream.WriteByte(0x00);
                    break;
                case GameState.PLAY:
                    stream.WriteByte(0x1A);
                    break;
                default:
                    stream.WriteByte(0x00);
                    break;
            }
            stream.Write(json);
            stream.Close();
        }

        public static void kickClient(Stream stream, GameClient client, string reason)
        {
            var res = new MCJsonString(reason);
            kickClient(stream, client, res);
        }

        public static void Write64(ulong value, byte[] buffer, int offset)
        {
            buffer[offset++] = (byte)((value >> 56) & 0xFF);
            buffer[offset++] = (byte)((value >> 48) & 0xFF);
            buffer[offset++] = (byte)((value >> 40) & 0xFF);
            buffer[offset++] = (byte)((value >> 32) & 0xFF);
            buffer[offset++] = (byte)((value >> 24) & 0xFF);
            buffer[offset++] = (byte)((value >> 16) & 0xFF);
            buffer[offset++] = (byte)((value >> 8) & 0xFF);
            buffer[offset] = (byte)(value & 0xFF);
        }

        public static int ReadPacket(NetworkStream stream, GameClient gameClient)
        {
            var bebc = new BigEndianBitConverter();
            int packetLen = stream.ReadByte();
            //Console.WriteLine("Packet is {0} bytes long", packetLen);
            if (packetLen < 0)
            {
                Console.WriteLine("ERROR: Packet Length less than 0 [{0}]", packetLen);
                return 0;
            }

            /*
            Byte[] bytes = new byte[packetLen];

            int readBytes = stream.Read(bytes, 0, packetLen);
           
            Console.WriteLine("Recieved: {0}", BitConverter.ToString(bytes));

            return readBytes;
            */

            byte packetId = (byte)stream.ReadByte();

            if (packetId < 0 || packetId > 255) 
            {
                Console.WriteLine("ERROR: Packet ID less than 0 or greater than 255");
                return 0; 
            }
            else if (packetLen == 0xFE && packetId == 0x01) // Legacy handshake request, handle later.
            {
                Console.WriteLine("Legacy Handshake");

                stream.WriteByte(0xFF);

                stream.WriteByte(0xFF);
                stream.WriteByte(0xFF);

                byte[] payload = System.Text.Encoding.BigEndianUnicode.GetBytes("§1\0127\0fuck\0Pumpkin 1.12.2\00\01");

                stream.Write(payload);

                Console.WriteLine(BitConverter.ToString(payload));
                return 0;
            }

            if(!(gameClient.state==GameState.PLAY && packetId==0x0B)) Console.WriteLine("[{2}] Received packet 0x{0}, length {1}", packetId.ToString("X2"), packetLen, gameClient.state.ToString());

            switch(gameClient.state)
            {
                case GameState.HANDSHAKE:
                    switch (packetId)
                    {
                        case 0x00:
                            var packet = new C00Handshake();
                            packet.ReadPacket(stream);
                            packet.UpdateClient(gameClient);

                            Console.WriteLine("Client is protocol {0}", gameClient.protocol);

                            if (gameClient.protocol != PROTOCOL_VERSION) 
                                kickClient(stream, gameClient, String.Format("Wrong protocol, got {0}, expected {1}", gameClient.protocol, PROTOCOL_VERSION));

                            break;
                        default:
                            Console.WriteLine("ERROR: handshake default");
                            return 0;
                    }

                    return packetLen;
                case GameState.STATUS:
                    switch (packetId)
                    {
                        case 0x00:
                            Console.WriteLine("Recieved status request");

                            ServerStatus status = new ServerStatus();
                            status.version = new ServerStatusVersion("Pumpkin 1.12.2", PROTOCOL_VERSION);
                            status.players = new ServerStatusPlayerInfo(1, 0, new List<ServerStatusPlayer> { });
                            status.description = new MCJsonString("PumpkinMC Server, by Yellowberry");
                            //status.favicon = "data:image/png;base64,...";

                            /*
                            var mcjsTemp = new MCJsonString();
                            mcjsTemp.color = "yellow";
                            mcjsTemp.bold = true;
                            mcjsTemp.text = "Yellowberry";

                            status.description.extra = new List<MCJsonString> { mcjsTemp };
                            */

                            string json = status.ToString();
                            Console.WriteLine(json);

                            byte[] statusBytes = MCString.New(json);

                            byte[] statusLen = VarInt.New(statusBytes.Length+1);
                            Console.WriteLine(BitConverter.ToString(statusLen).Replace('-',' '));
                            stream.Write(statusLen);
                            stream.WriteByte(0x00);
                            stream.Write(statusBytes);
                            break;
                        case 0x01: // ping!
                            stream.WriteByte(0x09); // long + 1
                            stream.WriteByte(0x01); // pong!
                            byte[] pingBytes = new byte[8];
                            stream.Read(pingBytes, 0, 8);
                            stream.Write(pingBytes, 0, 8);
                            Console.WriteLine("DISC: status pong");
                            return 0;
                        default:
                            byte[] packetTemp = new byte[packetLen];
                            Buffer.SetByte(packetTemp, 0, (byte)packetId);
                            stream.Read(packetTemp, 1, packetLen - 1);

                            Console.WriteLine("{0} [{1}]", BitConverter.ToString(packetTemp), System.Text.Encoding.UTF8.GetString(packetTemp));
                            break;
                    }
                    return packetLen;
                case GameState.LOGIN:
                    switch (packetId)
                    {
                        case 0x00:
                            Console.WriteLine("Recieved login request");

                            var loginStartPacket = new C00LoginStart();
                            loginStartPacket.ReadPacket(stream);
                            loginStartPacket.UpdateClient(gameClient);

                            // Login Success
                            // say hello back!

                            var loginSuccessPacket = new S02LoginSuccess(gameClient.username);
                            loginSuccessPacket.WritePacket(stream);
                            loginSuccessPacket.UpdateClient(gameClient);

                            // WE ARE NOW IN PLAY STATE

                            // Supply the state information required by the task.
                            var kat = new KAthread(stream, gameClient);

                            // Create a thread to execute the task, and then
                            // start the thread.
                            var t = new Thread(new ThreadStart(kat.sendKA));
                            t.Start();
                            Console.WriteLine("Heartbeat thread started.");

                            // Join Game
                            // exciting!

                            var joinGamePacket = new S35JoinGame(0, 0x00, 0, 0x00, 0x00, "default");
                            joinGamePacket.WritePacket(stream);

                            /*
                            
                            Entity ID - int - 4 bytes
                            Gamemode - ubyte - 1 byte
                            Dimension - int - 4 bytes
                            Difficulty - ubyte - 1 byte
                            Max Players - ubyte - 1 byte (ignored)
                            Level Type - string - <=17 bytes
                            Less Debug Info - bool - 1 byte

                            4+1+4+1+1+1 
                            
                            */
                            /*


                            // I fucking hate this code
                            */

                            // Plugin Message (from Server)

                            var pluginMessagePacket = new S24PluginMessage("MC|Brand");
                            pluginMessagePacket.data = MCString.New("pumpkin");
                            pluginMessagePacket.WritePacket(stream);

                            /*
                            byte[] mcBrand = MCString.New("MC|Brand");
                            byte[] modId = MCString.New("pumpkin");

                            stream.WriteByte((byte)(mcBrand.Length+modId.Length+1));
                            stream.WriteByte(0x18);

                            stream.Write(mcBrand);
                            stream.Write(modId);
                            */
                            /*
                            // Spawn Position
                            byte[] pos0 = new byte[8];
                            Write64(mcPosition(0, 0, 0), pos0, 0);

                            stream.WriteByte(0x06);
                            stream.WriteByte(0x4E);
                            //stream.Write(pos0);
                            for (int i = 0; i < 5; i++)
                            {
                                stream.WriteByte(0x00);
                            }
                            //stream.WriteByte
                            */

                            var serverDifficultyPacket = new S13ServerDifficulty(3);
                            serverDifficultyPacket.WritePacket(stream);

                            var spawnPositionPacket = new S70SpawnPosition(MCPosition.New(0,64,0));
                            spawnPositionPacket.WritePacket(stream);

                            // Player Position and Look (to Client)

                            var PPALPacket = new S47PlayerPositionAndLook(0f, 64f, 0f, 0f, 0f);
                            PPALPacket.teleportId = 0;
                            PPALPacket.WritePacket(stream);

                            gameClient.exists = true;

                            //kickClient(stream, gameClient, "fuck you");

                            return packetLen;
                        default:
                            Console.WriteLine("ERROR: login default");
                            return 0;
                    }
                case GameState.PLAY:
                    switch(packetId)
                    {
                        case 0x01: // Tab-Complete (from Client)
                            var tabPacket = new C01TabComplete();
                            tabPacket.ReadPacket(stream);

                            return packetLen;
                        case 0x02: // Chat Message (to Server)
                            var chatPacket = new C02Chat();
                            chatPacket.ReadPacket(stream, gameClient);

                            /*

                            if(message == "/kick")
                            {
                                kickClient(stream, gameClient, "kick requested by client");
                            }
                            else if(message == "/test")
                            {
                                byte[] wndType = MCString.New("minecraft:container");
                                byte[] wndTitle = MCString.New(new MCJsonString("the test window").ToString());

                                VarInt.Write(wndType.Length+wndTitle.Length+2+1, stream);
                                stream.WriteByte(0x13);
                                stream.WriteByte(0x01);
                                stream.Write(wndType);
                                stream.Write(wndTitle);
                                stream.WriteByte(0x09);
                            }
                            else if (message == "/bar")
                            {
                                byte[] barTitle = MCString.New(new MCJsonString("the epic bar").ToString());

                                VarInt.Write(barTitle.Length + 16 + 1 + 4 + 1 + 1 + 1 + 1, stream);
                                stream.WriteByte(0x0C);
                                stream.Write(Guid.NewGuid().ToByteArray());
                                stream.WriteByte(0x00);
                                stream.Write(barTitle);
                                for (int i = 0; i < 4; i++)
                                {
                                    stream.WriteByte(0x00);
                                }
                                VarInt.Write(4, stream);
                                VarInt.Write(4, stream);
                                stream.WriteByte(0x00);
                            }
                            else if (message == "/chunk") // TODO: WORK ON THIS, BROKEN
                            {
                                VarInt.Write(13, stream);
                                stream.WriteByte(0x20);
                                stream.Write(bebc.GetBytes(0));
                                stream.Write(bebc.GetBytes(0));
                                stream.WriteByte(0);
                                VarInt.Write(0b00000001, stream);
                                VarInt.Write(0, stream);    
                                VarInt.Write(0, stream);
                            }
                            else if (message == "/health")
                            {
                                VarInt.Write(10, stream);
                                stream.WriteByte(0x41);
                                stream.Write(bebc.GetBytes(2f));
                                VarInt.Write(10, stream);
                                stream.Write(bebc.GetBytes(2f));
                            }
                            else if (message == "/item")
                            {
                                byte[] diamond = new byte[] { 0x01, 0x16, 0x01, 0x00, 0x00, 0x00 };
                                VarInt.Write(10, stream);
                                stream.WriteByte(0x14);
                                stream.Write(bebc.GetBytes((short)1));
                                stream.Write(diamond);    
                            }
                            else if (message.StartsWith('/'))
                            {

                            }
                            else
                            {
                                byte[] respBytes = MCString.New(new MCJsonString(string.Format("[{0}] {1}", gameClient.username, message)).ToString());
                                VarInt.Write(respBytes.Length + 2, stream);
                                stream.WriteByte(0x0F);
                                stream.Write(respBytes);
                                stream.WriteByte(0x00);
                            }
                            */

                            return packetLen;
                        case 0x03: // Client Status
                            var status = stream.ReadByte();
                            if (status == 0)
                            {
                                var respawnPacket = new S53Respawn();
                                respawnPacket.WritePacket(stream);

                                var PPALPacket = new S47PlayerPositionAndLook(0f, 64f, 0f, 0f, 0f);
                                PPALPacket.teleportId = 0;
                                PPALPacket.WritePacket(stream);
                            }
                            else
                            {
                                var statisticsPacket = new S07Statistics();
                                statisticsPacket.WritePacket(stream);
                            }
                            
                            return packetLen;
                        case 0x04: // Client Info
                            byte[] locale = new byte[VarInt.Read(stream)];
                            stream.Read(locale, 0, locale.Length);

                            gameClient.renderDistance = stream.ReadByte();
                            gameClient.chatMode = (ChatMode)VarInt.Read(stream);
                            gameClient.chatColors = stream.ReadByte() != 0;
                            gameClient.skinBitmask = (byte)stream.ReadByte();
                            gameClient.mainHand = (MainHand)VarInt.Read(stream);

                            Console.WriteLine("[0x04] Client Info handled.");

                            return packetLen;

                        case 0x07:
                            var clickClass = new C07ClickWindow();

                            clickClass.windowId = stream.ReadByte();
                            byte[] _slot = new byte[2];
                            stream.Read(_slot, 0, 2);
                            clickClass.slot = bebc.ToInt16(_slot, 0);
                            clickClass.button = stream.ReadByte();
                            byte[] _action = new byte[2];
                            stream.Read(_action, 0, 2);
                            clickClass.actionNum = bebc.ToInt16(_action, 0);
                            clickClass.mode = VarInt.Read(stream);
                            byte[] _item = new byte[2];
                            stream.Read(_item, 0, 2);
                            clickClass.item = _item;

                            Console.WriteLine("[0x07] Click #{3}, Wnd={0}, Slot={1}, Btn={2}, Mode={4}, Item={5}",
                                clickClass.windowId,
                                clickClass.slot,
                                clickClass.button,
                                clickClass.actionNum,
                                clickClass.mode,
                                BitConverter.ToInt16(clickClass.item)
                            );

                            return packetLen;

                        case 0x09: // Plugin Info
                            byte[] pluginBytes = new byte[VarInt.Read(stream)];

                            stream.Read(pluginBytes, 0, pluginBytes.Length);
                            string plugin = System.Text.Encoding.UTF8.GetString(pluginBytes);

                            switch(plugin)
                            {
                                case "MC|Brand":
                                    byte[] brandBytes = new byte[VarInt.Read(stream)];
                                    stream.Read(brandBytes, 0, brandBytes.Length);
                                    gameClient.clientBrand = System.Text.Encoding.UTF8.GetString(brandBytes);
                                    break;
                                default:
                                    break;
                            }

                            Console.WriteLine("[0x09] Got brand \"{0}\"", gameClient.clientBrand);

                            return packetLen;

                        case 0x0B:
                            //Console.WriteLine("Got back keepalive packet.");
                            byte[] tempKA = new byte[8];
                            stream.Read(tempKA, 0, tempKA.Length);
                            Console.WriteLine("KA C->S");

                            // we don't care about the contents, just that we got it. not smart, but it works.

                            return packetLen;
                        case 0x1D:
                            int swingType = stream.ReadByte();
                            Console.WriteLine("[0x1D] Hand {0} swung", swingType);
                            return packetLen;
                        default:
                            byte[] packetTemp = new byte[packetLen];
                            Buffer.SetByte(packetTemp, 0, (byte)packetId);
                            stream.Read(packetTemp, 1, packetLen-1);

                            Console.WriteLine("{0} [{1}]",BitConverter.ToString(packetTemp),System.Text.Encoding.UTF8.GetString(packetTemp));
                            return packetLen; // capture, parse and continue
                    }
                default:
                    Console.WriteLine("ERROR: unknown gamestate, panic!");
                    return 0;
            }
        }

        public static void Main()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 25565.
                int port = 25565;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    var client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    // Get a stream object for reading and writing
                    var stream = client.GetStream();

                    int i = -1;

                    var gameClient = new GameClient();

                    // Handshake time
                    // FIXME: This is a bad idea. Breaks for empty packets.
                    while (i != 0 && client.Connected)
                    {
                        try
                        {
                            i = ReadPacket(stream, gameClient);
                        }
                        catch (NotImplementedException e)
                        {
                            Console.WriteLine("NotImplementedException: {0}", e);
                            client.Close();
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine("IOException: {0}", e);
                            client.Close();
                        }
                    }

                    Console.WriteLine("[\n{0}\n]", gameClient.ToString());

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
    }
}
