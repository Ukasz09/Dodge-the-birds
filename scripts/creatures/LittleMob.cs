public class LittleMob : Mob
{
    public static string ScenePath = "res://scripts/creatures/LittleMob.tscn";
    private readonly string[] _mobTypes = {"grey", "pink", "yellow", "brown", "duck", "green","dragon"};

    public override void _Ready()
    {
        InitMob(_mobTypes);
    }
}