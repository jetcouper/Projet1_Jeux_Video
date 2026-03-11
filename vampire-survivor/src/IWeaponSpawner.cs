using Godot;
using System;

public interface IWeaponSpawner
{
    void Spawn(Node2D joueur, float typeWeapon);
}