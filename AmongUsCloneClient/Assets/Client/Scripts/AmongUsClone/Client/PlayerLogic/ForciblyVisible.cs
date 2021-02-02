using UnityEngine;

namespace AmongUsClone.Client.PlayerLogic
{
    /**
     * Players may be hidden in Among Us, and this class may forces players to always be visible
     */
    public class ForciblyVisible : MonoBehaviour
    {
        [SerializeField] private GameObject revealMask;

        public void AllowHiding()
        {
            revealMask.SetActive(false);
        }
    }
}
