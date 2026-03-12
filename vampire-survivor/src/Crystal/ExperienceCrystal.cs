using System;
using System.Reflection.Metadata;
using System.Threading.Tasks.Dataflow;
using Godot;

public partial class ExperienceCrystal : Node2D
{
    [Export]
    Joueur Player;

    [Export]
    float Velocity = 10f;

    [Export]
    int ScoreValue = 1;

    public override void _Process(double delta)
    {
        Vector2 direction = GlobalPosition.DirectionTo(Player.GlobalPosition);
        float distance = GlobalPosition.DistanceTo(Player.GlobalPosition);
        Vector2 deplacement = direction * Velocity * (float)delta;

        // Se déplace seulement si a distance du joueur
        if (distance < Player.gatherRadius)
        {
            GlobalPosition += deplacement;
        }
    }

    public void setPlayer(Node2D player)
    {
        Player = (Joueur)player;
    }

    public void _on_area_2d_area_entered(Area2D collision)
    {
        if (collision.GetParent() is Joueur joueur)
        {
            joueur.addScore(ScoreValue);
            QueueFree();
        }
    }
}
