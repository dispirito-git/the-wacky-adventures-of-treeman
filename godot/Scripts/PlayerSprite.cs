using Godot;
using System;

public class PlayerSprite : AnimatedSprite
{

	[Signal]
	delegate void SpriteRooting(int frame);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	private void _on_Player_ChangeSprite(PlayerStage stage)
	{
		if (stage == PlayerStage.Autumn)
		{
			// Modulate = new Color(220, 20, 60);
		}
		
		if (stage == PlayerStage.Winter)
		{
			// Modulate = new Color(0, 0, 0);
		}
	}
	
	/*
	public void AnimateRooting()
	{
		if (GetFrame() == 7)
		{
			Playing = false;
		}
	}
	*/
	
	public override void _PhysicsProcess(float delta)
	{
		if (GetAnimation() == "rooting" && Playing)
		{
			EmitSignal(nameof(SpriteRooting), GetFrame());
		}
		
		if (GetAnimation() == "rooting" && GetFrame() == 7)
		{
			Playing = false;
		}
		
	}
}
