using Godot;
using System;

public class Family : Area2D
{
	
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
	
	private void _on_Family_body_entered(object body)
	{
		GameState.Instance.CurrentLevel +=1;
		GetTree().ChangeScene(levels[GameState.Instance.CurrentLevel]);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
