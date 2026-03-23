using Godot;

public partial class DcmItemSpawner : Node2D, ISpawnable
{
    [Export]
    private PackedScene[] SpawneeScenes;

    [Export]
    public TileMapLayer FloorLayer;

    [Export]
    public TileMapLayer ObstacleLayer;

    [Export]
    public float SpawnIntervalMin = 5f;

    [Export]
    public float SpawnIntervalMax = 10f;

    private float _timer;
    private float _nextSpawn;

    public override void _Ready()
    {
        _nextSpawn = (float)GD.RandRange(SpawnIntervalMin, SpawnIntervalMax);
    }

    public override void _Process(double delta)
    {
        _timer += (float)delta;
        if (_timer >= _nextSpawn)
        {
            _timer = 0f;
            _nextSpawn = (float)GD.RandRange(SpawnIntervalMin, SpawnIntervalMax);
            if (FindValidPosition() is Vector2 pos)
                SpawnAt(pos);
        }
    }

    public void SpawnAt(Vector2 position)
    {
        if (SpawneeScenes == null || SpawneeScenes.Length == 0)
            return;

        PackedScene chosenScene = SpawneeScenes[GD.RandRange(0, SpawneeScenes.Length - 1)];
        if (chosenScene == null)
            return;

        Node2D newInstance = chosenScene.Instantiate<Node2D>();
        newInstance.GlobalPosition = position;

        if (newInstance is ItemTeleporter teleporter)
        {
            teleporter.FloorLayer = FloorLayer;
            teleporter.ObstacleLayer = ObstacleLayer;
        }
        GetTree().CurrentScene.AddChild(newInstance);
    }

    private Vector2? FindValidPosition()
    {
        Camera2D camera = GetViewport().GetCamera2D();
        Vector2 cameraCenter = camera != null ? camera.GlobalPosition : Vector2.Zero;

        float zoomValue = camera != null ? camera.Zoom.X : 2.5f;
        Vector2 halfViewportSize = GetViewportRect().Size / zoomValue / 2f;
        float spawnRadius = halfViewportSize.Length() + 100f;

        int tentatives = 0;
        while (tentatives < 20)
        {
            float randomAngle = (float)GD.RandRange(0, Mathf.Tau);
            Vector2 direction = Vector2.FromAngle(randomAngle);
            Vector2 testPos = cameraCenter + (direction * spawnRadius);

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
                    return FloorLayer.ToGlobal(FloorLayer.MapToLocal(floorCoord));
                }
            }

            tentatives++;
        }

        return null;
    }
}
