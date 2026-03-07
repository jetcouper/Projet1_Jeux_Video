using System;
using Godot;

// Script
public partial class EditorDebugOnly : Node2D
{
	public override void _Ready()
	{
		// est visible seulement dans l'éditeur ou quand on affiche les collisions dans le jeu
		Visible = Engine.IsEditorHint() || GetTree().DebugCollisionsHint;
	}
}
