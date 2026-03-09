using System.Collections.Generic;
using Godot;
using Utils;

public partial class DcmSpawner : Node2D, ISpawner
{
    [Export]
    private PackedScene SpawneeScene; // experience_crystal.tscn

    public void SpawnAt(Vector2 position)
    {
        SpawneeScene.EnsureValid();

        Node2D newInstance = SpawneeScene.Instantiate<Node2D>();
        newInstance.EnsureValid();

        AddChild(newInstance);

        newInstance.GlobalPosition = position;
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
