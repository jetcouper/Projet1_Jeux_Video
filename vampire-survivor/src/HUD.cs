using Godot;

public partial class HUD : Node
{
    [Export]
    public Joueur player;
    private TextureProgressBar HealthBar;
    private TextureProgressBar XpBar;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //connections aux signaux du joueur
        player.HealthChanged += OnHealthChanged;
        player.XpChanged += OnXpChanged;
        player.LevelUpSignal += OnLevelUp;

        HealthBar = GetNode<TextureProgressBar>("%HealthBar");
        HealthBar.MinValue = 0;
        HealthBar.MaxValue = player.MaxHealth;
        HealthBar.Value = player.Health;
        GD.Print(player.Health, player.MaxHealth);

        XpBar = GetNode<TextureProgressBar>("%XpBar");
        XpBar.MinValue = 0;
        XpBar.MaxValue = player.XpForNextLevel;
        XpBar.Value = player.Xp;


    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public void OnHealthChanged()
    {
        HealthBar.Value = player.Health;
        HealthBar.MaxValue = player.MaxHealth;
    }

    public void OnXpChanged()
    {
        XpBar.Value = player.Xp;
        XpBar.MaxValue = player.XpForNextLevel;
    }

    public void OnLevelUp()
    {
        XpBar.MinValue = player.XpForNextLevel;
    }
}
