using Godot;
using System;

public partial class DcmSprayBulletSpawner : Node2D, IWeaponSpawner
{
	[ExportGroup("External")]
	[Export]
	public PackedScene Weapon;

    [ExportGroup("Internal")]
	[Export]
	private Timer timer;

	public void Spawn(Node2D player, float typeWeapon)
	{
		int bulletCount = 12;
		float angleStep = 2 * Mathf.Pi / bulletCount;

		for (int i = 0; i < bulletCount; i++)
		{
			Node2D weapon = Weapon.Instantiate<Node2D>();
			AddChild(weapon);
			float angle = i * angleStep;
			if (weapon is SprayBullet bullet)
			{
				bullet.startingPosition = angle;
				bullet.setPlayer(player);
				bullet.AngularSpeed = typeWeapon;
				bullet.Use();
			}
		}
	}
}
