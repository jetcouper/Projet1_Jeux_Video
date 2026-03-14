using Godot;

public partial class DcmItemSpawner : Node2D, ISpawnable
{
    [Export]
    private PackedScene SpawneeScene;

    [Export]
    public TileMapLayer FloorLayer;

    [Export]
    public float SpawnIntervalMin = 3f;

    [Export]
    public float SpawnIntervalMax = 8f;

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
        if (SpawneeScene == null)
            return;

        Node2D newInstance = SpawneeScene.Instantiate<Node2D>();
        newInstance.GlobalPosition = position;
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

            Vector2I tileCoord = FloorLayer.LocalToMap(FloorLayer.ToLocal(testPos));
            if (FloorLayer.GetCellSourceId(tileCoord) != -1)
                return testPos;

            tentatives++;
        }

        return null;
    }
}
