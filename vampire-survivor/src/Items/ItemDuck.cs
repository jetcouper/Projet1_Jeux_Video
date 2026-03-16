using Godot;

public partial class ItemDuck : Area2D
{
    [Export]
    private int HealPoint = 1;

    public override void _Ready()
    {
        AreaEntered += OnTouch;
    }

    private void OnTouch(Area2D InArea)
    {
        Node2D parent = InArea.GetParent<Node2D>();
        if (parent is IHealable healable)
        {
            healable.Heal(HealPoint);
            QueueFree();
        }
    }
}
