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

    public override void _PhysicsProcess(double InDelta)
    {
        base._PhysicsProcess(InDelta);
        Poursuivant.EnsureValid();
        Cible.EnsureValid();

        // Calcul du mouvement vers la cible
        Vector2 direction = Poursuivant.GlobalPosition.DirectionTo(Cible.GlobalPosition);
        Vector2 deplacement = direction * Velocity * (float)InDelta;

        Poursuivant.GlobalPosition += deplacement;

        // Retourne le sprite selon la direction
        if (deplacement.X != 0)
        {
            float directionX = deplacement.X > 0 ? 1 : -1;
            Poursuivant.Scale = new Vector2(
                Math.Abs(Poursuivant.Scale.X) * directionX,
                Poursuivant.Scale.Y
            );
        }
    }
}
