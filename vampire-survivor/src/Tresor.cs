using System;
using Godot;

public partial class Tresor : Node2D
{
    [Export]
    int XpValue = 10;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() { }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public void _on_area_2d_area_entered(Node2D collision)
    {
        if (collision.GetParent() is Joueur joueur)
        {
            joueur.AddXp(XpValue);
            QueueFree();
        }
    }
}
