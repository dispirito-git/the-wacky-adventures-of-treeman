using Godot;
using System;

public class Roots : AnimatedSprite
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetFrame(0);
	}


	private void _on_Player_IsRooted(bool isRooted, float delta)
	{
		if (!isRooted)
		{
			SetFrame(0);
		}
	}

	
	private void _on_AnimatedSprite_SpriteRooting(int frame)
	{
		if (frame == 7)
		{
			Playing = true;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		if (Playing && GetFrame() == 5)
		{
			Playing = false;
		}
	}
	
}
