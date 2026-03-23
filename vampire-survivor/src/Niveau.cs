using System;
using System.Collections.Generic;
using Godot;

public partial class Niveau : Node2D
{
    private bool _enPause = true;
    public static bool IsGameStarted { get; private set; } = false;
    public static event Action OnGameStarted;
    public static event Action OnVictoire;
    private bool _partieTerminee = false;

    [ExportGroup("Internal")]
    [Export]
    private Timer _timer;

    [Export]
    private Label _timerLabel;

    public override void _Ready()
    {
        Joueur.OnPlayerDied += _onPlayerDeath;
        Joueur.OnPlayerVictory += OnPlayerVictoryHandler;

        GetTree().CallGroup("debut", "show");
        IsGameStarted = false;
        SetChildrenPaused(true);
        GD.Print("Niveau prêt");
        _timer.Timeout += _onTimer_timeout;
    }

    public override void _Process(double delta)
    {
        if (_timer != null && _timerLabel != null)
        {
            if (_timer.TimeLeft > 0)
            {
                double timeLeft = _timer.TimeLeft;
                int minutes = (int)(timeLeft / 60.0);
                int seconds = (int)(timeLeft % 60.0);
                _timerLabel.Text = $"{minutes:00}:{seconds:00}"; // Formats as 05:00, etc.
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (_enPause && @event.IsActionPressed("ui_select") && _partieTerminee == false)
        {
            GetTree().CallGroup("debut", "hide");
            GetTree().CallGroup("hud", "show");
            _enPause = false;
            IsGameStarted = true;
            SetChildrenPaused(false);
            GD.Print("Niveau démarré");
            OnGameStarted?.Invoke();
            _timer.Start();
        }
    }

    private void SetChildrenPaused(bool paused)
    {
        foreach (Node child in GetChildren())
        {
            child.ProcessMode = paused ? ProcessModeEnum.Disabled : ProcessModeEnum.Inherit;
        }
    }

    public static void TriggerVictoire()
    {
        OnVictoire?.Invoke();
    }

    public void _onTimer_timeout()
    {
        _partieTerminee = true;
        _enPause = true;
        IsGameStarted = false;
        OnVictoire?.Invoke();
        //GD.Print("Temps écoulé : " + _timer.TimeLeft);
    }

    public void OnPlayerVictoryHandler()
    {
        SetChildrenPaused(true);
        GetTree().CallGroup("finVictoire", "show");
    }

    public void _onPlayerDeath()
    {
        _partieTerminee = true;
        GetTree().CallGroup("finDefaite", "show");
        _enPause = true;
        IsGameStarted = false;
        SetChildrenPaused(true);
    }
}
