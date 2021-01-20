using System.Collections.Generic;
using AmongUsClone.Server.Networking;
using UnityEngine;

namespace AmongUsClone.Server.Game
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "PlayersManager", menuName = "PlayersManager")]
    public class PlayersManager : ScriptableObject
    {
        public const int MinPlayerId = 0;
        public const int MaxPlayerId = 9;
        public const int MinRequiredPlayersAmountForGame = 4;
        public const int SecondsForGameLaunch = 5;

        public readonly Dictionary<int, Client> clients = new Dictionary<int, Client>();
    }
}
