using Godot;
using System;

public partial class BoxingGlove : Node2D, IUseable
{
	[ExportGroup("Internal")]
	[Export]
	private DpmMovementGlove DpmMovementGlove;

	private Node2D Player;

	public void Use()
	{
		DpmMovementGlove.StartSwing();
	}

	public void setPlayer(Node2D player)
	{
		Player = player;
	}
}
