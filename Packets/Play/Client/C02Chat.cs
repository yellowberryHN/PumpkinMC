using PumpkinMC.Packets.Play.Server;
using PumpkinMC.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PumpkinMC.Packets.Play.Client
{
    class C02Chat : Packet
    {
        private const byte packetId = 0x07;
        public string message;

        public override byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

        public override bool isValid()
        {
            throw new NotImplementedException();
        }

        public override void ReadPacket(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void ReadPacket(Stream stream, GameClient gameClient)
        {
            byte[] messageBytes = new byte[VarInt.Read(stream)];
            stream.Read(messageBytes, 0, messageBytes.Length);
            string message = System.Text.Encoding.UTF8.GetString(messageBytes);
            if (message.Length < 1) return;
            bool isCmd = message.Substring(0, 1) == "/";
            Console.WriteLine("[0x02] Got " + (isCmd ? "command" : "chat message") + ": \"{0}\"", (isCmd ? message.Substring(1) : message));

            if(isCmd)
            {
                message = message.Substring(1);
                string[] arr = message.Split(' ');

                switch (arr[0])
                {
                    case "test":
                        var wndText = new MCJsonString("the test container!");
                        wndText.color = "dark_blue";
                        var window = new S19OpenWindow(1, "minecraft:container", wndText, 9);
                        window.WritePacket(stream);
                        break;
                    case "kick":
                        Program.kickClient(stream, gameClient, "kick requested by client");
                        break;
                    case "bar":
                        var bar = new S12BossBar();
                        bar.uuid = Guid.NewGuid();
                        Console.WriteLine("BAR UUID: {0}", bar.uuid.ToString());
                        bar.title = new MCJsonString("the test bar!");
                        bar.title.bold = true;
                        bar.title.color = "dark_blue";
                        bar.health = 0.5f;
                        bar.divider = S12BossBar.BossBarDivider.Six;
                        bar.color = S12BossBar.BossBarColor.Blue;
                        //bar.flags = S12BossBar.BossBarFlags.DarkSky;
                        bar.WritePacket(stream);
                        break;
                    case "delbar":
                        if(arr.Length > 1)
                        {
                            var delbar = new S12BossBar();
                            delbar.uuid = Guid.Parse(arr[1]);
                            delbar.action = S12BossBar.BossBarAction.Remove;
                            delbar.WritePacket(stream);
                        }
                        break;
                    case "health":
                        int val = (arr.Length > 1) ? int.Parse(arr[1]) : 10;

                        var health = new S65UpdateHealth();
                        health.health = (float)val;
                        health.food = val;
                        health.saturation = 5f;
                        health.WritePacket(stream);
                        break;
                    default:
                        break;
                }

            }
            else
            {
                byte[] respBytes = MCString.New(new MCJsonString(string.Format("[{0}] {1}", gameClient.username, message)).ToString());
                VarInt.Write(respBytes.Length + 2, stream);
                stream.WriteByte(0x0F);
                stream.Write(respBytes);
                stream.WriteByte(0x00);
            }
        }

        public override void UpdateClient(GameClient gameClient)
        {
            throw new NotImplementedException();
        }
    }
}
