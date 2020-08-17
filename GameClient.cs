using System;

namespace PumpkinMC
{
    public enum GameState
    {
        HANDSHAKE = 0,
        STATUS = 1,
        LOGIN = 2,
        PLAY = 3
    }

    public enum ChatMode
    {
        ALL = 0,
        COMMAND_ONLY = 1,
        HIDDEN = 2
    }

    public enum MainHand
    {
        LEFT = 0,
        RIGHT = 1
    }

    public class GameClient
    {
        public bool exists = false;
        public string username = "";
        public int protocol = 0;
        public GameState state = GameState.HANDSHAKE;
        public string connectHost = "";
        public int connectPort = 0;
        public string uuid = "";
        public int renderDistance = 0;
        public ChatMode chatMode = ChatMode.ALL;
        public bool chatColors = false;
        public byte skinBitmask = 0x0;
        public MainHand mainHand = MainHand.LEFT;

        public string clientBrand = "";

        public long lastKA = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        public override string ToString()
        {
            string shit = "Username: "+username+"\n" +
                "Protocol: " + protocol + "\n" +
                "Current State: " + state.ToString() + "\n" +
                "Connect Location: " + connectHost + ((connectPort>0)?":"+connectPort:"") + "\n";

            return shit;
        }
    }
}
