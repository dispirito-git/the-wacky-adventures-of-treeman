using Godot;
using System;

public class EnemyAlt : KinematicBody2D
{
	private int _direction = 1;
	private float _speed = 100f;

	public override void _Process(float delta)
	{
		Vector2 velocity = new Vector2(_direction * _speed, 0f);
		MoveAndSlide(velocity);

		// Check if we've collided with something in the x direction
		var collisions = GetSlideCount();
		if (collisions > 0)
		{
			// Turn around
			_direction *= -1;
		}
	}
}
