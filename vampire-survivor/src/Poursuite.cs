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
    private AnimatedSprite2D _Anim;

    public override void _PhysicsProcess(double InDelta)
    {
        if (Poursuivant is Zombie zombie && zombie.IsDead)
        {
            return;
        }

        base._PhysicsProcess(InDelta);
        Poursuivant.EnsureValid();

        if (Poursuivant == null || Cible == null)
        {
            return;
        }

        Poursuivant.EnsureValid();

        Vector2 direction = Poursuivant.GlobalPosition.DirectionTo(Cible.GlobalPosition);
        float distance = Poursuivant.GlobalPosition.DistanceTo(Cible.GlobalPosition);
        Vector2 deplacement = direction * Velocity * (float)InDelta;

        // Se déplace seulement si a distance du joueur
        if (distance > 2.0f)
        {
            Poursuivant.GlobalPosition += deplacement;
        }

        if (_Anim != null)
        {
            if (distance > 2.0f)
            {
                string anim = "";

                if (Math.Abs(direction.X) > Math.Abs(direction.Y))
                {
                    if (direction.X > 0)
                        anim = "Droite";
                    else
                        anim = "Gauche";
                }
                else
                {
                    if (direction.Y > 0)
                        anim = "Bas";
                    else
                        anim = "Haut";
                }

                // Change d'animation seulement si elle n est pas deja jouée
                if (_Anim.Animation != anim)
                    _Anim.Play(anim);
            }
            else
            {
                if (_Anim.Animation != "Pause")
                    _Anim.Play("Pause");
            }
        }
    }
}
