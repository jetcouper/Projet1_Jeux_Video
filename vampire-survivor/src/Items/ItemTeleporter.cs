using Godot;

public partial class ItemTeleporter : Area2D
{
    [Export]
    public TileMapLayer FloorLayer;

    [Export]
    public TileMapLayer ObstacleLayer;

    [Export]
    public float MinDistance = 200f;

    [Export]
    public float MaxDistance = 300f;

    public override void _Ready()
    {
        AreaEntered += OnTouch;
    }

    private void OnTouch(Area2D InArea)
    {
        Node2D parent = InArea.GetParent<Node2D>();
        if (parent is ITeleportable teleportable)
        {
            Vector2? dest = FindValidPosition(parent.GlobalPosition);
            if (dest is Vector2 pos)
            {
                teleportable.Teleport(pos);
                QueueFree();
            }
        }
    }

    private Vector2? FindValidPosition(Vector2 origin)
    {
        int tentatives = 0;
        while (tentatives < 20)
        {
            float randomAngle = (float)GD.RandRange(0, Mathf.Tau);
            float randomDist = (float)GD.RandRange(MinDistance, MaxDistance);
            Vector2 testPos = origin + Vector2.FromAngle(randomAngle) * randomDist;

            Vector2I floorCoord = FloorLayer.LocalToMap(FloorLayer.ToLocal(testPos));

            if (FloorLayer.GetCellSourceId(floorCoord) != -1)
            {
                bool safeFromObstacle =
                    ObstacleLayer == null
                    || ObstacleLayer.GetCellSourceId(
                        ObstacleLayer.LocalToMap(ObstacleLayer.ToLocal(testPos))
                    ) == -1;

                if (safeFromObstacle)
                {
                    Vector2 centeredPos = FloorLayer.ToGlobal(FloorLayer.MapToLocal(floorCoord));
                    return centeredPos;
                }
            }
            tentatives++;
        }
        return null;
    }
}
