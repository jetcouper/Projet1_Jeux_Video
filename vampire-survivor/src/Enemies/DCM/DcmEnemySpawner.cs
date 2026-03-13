using Godot;

public partial class DcmEnemySpawner : Node2D
{
    [Export]
    private PackedScene EnemyScene;

    [Export]
    private Node2D Player;

    [Export]
    public TileMapLayer FloorLayer;

    [Export]
    private float SpawnDistance = 100f;

    [Export]
    private Node MedCrystalNode;

    public void SpawnEnemy()
    {
        if (EnemyScene == null || Player == null)
            return;

        Node2D enemyInstance = EnemyScene.Instantiate<Node2D>();

        if (enemyInstance is Ennemy enemy)
        {
            enemy.INodeMedCrystal = MedCrystalNode;
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
}
