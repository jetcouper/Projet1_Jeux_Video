using System;
using Godot;
using static IWeaponSpawner;

public partial class DpmMovementBullet : Node2D
{
    [ExportGroup("External")]
    [Export]
    public Node2D NodeToControl;

    public float spinSpeed = 0f; 

    private float spinAngle = 0f;

    public Node2D Player;

    private float _distFromPlayer = 50f;
    private float _speed = 50f;

    public float startingPosition;

    private Vector2 _originPosition;

    public Pattern BulletPattern;

    public Node2D TargetNode;

    public Vector2 _lastDirection;

    public bool isHit = false;

    public override void _Ready() {
        base._Ready();
        NodeToControl.Scale = Vector2.Zero;
        Tween tween = CreateTween();
        tween
            .TweenProperty(NodeToControl, "scale", new Vector2(1.5f, 1.5f), 0.18f)
            .SetTrans(Tween.TransitionType.Elastic)
            .SetEase(Tween.EaseType.Out);
        tween
            .TweenProperty(NodeToControl, "scale", Vector2.One, 0.08f)
            .SetTrans(Tween.TransitionType.Bounce)
            .SetEase(Tween.EaseType.Out);
    }   

    public override void _Process(double delta)
    {
        base._Process(delta);
        MoveBullet(delta);
    }

    public void StartSwing()
    {
        if (BulletPattern == Pattern.Corkscrew)
        {
            spinSpeed = 10f;
        }

        _originPosition = Player.GlobalPosition;
        if (TargetNode != null)
        {
            NodeToControl.GlobalPosition = Player.GlobalPosition + new Vector2(0, -10);
        }
        else
        {
            _lastDirection = new Vector2(Mathf.Cos(startingPosition), Mathf.Sin(startingPosition)).Normalized();
            NodeToControl.GlobalPosition = _originPosition + _lastDirection * _distFromPlayer;
        }

        MoveBullet(0);
        NodeToControl.Rotation = startingPosition + 55;
    }

    private void MoveBullet(double delta)
    {
        if (isHit)
            return;

        if (TargetNode != null && IsInstanceValid(TargetNode))
        {
            _lastDirection = (TargetNode.GlobalPosition - NodeToControl.GlobalPosition).Normalized();
        }
        else
        {
            TargetNode = null;
        }

        if (BulletPattern == Pattern.Corkscrew)
        {
            spinAngle += spinSpeed * (float)delta;

            NodeToControl.GlobalPosition += _lastDirection * _speed * (float)delta;
            NodeToControl.Rotation += spinSpeed * (float)delta;

            NodeToControl.GlobalPosition += _lastDirection * _speed * (float)delta + _lastDirection * (float)delta;

            NodeToControl.Rotation += spinSpeed * (float)delta;
        }
        else
        {
            NodeToControl.GlobalPosition += _lastDirection * _speed * (float)delta;
        }
    }
}
