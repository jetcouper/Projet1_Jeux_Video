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
    private DcmSprayBulletSpawner LinearSpiralBullets;

	[Export]
	public EAlgoSelectionCible InAlgoSelectionWeapon;

    public enum EAlgoSelectionCible
    {
        eSword,
		eAxe,
        eLinearSprayBullets,
        eCircularSprayBullets,
        eLinearSpiralBullets,

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
            case EAlgoSelectionCible.eLinearSprayBullets:
                {
                   LinearSprayBullets.Spawn(player, 0, 12);
                }
                break;
            case EAlgoSelectionCible.eCircularSprayBullets:
                {
                    CircularSprayBullets.Spawn(player, 1, 12);

                }
                break;
            case EAlgoSelectionCible.eLinearSpiralBullets:
            {
                LinearSpiralBullets.Spawn(player, 0);

            }
            break;
            default:
            break;
        }

    }








}
