using Godot;

public partial class DcmItemSpawner : Node2D, ISpawnable
{
    [Export]
    private PackedScene SpawneeScene;

    [Export]
    public TileMapLayer FloorLayer;

    public void SpawnAt(Vector2 position)
    {
        if (SpawneeScene == null)
            return;

        Node2D newInstance = SpawneeScene.Instantiate<Node2D>();

        newInstance.GlobalPosition = position;

        GetTree().CurrentScene.CallDeferred(Node.MethodName.AddChild, newInstance);
    }

    private Vector2 FindValidPosition()
    {
        Camera2D camera = GetViewport().GetCamera2D();
        Vector2 cameraCenter = camera != null ? camera.GlobalPosition : Vector2.Zero;

        float zoomValue = camera != null ? camera.Zoom.X : 2.5f;
        Vector2 halfViewportSize = GetViewportRect().Size / zoomValue / 2f;
        float spawnRadius = halfViewportSize.Length() + 100f;

        int tentatives = 0;
        while (tentatives < 30)
        {
            float randomAngle = (float)GD.RandRange(0, Mathf.Tau);
            Vector2 direction = Vector2.FromAngle(randomAngle);

            Vector2 testPos = cameraCenter + (direction * spawnRadius);

            Vector2I tileCoord = FloorLayer.LocalToMap(FloorLayer.ToLocal(testPos));

            if (FloorLayer.GetCellSourceId(tileCoord) != -1)
            {
                return testPos;
            }

            tentatives++;
        }

        return Vector2.Inf;
    }
}
