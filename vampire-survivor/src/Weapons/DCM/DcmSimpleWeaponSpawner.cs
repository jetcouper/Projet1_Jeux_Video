using System;
using Godot;
using static IWeaponSpawner;

public partial class DcmSimpleWeaponSpawner : Node2D, IWeaponSpawner
{
    [ExportGroup("External")]
    [Export]
    public PackedScene WeaponScene;

    [Export]
    public MedWeaponSpawner MedWeaponSpawner;

    [ExportGroup("Internal")]
    [Export]
    private Timer timer;

    private Node2D player;
    private IUseable activeWeapon;

    public override void _Ready()
    {
        base._Ready();
        player = MedWeaponSpawner.GetPlayer();
    }

    public void Spawn(Pattern pattern = Pattern.None, Node2D target = null)
    {
        Node2D weapon = WeaponScene.Instantiate<Node2D>();

        if (weapon is IUseable usable)
        {
            usable.setPlayer(player);
            activeWeapon = usable;
        }

        CallDeferred(Node.MethodName.AddChild, weapon);
    }

    public void Activate()
    {
        activeWeapon?.Use();
    }

    public void RemoveActiveWeapon()
    {
        if (activeWeapon is Node node && IsInstanceValid(node))
        {
            node.QueueFree();
            activeWeapon = null;
        }
    }
}
