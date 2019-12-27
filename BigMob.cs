using Godot;
using System;

public class BigMob : RigidBody2D
{
private AnimatedSprite animatedSprite;

     [Export]
    public int MinSpeed = 150; // Minimum speed range.

    [Export]
    public int MaxSpeed = 250; // Maximum speed range.

    //private String[] _smallMobTypes = {"pink", "grey", "yellow"};
	private String[] _bigMobTypes = {"green","blue"};
	
	static private Random _random = new Random();

public override void _Ready()
{
	//if(_random.Next(0,5)==0){
		animatedSprite=GetNode<AnimatedSprite>("AnimatedSprite");
		animatedSprite.Animation = _bigMobTypes[_random.Next(0, _bigMobTypes.Length)];
		//}
		//else {
		//	animatedSprite=GetNode<AnimatedSprite>("AnimatedSmall");
			//animatedSprite.Animation = _smallMobTypes[_random.Next(0, _smallMobTypes.Length)];
			//}
			
		
	 
	
}

private void OnVisibilityScreenExited()
{
    QueueFree();
}

public void OnStartGame()
{
    QueueFree();
}


public override void _Process(float delta)
{
    //var animatedSprite = GetNode<AnimatedSprite>("AnimatedSmall");

        animatedSprite.Play();
}
}
