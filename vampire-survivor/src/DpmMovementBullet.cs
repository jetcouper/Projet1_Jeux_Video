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

    private int _direction;

    public override void _Process(double delta)
    {
        base._Process(delta);

        Positionning();
    }

    private Random rand = new Random();

    public void StartSwing()
    {
        _direction = rand.Next(0, 4);

        float[] rotations = { 135, 45, 315, 225 };

        Vector2[] offsets =
        {
            new Vector2(0, DistFromPlayer),
            new Vector2(DistFromPlayer, 0),
            new Vector2(0, -DistFromPlayer),
            new Vector2(-DistFromPlayer, 0),
        };

        NodeToControl.RotationDegrees = rotations[_direction];
        NodeToControl.GlobalPosition = Player.GlobalPosition + offsets[_direction];
    }

    public void Positionning()
    {
        Vector2[] directions =
        {
            new Vector2(0, 1),
            new Vector2(1, 0),
            new Vector2(0, -1),
            new Vector2(-1, 0),
        };

        NodeToControl.GlobalPosition += directions[_direction] * _speed;
    }
}
