// ReSharper disable UnusedMember.Global

namespace AmongUsClone.Shared.Networking.PacketTypes
{
    // Todo: consider migrating a lot of packet types to GameSnapshot
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
