using System;
using AmongUsClone.Client.Networking;
using UnityEngine;

namespace AmongUsClone.Client
{
    public class PlayerController : MonoBehaviour
    {
        private void Update()
        {
            SendInputToServer();
        }

        private static void SendInputToServer()
        {
            bool[] playerInputs = {
                Input.GetKey(KeyCode.W),
                Input.GetKey(KeyCode.A),
                Input.GetKey(KeyCode.S),
                Input.GetKey(KeyCode.D),
            };

            PacketsSender.SendPlayerInputPacket(playerInputs);
        }
    }
}
