using System;
using Godot;

public partial class MedCrystal : Node2D, IDeathHandler
{
    [ExportGroup("Drop")]
    [Export]
    private EAlgoDeath Algo = EAlgoDeath.eSpawnCristal;

    [ExportGroup("External")]
    [Export]
    private Node ICristalSpawner; //  Pour DCM_Spawner Crystal

    [Export]
    private Node IEnemy; // Pour Ennemi

    private ISpawner CristalSpawner;

    public override void _Ready()
    {
        CristalSpawner = ICristalSpawner as ISpawner;
    }

    public enum EAlgoDeath
    {
        eSpawnCristal,
        eNothing,
    }

    public void HandleDeath(Vector2 InPosition)
    {
        switch (Algo)
        {
            case EAlgoDeath.eSpawnCristal:
            {
                // On demande au spawner de creer un cristal à cette position
                CristalSpawner?.SpawnAt(InPosition);
                break;
            }
        }
    }
}
