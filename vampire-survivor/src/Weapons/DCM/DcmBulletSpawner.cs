using Godot;
using System;

public partial class DcmBulletSpawner : Node2D, IWeaponSpawner
{
	[ExportGroup("External")]
	[Export]
	public PackedScene Weapon;

	[Export]
	public MedWeaponSpawner MedWeaponSpawner;

	private Node2D player;

	public override void _Ready() {
		base._Ready();
		player = MedWeaponSpawner.GetPlayer();
	}
	public async void Spawn(float typeWeapon, int bulletCount = 12)
	{
		float angleStep = 2 * Mathf.Pi / bulletCount;

		while (true)
		{
			for (int i = 0; i < bulletCount; i++)
			{
				float angle = i * angleStep;
				SpawnBullet(typeWeapon, angle);
			}

			await ToSignal(GetTree().CreateTimer(2f), "timeout");
		}
	}

	public async void SpawnSequential(float typeWeapon, int bulletCount = 12)
	{
		
		float delay = 0.5f;
		
		float angleStep = 2 * Mathf.Pi / bulletCount;

		int index = 0;

		while (true)
		{
			float angle = index * angleStep;

			SpawnBullet(typeWeapon, angle);

			index = (index + 1) % bulletCount;

			await ToSignal(GetTree().CreateTimer(delay), "timeout");
		}
	
	}

	private void SpawnBullet(float typeWeapon, float angle)
	{
		Node2D weapon = Weapon.Instantiate<Node2D>();
		AddChild(weapon);

		if (weapon is Bullet bullet)
		{
			bullet.startingPosition = angle;
			bullet.AngularSpeed = typeWeapon;
			bullet.Use();
		}
	}
}
