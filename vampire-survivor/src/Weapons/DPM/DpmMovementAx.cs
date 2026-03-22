using System;
using System.Threading.Tasks;
using Godot;

public partial class DpmMovementAx : Node2D, IDPMSimpleMovement
{
    [ExportGroup("External")]
    [Export] public Node2D NodeToControl;

    [Export]
    public CollisionShape2D CollisionShape;

    [Export] public Timer TimerHold;

    [Export] public float DistanceFromPlayer = 50f;
    [Export] public float SwingSpeed = 180f;

    public Node2D Player { get; set; }

    private float _angle = 0f;
    private bool _isSwinging = false;

    private Vector2 _playerOffset = new Vector2(0, -10);

    public override void _Ready()
    {
        DisableCollision();

        TimerHold.OneShot = true;
        TimerHold.WaitTime = 0.75f;
        TimerHold.Timeout += RestartSwing;
    }

    public override void _Process(double delta)
    {
        Positionning();

        if (!_isSwinging)
            return;

        _angle += SwingSpeed * (float)delta;

        if (_angle >= 360f)
        {
            StopSwing();
        }
    }

    public void StartSwing()
    {
        if (!Niveau.IsGameStarted)
            return;

        DisableCollision();
        _angle = 0f;
        Positionning();
        WeaponAppear();
    }

    public async void StopSwing()
    {
        _isSwinging = false;

        await WeaponDisappear();

        TimerHold.Start();
    }


    public void RestartSwing()
    {
        StartSwing();
    }

    public void Positionning()
    {
        if (Player == null)
            return;

        Vector2 offset = new Vector2(DistanceFromPlayer, 0)
            .Rotated(Mathf.DegToRad(_angle));

        NodeToControl.GlobalPosition = Player.GlobalPosition + _playerOffset + offset;
        NodeToControl.RotationDegrees = _angle;
    }

    public async void WeaponAppear()
    {
        EnableCollision();
        Tween tween = CreateTween();

        tween.TweenProperty(NodeToControl, "rotation_degrees", -45f, 0.2f)
            .SetTrans(Tween.TransitionType.Expo)
            .SetEase(Tween.EaseType.Out);

        await ToSignal(tween, Tween.SignalName.Finished);

        _isSwinging = true;
    }

    public async Task WeaponDisappear()
    {
        NodeToControl.RotationDegrees = 0f;

        Tween tween = CreateTween();

        tween.TweenProperty(NodeToControl, "rotation_degrees", -180f, 0.2f)
            .SetTrans(Tween.TransitionType.Expo)
            .SetEase(Tween.EaseType.In);

        await ToSignal(tween, Tween.SignalName.Finished);

        DisableCollision();
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
