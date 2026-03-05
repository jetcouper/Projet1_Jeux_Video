using System;
using Godot;
using Utils;

public partial class Vie : Node
{
    [ExportGroup("External")]
    [Export]
    Node2D RootToEliminate;

    [Export]
    public int MaxPoints = 2;
    public int CurrentPoints;

    public override void _Ready()
    {
        CurrentPoints = MaxPoints;
    }

    public void TakeDamage(int damage)
    {
        CurrentPoints -= damage;
        if (CurrentPoints <= 0)
        {
            RootToEliminate.EnsureValid();

            Tween tween = RootToEliminate.CreateTween();
            tween
                .TweenProperty(RootToEliminate, "scale", Vector2.One * 3.0f, 0.2f)
                .SetTrans(Tween.TransitionType.Quad)
                .SetEase(Tween.EaseType.Out);
            tween.SetParallel(true);
            tween
                .TweenProperty(RootToEliminate, "modulate:a", 0.0f, 0.2f)
                .SetTrans(Tween.TransitionType.Quad)
                .SetEase(Tween.EaseType.In);
            tween.SetParallel(false);
            tween.Finished += RootToEliminate.QueueFree;
        }
    }
}
