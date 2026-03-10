using System;
using Godot;
using Utils;

public partial class Poursuite : Node2D
{
    [Export]
    public Node2D Cible;

    [Export]
    private Node2D Poursuivant;

    [Export]
    public float Velocity = 100.0f;

    [Export]
    private float StopMove = 50.0f;

    [Export]
    private AnimatedSprite2D _Anim;

    public override void _PhysicsProcess(double InDelta)
    {
        if (Poursuivant is Zombie zombie && zombie.IsDead)
        {
            return;
        }

        if (Poursuivant == null || Cible == null)
        {
            return;
        }

        Poursuivant.EnsureValid();

        float distance = Poursuivant.GlobalPosition.DistanceTo(Cible.GlobalPosition);
        Vector2 direction = Poursuivant.GlobalPosition.DirectionTo(Cible.GlobalPosition);

        if (distance > StopMove)
        {
            Vector2 deplacement = direction * Velocity * (float)InDelta;
            Poursuivant.GlobalPosition += deplacement;

            UpdateAnimation(direction, false);
        }
        else
        {
            UpdateAnimation(direction, true);
        }
    }

    private void UpdateAnimation(Vector2 direction, bool isPaused)
    {
        if (_Anim == null)
            return;

        if (isPaused)
        {
            if (_Anim.Animation != "Pause")
                _Anim.Play("Pause");
            return;
        }

        string anim = "";

        if (Math.Abs(direction.X) > Math.Abs(direction.Y))
        {
            anim = direction.X > 0 ? "Droite" : "Gauche";
        }
        else
        {
            anim = direction.Y > 0 ? "Bas" : "Haut";
        }

        if (_Anim.Animation != anim)
            _Anim.Play(anim);
    }
}
