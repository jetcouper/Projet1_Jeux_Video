using Godot;
using System;

public partial class Ax : Node2D, IUseable
{
	[ExportGroup("Internal")]
	[Export]
	private DpmMovementAx DpmMovementAx;

	public void Use()
	{
		DpmMovementAx.StartSwing();
	}

	public void setPlayer(Node2D player)
	{
		DpmMovementAx.Player = player;
	}
}
