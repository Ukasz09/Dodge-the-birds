using Godot;

public class Hud : CanvasLayer
{
    [Signal]
    public delegate void StartGame();

    private const string GameOverMsg = "Game over";
    private const string AimOfTheGameMsg = "Dodge the birds !";

    private Button _endButton;
    private Button _startButton;
    private Timer _msgTimer;
    private Label _msgLabel;
    private Label _scoreLabel;
    private AnimatedSprite _coin;

    public override void _Ready()
    {
        InitFields();
        ShowMessage(AimOfTheGameMsg);
    }

    private void InitFields()
    {
        _endButton = GetNode<Button>("EndButton");
        _msgLabel = GetNode<Label>("MessageLabel");
        _msgTimer = GetNode<Timer>("MessageTimer");
        _startButton = GetNode<Button>("StartButton");
        _scoreLabel = GetNode<Label>("ScoreLabel");
        _coin = GetNode<AnimatedSprite>("CoinSprite");
    }

    public async void ShowGameOver()
    {
        ShowMessage(GameOverMsg);
        _msgTimer.Start();
        await ToSignal(_msgTimer, "timeout");

        ShowMessage(AimOfTheGameMsg);
        _startButton.Show();
        _endButton.Show();
        StopCoinAnimation();
    }

    public void ShowMessage(string text)
    {
        _msgLabel.Text = text;
        _msgLabel.Show();
    }

    private void StopCoinAnimation()
    {
        _coin.Stop();
        _coin.SetFrame(0);
    }


    public void UpdateScore(int score)
    {
        _scoreLabel.Text = score.ToString();
    }

    public void OnStartButtonPressed()
    {
        _startButton.Hide();
        _endButton.Hide();
        _coin.Play();
        EmitSignal("StartGame");
    }

    public void OnMessageTimerTimeout()
    {
        HideMsgLabel();
    }

    public void HideMsgLabel()
    {
        _msgLabel.Hide();
    }

    private void OnEndButtonPressed()
    {
        GetTree().Quit();
    }
}