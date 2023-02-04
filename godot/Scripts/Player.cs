using Godot;
using System;

public class Player : KinematicBody2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	
	public Vector2 velocity;
	public int speed = 50;
	public int maxSpeed = 100;
	public int gravity = 1200;
	int maxFallSpeed = 500;
	int jumpForce = 400;
	
	/*
	public enum PlayerStage
	{
		Spring,
		Autumn,
		Winter
	}
	*/
	
	[Signal]
	delegate void ChangeSprite(PlayerStage stage);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		velocity = new Vector2();
	}
	
	public void Move(float delta) 
	{
		
		velocity.x = Mathf.Clamp(velocity.x, -1 * maxSpeed, maxSpeed);
		
		// Gravity:
		velocity.y += delta * gravity;
		if (velocity.y > maxFallSpeed)
		{
			velocity.y = maxFallSpeed;
		}
		
		if (Input.IsActionPressed("left") && Input.IsActionPressed("right")) 
		{
			velocity.x = 0;
		}
		
		else if (Input.IsActionPressed("left"))
		{
			velocity.x -= speed;
		}

		else if (Input.IsActionPressed("right"))
		{
			velocity.x += speed;
		}	
		
		else 
		{
			velocity.x = 0;
		}
		
		if (Input.IsActionPressed("jump") && IsOnFloor())
		{
			velocity.y = -1 * jumpForce;
		}
		
		
		//velocity = velocity.Normalized() * this.speed;
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



