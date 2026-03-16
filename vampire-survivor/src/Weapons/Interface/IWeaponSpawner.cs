using Godot;
using System;

public interface IWeaponSpawner
{
    void Spawn(float typeWeapon, int count = 0);
    void Activate();
}