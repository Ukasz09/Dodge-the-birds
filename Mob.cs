using Godot;
using System;

public class Mob : RigidBody2D
{	
     [Export]
    public int MinSpeed = 150; // Minimum speed range.

    [Export]
    public int MaxSpeed = 250; // Maximum speed range.

    private String[] _mobTypes = {"green", "pink", "grey", "yellow"};
	
	static private Random _random = new Random();

public override void _Ready()
{
    GetNode<AnimatedSprite>("AnimatedSprite").Animation = _mobTypes[_random.Next(0, _mobTypes.Length)];
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
    var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

        animatedSprite.Play();
}

}