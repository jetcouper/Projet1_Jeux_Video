using Godot;
using System;
using static IWeaponSpawner;

public partial class DcmBulletSpawner : Node2D, IWeaponSpawner
{
    [ExportGroup("External")]
    [Export]
    public PackedScene WeaponScene;

    [Export]
    public MedWeaponSpawner MedWeaponSpawner;

    private Node2D player;

    private IUseable activeWeapon;

    private bool isActivated = false;

    private bool isSpawning = false;

    private int spawnToken = 0;


    public async void Spawn(Pattern typeWeapon, Node2D target = null)
    {
        float angleStep = 2 * Mathf.Pi / 12;
        while (true)
        {
            for (int i = 0; i < 12; i++)
            {
                float angle = i * angleStep;
                SpawnBullet(typeWeapon, angle);
            }
			
            await ToSignal(GetTree().CreateTimer(2.5f), "timeout");
        }
    }

    public async void SpawnSequential(Pattern typeWeapon, Node2D target = null)
    {
        float delay = 1.5f;
        float angleStep = 2 * Mathf.Pi / 12;
        int index = 0;
        while (true)
        {
            float angle = index * angleStep;
            SpawnBullet(typeWeapon, angle);
            index = (index + 1) % 12;
            await ToSignal(GetTree().CreateTimer(delay), "timeout");
        }
    }

    public async void SpawnTargeted(Pattern typeWeapon)
    {
        float delay = 1.5f;
        if (player == null)
        {
            player = MedWeaponSpawner.GetPlayer();
        }
        while (true)
        {
            Node2D TargetNode = MedWeaponSpawner.GetTarget();
            if (TargetNode != null)
            {
                Vector2 direction = (TargetNode.GlobalPosition - player.GlobalPosition).Normalized();
                float angle = Mathf.Atan2(direction.Y, direction.X);
                SpawnBullet(typeWeapon, angle, TargetNode);
            }
            await ToSignal(GetTree().CreateTimer(delay), "timeout");
        }
    }

    private void SpawnBullet(Pattern typeWeapon, float angle, Node2D TargetNode = null)
    {
        if (isActivated)
        {
            if (player == null)
            {
                player = MedWeaponSpawner.GetPlayer();
            }
            Node2D weapon = WeaponScene.Instantiate<Node2D>();

            if (weapon is Bullet bullet)
            {
                bullet.startingPosition = angle;
                bullet.BulletPattern = typeWeapon;

                bullet.setPlayer(player);
                bullet.TargetNode = TargetNode;
                activeWeapon = bullet;
                bullet.TreeEntered += () => bullet.Use();
            }

            CallDeferred(Node.MethodName.AddChild, weapon);
        }
    }

    public void Activate()
    {
        isActivated = true;
        activeWeapon?.Use();
    }

}
