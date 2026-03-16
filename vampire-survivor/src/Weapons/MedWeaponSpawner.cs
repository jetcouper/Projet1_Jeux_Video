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

	public void Spawn()
	{
		switch (InAlgoSelectionWeapon)
		{
			case EAlgoSelectionCible.eSword:
				{
					SpawnerSword.Spawn(0);
				}
				break;
			case EAlgoSelectionCible.eAxe:
				{
					SpawnerAxe.Spawn(0);
				}
				break;
			case EAlgoSelectionCible.eLinearSprayBullets:
				{
					BulletSpawner.Spawn(0);
				}
				break;
			case EAlgoSelectionCible.eCircularSprayBullets:
				{
					BulletSpawner.Spawn(1);
				}
				break;
			case EAlgoSelectionCible.eLinearSpiralBullets:
				{
					BulletSpawner.SpawnSequential(0);
				}
				break;
			case EAlgoSelectionCible.eCircularSpiralBullets:
				{
					BulletSpawner.SpawnSequential(1);
				}
				break;
			default:
				break;
		}
	}

	public Node2D GetPlayer()
	{
		return MedPositions.choisirObjet(EAlgoSelectionObjet.ePlayer, new(0, 0));
	}
}
