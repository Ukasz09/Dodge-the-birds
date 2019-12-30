using Godot;
using System;

public class MobTimer : Timer
{
    [Export] public float StartedSpawnCooldown = 10;

    public override void _Ready()
    {
        SetWaitTime(1);
    }
}