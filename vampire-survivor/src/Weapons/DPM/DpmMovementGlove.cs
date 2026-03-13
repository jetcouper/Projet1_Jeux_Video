using Godot;
using System;

public partial class DpmMovementGlove : Node2D
{
	[ExportGroup("External")]
	[Export] public Node2D NodeToControl;

	[Export] public Timer TimerHold;

	[Export] public float DistanceFromPlayer = 50f;

	public Node2D Player;

	private float _angle = 0f;
	private bool _isSwinging = false;

	public void StartSwing()
	{
		
	}
}
