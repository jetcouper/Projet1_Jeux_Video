using System;
using Godot;

public partial class WeaponSpawner : Node2D
{
	[ExportGroup("External")]
	[Export]
	public PackedScene Weapon;

	[Export]
	public Node2D player; //à enlever pour que ce soit le médiateur qui gère ça

	[ExportGroup("Internal")]
	[Export]
	private Timer timer;

	public override void _Ready()
	{
		base._Ready();
		timer.Timeout += Spawn;
	}

	public void Spawn()
	{
		Node2D weapon = Weapon.Instantiate<Node2D>();
		AddChild(weapon);
		if (weapon is IUseable usable)
		{
			usable.setPlayer(player);
			usable.Use();

			timer.Stop(); //à enlever si on veut que les armes spawnent à intervalle régulier. Mettre Timer.WaitTime à qqch
		}
	}
}
