using Godot;
using System;

public class Sqrt : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	public string[] levels = 
	{
		"Scenes/Level1.tscn",
		"Scenes/Level2.tscn",
		"Scenes/World.tscn",
		"Scenes/Win.tscn"
	};
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	private void _on_Sqrt_body_entered(object body)
	{
		GameState.Instance.CurrentLevel +=1;
		GetTree().ChangeScene(levels[GameState.Instance.CurrentLevel]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		
	}
}
