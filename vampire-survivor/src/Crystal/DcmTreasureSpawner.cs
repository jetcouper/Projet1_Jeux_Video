using Godot;

public partial class DcmTreasureSpawner : Node2D, ISpawnable
{
    [Export]
    private PackedScene TresorScene;

    [Export]
    private TileMapLayer validTiles;
    [Export]
    private float SpawnDistance = 100f;
    [Export]
    private double SpawnInterval = 10.0; //en secondes

    private double IntervalCounter = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        IntervalCounter = SpawnInterval;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        IntervalCounter -= delta;
        if (IntervalCounter <= 0)
        {
            if (FindValidPosition() is Vector2 pos)
                SpawnAt(pos);
            IntervalCounter = SpawnInterval;
        }
    }
    public void SpawnAt(Vector2 position)
    {

        Node2D tresorInstance = TresorScene.Instantiate<Node2D>();
        tresorInstance.GlobalPosition = position;
        CallDeferred(Node.MethodName.AddChild, tresorInstance);


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

            Vector2I tileCoord = validTiles.LocalToMap(validTiles.ToLocal(testPos));
            if (validTiles.GetCellSourceId(tileCoord) != -1)
                return testPos;

            tentatives++;
        }

        return null;
    }
}
