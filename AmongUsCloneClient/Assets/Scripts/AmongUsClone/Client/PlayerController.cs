using AmongUsClone.Client.Networking;
using AmongUsClone.Shared;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace AmongUsClone.Client
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerInput playerInput = new PlayerInput();
        
        private void Update()
        {
            UpdatePlayerInput();
            SendInputToServer();
        }

        private void UpdatePlayerInput()
        {
            playerInput.MoveTop = Input.GetKey(KeyCode.W);
            playerInput.MoveLeft = Input.GetKey(KeyCode.A);
            playerInput.MoveBottom = Input.GetKey(KeyCode.S);
            playerInput.MoveRight = Input.GetKey(KeyCode.D);
        }

        private void SendInputToServer()
        {
            PacketsSender.SendPlayerInputPacket(playerInput);
        }
    }
}
