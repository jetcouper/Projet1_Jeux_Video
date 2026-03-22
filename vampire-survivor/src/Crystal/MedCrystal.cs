using System;
using Godot;
using static MedPositions;

public partial class MedCrystal : Node2D, IDeathHandler
{
    [ExportGroup("External")]
    [Export]
    public MedPositions MedPosition;

    [ExportGroup("Internal")]
    [Export]
    public DcmCrystalSpawner ICristalSpawner; //  Pour DCM_Spawner Crystal

    [ExportGroup("Drop")]
    [Export]
    private EAlgoDeath Algo = EAlgoDeath.eSpawnCristal;

    [Export]
    private Node IEnemy; // Pour Ennemi

    public override void _Ready()
    {
        base._Ready();
        ICristalSpawner.Player = MedPosition.choisirObjet(
            EAlgoSelectionObjet.ePlayer,
            new Vector2(0, 0)
        ) as Joueur;
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
                    if (ICristalSpawner is ISpawnable ispawnable)
                        ispawnable.SpawnAt(InPosition);
                    break;
                }
        }
    }
}
