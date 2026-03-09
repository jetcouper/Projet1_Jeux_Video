using Godot;
using Utils;

public partial class Zombie : Node2D
{
    [ExportGroup("External")]
    [Export]
    public Node2D Cible
    {
        get { return _Poursuite.EnsureValid().Cible; }
        set { _Poursuite.EnsureValid().Cible = value; }
    }

    [ExportGroup("Internal")]
    [Export]
    Poursuite _Poursuite;

    [Export]
    DpmLifeAndVisual _Life;

    public bool IsDead
    {
        get { return _Life.EnsureValid().IsDead; }
    }

    public override void _Ready()
    {
        Position += new Vector2(0, 20);
        Modulate = new Color(1, 1, 1, 0);

        Tween tween = CreateTween().SetParallel(true);

        tween
            .TweenProperty(this, "position", Position + new Vector2(0, -20), 0.6f)
            .SetTrans(Tween.TransitionType.Quart)
            .SetEase(Tween.EaseType.Out);

        tween.TweenProperty(this, "modulate:a", 1.0f, 0.6f);
    }
}
