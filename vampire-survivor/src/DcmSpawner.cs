using System.Collections.Generic;
using Godot;
using Utils;

public partial class DcmSpawner : Node2D, ISpawner
{
    [Export]
    private PackedScene SpawneeScene; // experience_crystal.tscn

    [Export]
    private Node2D Player;

    public void SpawnAt(Vector2 position)
    {
        SpawneeScene.EnsureValid();

        Node2D newInstance = SpawneeScene.Instantiate<Node2D>();
        if (newInstance is ExperienceCrystal crystal)
        {
            crystal.setPlayer(Player);
        }

        newInstance.EnsureValid();

        // Méthode différée pour spawn apres la fin de gestion des collisions
        CallDeferred(Node.MethodName.AddChild, newInstance);

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
