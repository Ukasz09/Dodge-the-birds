using Godot;
using System;

public class Main : Node2D
{
    [Export] public PackedScene Mob;

    private int _score;
    private Random _random = new Random();

    public override void _Ready()
    {
    }

    private float RandRange(float min, float max)
    {
        return (float) _random.NextDouble() * (max - min) + min;
    }

    private void gameOver()
    {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Timer>("ScoreTimer").Stop();
        GetNode<HUD>("HUD").ShowGameOver();
        GetNode<AudioStreamPlayer>("Music").Stop();
        GetNode<AudioStreamPlayer>("DeathSound").Play();
    }

    public void NewGame()
    {
        _score = 0;
        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Position2D>("StartPosition");
        player.Start(startPosition.Position);

        GetNode<Timer>("StartTimer").Start();

        GetNode<AudioStreamPlayer>("Music").Play();


        var hud = GetNode<HUD>("HUD");
        hud.UpdateScore(_score);
        hud.ShowMessage("Get Ready!");
    }

    private void onScoreTimerTimeout()
    {
        _score++;
        GetNode<HUD>("HUD").UpdateScore(_score);
        var MobTimer = GetNode<MobTimer>("MobTimer");
        MobTimer.SetWaitTime(MobTimer.StartedSpawnCooldown / (_score + 1));
    }


    private void onStartTimerTimeout()
    {
        GetNode<Timer>("MobTimer").Start();
        GetNode<Timer>("ScoreTimer").Start();
    }

    private void onMobTimerTimeout()
    {
        // Choose a random location on Path2D.
        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
        mobSpawnLocation.SetOffset(_random.Next());

        // Create a Mob instance and add it to the scene.
        //todo: tmp - dac abstrakcje na moba
        RigidBody2D mobInstance;
        
        if (_score % 10 == 0)
        {
        Mob = (PackedScene) ResourceLoader.Load("res://BigMob.tscn");
         mobInstance =(BigMob) Mob.Instance(); //(RigidBody2D)Mob.Instance();
        //todo: rzutowanie na littlemob
        }
        else
        {
            Mob = (PackedScene) ResourceLoader.Load("res://LittleMob.tscn");
             mobInstance =(LittleMob) Mob.Instance(); //(RigidBody2D)Mob.Instance();
        }
        AddChild(mobInstance);

        // Set the mob's direction perpendicular to the path direction.
        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

        // Set the mob's position to a random location.
        mobInstance.Position = mobSpawnLocation.Position;

        // Add some randomness to the direction.
        direction += RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mobInstance.Rotation = direction;

        // Choose the velocity.
        mobInstance.SetLinearVelocity(
            new Vector2(RandRange(400, 900), 0).Rotated(direction));
        //todo: speed tmp 
        
        GetNode("HUD").Connect("StartGame", mobInstance, "OnStartGame");
    }

}