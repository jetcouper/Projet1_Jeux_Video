using Godot;
using Utils;

public partial class DpmDamagePhysical : Area2D
{
    [Export]
    public float Force = 10f;

    [Export]
    public Node2D _Cible;

    [Export]
    public DpmLifeAndVisual _Life;

    public override void _Ready()
    {
        base._Ready();
        AreaEntered += OnTouch;
    }

    private void OnTouch(Area2D InArea)
    {
        if (_Life != null && _Life.IsDead)
            return;

        if (InArea.GetParent() is Joueur)
            return;

        _Cible.EnsureValid();

        //Récupère le noeud parent de ce qui a touché
        Node2D attacker = InArea.GetParent<Node2D>();

        //Direction de l attque
        Vector2 direction = (_Cible.GlobalPosition - attacker.GlobalPosition).Normalized();

        //Position finale
        Vector2 targetPos = _Cible.Position + (direction * Force);

        _Cible
            .CreateTween()
            .TweenProperty(_Cible, "position", targetPos, 0.15f)
            .SetTrans(Tween.TransitionType.Quad)
            .SetEase(Tween.EaseType.Out);

        // Modifie la vie
        _Life?.TakeDamage(1);
    }
}
