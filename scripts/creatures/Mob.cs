using System;
using Godot;

public abstract class Mob : RigidBody2D
{
    [Export] public int MinSpeed = 150;
    [Export] public int MaxSpeed = 250;

    private string _animatedSpritePath = "AnimatedSprite";
    private AnimatedSprite _animatedSprite;
    private readonly Random _random = new Random();
    private string[] _mobTypes;

    protected void InitMob(string[] mobTypes)
    {
        InitMob(mobTypes, _animatedSpritePath);
    }

    protected void InitMob(string[] mobTypes, string animatedSpritePath)
    {
        _animatedSprite = GetNode<AnimatedSprite>(animatedSpritePath);
        SetMobTypes(mobTypes);
        SetRandomSpriteImage();
        _animatedSprite.Play();
    }


    private void SetMobTypes(string[] mobTypes)
    {
        _mobTypes = mobTypes;
    }

    private void SetRandomSpriteImage()
    {
        _animatedSprite.Animation = _mobTypes[_random.Next(0, _mobTypes.Length)];
    }

    private void OnVisibilityScreenExited()
    {
        QueueFree();
    }

    public void OnStartGame()
    {
        QueueFree();
    }
}