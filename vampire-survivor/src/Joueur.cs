using System;
using System.Threading.Tasks.Dataflow;
using Godot;
using Utils;

public partial class Joueur : Node2D
{
	[ExportGroup("External")]
	[Export]
	public bool IsActive
	{
		get => SimplePlayer.EnsureValid().IsActive;
		set { SimplePlayer.EnsureValid().IsActive = value; }
	}

	[ExportGroup("Internal")]
	[Export]
	SimplePlayer SimplePlayer;

    [Export]
    private float CameraZoom = 3f;
    private Camera2D _camera;

    [Export]
    public float gatherRadius = 10f;

    public int score = 0;

	public override void _Ready()
	{
		base._Ready();
		_camera = GetNode<Camera2D>("Camera2D");
		_camera.MakeCurrent();
		_camera.Zoom = new Vector2(CameraZoom, CameraZoom);

		// var tileMap = GetParent().GetNode<TileMapLayer>("TileMapLayer");
		// var usedRect = tileMap.GetUsedRect();
		// var tileSize = tileMap.TileSet.TileSize;

		// float mapWidth = usedRect.Size.X * tileSize.X;
		// float mapHeight = usedRect.Size.Y * tileSize.Y;

		// Vector2 screenSize = GetViewport().GetVisibleRect().Size;

		// float zoomX = screenSize.X / mapWidth;
		// float zoomY = screenSize.Y / mapHeight;
		// float zoom = Mathf.Min(zoomX, zoomY);
		// GD.Print($"ScreenSize: {screenSize}");
		// GD.Print($"ZoomX: {zoomX}, ZoomY: {zoomY}, Zoom final: {zoom}");
		// GD.Print($"Map Rect: {usedRect}");
		// GD.Print($"Tile Size: {tileMap.TileSet.TileSize}");
		// GD.Print($"Map pixels W: {usedRect.Size.X * tileMap.TileSet.TileSize.X}");
		// GD.Print($"Map pixels H: {usedRect.Size.Y * tileMap.TileSet.TileSize.Y}");
		//camera.Zoom = new Vector2(zoom, zoom);
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
        _camera.Zoom = new Vector2(CameraZoom, CameraZoom);
    }

    public void addScore(int scoreToAdd)
    {
        score += scoreToAdd;
        
    }
}
