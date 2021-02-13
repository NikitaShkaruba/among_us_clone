using System.Collections.Generic;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Shared.Game
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "BasePlayersManager", menuName = "BasePlayersManager", order = 0)]
    public class BasePlayersManager : ScriptableObject
    {
        public Dictionary<int, BasePlayer> players = new Dictionary<int, BasePlayer>();
    }
}
