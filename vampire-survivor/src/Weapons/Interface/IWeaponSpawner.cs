using Godot;
using System;

public interface IWeaponSpawner
{


    public enum Pattern
    {
        Linear,
        Corkscrew,
        None,
    }


    void Spawn(Pattern pattern = Pattern.None, Node2D target = null);
    void Activate();
}