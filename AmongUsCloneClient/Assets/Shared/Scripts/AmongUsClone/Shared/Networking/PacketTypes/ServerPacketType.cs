// ReSharper disable UnusedMember.Global

namespace AmongUsClone.Shared.Networking.PacketTypes
{
    // Todo: migrate player connected, player disconnected into GameSnapshot packet
    public enum ServerPacketType
    {
        Welcome,
        Kicked,
        PlayerConnected,
        PlayerDisconnected,
        GameSnapshot,
    }
}
