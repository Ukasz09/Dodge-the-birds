using Godot;
using System;

public class Main : Node2D
{
    private const string PrepareToGameMsg = "Get ready ...";
    private const float BigMobSpawnProbability = 15;

    private readonly Random _random = new Random();
    private int _score;
    private bool gameIsOver;
    private MobTimer _mobTimer;
    private Timer _scoreTimer;
    private Hud _hud;
    private AudioStreamPlayer _backgroundSound;
    private AudioStreamPlayer _houseflySound;
    private AudioStreamPlayer _rainforestSound;
    private AudioStreamPlayer _waterfallSound;
    private AudioStreamPlayer _gameOverSound;
    private Player _player;
    private PathFollow2D _mobSpawnLocation;

    private PackedScene Mob;


    public override void _Ready()
    {
        InitFields();
        _rainforestSound.Play();
        _waterfallSound.Play();
        gameIsOver = false;
    }

    private void InitFields()
    {
        _mobTimer = GetNode<MobTimer>("MobTimer");
        _scoreTimer = GetNode<Timer>("ScoreTimer");
        _hud = GetNode<Hud>("HUD");
        _backgroundSound = GetNode<AudioStreamPlayer>("BackgroundSound");
        _houseflySound = GetNode<AudioStreamPlayer>("HouseflySound");
        _gameOverSound = GetNode<AudioStreamPlayer>("GameOverSound");
        _rainforestSound = GetNode<AudioStreamPlayer>("RainforestSound");
        _waterfallSound = GetNode<AudioStreamPlayer>("WaterfallSound");
        _player = GetNode<Player>("Player");
        _mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
    }

    private void GameOver()
    {
        gameIsOver = true;
        _mobTimer.Stop();
        _scoreTimer.Stop();
        _hud.ShowGameOver();
        _houseflySound.Stop();
        _backgroundSound.Stop();
        _gameOverSound.Play();
    }

    public void NewGame()
    {
        gameIsOver = false;
        _score = 0;
        SpawnPlayer();
        GetNode<Timer>("StartTimer").Start();
        _backgroundSound.Play();
        _houseflySound.Play();
        ShowStartGameMsg();
    }

    private void SpawnPlayer()
    {
        var playerStartPosition = GetNode<Position2D>("StartPosition");
        _player.Start(playerStartPosition.Position);
    }

    private void ShowStartGameMsg()
    {
        _hud.UpdateScore(_score);
        _hud.ShowMessage(PrepareToGameMsg);
    }

    private void OnScoreTimerTimeout()
    {
        _score++;
        _hud.UpdateScore(_score);
        UpdateMobSpawnFrequency();
    }

    private void UpdateMobSpawnFrequency()
    {
        if (_score > 10)
            _mobTimer.SetWaitTime(_mobTimer.StartedSpawnCooldown / _score);
        else _mobTimer.SetWaitTime(_mobTimer.StartedSpawnCooldown / 10);
    }

    private void OnStartTimerTimeout()
    {
        _mobTimer.Start();
        _scoreTimer.Start();
        _hud.HideMsgLabel();
    }

    private void OnMobTimerTimeout()
    {
        var spawnedMob = SpawnMob();
        _hud.Connect("StartGame", spawnedMob, "OnStartGame");
    }

    private Mob SpawnMob()
    {
        var mobInstance = GetMobInstance(BigMobSpawnProbability);

        _mobSpawnLocation.SetOffset(_random.Next());
        SetRandomMobSpawnPosition(mobInstance, _mobSpawnLocation);
        float direction = CalculateMobDirection();
        mobInstance.Rotation = direction;
        SetRandomVelocity(mobInstance, direction, mobInstance.MinSpeed, mobInstance.MaxSpeed);

        AddChild(mobInstance);

        return mobInstance;
    }

    private void SetRandomMobSpawnPosition(Mob mobInstance, PathFollow2D mobSpawnLocation)
    {
        mobInstance.Position = mobSpawnLocation.Position;
    }

    private Mob GetMobInstance(float bigMobProbabilityInPercents)
    {
        if (bigMobProbabilityInPercents > 100 || bigMobProbabilityInPercents < 0)
            throw new ArgumentOutOfRangeException("Percents must be between 0 to 100");

        if (_random.Next(0, 100) < bigMobProbabilityInPercents)
            Mob = (PackedScene) ResourceLoader.Load(BigMob.ScenePath);
        else Mob = (PackedScene) ResourceLoader.Load(LittleMob.ScenePath);

        return (Mob) Mob.Instance();
    }

    private float CalculateMobDirection()
    {
        float direction = _mobSpawnLocation.Rotation + Mathf.Pi / 2 + RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        return direction;
    }

    private void SetRandomVelocity(Mob mobInstance, float direction, float min, float max)
    {
        Vector2 velocityVector = new Vector2(RandRange(min, max), 0).Rotated(direction);
        mobInstance.SetLinearVelocity(velocityVector);
    }

    private float RandRange(float min, float max)
    {
        return (float) _random.NextDouble() * (max - min) + min;
    }

    private void OnHouseflySoundFinished()
    {
        if (!gameIsOver)
            _houseflySound.Play();
    }

    private void OnRainforestSoundFinished()
    {
        _rainforestSound.Play();
    }

    private void OnWaterfallSoundFinished()
    {
        _waterfallSound.Play();
    }
}