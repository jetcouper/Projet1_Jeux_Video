using System;
using System.Collections.Generic;
using Godot;

public partial class Niveau : Node2D
{
    private bool _enPause = true;

    public override void _Ready()
    {
        SetChildrenPaused(true);
        GD.Print("Niveau prêt");
    }

    public override void _Input(InputEvent @event)
    {
        if (_enPause && @event.IsActionPressed("ui_select"))
        {
            _enPause = false;
            SetChildrenPaused(false);
            GD.Print("Niveau démarré");
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
