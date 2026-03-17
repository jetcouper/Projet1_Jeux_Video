using System;
using Godot;
using Utils;
using static MedPositions;

public partial class Joueur : Node2D, IHealable, IBoostable
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

    [Export]
    public TileMapLayer SpikeLayer
    {
        get => SimplePlayer.EnsureValid().spikeLayer;
        set { SimplePlayer.EnsureValid().spikeLayer = value; }
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

    public int score = 0;

    public override void _Ready()
    {
        base._Ready();
        SimplePlayer.EnsureValid().SpikeHit += () => TakeDamage(1);
        CollisionLayer =
            MedPositions.choisirObjet(EAlgoSelectionObjet.eCollisionLayer, GlobalPosition)
            as TileMapLayer;

        SpikeLayer =
            MedPositions.choisirObjet(EAlgoSelectionObjet.eSpikeLayer, GlobalPosition)
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

    public void addScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public void Heal(int quantity)
    {
        Health = Mathf.Min(Health + quantity, MaxHealth);
        GD.Print("Vie: " + Health + "/" + MaxHealth);
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        SimplePlayer.EnsureValid().ApplySpeedBoost(multiplier, duration);
        GD.Print("Speed boost!");
    }

    public void TakeDamage(int quantity)
    {
        Health -= quantity;
        GD.Print("Vie: " + Health + "/" + MaxHealth);
        if (Health <= 0)
            Die();
    }

    private void Die()
    {
        GD.Print("GAME OVER !");
        IsActive = false;
    }
}
