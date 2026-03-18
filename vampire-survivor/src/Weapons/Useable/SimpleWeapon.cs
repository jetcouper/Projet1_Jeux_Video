using Godot;

public partial class SimpleWeapon : Node2D, IUseable
{
	[Export]
	public Node2D MovementNode; // Assign DpmMovementAxe, DpmMovementSword, or DpmMovementGlove in the editor

	public Node2D Player
	{
		get => (MovementNode as IDPMSimpleMovement)?.Player;
		set
		{
			if (MovementNode is IDPMSimpleMovement movement)
				movement.Player = value;
		}
	}

	public void Use()
	{
		if (MovementNode is IDPMSimpleMovement movement)
			movement.StartSwing();
	}

	public void setPlayer(Node2D player)
	{
		Player = player;
	}
}
