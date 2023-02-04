using Godot;
using System;

public class PlayerSprite : Sprite
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	private void _on_Player_ChangeSprite(PlayerStage stage)
	{
		if (stage == PlayerStage.Autumn)
		{
			Modulate = new Color(220, 20, 60);
		}
		
		if (stage == PlayerStage.Winter)
		{
			Modulate = new Color(0, 0, 0);
		}
	}
	
	public override void _PhysicsProcess(float delta)
	{
		
	}
}
