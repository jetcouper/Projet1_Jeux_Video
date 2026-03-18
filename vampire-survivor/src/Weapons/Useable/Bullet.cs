using System;
using Godot;
using static DpmMovementBullet;
using static IWeaponSpawner;

public partial class Bullet : Node2D, IUseable
{
	[ExportGroup("Internal")]
	[Export]
	private DpmMovementBullet DpmMovementBullet;

	[Export]
	private EntityAnimation entityAnimation;

	[Export]
	private Area2D BulletArea;

	public float startingPosition;

	public Pattern BulletPattern;

	public Node2D Player
	{
		get { return DpmMovementBullet.Player; }
		set { DpmMovementBullet.Player = value; }
	}

	public Node2D TargetNode;

	public override void _Ready() {
		base._Ready();
		BulletArea.AreaEntered += OnBulletHit;
	}

    public void Use()
	{
		DpmMovementBullet.startingPosition = startingPosition;
		DpmMovementBullet.BulletPattern = BulletPattern;
		DpmMovementBullet.TargetNode = TargetNode;
		DpmMovementBullet.StartSwing();
		if (BulletPattern == Pattern.Linear)
		{
			entityAnimation.Play("Linear");
		}
		else if (BulletPattern == Pattern.Corkscrew)
		{
			entityAnimation.Play("Corkscrew");
		}
	}

	public void setPlayer(Node2D player)
	{
		Player = player;
	}
	private void OnBulletHit(Area2D area)
	{
		if (area.GetParent() is Ennemy)
		{
			QueueFree();
		}
	}
}
