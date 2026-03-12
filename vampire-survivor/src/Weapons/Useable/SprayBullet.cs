using System;
using Godot;

public partial class SprayBullet : Node2D, IUseable
{
	[ExportGroup("Internal")]
	[Export]
	private DpmMovementBullet DpmMovement;

	[Export]
	EntityAnimation entityAnimation;

	public float startingPosition;

	public float AngularSpeed;

	public void Use()
	{
		DpmMovement.startingPosition = startingPosition;
		DpmMovement.AngularSpeed = AngularSpeed;
		DpmMovement.StartSwing();
		entityAnimation.Play("Shoot");
	}

	public void setPlayer(Node2D player)
	{
		DpmMovement.Player = player;
	}
}
