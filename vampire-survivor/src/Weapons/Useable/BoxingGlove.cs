using Godot;
using System;

public partial class BoxingGlove : Node2D, IUseable
{
	[ExportGroup("Internal")]
	[Export]
	private DpmMovementGlove DpmMovementGlove;

	public void Use()
	{
		DpmMovementGlove.StartSwing();
	}

	public void setPlayer(Node2D player)
	{
		DpmMovementGlove.Player = player;
	}
}
