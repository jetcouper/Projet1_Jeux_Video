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
	private Joueur joueur;

	[Export]
	private MedWaveManager medWaveManager;

	[Export]
	private MedCrystal medCrystal;

	[Export]
	private MedWeaponSpawner medWeaponSpawner;

	public enum EAlgoSelectionObjet
	{
		ePlayer,
		eEnemiPlusProche,
		eMedCrystal,
		eCollisionLayer,
		eFloorLayer,
		eHandleDeath
	}

	public override void _Ready()
	{
		base._Ready();
	}

	public Node2D choisirObjet(EAlgoSelectionObjet InAlgoSelectionObjet, Vector2 InPosition)
	{
		Node2D node = null;

		switch (InAlgoSelectionObjet)
		{
			case EAlgoSelectionObjet.ePlayer:
				{
					node = joueur;
				}
				break;
			case EAlgoSelectionObjet.eEnemiPlusProche:
				{
					var enemies = medWaveManager.GatherAllEnemies();

					node = enemies
						.OrderBy(e => e.GlobalPosition.DistanceTo(InPosition))
						.FirstOrDefault();
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
		}

		return node;
	}
}
