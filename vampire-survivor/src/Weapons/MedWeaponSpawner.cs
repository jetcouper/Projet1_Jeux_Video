using System;
using System.Runtime.Intrinsics.Arm;
using Godot;
using Utils;
using static MedPositions;

public partial class MedWeaponSpawner : Node2D
{
    [ExportGroup("Internal")]
    [Export]
    private DcmSimpleWeaponSpawner SpawnerSword;

    [Export]
    private DcmSimpleWeaponSpawner SpawnerAxe;

    [Export]
    private DcmBulletSpawner BulletSpawner;

    [Export]
    public EAlgoSelectionCible InAlgoSelectionWeapon;

    [Export]
    public MedPositions MedPositions;

    private IWeaponSpawner activeWeapon;

    private bool isActivated = false;

    public enum EAlgoSelectionCible
    {
        eSword,
        eAxe,
        eLinearSprayBullets,
        eCircularSprayBullets,
        eLinearSpiralBullets,
        eCircularSpiralBullets,
    }

    public override void _Ready()
    {
        base._Ready();
        Spawn();
    }

    public Node2D GetPlayer()
    {
        return MedPositions.choisirObjet(EAlgoSelectionObjet.ePlayer, new(0, 0));
    }

    public void Spawn()
    {
        switch (InAlgoSelectionWeapon)
        {
            case EAlgoSelectionCible.eSword:
                {
                    SpawnerSword?.Spawn(0);
                    activeWeapon = SpawnerSword;

                    if (isActivated)
                        SpawnerSword?.Activate();
                }
                break;
            case EAlgoSelectionCible.eAxe:
                {
                    SpawnerAxe?.Spawn(0);
                    activeWeapon = SpawnerAxe;
                    if (isActivated)
                        SpawnerAxe?.Activate();
                }
                break;
            case EAlgoSelectionCible.eLinearSprayBullets:
                {
                    BulletSpawner?.Spawn(0);
                    activeWeapon = BulletSpawner;
                }
                break;
            case EAlgoSelectionCible.eCircularSprayBullets:
                {
                    BulletSpawner?.Spawn(1);
                    activeWeapon = BulletSpawner;
                }
                break;
            case EAlgoSelectionCible.eLinearSpiralBullets:
                {
                    BulletSpawner?.SpawnSequential(0);
                    activeWeapon = BulletSpawner;
                }
                break;
            case EAlgoSelectionCible.eCircularSpiralBullets:
                {
                    BulletSpawner?.SpawnSequential(1);
                    activeWeapon = BulletSpawner;
                }
                break;
            default:
                break;
        }
    }

    public void Activate()
    {
        isActivated = true;
        activeWeapon?.Activate();
    }
}
