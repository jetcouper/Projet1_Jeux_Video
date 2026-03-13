using System;
using Godot;

public partial class Sword : Node2D, IUseable
{
    [ExportGroup("Internal")]
    [Export]
    private DpmMovementSword DpmMovementSword;

    public void Use()
    {
        DpmMovementSword.StartSwing();
    }

    public void setPlayer(Node2D player)
    {
        DpmMovementSword.Player = player;
    }
}
