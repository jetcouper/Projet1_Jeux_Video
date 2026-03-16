using Godot;

public partial class ItemBoots : Area2D
{
    [Export]
    public float SpeedMultiplier = 1.5f;

    [Export]
    public float BoostDuration = 20f;

    public override void _Ready()
    {
        AreaEntered += OnTouch;
    }

    private void OnTouch(Area2D InArea)
    {
        Node2D parent = InArea.GetParent<Node2D>();
        if (parent is IBoostable boostable)
        {
            boostable.ApplySpeedBoost(SpeedMultiplier, BoostDuration);
            QueueFree();
        }
    }
}
