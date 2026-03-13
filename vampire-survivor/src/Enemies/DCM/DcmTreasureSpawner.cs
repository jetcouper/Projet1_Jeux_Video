using Godot;
using System;
using System.Threading.Tasks.Dataflow;

public partial class DcmTreasureSpawner : Node
{
	[Export]
    private PackedScene TresorScene;

	[Export]
	private TileMapLayer validTiles;
	[Export]
	private float SpawnDistance = 100f;

    [Export]
    private Node2D Player;

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
			SpawnTreasure();
			IntervalCounter = SpawnInterval;
		}
	}
	private void SpawnTreasure()
	{
		 if (Player == null)
            return;
		float zoom = 2.5f;
        Vector2 size = GetViewport().GetVisibleRect().Size / zoom;
		float leftBound = (Player.GlobalPosition - size / 2f).X;
		float rightBound = leftBound + size.X;
		float topBound = (Player.GlobalPosition - size / 2f).Y;
		float bottomBound = topBound + size.Y;

		int loopcount = 0;
		while (true)
		{
			Vector2 randomTile = validTiles.GetUsedCells().PickRandom();
			if(!(randomTile.X >= leftBound && randomTile.X <= rightBound || randomTile.Y >= topBound && randomTile.Y <= bottomBound))
			{
				 Node2D tresorInstance = TresorScene.Instantiate<Node2D>();
				 tresorInstance.GlobalPosition = randomTile;
				 CallDeferred(Node.MethodName.AddChild, tresorInstance);
				 GD.Print(randomTile, "spawn treasure");
				 break;
			}
			loopcount++;
			if (loopcount > 10) break;
		}
		
	}
}
