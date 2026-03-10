using System;
using Godot;

public partial class DpmMovementBullet : Node2D
{
    [ExportGroup("External")]
    [Export]
    public Node2D NodeToControl;

    public Node2D Player;

    [Export]
    public float DistFromPlayer = 25f;

    private float _speed = 0.1f;

    public float angle;

    private int _direction;

    public override void _Process(double delta)
    {
        base._Process(delta);

        Positionning();
    }

    private Random rand = new Random();

    public void StartSwing()
    {
        // Set sword rotation
        NodeToControl.RotationDegrees = angle;

        // Offset from player using the angle
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * DistFromPlayer;

        NodeToControl.GlobalPosition = Player.GlobalPosition + offset;
    }

    public void Positionning()
    {
        float angle = Mathf.DegToRad(this.angle); // if your angle is in degrees

        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        NodeToControl.GlobalPosition += direction * _speed;
    }
}
