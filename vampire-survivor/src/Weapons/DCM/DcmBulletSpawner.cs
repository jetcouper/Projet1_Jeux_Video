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
        //Utilisation de l'IA pour les tokens de spawn afin d'assurer que les anciennes tâches de spawn ne continuent pas à faire spawn des projectiles après un changement d'arme ou une désactivation du spawner 
		spawnToken++;
        int myToken = spawnToken;
        isSpawning = true;
        float angleStep = 2 * Mathf.Pi / 12;
        while (isSpawning && myToken == spawnToken)
        {
            for (int i = 0; i < 12; i++)
            {
                
				if (!isSpawning || myToken != spawnToken) break;

                float angle = i * angleStep;
                SpawnBullet(typeWeapon, angle);
            }
			
            if (!isSpawning || myToken != spawnToken) break;
            await ToSignal(GetTree().CreateTimer(2.5f), "timeout");
        }
    }

    public async void SpawnSequential(Pattern typeWeapon, Node2D target = null)
    {
        //Utilisation de l'IA pour les tokens de spawn afin d'assurer que les anciennes tâches de spawn ne continuent pas à faire spawn des projectiles après un changement d'arme ou une désactivation du spawner 
		spawnToken++;
        int myToken = spawnToken;
        isSpawning = true;
        float delay = 1.5f;
        float angleStep = 2 * Mathf.Pi / 12;
        int index = 0;
        while (isSpawning && myToken == spawnToken)
        {
            float angle = index * angleStep;
            SpawnBullet(typeWeapon, angle);
            index = (index + 1) % 12;
            if (!isSpawning || myToken != spawnToken) break;
            await ToSignal(GetTree().CreateTimer(delay), "timeout");
        }
    }

    public async void SpawnTargeted(Pattern typeWeapon)
    {
        //Utilisation de l'IA pour les tokens de spawn afin d'assurer que les anciennes tâches de spawn ne continuent pas à faire spawn des projectiles après un changement d'arme ou une désactivation du spawner 
		spawnToken++;
        int myToken = spawnToken;
        isSpawning = true;
        float delay = 1.5f;
        if (player == null)
        {
            player = MedWeaponSpawner.GetPlayer();
        }
        while (isSpawning && myToken == spawnToken)
        {
            Node2D TargetNode = MedWeaponSpawner.GetTarget();
            if (TargetNode != null)
            {
                Vector2 direction = (TargetNode.GlobalPosition - player.GlobalPosition).Normalized();
                float angle = Mathf.Atan2(direction.Y, direction.X);
                SpawnBullet(typeWeapon, angle, TargetNode);
            }
            if (!isSpawning || myToken != spawnToken) break;
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
            AddChild(weapon);

            if (weapon is Bullet bullet)
            {
                bullet.startingPosition = angle;
                bullet.BulletPattern = typeWeapon;

                bullet.setPlayer(player);
                bullet.TargetNode = TargetNode;
                activeWeapon = bullet;
                activeWeapon.Use();
            }
        }
    }

    public void Activate()
    {
        isActivated = true;
        activeWeapon?.Use();
    }

    public void RemoveActiveWeapon()
    {
        //Utilisation de l'IA pour les tokens de spawn afin d'assurer que les anciennes tâches de spawn ne continuent pas à faire spawn des projectiles après un changement d'arme ou une désactivation du spawner 
		isSpawning = false;
        spawnToken++;
        if (activeWeapon is Node node && IsInstanceValid(node))
        {
            node.QueueFree();
            activeWeapon = null;
        }
    }

}
