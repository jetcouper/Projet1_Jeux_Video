using System;
using Godot;
using Utils;

public partial class Sword : Node2D, IUseable
{
    [ExportGroup("Internal")]
    [Export]
    private DpmMovementSword DpmMovementSword;

    public Node2D Player
    {
        get { return DpmMovementSword.EnsureValid().Player; }
        set { DpmMovementSword.EnsureValid().Player = value; }
    }

    public void Use()
    {
        DpmMovementSword.StartSwing();
    }

    public void setPlayer(Node2D player)
    {
        Player = player;
    }
}
