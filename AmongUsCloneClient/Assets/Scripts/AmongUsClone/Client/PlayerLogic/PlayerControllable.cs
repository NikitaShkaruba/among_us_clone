using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared;
using UnityEngine;

namespace AmongUsClone.Client.PlayerLogic
{
    public class PlayerControllable
    {
        private readonly PlayerInput playerInput = new PlayerInput();

        public void Update()
        {
            UpdatePlayerInput();
            SendInputToServer();
        }

        private void UpdatePlayerInput()
        {
            playerInput.moveTop = Input.GetKey(KeyCode.W);
            playerInput.moveLeft = Input.GetKey(KeyCode.A);
            playerInput.moveBottom = Input.GetKey(KeyCode.S);
            playerInput.moveRight = Input.GetKey(KeyCode.D);
        }

        private void SendInputToServer()
        {
            PacketsSender.SendPlayerInputPacket(playerInput);
        }
    }
}
