using godot.Objects;
using godot.scenes.Controllers;
using Godot;
using System;
public partial class TestChar : CharacterBody2D
{
	private float Jump = 400f;
	private float Speed = 500f;
	private float Gravity = 2000f;
	public int NetworkId = 1;
	public PlayerInfo PlayerInfo;
	public static string Path = "res://scenes/Player/test_char.tscn";
	private Vector2 InputDirection = Vector2.Zero;
	public void Initialize(PlayerInfo playerInfo) {
		PlayerInfo = playerInfo;
		Name = PlayerInfo.NetworkId.ToString();
		NetworkId = PlayerInfo.NetworkId;
		// GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(1);
	}
	public override void _Ready() {
		// set color
		Modulate = new Color(((float)PlayerInfo.Red)/255, ((float)PlayerInfo.Green)/255, ((float)PlayerInfo.Blue)/255);
		// set name
		// set network id
		if (NetworkId == Multiplayer.GetUniqueId()) {
			AddChild(new Camera2D());
		}
	}
	public override void _PhysicsProcess(double delta)
	{
		if (Multiplayer.IsServer()) {
			var vel = Vector2.Zero;
			vel.X = InputDirection.X * Speed;
			vel.Y = InputDirection.Y * Speed;
			Velocity = vel;
			MoveAndSlide();
			Rpc(nameof(SendPlayerPosition), Position);
		}
		else {
			if (Multiplayer.GetUniqueId() != NetworkId) {
				return;
			}
			var inputDirection = Input.GetVector("Left", "Right", "Up", "Down");
			RpcId(1, nameof(SendPlayerInput), inputDirection);
		}
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.UnreliableOrdered)]
	public void SendPlayerPosition(Vector2 position) {
		if (Multiplayer.GetRemoteSenderId() != 1) {
			GD.Print("sender of position:", Multiplayer.GetRemoteSenderId());
			return;
		}
		Position = position;
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer,TransferMode = MultiplayerPeer.TransferModeEnum.UnreliableOrdered)]
	public void SendPlayerInput(Vector2 inputDirection) {
		if (Multiplayer.GetRemoteSenderId() != NetworkId) {
			GD.Print("sender of input:", Multiplayer.GetRemoteSenderId());
			return;
		}
		InputDirection = inputDirection;
	}
}
