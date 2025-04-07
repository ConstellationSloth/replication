using godot.Enums;
using godot.Objects;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace godot.scenes.Controllers
{
	public partial class PlayerManager : Node
	{
		public static PlayerManager Instance { get; private set; }
		// TODO: actually use char id instead of network id
		public Dictionary<int, PlayerInfo> playersByCharacterId = new Dictionary<int, PlayerInfo>();
		public override void _Ready()
		{
			Instance = this;
		}
		public void AddPlayer(int networkId, int red, int green, int blue, string Name) { 
			var player = new PlayerInfo {
				Name = Name,
				NetworkId = networkId,
				Red = red,
				Green = green,
				Blue = blue,
			};
			playersByCharacterId.Add(networkId, player);
			// TODO: we would want to emit a signal here when a player is added
			// This class and the map need to communicate in some way
		}
		public PlayerInfo GetPlayerByNetworkId(int networkId) {
			if (playersByCharacterId.ContainsKey(networkId)) {
				return playersByCharacterId[networkId];
			}
			return null;
		}
	}
}
