using System;
using Godot;

public partial class SimplePlayer : Node
{
    [Export]
    private Node2D NodeToControl;

    [Export]
    public TileMapLayer collisionLayer;

    [Export]
    private float VelocityPixelPerSecond = 100.0f;

    [Export]
    private AnimatedSprite2D AnimatedSprite2D;

    private bool _isActive = true;

    [Export]
    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            //Alternative aux if dans les fonctions process et physics process
            SetProcess(value);
            SetPhysicsProcess(value);
        }
    }

    Vector2 _inputVector = new(0.0f, 0.0f);

    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double InDelta)
    {
        base._Process(InDelta);
        _inputVector = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        // if (_inputVector.Length() < 1.0f)
        // {
        //     return;
        // }
        _inputVector = _inputVector.Normalized();

        if (_inputVector == Vector2.Zero)
        {
            AnimatedSprite2D?.Play("idle");
        }
        else if (Mathf.Abs(_inputVector.X) >= Mathf.Abs(_inputVector.Y))
        {
            if (_inputVector.X > 0)
            {
                AnimatedSprite2D.FlipH = false;
                AnimatedSprite2D?.Play("right");
            }
            else
            {
                AnimatedSprite2D.FlipH = true;
                AnimatedSprite2D?.Play("left");
            }
        }
        else
        {
            if (_inputVector.Y > 0)
            {
                AnimatedSprite2D?.Play("down");
            }
            else
            {
                AnimatedSprite2D?.Play("up");
            }
        }
    }

    public override void _PhysicsProcess(double InDelta)
    {
        base._PhysicsProcess(InDelta);
        if (NodeToControl is null || collisionLayer is null)
        {
            return;
        }
        Vector2 prochainePosition =
            NodeToControl.GlobalPosition + VelocityPixelPerSecond * (float)InDelta * _inputVector;
        Vector2I tileCoord = collisionLayer.LocalToMap(collisionLayer.ToLocal(prochainePosition));
        if (collisionLayer.GetCellSourceId(tileCoord) == -1)
        {
            NodeToControl.GlobalPosition = prochainePosition;
        }

        NodeToControl.GlobalPosition = new Vector2(
            Mathf.Clamp(NodeToControl.GlobalPosition.X, -600, 600),
            Mathf.Clamp(NodeToControl.GlobalPosition.Y, -400, 400)
        );
    }
}
