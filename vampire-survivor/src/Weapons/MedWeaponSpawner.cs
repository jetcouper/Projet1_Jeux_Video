using System;
using System.Runtime;
using System.Runtime.Intrinsics.Arm;
using Godot;
using Utils;
using static IWeaponSpawner;
using static MedPositions;

public partial class MedWeaponSpawner : Node2D
{
	[ExportGroup("External")]
	[Export]
	public MedPositions MedPositions;

	[ExportGroup("Internal")]
	[Export]
	private DcmSimpleWeaponSpawner SpawnerSword;

	[Export]
	private DcmSimpleWeaponSpawner SpawnerAxe;

	[Export]
	private DcmSimpleWeaponSpawner SpawnerBoxingGlove;
	
	[Export]
	private DcmBulletSpawner BulletSpawner;

	[Export]
	public EAlgoSelectionCible InAlgoSelectionWeapon;

	
	private IWeaponSpawner activeWeapon;

	private bool isActivated = false;

	private Node2D joueur;

	public enum EAlgoSelectionCible
	{
		eSword,
		eAxe,
		eBoxingGlove,
		eLinearSprayBullets,
		eCorkScrewSprayBullets,
		eLinearSpiralBullets,
		eCorkScrewSpiralBullets,
		eLinearSeekingBullets,
		eCorkScrewSeekingBullets,
	}

	public override void _Ready()
	{
		base._Ready();
		Spawn();
	}

	public Node2D GetPlayer()
	{
		if (joueur == null)
		{
			joueur = MedPositions.choisirObjet(EAlgoSelectionObjet.ePlayer, new(0, 0));
		}
		return joueur;
	}

	public void Spawn()
	{
		
		switch (InAlgoSelectionWeapon)
		{
			case EAlgoSelectionCible.eSword:
				{
					SpawnerSword?.Spawn(Pattern.None);
					activeWeapon = SpawnerSword;
					
					GameisActivated();
				}
				break;
			case EAlgoSelectionCible.eAxe:
				{
					SpawnerAxe?.Spawn(Pattern.None);
					activeWeapon = SpawnerAxe;
					GameisActivated();
				}
				break;
			case EAlgoSelectionCible.eBoxingGlove:
				{
					SpawnerBoxingGlove?.Spawn(Pattern.None);
					activeWeapon = SpawnerBoxingGlove;
					
					GameisActivated();
				}
				break;
			case EAlgoSelectionCible.eLinearSprayBullets:
				{
					BulletSpawner?.Spawn(Pattern.Linear);
					activeWeapon = BulletSpawner;
					GameisActivated();
				}
				break;
			case EAlgoSelectionCible.eCorkScrewSprayBullets:
				{
					BulletSpawner?.Spawn(Pattern.Corkscrew);
					activeWeapon = BulletSpawner;
					GameisActivated();
				}
				break;
			case EAlgoSelectionCible.eLinearSpiralBullets:
				{
					BulletSpawner?.SpawnSequential(Pattern.Linear);
					activeWeapon = BulletSpawner;
					GameisActivated();
				}
				break;
			case EAlgoSelectionCible.eCorkScrewSpiralBullets:
				{
					BulletSpawner?.SpawnSequential(Pattern.Corkscrew);
					activeWeapon = BulletSpawner;
					GameisActivated();
				}
				break;
			case EAlgoSelectionCible.eLinearSeekingBullets:
				{
					BulletSpawner?.SpawnTargeted(Pattern.Linear);
					activeWeapon = BulletSpawner;
					GameisActivated();
				}
				break;
			case EAlgoSelectionCible.eCorkScrewSeekingBullets:
				{
					BulletSpawner?.SpawnTargeted(Pattern.Corkscrew);
					activeWeapon = BulletSpawner;
					GameisActivated();
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

	public void GameisActivated()
	{
		if (isActivated) 
		{			
			activeWeapon.Activate();
		}
	}

	public Node2D GetTarget()
	{
		return MedPositions.choisirObjet(EAlgoSelectionObjet.eEnemiPlusProche, joueur.GlobalPosition);
	}
}
