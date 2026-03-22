using System;
using System.Linq;
using System.Timers;
using Godot;
using Utils;

public partial class MedPositions : Node2D
{
    [ExportGroup("External")]
    [Export]
    public TileMapLayer CollisionLayer;

    [Export]
    public TileMapLayer FloorLayer;

    [ExportGroup("Internal")]
    [Export]
    private Joueur Joueur;

    [Export]
    private MedWaveManager MedWaveManager;

    [Export]
    private MedCrystal medCrystal;

    [Export]
    private MedWeaponSpawner MedWeaponSpawner;

    public enum EAlgoSelectionObjet
    {
        ePlayer,
        eEnemiPlusProche,
        eMedCrystal,
        eCollisionLayer,
        eFloorLayer,
        eHandleDeath,
        eChangeWeapon,
    }

    public override void _Ready()
    {
        base._Ready();
        Niveau.OnGameStarted += OnGameStartedHandler;
        if (Joueur != null)
        {
            Joueur.LevelUpSignal += OnLevelUp;
        }

    }

    private void OnLevelUp()
    {
        choisirObjet(EAlgoSelectionObjet.eChangeWeapon, Joueur.GlobalPosition);
    }
    private void OnGameStartedHandler()
    {
        MedWeaponSpawner.Activate();
        Niveau.OnGameStarted -= OnGameStartedHandler;
    }

    public Node2D choisirObjet(EAlgoSelectionObjet InAlgoSelectionObjet, Vector2 InPosition)
    {
        Node2D node = null;

        switch (InAlgoSelectionObjet)
        {
            case EAlgoSelectionObjet.ePlayer:
                {
                    node = Joueur;
                }
                break;
            case EAlgoSelectionObjet.eEnemiPlusProche:
                {
                    var enemies = MedWaveManager.GatherAllEnemies();

                    node = enemies
                        .Where(IsInstanceValid)
                        .OrderBy(e => e.GlobalPosition.DistanceTo(InPosition))
                        .FirstOrDefault();
                }
                break;
            case EAlgoSelectionObjet.eMedCrystal:
                {
                    node = medCrystal;
                }
                break;
            case EAlgoSelectionObjet.eCollisionLayer:
                {
                    node = CollisionLayer;
                }
                break;
            case EAlgoSelectionObjet.eFloorLayer:
                {
                    node = FloorLayer;
                }
                break;
            default:
            case EAlgoSelectionObjet.eHandleDeath:
                {
                    medCrystal.HandleDeath(InPosition);

                    node = null;
                }
                break;
            case EAlgoSelectionObjet.eChangeWeapon:
                {
                    MedWeaponSpawner.ChangeWeapon(InPosition);

                    node = null;
                }
                break;
        }

        return node;
    }
}
