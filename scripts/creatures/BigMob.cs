public class BigMob : Mob
{
    public static string ScenePath = "res://scripts/creatures/BigMob.tscn";
    private readonly string[] _mobTypes = {"green", "blue", "pink", "yellow"};

    public override void _Ready()
    {
        InitMob(_mobTypes);
    }
}