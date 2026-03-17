using System;
using Godot;

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

    public void Spawn(float typeWeapon, int count = 0)
    {
        Node2D weapon = WeaponScene.Instantiate<Node2D>();
        AddChild(weapon);

        if (weapon is IUseable usable)
        {
            usable.setPlayer(player);
            activeWeapon = usable;
        }
    }

    public void Activate()
    {
        activeWeapon?.Use();
    }
}
