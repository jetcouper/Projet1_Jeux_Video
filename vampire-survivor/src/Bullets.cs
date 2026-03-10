using System;
using Godot;

public partial class Bullets : Node2D, IUseable
{
    [ExportGroup("Internal")]
    [Export]
    private DpmMovementBullet DpmMovement;

    [Export]
    EntityAnimation entityAnimation;

    public float angle;

    public void Use()
    {
        DpmMovement.angle = angle;
        DpmMovement.StartSwing();
        entityAnimation.Play("Shoot");
    }

    public void setPlayer(Node2D player)
    {
        DpmMovement.Player = player;
    }
}
