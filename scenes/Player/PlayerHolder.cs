using godot.Objects;
using Godot;
using System;
using godot.scenes.Controllers;
public partial class PlayerHolder : Node2D
{
	// TODO: link to player script here ready to create
	public override void _Ready()
	{
		foreach (var playerInfo in PlayerManager.Instance.playersByCharacterId.Values) {
			GD.Print("adding existing name ",playerInfo.Name);
			OnPlayerAdded(playerInfo);
		}
		MapServerConnector.Instance.OnPlayerAdded += OnPlayerAdded;
		// TODO: need to connect to the players controller, spawn the players that we already know about
		// and get connected to signals for when information about those players change
	}

	public void OnPlayerAdded(PlayerInfo playerInfo) {
		GD.Print("adding new name ",playerInfo.Name);
		TestChar testChar = (TestChar) GD.Load<PackedScene>(TestChar.Path).Instantiate();
		testChar.Initialize(playerInfo);
		testChar.Name = playerInfo.Name;
		AddChild(testChar);
		GD.Print(Multiplayer.GetUniqueId().ToString() + " " + GetChildren().Count.ToString());
	}
}
