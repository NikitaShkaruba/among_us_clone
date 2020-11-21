using UnityEngine;

namespace AmongUsClone.Client.PlayerLogic
{
    public class Player : MonoBehaviour
    {
        // ReSharper disable once NotAccessedField.Global
        public int id;
        // ReSharper disable once NotAccessedField.Global
        public new string name;
        public bool isControllable;

        private PlayerControllable playerControllable;

        public void Awake()
        {
            if (isControllable)
            {
                playerControllable = new PlayerControllable();
            }
        }

        public void Update()
        {
            if (isControllable)
            {
                playerControllable.Update();
            }
        }
    }
}
