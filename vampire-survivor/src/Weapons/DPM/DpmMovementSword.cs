using System;
using Godot;

public partial class DpmMovementSword : Node2D, IDPMSimpleMovement
{
    [ExportGroup("External")]
    [Export]
    public Node2D NodeToControl;

    [Export]
    public CollisionShape2D CollisionShape;

    [Export]
    public Timer TimerSwing;

    [Export]
    public Timer TimerHold;

    public Node2D Player { get; set; }

    [Export]
    public float DistFromPlayer = 50f;

    private int maxSteps = 3;
    private int steps = 0;
    private int _lastDirection = 1;
    private Vector2 _playerOffset = new Vector2(0, -10);

    public override void _Ready()
    {
        base._Ready();
        DisableCollision();

        TimerSwing.Timeout += Swing;
        TimerSwing.OneShot = false;
        TimerSwing.WaitTime = 0.50f;

        TimerHold.OneShot = true;
        TimerHold.WaitTime = 0.75f;
        TimerHold.Timeout += StartSwing;
    }

    public void StartSwing()
    {
        if (!Niveau.IsGameStarted)
            return;

        steps = 0;
        TimerSwing.Start();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (!NodeToControl.Visible)
            return;

        Positionning();
    }

    private async void Swing()
    {
        DisableCollision();

        if (steps >= maxSteps)
        {
            TimerSwing.Stop();
            TimerHold.Start();
            return;
        }

        _lastDirection *= -1;

        NodeToControl.RotationDegrees = (_lastDirection == 1) ? 180 : 0;

        Positionning();

        WeaponAppear();

        steps++;
    }

    public void Positionning()
    {
        NodeToControl.GlobalPosition =
            Player.GlobalPosition
            + _playerOffset
            + new Vector2(DistFromPlayer * _lastDirection, 0f);
    }

    public void WeaponAppear()
    {
        EnableCollision();
        Tween tween = CreateTween();
        tween
            .TweenProperty(NodeToControl, "scale", new Vector2(1.5f, 1.5f), 0.25f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);

        tween
            .TweenProperty(NodeToControl, "scale", Vector2.One, 0.15f)
            .SetTrans(Tween.TransitionType.Quad)
            .SetEase(Tween.EaseType.In);
    }

    private void EnableCollision()
    {
        NodeToControl.Visible = true;
        CollisionShape.Disabled = false;
    }

    private void DisableCollision()
    {
        NodeToControl.Visible = false;
        CollisionShape.Disabled = true;
    }
}
