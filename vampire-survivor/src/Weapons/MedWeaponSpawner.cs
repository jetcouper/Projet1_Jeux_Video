using System;
using Godot;

public partial class MedWeaponSpawner : Node2D
{
    [ExportGroup("External")]
    [Export]
    public Node2D player; //à enlever pour que ce soit le médiateur qui gère ça

    [ExportGroup("Internal")]
    [Export]
    private DcmSimpleWeaponSpawner SpawnerSword;

    [Export]
    private DcmSimpleWeaponSpawner SpawnerAxe;

    [Export]
    private DcmBulletSpawner BulletSpawner;

    // [Export]
    // private DcmSprayBulletSpawner CircularSprayBullets;

    // [Export]
    // private DcmSprayBulletSpawner LinearSpiralBullets;

    // [Export]
    // private DcmSprayBulletSpawner CircularSpiralBullets;

    [Export]
    public EAlgoSelectionCible InAlgoSelectionWeapon;

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
        //InAlgoSelectionWeapon = EAlgoSelectionCible.eSword; // Changer pour que ce soit au hasard
        Spawn();
    }

    public void Spawn()
    {
        switch (InAlgoSelectionWeapon)
        {
            case EAlgoSelectionCible.eSword:
                {
                    GD.Print("Spawn Sword");
                    SpawnerSword.Spawn(player, 0);
                }
                break;
            case EAlgoSelectionCible.eAxe:
                {
                    SpawnerAxe.Spawn(player, 0);
                }
                break;
            case EAlgoSelectionCible.eLinearSprayBullets:
                {
                    BulletSpawner.Spawn(player, 0);
                }
                break;
            case EAlgoSelectionCible.eCircularSprayBullets:
                {
                    BulletSpawner.Spawn(player, 1);
                }
                break;
            case EAlgoSelectionCible.eLinearSpiralBullets:
                {
                    BulletSpawner.SpawnSequential(player, 0);
                }
                break;
            case EAlgoSelectionCible.eCircularSpiralBullets:
                {
                    BulletSpawner.SpawnSequential(player, 1);
                }
                break;
            default:
                break;
        }
    }
}
