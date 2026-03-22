using System;
using System.Collections.Generic;
using Godot;

public partial class Niveau : Node2D
{
    private bool _enPause = true;
    public static bool IsGameStarted { get; private set; } = false;
    public static event Action OnGameStarted;

    public override void _Ready()
    {
        IsGameStarted = false;
        SetChildrenPaused(true);
        GD.Print("Niveau prêt");
    }

    public override void _Input(InputEvent @event)
    {
        if (_enPause && @event.IsActionPressed("ui_select"))
        {
            _enPause = false;
            IsGameStarted = true;
            SetChildrenPaused(false);
            GD.Print("Niveau démarré");
            OnGameStarted?.Invoke();
        }
    }

    private void SetChildrenPaused(bool paused)
    {
        foreach (Node child in GetChildren())
        {
            child.ProcessMode = paused ? ProcessModeEnum.Disabled : ProcessModeEnum.Inherit;
        }
    }
}
