using Godot;
using System;

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
    private DcmSprayBulletSpawner LinearSprayBullets;

    [Export]
    private DcmSprayBulletSpawner CircularSprayBullets;

	[Export]
	public EAlgoSelectionCible InAlgoSelectionWeapon;

    public enum EAlgoSelectionCible
    {
        eSword,
		eAxe,
        eBoxingGlove,
        eLinearSprayBullets,
        eCircularSprayBullets,
        eSeekingBullet,
        eExplosionBullet
    }

	public override void _Ready() {
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
            case EAlgoSelectionCible.eBoxingGlove:
                { }
                break;
            case EAlgoSelectionCible.eLinearSprayBullets:
                {
                   LinearSprayBullets.Spawn(player, 0);
                }
                break;
            case EAlgoSelectionCible.eCircularSprayBullets:
                {
                    CircularSprayBullets.Spawn(player, 1);

                }
                break;
            default:
            case EAlgoSelectionCible.eExplosionBullet:
                { }
                break;
        }

    }








}
