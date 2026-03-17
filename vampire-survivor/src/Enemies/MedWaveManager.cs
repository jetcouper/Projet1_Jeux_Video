using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Utils;
using static MedPositions;

public partial class MedWaveManager : Node
{
    [ExportGroup("External")]
    [Export]
    public MedPositions MedPositions;

    [ExportGroup("Internal")]
    [Export]
    private DcmEnemySpawner ZombieSpawner;

    [Export]
    private DcmEnemySpawner ZombiePresseSpawner;

    [Export]
    private DcmEnemySpawner TortueSpawner;

    [Export]
    private DcmEnemySpawner GeneSpawner;

    [Export]
    private DcmEnemySpawner BossSpawner;

    [Export]
    private Vector2 SpawnInterval = new Vector2(1.0f, 2.0f);

    public enum EGamePhase
    {
        eWave1_Zombie,
        eWave2_ZombiePresse,
        eWave3_Tortue,
        eWave4_Gene,
        eFinalBoss,
    }

    public EGamePhase CurrentPhase;

    private double _gameTime = 0;
    private int _playerLevel = 1;
    private Timer _spawnTimer;

    public override void _Ready()
    {
        CurrentPhase = EGamePhase.eWave1_Zombie;

        _spawnTimer = new Timer
        {
            WaitTime = (float)GD.RandRange(SpawnInterval.X, SpawnInterval.Y),
        };
        _spawnTimer.Timeout += ExecuteSpawnAlgo;
        AddChild(_spawnTimer);

        _spawnTimer.Start();
    }

    public override void _Process(double delta)
    {
        _gameTime += delta;
        UpdateGamePhase();
    }

    public void ReceiveNewLevel(int newLevel)
    {
        _playerLevel = newLevel;
        UpdateGamePhase();
    }

    private void UpdateGamePhase()
    {
        EGamePhase newPhase = CurrentPhase;

        if (_gameTime > 170 || _playerLevel >= 10)
            newPhase = EGamePhase.eFinalBoss;
        else if (_gameTime > 150 || _playerLevel >= 7)
            newPhase = EGamePhase.eWave4_Gene;
        else if (_gameTime > 120 || _playerLevel >= 5)
            newPhase = EGamePhase.eWave3_Tortue;
        else if (_gameTime > 90 || _playerLevel >= 3)
            newPhase = EGamePhase.eWave2_ZombiePresse;

        if (newPhase != CurrentPhase)
        {
            CurrentPhase = newPhase;

            switch (CurrentPhase)
            {
                case EGamePhase.eWave2_ZombiePresse:
                    SpawnInterval = new Vector2(4.0f, 6.5f);
                    break;
                case EGamePhase.eWave3_Tortue:
                    SpawnInterval = new Vector2(2.0f, 4.0f);
                    break;
                case EGamePhase.eWave4_Gene:
                    SpawnInterval = new Vector2(1.0f, 2.0f);
                    break;
                case EGamePhase.eFinalBoss:
                    _spawnTimer.Stop();
                    ExecuteSpawnAlgo();
                    return;
            }

            _spawnTimer.WaitTime = (float)GD.RandRange(SpawnInterval.X, SpawnInterval.Y);
        }
    }

    private void ExecuteSpawnAlgo()
    {
        switch (CurrentPhase)
        {
            case EGamePhase.eWave1_Zombie:
                ZombieSpawner?.SpawnEnemy();
                break;

            case EGamePhase.eWave2_ZombiePresse:
                ZombieSpawner?.SpawnEnemy();
                if (GD.Randi() % 3 == 0)
                    ZombiePresseSpawner?.SpawnEnemy();
                break;

            case EGamePhase.eWave3_Tortue:
                ZombieSpawner?.SpawnEnemy();
                ZombiePresseSpawner?.SpawnEnemy();
                if (GD.Randi() % 4 == 0)
                    TortueSpawner?.SpawnEnemy();
                break;

            case EGamePhase.eWave4_Gene:
                ZombieSpawner?.SpawnEnemy();
                ZombiePresseSpawner?.SpawnEnemy();

                int randValue = (int)(GD.Randi() % 3);
                if (randValue == 0)
                    TortueSpawner?.SpawnEnemy();
                else if (randValue == 1)
                    GeneSpawner?.SpawnEnemy();
                break;

            case EGamePhase.eFinalBoss:
                BossSpawner?.SpawnEnemy();
                break;
        }
        _spawnTimer.WaitTime = (float)GD.RandRange(SpawnInterval.X, SpawnInterval.Y);
    }

    public IEnumerable<Ennemy> GatherAllEnemies()
    {
        return new[] { ZombieSpawner, ZombiePresseSpawner, TortueSpawner, GeneSpawner, BossSpawner }
            .Where(spawner => spawner != null)
            .SelectMany(spawner => spawner.GatherChildren())
            .OfType<Ennemy>();
    }

    public Node2D GetPlayer()
    {
        return MedPositions.choisirObjet(EAlgoSelectionObjet.ePlayer, new(0, 0));
    }

    public void HandleDeath(Vector2 InPosition)
    {
        MedPositions.choisirObjet(EAlgoSelectionObjet.eHandleDeath, InPosition);
    }

    public TileMapLayer getFloorLayer()
    {
        return MedPositions.choisirObjet(EAlgoSelectionObjet.eFloorLayer, new(0, 0))
            as TileMapLayer;
    }

    public TileMapLayer getCollisionLayer()
    {
        return MedPositions.choisirObjet(EAlgoSelectionObjet.eCollisionLayer, new(0, 0))
            as TileMapLayer;
    }

    public Node GetMedCrystalNode()
    {
        return MedPositions.choisirObjet(EAlgoSelectionObjet.eMedCrystal, new(0, 0));
    }
}
