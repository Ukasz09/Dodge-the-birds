using System;
using Godot;

public class Player : Area2D
{
    [Signal]
    public delegate void Hit();

    [Export] private int _speed = 400;

    private string _animatedSpritePath = "AnimatedSprite";
    private string _collisionShapePath = "CollisionShape2D";
    private readonly string[] _animationTypes = {"fly"};
    private readonly Random _random = new Random();

    private Vector2 _screenSize;
    private AnimatedSprite _animatedSprite;
    private Vector2 _velocity;
    private CollisionShape2D _collisionShape;


    public override void _Ready()
    {
        InitFields();
        PlayAnimation();
        ChangeAnimationType(0);
        Hide();
    }

    private void InitFields()
    {
        _velocity = new Vector2();
        _animatedSprite = GetNode<AnimatedSprite>(_animatedSpritePath);
        _screenSize = GetViewport().GetSize();
        _collisionShape = GetNode<CollisionShape2D>(_collisionShapePath);
    }

    private void PlayAnimation()
    {
        _animatedSprite.Play();
    }

    private void ChangeAnimationType(int index)
    {
        _animatedSprite.Animation = _animationTypes[index];
    }

    public override void _Process(float delta)
    {
        CalculateVelocity();
        SetPositionByVelocity(delta);
        ClampPosition();
        FlipSpriteByVelocity();
    }

    private void CalculateVelocity()
    {
        SetVelocityByMovement();
        NormalizeVelocityVector();
    }

    private void SetVelocityByMovement()
    {
        _velocity.Set(0, 0);

        if (Input.IsActionPressed("ui_right"))
        {
            _velocity.x += 1;
        }

        if (Input.IsActionPressed("ui_left"))
        {
            _velocity.x -= 1;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            _velocity.y += 1;
        }

        if (Input.IsActionPressed("ui_up"))
        {
            _velocity.y -= 1;
        }
    }

    private void NormalizeVelocityVector()
    {
        if (_velocity.Length() > 0)
        {
            _velocity = _velocity.Normalized() * _speed;
        }
    }

    private void SetPositionByVelocity(float delta)
    {
        Position += _velocity * delta;
    }

    private void ClampPosition()
    {
        Position = new Vector2(
            x: Mathf.Clamp(Position.x, 0, _screenSize.x),
            y: Mathf.Clamp(Position.y, 0, _screenSize.y)
        );
    }

    private void FlipSpriteByVelocity()
    {
        if (_velocity.x != 0)
            _animatedSprite.FlipH = (_velocity.x < 0);
    }

    private void OnPlayerBodyEntered(object body)
    {
        Hide();
        EmitSignal("Hit");
        DisableCollisionShape(true);
    }

    private void DisableCollisionShape(bool viaCallDeferred)
    {
        if (viaCallDeferred)
            _collisionShape.SetDeferred("disabled", true);
        else _collisionShape.Disabled = false;
    }

    public void Start(Vector2 pos)
    {
        Position = pos;
        Show();
        DisableCollisionShape(false);
    }
}