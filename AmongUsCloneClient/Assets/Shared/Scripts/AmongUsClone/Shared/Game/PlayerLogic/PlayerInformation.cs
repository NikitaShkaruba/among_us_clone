// ReSharper disable UnusedMember.Global
// ReSharper disable NotAccessedField.Global

using UnityEngine;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    public class PlayerInformation : MonoBehaviour
    {
        public int id;
        public new string name;

        public void Initialize(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}