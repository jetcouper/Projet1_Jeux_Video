using System.Collections.Generic;
using Godot;

public partial class DcmEnemySpawner : Node2D
{
    [Export]
    public MedWaveManager MedWaveManager;

    [Export]
    private PackedScene EnemyScene;

    [Export]
    private float SpawnDistance = 100f;

    [Export]
    private Node MedCrystalNode;

    private Node2D Player;

    public void SpawnEnemy()
    {
        Player = MedWaveManager.GetPlayer();
        if (EnemyScene == null || Player == null)
            return;

        Node2D enemyInstance = EnemyScene.Instantiate<Node2D>();

        if (enemyInstance is Ennemy enemy)
        {
            enemy.Spawner = this;
        }

        // Taille  écran visible /zoom /2
        float zoom = 2.5f;
        Vector2 vp = GetViewport().GetVisibleRect().Size / zoom / 2f;

        // Diagonale demi ecran + marge
        float minDistance = vp.Length() + 50f;

        float randomAngle = (float)GD.RandRange(0, Mathf.Tau);
        Vector2 direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        Vector2 spawnPosition = Player.GlobalPosition + (direction * minDistance);

        if (enemyInstance is ITargetable targetable)
        {
            targetable.SetTarget(Player);
        }

        enemyInstance.GlobalPosition = spawnPosition;
        CallDeferred(Node.MethodName.AddChild, enemyInstance);
    }

    public IEnumerable<Node2D> GatherChildren()
    {
        return ChildManipulator.GatherChildren(EnemyScene, this);
    }

    public void HandleDeath(Vector2 InPosition)
    {
        GD.Print($"DcmEnemySpawner HandleDeath at {InPosition}");
        MedWaveManager?.HandleDeath(InPosition);
    }
}
