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
    private AnimatedSprite2D AnimatedSprite2D;

    [Export]
    private float CameraZoom = 3f;
    private Camera2D _camera;

    [Export]
    public float gatherRadius = 10f;

    [Export]
    public int MaxHealth = 5;
    public int Health;

    public int score = 0;
    public bool IsInvincible = false;
    public static event Action OnPlayerDied;
    public static event Action OnPlayerVictory;
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
        SimplePlayer.EnsureValid().SpikeHit += () => TakeDamage(1);
        Niveau.OnVictoire += Victory;
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

    public void TakeDamage(int quantity, Vector2? attackerPosition = null)
    {
        if (IsInvincible)
            return;
        IsInvincible = true;

        Tween blinkTween = CreateTween().SetLoops();
        blinkTween.TweenProperty(this, "modulate:a", 0.0f, 0.1f);
        blinkTween.TweenProperty(this, "modulate:a", 1.0f, 0.1f);

        Timer invincibilityTimer = new Timer();
        invincibilityTimer.WaitTime = 1.0f;
        invincibilityTimer.OneShot = true;
        invincibilityTimer.Timeout += () =>
        {
            IsInvincible = false;
            blinkTween.Kill();
            Modulate = Colors.White;
        };
        AddChild(invincibilityTimer);
        invincibilityTimer.Start();

        Tween squashTween = CreateTween();
        squashTween.TweenProperty(this, "scale", new Vector2(1.2f, 0.8f), 0.05f);
        squashTween.TweenProperty(this, "scale", Vector2.One, 0.1f);

        if (attackerPosition.HasValue)
        {
            Vector2 direction = (GlobalPosition - attackerPosition.Value).Normalized();
            Vector2 targetPos = Position + (direction * 10f);
            CreateTween()
                .TweenProperty(this, "position", targetPos, 0.15f)
                .SetTrans(Tween.TransitionType.Quad)
                .SetEase(Tween.EaseType.Out);
        }

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
        AnimatedSprite2D.Play("idle");

        Tween dieTween = CreateTween().SetParallel(true);
        dieTween
            .TweenProperty(AnimatedSprite2D, "rotation_degrees", -90f, 0.6f)
            .SetTrans(Tween.TransitionType.Quart)
            .SetEase(Tween.EaseType.Out);
        dieTween.TweenProperty(this, "modulate", Colors.Red, 0.6f);

        dieTween.Chain().TweenCallback(Callable.From(() => OnPlayerDied?.Invoke()));
    }

    private void Victory()
    {
        GD.Print("VICTORY !");
        IsActive = false;
        AnimatedSprite2D.Play("idle");

        Tween victoryTween = CreateTween();
        victoryTween
            .TweenProperty(AnimatedSprite2D, "scale:x", 0.0f, 0.15f)
            .SetTrans(Tween.TransitionType.Sine);
        victoryTween
            .TweenProperty(AnimatedSprite2D, "scale:x", -1.0f, 0.15f)
            .SetTrans(Tween.TransitionType.Sine);
        victoryTween
            .TweenProperty(AnimatedSprite2D, "scale:x", 0.0f, 0.15f)
            .SetTrans(Tween.TransitionType.Sine);
        victoryTween
            .TweenProperty(AnimatedSprite2D, "scale:x", 1.0f, 0.15f)
            .SetTrans(Tween.TransitionType.Sine);
        Tween victoryTween2 = CreateTween();
        victoryTween2.TweenProperty(this, "modulate", Colors.Green, 0.6f);

        victoryTween2.Chain().TweenCallback(Callable.From(() => OnPlayerVictory?.Invoke()));
    }
}
