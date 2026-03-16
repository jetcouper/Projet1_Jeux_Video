using System.Collections.Generic;
using Godot;

public partial class DcmEnemySpawner : Node2D
{
    [Export]
    public MedWaveManager MedWaveManager;

    [Export]
    private PackedScene EnemyScene;

    public TileMapLayer FloorLayer;

    [Export]
    private float SpawnDistance = 100f;

    public Node MedCrystalNode;

    private Node2D Player;

    public override void _Ready() {
        base._Ready();
        FloorLayer = MedWaveManager.getFloorLayer();
        MedCrystalNode = MedWaveManager.GetMedCrystalNode();
    }

    public void SpawnEnemy()
    {
        Player = MedWaveManager.GetPlayer();
        if (EnemyScene == null || Player == null)
            return;

        Node2D enemyInstance = EnemyScene.Instantiate<Node2D>();

        if (enemyInstance is Ennemy enemy)
        {
            enemy.Spawner = this;
            enemy.gestionnaireMort = MedCrystalNode as IDeathHandler;
        }

        Vector2 spawnPosition = FindValidPosition();

        if (enemyInstance is ITargetable targetable)
        {
            targetable.SetTarget(Player);
        }

        enemyInstance.GlobalPosition = spawnPosition;
        CallDeferred(Node.MethodName.AddChild, enemyInstance);
    }
    private Vector2 FindValidPosition()
    {
        // Taille  écran visible /zoom /2
        float zoom = 2.5f;
        Vector2 vp = GetViewport().GetVisibleRect().Size / zoom / 2f;

        // Diagonale demi ecran + marge
        float minDistance = vp.Length() + 50f;

        int tentatives = 0;
        while (tentatives < 30)
        {
            float randomAngle = (float)GD.RandRange(0, Mathf.Tau);
            Vector2 direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            Vector2 testPos = Player.GlobalPosition + (direction * minDistance);
            Vector2I tileCoord = FloorLayer.LocalToMap(FloorLayer.ToLocal(testPos));

            if (FloorLayer.GetCellSourceId(tileCoord) != -1)
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
