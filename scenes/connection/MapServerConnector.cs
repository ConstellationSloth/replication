using godot.Objects;
using godot.scenes.Controllers;
using godot.scenes.Maps;
using Godot;
using System;
using godot.Enums;
public partial class MapServerConnector : Node
{
	public static MapServerConnector Instance { get; private set; }
	public int NetworkId { get; private set; }
	public PlayerInfo PlayerInfo { get; set; }
	public event Action<PlayerInfo> OnPlayerAdded;
	public MapNames MapName { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void SendPlayerInfo(int networkId, int red, int green, int blue, string name) {
		var pm = PlayerManager.Instance;
		var info = new PlayerInfo {
			Name = name,
			Red = red,
			Green = green,
			Blue = blue,
			NetworkId = networkId
		};

		// this will need to connect network id to an authenticated session of a player
		
		if (Multiplayer.IsServer()) {
			foreach (var player in pm.playersByCharacterId.Values) {
				if (player == null) {
					GD.Print("Player is null");
					continue;
				};
				GD.Print("Sending player" + player.Name + " to ", player.NetworkId.ToString());
				RpcId(networkId, nameof(SendPlayerInfo), player.NetworkId, player.Red, player.Green, player.Blue, player.Name);
			}
			Rpc(nameof(SendPlayerInfo), networkId, red, green, blue, name);
		}
		if (!pm.playersByCharacterId.ContainsKey(networkId)) {
			GD.Print("Adding player ", name, " on ", NetworkId);
			pm.playersByCharacterId.Add(networkId, info);
			OnPlayerAdded?.Invoke(info);
		}
		
	}


	public void ConnectToServer(string address, int port) { 
		ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
		var err = peer.CreateClient(address, port);
		Multiplayer.MultiplayerPeer = peer;
		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.ConnectedToServer += ConnectedToServer;
	}

	public void HostServer(int port)
	{
		ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
		peer.CreateServer(port);
		Multiplayer.MultiplayerPeer = peer;
		Multiplayer.PeerConnected += PeerConnected;
		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		LoadMap(MapName);
	}

	public void ConnectedToServer() {
		GD.Print("Connected to the server");
		NetworkId = Multiplayer.GetUniqueId();
		PlayerInfo.NetworkId = NetworkId;
		GD.Print(NetworkId.ToString() + " is " + PlayerInfo.Name);
		RpcId(1, nameof(SendPlayerInfo), NetworkId, PlayerInfo.Red, PlayerInfo.Green, PlayerInfo.Blue, PlayerInfo.Name);
		// TODO: Now need to switch to the correct map
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void LoadMap(MapNames name) {
		GD.Print("Loading map: " + name);
		var map = GD.Load<PackedScene>(MapLoader.GetPathForMapName(name)).Instantiate();
		GetTree().Root.CallDeferred("add_child", map);
		foreach (var node in GetTree().Root.GetChildren()) {
			GD.Print("Node: " + node.Name);
		}
	}	

	public void PeerConnected(long id) {
		if (!Multiplayer.IsServer()) {
			return;
		}

		// tell them what map to load
		RpcId(id, nameof(LoadMap), (int)MapName);
		
		
		// RpcId(id, nameof(SendPlayerInfo), NetworkId, PlayerInfo.Color, PlayerInfo.Name);

		// if we are the server, we want to get information on the player
		// then send out the new players information to everyone such as equipment
		// and we want to send the newly connected player everyone elses equipment information.
		// we are only going to update equipment when it happens rather than send with animation information
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
