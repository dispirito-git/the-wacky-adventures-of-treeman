extends KinematicBody2D

var speed = 50
var velocity = Vector2()
export var direction = 1

func _ready():
	if direction ==-1:
		$AnimatedSprite.flip_h = true
	
		

func _physics_process(delta):
	if is_on_wall():
		direction = direction*-1
		$AnimatedSprite.flip_h = not $AnimatedSprite.flip_h
		
	velocity.y += 20
	velocity.x = speed*direction
	velocity = move_and_slide(velocity, Vector2.UP)
	


func _on_top_checker_body_entered(body):
	for i in get_slide_count():
		var collision = get_slide_collision(i)
		if collision.collider.name == "TileMap" || collision.collider.name == "TileMap2":
			speed = 0
			set_collision_layer_bit(0,false)
			set_collision_mask_bit(0,false)
			$top_checker.set_collision_layer_bit(0,false)
			$top_checker.set_collision_mask_bit(0,false)
		print("Collided with: ", collision.collider.name)
	
	
	
	
	
	
	
