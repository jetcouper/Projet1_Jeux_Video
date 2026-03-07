using System;
using Godot;

public partial class Sword : Node2D, IUseable
{
    [ExportGroup("Internal")]
    [Export]
    private DpmMovement DpmMovement;

    public void Use()
    {
        DpmMovement.StartSwing();
    }

    public void setPlayer(Node2D player)
    {
        DpmMovement.Player = player;
    }
}
