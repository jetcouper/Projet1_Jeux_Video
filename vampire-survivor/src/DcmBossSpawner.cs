using Godot;
using Utils;

public partial class DcmBossSpawner : Node2D
{
    private Node2D Player;

    [Export]
    private Node2D Root;

    [Export]
    private PackedScene[] EnemyScenes;

    [ExportGroup("Internal")]
    [Export]
    private Vector2 IntervalRange = new(4.0f, 6.0f); // temps entre chaque vague

    [Export]
    private Vector2 SpawnEnnemyRange = new Vector2(2.0f, 5.0f); // nombre d'ennemis par vague

    [Export]
    private float SpawnRadiusMin = 40.0f;

    [Export]
    private float SpawnRadiusMax = 80.0f;

    private Timer _timer;

    public override void _Ready()
    {
        if (Root is Ennemy bossEnnemy)
        {
            Player = bossEnnemy.Cible;
        }

        if (_timer == null)
        {
            _timer = new Timer();
            AddChild(_timer);

            _timer.Timeout += SpawnWave;

            _timer.WaitTime = (float)GD.RandRange(IntervalRange.X, IntervalRange.Y);
            _timer.Start();
        }
    }

    private void SpawnWave()
    {
        if (Player == null || EnemyScenes == null || EnemyScenes.Length == 0)
            return;

        if (Root is Ennemy boss && boss.IsDead)
        {
            _timer?.Stop();
            return;
        }

        int currentSpawnCount = (int)GD.RandRange(SpawnEnnemyRange.X, SpawnEnnemyRange.Y + 1);

        for (int i = 0; i < currentSpawnCount; i++)
        {
            int index = GD.RandRange(0, EnemyScenes.Length - 1);
            PackedScene scene = EnemyScenes[index];

            if (scene == null)
                continue;

            Node2D enemy = scene.Instantiate<Node2D>();

            enemy.GlobalPosition = Player.GlobalPosition + RandomOffset();

            GetTree().CurrentScene.AddChild(enemy);

            if (enemy is Ennemy z)
            {
                z.Cible = Player;
            }
        }

        _timer.EnsureValid().WaitTime = (float)GD.RandRange(IntervalRange.X, IntervalRange.Y);
    }

    private Vector2 RandomOffset()
    {
        // Choix de l'angle de 0 à 360°
        float rad = Mathf.DegToRad((float)GD.RandRange(0, 360));

        // Calcul de la direction
        Vector2 direction = new(Mathf.Cos(rad), Mathf.Sin(rad));

        // Calcul distance selon le range
        float radius = (float)GD.RandRange(SpawnRadiusMin, SpawnRadiusMax);

        return direction * radius;
    }
}
