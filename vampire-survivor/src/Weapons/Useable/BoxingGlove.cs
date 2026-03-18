using Godot;
using System;

public partial class BoxingGlove : Node2D, IUseable
{
	[ExportGroup("Internal")]
	[Export]
	private DpmMovementGlove DpmMovementGlove;

	public Node2D Player
	{
		get { return DpmMovementGlove.Player; }
		set { DpmMovementGlove.Player = value; }
	}

	public void Use()
	{
		GD.Print("Player: ", Player);
		DpmMovementGlove.StartSwing();
	}

	public void setPlayer(Node2D player)
	{
		Player = player;
	}
}
