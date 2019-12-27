using Godot;
using System;

public class Player : Area2D {
	[Signal]
public delegate void Hit();

	[Export]
    public int Speed = 400;
	
	private Vector2 _screenSize;
	
	public override void _Ready() {
        _screenSize = GetViewport().GetSize();
		Hide();
		GetNode<AnimatedSprite>("AnimatedSprite").Play();
		
	}
	
	public override void _Process(float delta)
	{
    var velocity = new Vector2(); // The player's movement vector.

    if (Input.IsActionPressed("ui_right"))
    {
        velocity.x += 1;
    }

    if (Input.IsActionPressed("ui_left"))
    {
        velocity.x -= 1;
    }

    if (Input.IsActionPressed("ui_down"))
    {
        velocity.y += 1;
    }

    if (Input.IsActionPressed("ui_up"))
    {
        velocity.y -= 1;
    }

    var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

    if (velocity.Length() > 0)
    {
        velocity = velocity.Normalized() * Speed;
     
    }
	
	  // animatedSprite.Play();
	
	
	Position += velocity * delta;
	Position = new Vector2(
    	x: Mathf.Clamp(Position.x, 0, _screenSize.x),
    	y: Mathf.Clamp(Position.y, 0, _screenSize.y)
	);
	

    animatedSprite.Animation = "fly";
	if(velocity.x!=0)
    animatedSprite.FlipH = velocity.x<0;
    animatedSprite.FlipV = false;
}

private void _on_Player_body_entered(object body)
{
   Hide(); // Player disappears after being hit.
    EmitSignal("Hit");
    GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
}
public void Start(Vector2 pos)
{
    Position = pos;
    Show();
    GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
}
}
