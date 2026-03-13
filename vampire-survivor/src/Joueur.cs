using System;
using Godot;
using Utils;
using static MedPositions;

public partial class Joueur : Node2D
{
    [ExportGroup("External")]
    [Export]
    public MedPositions MedPositions;

    [Export]
    public bool IsActive
    {
        get => SimplePlayer.EnsureValid().IsActive;
        set { SimplePlayer.EnsureValid().IsActive = value; }
    }
    public TileMapLayer CollisionLayer
    {
        get => SimplePlayer.EnsureValid().collisionLayer;
        set { SimplePlayer.EnsureValid().collisionLayer = value; }
    }

    [ExportGroup("Internal")]
    [Export]
    SimplePlayer SimplePlayer;

    [Export]
    private float CameraZoom = 3f;
    private Camera2D _camera;

    [Export]
    public float gatherRadius = 10f;

    public int score = 0;

    public override void _Ready()
    {
        base._Ready();
        CollisionLayer =
            MedPositions.choisirObjet(EAlgoSelectionObjet.eCollisionLayer, new(0, 0))
            as TileMapLayer;
        _camera = GetNode<Camera2D>("Camera2D");
        _camera.MakeCurrent();
        _camera.Zoom = new Vector2(CameraZoom, CameraZoom);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        _camera.Zoom = new Vector2(CameraZoom, CameraZoom);
    }

    public void addScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }
}
