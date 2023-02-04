using Godot;
using System;

public class Player : KinematicBody2D
{
	
	public Vector2 velocity;
	public int horizontalSpeed = 50;
	public int maxSpeed = 100;
	public int gravity = 1200;
	public bool sprinting = false;
	int maxFallSpeed = 500;
	int jumpForce = 400;
	int jumpCharge = 0;
	
	int MAX_JUMP_CHARGE = 400;
	
	[Signal]
	delegate void ChangeSprite(PlayerStage stage);
	
	[Signal]
	delegate void IsSprinting(bool isSprinting);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		velocity = new Vector2();
	}
	
	private int SprintModifier()
	{
		return (sprinting && !Input.IsActionPressed("crouch")) ? 3 : 1;
	}
	
	public void Move(float delta) 
	{
		if (sprinting)
		{
			sprinting = Input.IsActionPressed("sprint");
		} 
		else
		{
			sprinting = Input.IsActionPressed("sprint") && IsOnFloor();
		}
		
		EmitSignal(nameof(IsSprinting), sprinting);
		
		if (Input.IsActionPressed("crouch") && IsOnFloor())
		{
			jumpCharge = Mathf.Min(
				MAX_JUMP_CHARGE, 
				jumpCharge + (int) (delta * MAX_JUMP_CHARGE));
		}
		else
		{
			jumpCharge = 0;
		}
		
		int speedLimit = maxSpeed * SprintModifier();
		velocity.x = Mathf.Clamp(velocity.x, -1 * speedLimit, speedLimit);
		
		// Gravity:
		velocity.y += delta * gravity;
		if (velocity.y > maxFallSpeed)
		{
			velocity.y = maxFallSpeed;
		}
		
		// Horizontal Movement:
		if (Input.IsActionPressed("left"))
		{
			velocity.x -= horizontalSpeed * SprintModifier();
		}
		else if (Input.IsActionPressed("right"))
		{
			velocity.x += horizontalSpeed * SprintModifier();
		}	
		else if (velocity.x > 0)
		{
			velocity.x = Mathf.Max(0, velocity.x - (int) (delta * maxSpeed * 4));
		}
		else if (velocity.x < 0)
		{
			velocity.x = Mathf.Min(0, velocity.x + (int) (delta * maxSpeed * 4));
		}

		// Jumping:
		if (Input.IsActionPressed("jump") && IsOnFloor())
		{
			velocity.y = -1 * (jumpForce + jumpCharge);
		}
		
	}

	private void _on_EnergyBar_NoEnergy()
	{
		EmitSignal(nameof(ChangeSprite), PlayerStage.Winter);
	}
	
	private void _on_EnergyBar_QuarterEnergy()
	{
		EmitSignal(nameof(ChangeSprite), PlayerStage.Autumn);
	}
	
	public override void _PhysicsProcess(float delta)
	{
		Move(delta); 
		MoveAndSlide(velocity, new Vector2(0,-1));
	}

}



