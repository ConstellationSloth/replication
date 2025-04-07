using godot.Enums;
using godot.Objects;
using Godot;
using System;

public partial class JoinServer : Control
{
	// Called when the node enters the scene tree for the first time.
	ColorPickerButton ColorButton;
	LineEdit PlayerName;
	public override void _Ready()
	{
		ColorButton = GetNode<ColorPickerButton>("PageContainer/CenterSection/Content/Line2/ColorPickerButton");
		PlayerName = GetNode<LineEdit>("PageContainer/CenterSection/Content/Line1/LineEdit");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_connect_button_pressed() {
		// set all the player information somewhere
		PlayerInfo playerInfo = new PlayerInfo() {
			Red = ColorButton.Color.R8,
			Green = ColorButton.Color.G8,
			Blue = ColorButton.Color.B8,
			Name = PlayerName.Text,
			NetworkId = 0, // This will be set by the server
			Id = 0, // This will be set by the server
		};
		MapServerConnector.Instance.PlayerInfo = playerInfo;
		MapServerConnector.Instance.ConnectToServer("127.0.0.1", 6820);
		QueueFree();
	}
}
