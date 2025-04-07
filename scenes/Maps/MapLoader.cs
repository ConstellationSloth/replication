using godot.Objects;
using Godot;
using System;
using System.Collections.Generic;
using godot.Enums;

namespace godot.scenes.Maps
{
	public partial class MapLoader : Node
	{
		// TODO: actually use char id instead of network id
		public Dictionary<int, PlayerInfo> playersByCharacterId = new Dictionary<int, PlayerInfo>();
		public override void _Ready()
		{
			var port = 6820;
			// these are the args after using --
			// ex: -- port=6830 thing=2
			var args = OS.GetCmdlineUserArgs();
			foreach (var item in args)
			{
				var arg = item.Split('=');
				switch (arg[0])
				{
					case "port":
						port = int.Parse(arg[1]);
						break;
					case "map":
						bool isValid = Enum.TryParse(arg[1], out MapNames map);
						if (isValid) {
							MapServerConnector.Instance.MapName = map;
						}
						break;
				}
			}
			MapServerConnector.Instance.HostServer(port);
			QueueFree();
		}

		public static string GetPathForMapName(MapNames mapName) {
			// PackedScene PlayerScene = GD.Load<PackedScene>("uid://cfcruyqyohth1");
			switch (mapName) {
				case MapNames.Starter:
					return "res://scenes/Maps/test_map.tscn";
				default:
					return "res://scenes/Maps/test_map.tscn";
			}
		}
		
	}
}
