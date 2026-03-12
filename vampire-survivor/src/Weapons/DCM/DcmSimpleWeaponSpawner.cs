using System;
using Godot;

public partial class DcmSimpleWeaponSpawner : Node2D, IWeaponSpawner
{
	[ExportGroup("External")]
	[Export]
	public PackedScene Weapon;

	[ExportGroup("Internal")]
	[Export]
	private Timer timer;

	public void Spawn(Node2D player, float typeWeapon, int count = 0)
	{
		GD.Print("Spawn Weapon");
		Node2D weapon = Weapon.Instantiate<Node2D>();
		AddChild(weapon);
		if (weapon is IUseable usable)
		{
			GD.Print("Weapon is useable");
			usable.setPlayer(player);
			usable.Use();

		}
	}

}
