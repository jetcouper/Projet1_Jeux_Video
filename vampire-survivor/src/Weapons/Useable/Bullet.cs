using System;
using Godot;

public partial class Bullet : Node2D, IUseable
{
    [ExportGroup("Internal")]
    [Export]
    private DpmMovementBullet DpmMovementBullet;

    [Export]
    EntityAnimation entityAnimation;

    public float startingPosition;

    public float AngularSpeed;

    public Node2D Player
    {
        get { return DpmMovementBullet.Player; }
        set { DpmMovementBullet.Player = value; }
    }

    public void Use()
    {
        DpmMovementBullet.startingPosition = startingPosition;
        DpmMovementBullet.AngularSpeed = AngularSpeed;
        DpmMovementBullet.StartSwing();
        entityAnimation.Play("Shoot");
    }

    public void setPlayer(Node2D player)
    {
        Player = player;
    }
}
