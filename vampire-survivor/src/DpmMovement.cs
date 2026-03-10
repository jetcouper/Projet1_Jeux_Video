using System;
using Godot;

public partial class DpmMovement : Node2D
{
    [ExportGroup("External")]
    [Export]
    public Node2D NodeToControl;

    [Export]
    public Timer TimerSwing;

    [Export]
    public Timer TimerHold;

    public Node2D Player;

    [Export]
    public float DistFromPlayer = 25f;

    private int maxSteps = 3;
    private int steps = 0;
    private int _lastDirection = 1;

    public override void _Ready()
    {
        base._Ready();
        NodeToControl.Visible = false;

        TimerSwing.Timeout += Swing;
        TimerSwing.OneShot = false;
        TimerSwing.WaitTime = 0.50f;

        TimerHold.OneShot = true;
        TimerHold.WaitTime = 0.75f;
        TimerHold.Timeout += RestartSwing;
    }

    public void StartSwing()
    {
        steps = 0;
        NodeToControl.Scale = new Vector2(1f, 1f);
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
        if (steps >= maxSteps)
        {
            TimerSwing.Stop();
            NodeToControl.Visible = false;
            TimerHold.Start();
            return;
        }

        NodeToControl.Visible = false;

        _lastDirection *= -1;

        NodeToControl.RotationDegrees = (_lastDirection == 1) ? 180 : 0;

        Positionning();

        WeaponAppear();
        steps++;
    }

    public void RestartSwing()
    {
        StartSwing();
    }

    public void Positionning()
    {
        NodeToControl.GlobalPosition =
            Player.GlobalPosition + new Vector2(DistFromPlayer * _lastDirection, 0f);
    }

    public void WeaponAppear()
    {
        NodeToControl.Visible = true;
        Tween tween = CreateTween();
        tween
            .TweenProperty(NodeToControl, "scale", new Vector2(1.5f, 1.5f), 0.25f)
            .SetTrans(Tween.TransitionType.Back)
            .SetEase(Tween.EaseType.Out);
    }
}
