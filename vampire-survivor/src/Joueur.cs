using System;
using Godot;
using Utils;
using static MedPositions;

public partial class Joueur : Node2D, IHealable, IBoostable, ITeleportable
{
    [ExportGroup("External")]
    [Export]
    public MedPositions MedPositions;

    [Export]
    public bool IsActive
    {
        get => SimplePlayer.EnsureValid().IsActive;
        set { SimplePlayer.EnsureValid().IsActive = value; }
    }

    [Export]
    public TileMapLayer CollisionLayer
    {
        get => SimplePlayer.EnsureValid().collisionLayer;
        set { SimplePlayer.EnsureValid().collisionLayer = value; }
    }

    [ExportGroup("Internal")]
    [Export]
    SimplePlayer SimplePlayer;

    [Export]
    private float CameraZoom = 3f;
    private Camera2D _camera;

    [Export]
    public float gatherRadius = 10f;

    [Export]
    public int MaxHealth = 5;
    public int Health;

    public int Xp = 0;

    [Export]
    public int XpForNextLevel = 10;
    public int Level = 1;

    [Signal]
    public delegate void HealthChangedEventHandler();

    [Signal]
    public delegate void XpChangedEventHandler();

    [Signal]
    public delegate void LevelUpSignalEventHandler();

    public override void _Ready()
    {
        base._Ready();
        CollisionLayer =
            MedPositions.choisirObjet(EAlgoSelectionObjet.eCollisionLayer, GlobalPosition)
            as TileMapLayer;
        Health = MaxHealth;
        _camera = GetNode<Camera2D>("Camera2D");
        _camera.MakeCurrent();
        _camera.Zoom = new Vector2(CameraZoom, CameraZoom);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        _camera.Zoom = new Vector2(CameraZoom, CameraZoom);
    }

    public void AddXp(int XpToAdd)
    {
        Xp += XpToAdd;
        if (Xp >= XpForNextLevel)
            LevelUp();
        EmitSignal(SignalName.XpChanged);
    }

    public void LevelUp()
    {
        Level += 1;
        EmitSignal(SignalName.LevelUpSignal);
        XpForNextLevel += (int)MathF.Round(XpForNextLevel * 1.1f);
        MaxHealth += 1;
        Heal(1);

        //Interesting stuff that happens when you level up
    }

    public void Heal(int quantity)
    {
        Health = Mathf.Min(Health + quantity, MaxHealth);
        EmitSignal(SignalName.HealthChanged);
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        SimplePlayer.EnsureValid().ApplySpeedBoost(multiplier, duration);
        GD.Print("Speed boost!");
    }

    public void TakeDamage(int quantity)
    {
        Health -= quantity;
        EmitSignal(SignalName.HealthChanged);
        if (Health <= 0)
            Die();
    }

    public void Teleport(Vector2 position)
    {
        GlobalPosition = position;
        GD.Print("Téléporté à: " + position);
    }

    private void Die()
    {
        GD.Print("GAME OVER !");
        IsActive = false;
    }
}
