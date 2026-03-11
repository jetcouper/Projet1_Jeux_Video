using System;
using Godot;
using Utils;

public partial class DpmLifeAndVisual : Node
{
    [Export]
    public Node2D RootToEliminate;

    [Export]
    public AnimatedSprite2D DeathAnimation;

    [Export]
    public int LifePoints = 2;

    public int CurrentPoints;
    private bool _isDead = false;

    public bool IsDead
    {
        get { return _isDead; }
    }

    public override void _Ready()
    {
        CurrentPoints = LifePoints;
        if (DeathAnimation != null)
            DeathAnimation.Visible = false;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        CurrentPoints -= damage;

        if (CurrentPoints <= 0)
        {
            _isDead = true;
            Die();
        }
        else
        {
            VisualHitEffect();
        }
    }

    private void VisualHitEffect()
    {
        if (RootToEliminate == null)
            return;

        Tween hitTween = RootToEliminate.CreateTween();
        hitTween.TweenProperty(RootToEliminate, "modulate", Colors.Red, 0.05f);
        hitTween.TweenProperty(RootToEliminate, "modulate", Colors.White, 0.1f);

        Tween squashTween = RootToEliminate.CreateTween();
        squashTween.TweenProperty(RootToEliminate, "scale", new Vector2(1.2f, 0.8f), 0.05f);
        squashTween.TweenProperty(RootToEliminate, "scale", Vector2.One, 0.1f);
    }

    private void Die()
    {
        if (RootToEliminate == null)
            return;

        RootToEliminate.EnsureValid();
        RootToEliminate.SetPhysicsProcess(false);
        RootToEliminate.SetProcess(false);

        if (RootToEliminate is IKillable identity)
        {
            identity.NotifyDeath();
        }

        if (DeathAnimation == null)
        {
            RootToEliminate.QueueFree();
            return;
        }

        // Cache le zombie
        var sprite = RootToEliminate.GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
        if (sprite != null)
            sprite.Visible = false;

        DeathAnimation.Show();
        DeathAnimation.Frame = 0;
        DeathAnimation.Play("death");

        // Supprime le zombie a la fin de l animation
        DeathAnimation.AnimationFinished += OnDeathAnimationFinished;
    }

    private void OnDeathAnimationFinished()
    {
        DeathAnimation.AnimationFinished -= OnDeathAnimationFinished;
        DeathAnimation.QueueFree();
    }
}
