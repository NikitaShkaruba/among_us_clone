// ReSharper disable UnusedMember.Global
// ReSharper disable NotAccessedField.Global

using UnityEngine;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    public class PlayerInformation : MonoBehaviour
    {
        public int id;
        public new string name;
        public bool isLobbyHost;
        public bool isImposter;

        public void Initialize(int id, string name, bool isLobbyHost)
        {
            this.id = id;
            this.name = name;
            this.isLobbyHost = isLobbyHost;
        }
    }
}
