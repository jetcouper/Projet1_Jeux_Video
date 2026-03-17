using System.Collections.Generic;
using Godot;

public partial class DcmEnemySpawner : Node2D
{
    [Export]
    public MedWaveManager MedWaveManager;

    [Export]
    private PackedScene EnemyScene;

    public TileMapLayer FloorLayer;
    public TileMapLayer CollisionLayer;

    [Export]
    private float SpawnDistance = 100f;

    public Node MedCrystalNode;
    private Node2D Player;

    public override void _Ready()
    {
        base._Ready();
        FloorLayer = MedWaveManager.getFloorLayer();
        CollisionLayer = MedWaveManager.getCollisionLayer();
        MedCrystalNode = MedWaveManager.GetMedCrystalNode();
    }

    public void SpawnEnemy()
    {
        Player = MedWaveManager.GetPlayer();
        if (EnemyScene == null || Player == null)
            return;

        Vector2 spawnPosition = FindValidPosition();
        if (spawnPosition == Vector2.Inf)
            return;

        Node2D enemyInstance = EnemyScene.Instantiate<Node2D>();

        if (enemyInstance is Ennemy enemy)
        {
            enemy.Spawner = this;
            enemy.gestionnaireMort = MedCrystalNode as IDeathHandler;
        }

        if (enemyInstance is ITargetable targetable)
        {
            targetable.SetTarget(Player);
        }

        enemyInstance.GlobalPosition = spawnPosition;
        CallDeferred(Node.MethodName.AddChild, enemyInstance);
    }

    private Vector2 FindValidPosition()
    {
        float zoom = 4.0f;
        Vector2 vp = GetViewport().GetVisibleRect().Size / zoom / 2f;

        float minDistance = vp.Length() + 250f;
        float maxDistance = minDistance + 300f;

        int tentatives = 0;
        while (tentatives < 30)
        {
            float randomAngle = (float)GD.RandRange(0, Mathf.Tau);
            float distance = (float)GD.RandRange(minDistance, maxDistance);

            Vector2 direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            Vector2 testPos = Player.GlobalPosition + direction * distance;

            Vector2I tileCoord = FloorLayer.LocalToMap(FloorLayer.ToLocal(testPos));
            Vector2I colCoord = CollisionLayer.LocalToMap(CollisionLayer.ToLocal(testPos));

            int floorSource = FloorLayer.GetCellSourceId(tileCoord);

            bool hasFloorTile = floorSource != -1;

            if (hasFloorTile)
            {
                return testPos;
            }

            tentatives++;
        }

        return Vector2.Inf;
    }

    public IEnumerable<Node2D> GatherChildren()
    {
        return ChildManipulator.GatherChildren(EnemyScene, this);
    }

    public void HandleDeath(Vector2 InPosition)
    {
        MedWaveManager?.HandleDeath(InPosition);
    }
}
