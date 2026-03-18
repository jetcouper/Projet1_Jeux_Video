using System;
using System.Formats.Asn1;
using System.Threading.Tasks;
using Godot;

public partial class DpmMovementGlove : Node2D
{
	[ExportGroup("External")]
	[Export]
	public Node2D NodeToControl;
	
	[Export]
	public CollisionShape2D CollisionShape;

	[Export]
	public Timer TimerHold;

	[Export]
	public float DistanceFromPlayer = 50f;

	public Node2D Player;

	private Vector2[] directions = new Vector2[]
	{
		new Vector2(1, 0),   // Right
		new Vector2(0, 1),   // Down
		new Vector2(-1, 0),  // Left
		new Vector2(0, -1)   // Up
	};

	private int attackDirection = 0; 

	private Vector2 _lastDirection = new Vector2(1, 0);
	private float _speed = 25f;

	private int _steps = 0;
	private int _maxSteps = 4;

	
	private Vector2 _playerOffset = new Vector2(0, -10);


	public override void _Ready() {
		base._Ready();
		DisableCollision();
	}
	public async void StartSwing()
	{
		if (!Niveau.IsGameStarted)
			return;


		NodeToControl.Rotation = Mathf.DegToRad(90);
		TimerHold.OneShot = true;
		TimerHold.WaitTime = 0.75f;
		TimerHold.Timeout += Swing;

		Swing();
	}

	private async void Swing(){
		
		for (int i = 0; i < 4; i++)
		{
			Vector2 dir = directions[attackDirection];
			Vector2 startPos = Player.GlobalPosition + dir * DistanceFromPlayer + _playerOffset;
			Vector2 endPos = startPos + dir * _speed;

			NodeToControl.GlobalPosition = startPos;
			await TweenAppear(NodeToControl);

			// Tween the glove forward
			var tween = CreateTween();
			tween.TweenProperty(NodeToControl, "global_position", endPos, 0.08f);
			await ToSignal(tween, "finished");

			await TweenDisappear(NodeToControl);
		}

		
		attackDirection = (attackDirection + 1) % 4;
		NodeToControl.Rotation += Mathf.DegToRad(90);
		TimerHold.Start();
	}

	private async Task TweenAppear(Node2D node)
	{
		EnableCollision();
		var tween = CreateTween();
		tween.TweenProperty(NodeToControl, "modulate:a", 1.0f, 0.08f);
		await ToSignal(tween, "finished");
	}

	private async Task TweenDisappear(Node2D node)
	{
		var tween = CreateTween();
		tween.TweenProperty(NodeToControl, "modulate:a", 0.0f, 0.08f);
		await ToSignal(tween, "finished");
		DisableCollision();
	}

	private void EnableCollision()
	{
		NodeToControl.Visible = true;
		CollisionShape.Disabled = false;
	}

	private void DisableCollision()
	{
		NodeToControl.Visible = false;
		CollisionShape.Disabled = true;
	}
}
