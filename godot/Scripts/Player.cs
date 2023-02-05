using Godot;
using System;
using System.Collections;

public class Player : KinematicBody2D
{
	
	public Vector2 velocity;
	public int horizontalSpeed = 50;
	public int maxSpeed = 100;
	public int gravity = 1200;
	public bool sprinting = false;
	public bool rooted = false;
	public bool crouching = false;
	public Tween fadeTween;
	public Tween secondTween;
	int maxFallSpeed = 500;
	int jumpForce = 400;
	int jumpCharge = 0;
	public string[] levels = {
		"Scenes/Level1.tscn",
		"Scenes/Level2.tscn",
		"Scenes/World.tscn"
	};
	
	
	int MAX_JUMP_CHARGE = 400;
	float ROOT_PER_SEC = 25.0f;
	
	[Signal]
	delegate void ChangeSprite(PlayerStage stage);
	
	[Signal]
	delegate void IsSprinting(bool isSprinting);
	
	[Signal]
	delegate void IsStill();
	
	[Signal]
	delegate void IsRooted(bool isRooted, float delta);

	private string GetLevel()
	{
		return levels[GameState.Instance.CurrentLevel];
	}


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
			AnimatedSprite sprite = GetNode<AnimatedSprite>("PlayerSprite");
			sprite.Animation = "running";
			sprite.Play();
		} 
		else
		{
			sprinting = Input.IsActionPressed("sprint") && IsOnFloor();
			AnimatedSprite sprite = GetNode<AnimatedSprite>("PlayerSprite");
			sprite.Animation = "idle";
			sprite.Play();
		}
		
		EmitSignal(nameof(IsSprinting), sprinting);
		
		if (Input.IsActionPressed("crouch") && IsOnFloor())
		{
			jumpCharge = Mathf.Min(
				MAX_JUMP_CHARGE, 
				jumpCharge + (int) (delta * MAX_JUMP_CHARGE));
			AnimatedSprite sprite = GetNode<AnimatedSprite>("PlayerSprite");
			sprite.Animation = "crouch";
			sprite.Play();
		}
		else
		{
			jumpCharge = 0;
			//AnimatedSprite sprite = GetNode<AnimatedSprite>("AnimatedSprite");
			//sprite.Animation = "idle";
			//sprite.Play();
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
			AnimatedSprite sprite = GetNode<AnimatedSprite>("PlayerSprite");
			sprite.Scale = new Vector2(-1.719f, 1.859f);
			
		}
		else if (Input.IsActionPressed("right"))
		{
			velocity.x += horizontalSpeed * SprintModifier();
			AnimatedSprite sprite = GetNode<AnimatedSprite>("PlayerSprite");
			sprite.Scale = new Vector2(1.719f, 1.859f);
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
		
		if (velocity.x == 0 && IsOnFloor())
		{
			EmitSignal(nameof(IsStill));
		}
		if(Input.IsActionPressed("reset")){
		TheR();
		}
			
		
	}
	
	public void TheR(){
			
				// Get the current scene
			
			WaitForAnimation();
			secondTween = new Tween();
			var currentScene = GetTree().CurrentScene;
			secondTween.InterpolateProperty(currentScene, "modulate", new Color(0, 0, 0, 1), new Color(1, 1, 1, 1), 1.0f, Tween.TransitionType.Linear, Tween.EaseType.In);
			currentScene.AddChild(secondTween);
			secondTween.Start();
			GetTree().ChangeScene(GetLevel());
			
	}
	
	public IEnumerator WaitForAnimation()
{
	
		var currentScene = GetTree().CurrentScene;

			// Create a Tween node to control the fade
			fadeTween = new Tween();
			fadeTween.InterpolateProperty(currentScene, "modulate", new Color(1, 1, 1, 1), new Color(0, 0, 0, 1), 1.0f, Tween.TransitionType.Linear, Tween.EaseType.In);
			currentScene.AddChild(fadeTween);
			//fadeTween.Connect("tween_completed", this, "_on_FadeOut_tween_completed");
			fadeTween.Start();
	// Wait for the animation to finish
	while (fadeTween.IsActive())
	{
		yield return null;
	}

}

	private void _on_EnergyBar_NoEnergy()
	{
		EmitSignal(nameof(ChangeSprite), PlayerStage.Winter);
		TheR();
		
	}
	
	private void _on_EnergyBar_QuarterEnergy()
	{
		EmitSignal(nameof(ChangeSprite), PlayerStage.Autumn);
	}
	
	private bool Crouched()
	{
		return Input.IsActionPressed("crouch");
	}
	
	private void CheckRooting(float delta)
	{
		if (!rooted && Crouched())
		{
			int slideCount = GetSlideCount();
			for (int i = 0; i < slideCount; i++) {
				KinematicCollision2D collision = GetSlideCollision(i);
				// Change this if statement???
				
				TileMap map = collision.Collider as TileMap;
				TileSet tileset = map.TileSet as TileSet;
				if (tileset.TileGetName(i).Equals("mound.png 0"))
				{
					rooted = true;
					Root(delta);
					AnimatedSprite sprite2 = GetNode<AnimatedSprite>("PlayerSprite");
					sprite2.Animation = "rooting";
					sprite2.Play();
					// add the roots to soil
					return;
				}
				if(collision.Collider.ToString().Equals("[StaticBody2D:1412]")){
					GameState.Instance.CurrentLevel +=1;
					GetTree().ChangeScene(GetLevel());
				}
				
			}
			if (!crouching)
			{
				GD.Print("heyo");
				crouching = true;
				AnimatedSprite sprite = GetNode<AnimatedSprite>("PlayerSprite");
				sprite.Animation = "crouch";
				sprite.Play();
			}
			
		}
		else if (Crouched())
		{
			Root(delta);
			
		}
		else if (rooted)
		{
			rooted = false;
			// Change the soil so you cannot root anymore?
		}
		else if (crouching)
		{
			crouching = false;
			// end crouching
		}
	}
	
	private void Root(float delta)
	{
		EmitSignal(nameof(IsRooted), true, delta * ROOT_PER_SEC);
	}
	
	public override void _PhysicsProcess(float delta)
	{
		if (!rooted) 
		{
			Move(delta); 
			MoveAndSlide(velocity, new Vector2(0,-1));
			EmitSignal(nameof(IsRooted), false, delta * ROOT_PER_SEC);
		}
		CheckRooting(delta);
	}
	
	}



