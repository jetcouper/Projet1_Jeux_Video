using Godot;
using System;

public interface IDPMSimpleMovement
{
    Node2D Player { get; set; }
    void StartSwing();
}
