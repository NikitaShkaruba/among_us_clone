// ReSharper disable UnusedMember.Global

namespace AmongUsClone.Shared.Networking.PacketTypes
{
    public enum ServerPacketType
    {
        Welcome,
        Kicked,
        PlayerConnected,
        PlayerDisconnected,
        GameSnapshot,
        ColorChanged,
        GameStarts,
        GameStarted,
    }
}
