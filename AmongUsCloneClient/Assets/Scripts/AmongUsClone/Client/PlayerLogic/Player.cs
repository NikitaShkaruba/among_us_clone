using UnityEngine;

namespace AmongUsClone.Client.PlayerLogic
{
    public class Player : MonoBehaviour
    {
        // ReSharper disable once NotAccessedField.Global
        public int id;
        // ReSharper disable once NotAccessedField.Global
        public new string name;

        private readonly PlayerControllable playerControllable = new PlayerControllable();

        public void Update()
        {
            playerControllable.Update();
        }
    }
}
