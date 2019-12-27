using Godot;
using System;

public class HUD : CanvasLayer
{
    [Signal]
    public delegate void StartGame();

 public override void _Ready()
    {
		GetNode<Button>("EndButton").Hide();
    }


public void ShowMessage(string text)
{
    var messageLabel = GetNode<Label>("MessageLabel");
    messageLabel.Text = text;
    messageLabel.Show();

    GetNode<Timer>("MessageTimer").Start();
}

async public void ShowGameOver()
{
    ShowMessage("Game Over");

    var messageTimer = GetNode<Timer>("MessageTimer");
    await ToSignal(messageTimer, "timeout");

    var messageLabel = GetNode<Label>("MessageLabel");
    messageLabel.Text = "Dodge the birds bro !";
    messageLabel.Show();

    GetNode<Button>("StartButton").Show();
	GetNode<Button>("EndButton").Show();
}

public void UpdateScore(int score)
{
    GetNode<Label>("ScoreLabel").Text = score.ToString();
}

public void OnStartButtonPressed()
{
    GetNode<Button>("StartButton").Hide();
	GetNode<Button>("EndButton").Hide();
    EmitSignal("StartGame");
}

public void OnMessageTimerTimeout()
{
    GetNode<Label>("MessageLabel").Hide();
}

private void OnEndButtonPressed()
{
    GetTree().Quit();
}
}