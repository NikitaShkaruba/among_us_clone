// ReSharper disable UnusedMember.Global

namespace AmongUsClone.Shared.Networking.PacketTypes
{
    // Todo: consider migrating a lot of server packets to GameSnapshot packet
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
        SecurityCamerasEnabled,
        SecurityCamerasDisabled,
    }
}
