using Godot;
using System;
using System.Reflection.Metadata;
using System.Threading.Tasks.Dataflow;

public partial class ExperienceCrystal : Node2D
{
	[Export]
	Joueur Player;
	[Export]
	float Velocity = 10f;
	[Export]
	int ScoreValue = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("test");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 direction = GlobalPosition.DirectionTo(Player.GlobalPosition);
        float distance = GlobalPosition.DistanceTo(Player.GlobalPosition);
        Vector2 deplacement = direction * Velocity * (float)delta;

        // Se déplace seulement si a distance du joueur
        
        if (distance < Player.gatherRadius)
        GlobalPosition += deplacement;

		//GD.Print(GlobalPosition);
		
        
	}

	public void setPlayer(Node2D player)
	{
		Player = (Joueur)player;
	}

	public void _on_area_2d_area_entered(Area2D collision)
	{
		if (collision.GetParent() is Joueur joueur)
		{
			joueur.addScore(ScoreValue);
			QueueFree();
		}
	}

	
	

	// public void go_to_mouse(double duration = 2.0, double delay = 0.0){
		
	// 	//# interputs current animation (if there was one)
	// 	if (tween != null) tween.();
		
	// 	tween = create_tween();
	// 	tween.set_trans(Tween.TRANS_EXPO);

	// 	//# delay in second to wait before moving to the mouse
	// 	tween.tween_interval(delay);
		
	// 	//# use a MethodTweener to call a method every frame
	// 	tween.tween_method(_lerp_to_target, 0.0, 1.0, duration);
	// }

	// public void _lerp_to_target(float progression)
	// {
	// 	//# Since this is in a method, it will be up to date all the time
	// 	var target_position = 0;//get_player_position;

	// 	//# lerp stands for linear interpolation
	// 	global_position = lerp(global_position, target_position, progression);
	
	// //# ensures that the animation stops if the mouse is near enough
	// 	if (global_position.distance_to(target_position)<=10.0){
	// 		target_reached.emit();
	// 		tween.kill();
	// 	}
	// }
	
}
