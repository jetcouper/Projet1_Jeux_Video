using System;
using Godot;

public partial class DpmMovementBullet : Node2D
{
    [ExportGroup("External")]
    [Export]
    public Node2D NodeToControl;

    public Node2D Player;

    public float DistFromPlayer = 50f;

    public float AngularSpeed;

    private float _speed = 50f;

    public float startingPosition;

    private Vector2 _originPosition;

    public override void _Process(double delta)
    {
        base._Process(delta);
        MoveBullet(delta);
    }

    public void StartSwing()
    {
        _originPosition = Player.GlobalPosition;
        Vector2 offset = new Vector2(Mathf.Cos(startingPosition), Mathf.Sin(startingPosition));
        NodeToControl.GlobalPosition = _originPosition + offset * DistFromPlayer;

        NodeToControl.Rotation = startingPosition + 55;
    }

    private void MoveBullet(double delta)
    {
        startingPosition += AngularSpeed * (float)delta;
        DistFromPlayer += _speed * (float)delta;

        // Move bullet straight along its angle
        Vector2 offset =
            new Vector2(Mathf.Cos(startingPosition), Mathf.Sin(startingPosition)) * DistFromPlayer;
        NodeToControl.GlobalPosition = _originPosition + offset;

        NodeToControl.Rotation = startingPosition + 55;
    }
}
