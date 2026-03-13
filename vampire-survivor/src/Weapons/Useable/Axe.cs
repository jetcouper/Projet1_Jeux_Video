using System;
using Godot;

public partial class Axe : Node2D, IUseable
{
    [ExportGroup("Internal")]
    [Export]
    private DpmMovementAx DpmMovementAx;

    public Node2D Player
    {
        get { return DpmMovementAx.Player; }
        set { DpmMovementAx.Player = value; }
    }

    public void Use()
    {
        DpmMovementAx.StartSwing();
    }

    public void setPlayer(Node2D player)
    {
        Player = player;
    }
}
