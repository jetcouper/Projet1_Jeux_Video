using Godot;
using System;

public partial class DcmSpiralBulletSpawner : Node2D, IWeaponSpawner
{
	[ExportGroup("External")]
	[Export]
	public PackedScene Weapon;

	[ExportGroup("Internal")]
	[Export]
	private Timer timer;

	private int _maxBulletCount;

	private int _currentBulletCount;

	private Node2D _player;

	private float _typeWeapon;

	private float _angle;

	float angleStep = 2 * Mathf.Pi / 12;


	public void Spawn(Node2D player, float typeWeapon, int bulletCount)
	{
		_player = player;
		_typeWeapon = typeWeapon;
		_maxBulletCount = bulletCount;
		_currentBulletCount = 0;

		timer.Timeout += newBullet;
		timer.OneShot = false;
		if (_maxBulletCount == 0)
			timer.WaitTime = 0.50f;
		else
			timer.WaitTime = 0f;
	}
	
	public void newBullet()
	{
		_angle += angleStep;
		_currentBulletCount++;

		Node2D weapon = Weapon.Instantiate<Node2D>();
		AddChild(weapon);
		if (weapon is SprayBullet bullet)
		{
			bullet.startingPosition = _angle;
				bullet.setPlayer(_player);
				bullet.AngularSpeed = _typeWeapon;
				bullet.Use();
		}
	}
}
