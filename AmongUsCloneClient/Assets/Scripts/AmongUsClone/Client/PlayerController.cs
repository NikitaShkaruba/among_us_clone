using AmongUsClone.Client.Networking;
using AmongUsClone.Shared;
using UnityEngine;

namespace AmongUsClone.Client
{
    public class PlayerController : MonoBehaviour
    {
        private readonly PlayerInput playerInput = new PlayerInput();
        
        private void Update()
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
