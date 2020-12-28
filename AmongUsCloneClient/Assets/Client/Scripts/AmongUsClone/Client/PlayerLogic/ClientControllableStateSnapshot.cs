using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Client.PlayerLogic
{
    public class ClientControllableStateSnapshot
    {
        public PlayerInput input;
        public Vector2 position;

        public ClientControllableStateSnapshot(PlayerInput input, Vector2 position)
        {
            this.input = input;
            this.position = position;
        }
    }
}
