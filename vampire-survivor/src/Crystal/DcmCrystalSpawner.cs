using System;
using System.Collections.Generic;
using Godot;
using Utils;

public partial class DcmCrystalSpawner : Node2D, ISpawnable
{
	[Export]
	private PackedScene SpawneeScene;
	public Node2D Player;

	[Export]
	public Node2D MedSpawnerCrystal;

	public void SpawnAt(Vector2 position)
	{
		if (SpawneeScene == null)
			return;

		Node2D newInstance = SpawneeScene.Instantiate<Node2D>();

		if (newInstance is ExperienceCrystal crystal)
		{
			crystal.setPlayer(Player);
		}

		newInstance.GlobalPosition = position;

		GetTree().CurrentScene.CallDeferred(Node.MethodName.AddChild, newInstance);
	}

	public IEnumerable<Node2D> GatherChildren()
	{
		return ChildManipulator.GatherChildren(SpawneeScene, this);
	}

	public IEnumerable<Node2D> GetCibles()
	{
		return GatherChildren();
	}
}
