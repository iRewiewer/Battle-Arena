using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class WorldManager : Node2D
{
	#region Public Vars
	[Export]
	public PackedScene enemyScene;
	#endregion

	#region Private Vars
	private List<(float, float)> spawnLocations = new List<(float, float)>();
	private Timer enemySpawnTimer = new Timer();
	private Random random = new Random();
	private int enemyCount = 0;
	#endregion

	#region Public Methods
	public override void _Ready()
	{
		if (enemyScene == null)
		{
			ErrorNoEnemyScene();
			return;
		}
		
		enemySpawnTimer = (Timer)this.FindChild("EnemySpawnTimer");
		this.FindChild("SpawnLocations").GetChildren().ToList().ForEach(node => {
			spawnLocations.Add((((Node2D)node).Position.X, ((Node2D)node).Position.Y));
		});

		enemySpawnTimer.Start(5);
	}
	#endregion

	#region Private Methods
	#endregion

	public override void _Process(double delta)
	{
		if(enemySpawnTimer.TimeLeft == 0)
		{
			Enemy enemy = enemyScene.Instantiate<Enemy>();
			(float, float) randomSpawnLocation = spawnLocations[random.Next(5)];
			enemyCount += 1;
			GD.Print($"Enemy spawned at {randomSpawnLocation}. Enemies remaining: {enemyCount}");
			enemy.Position = new Vector2(randomSpawnLocation.Item1, randomSpawnLocation.Item2);
			this.AddChild(enemy);
			enemySpawnTimer.Start(5);
		}
	}

	#region Error Handling
	private void ErrorNoEnemyScene()
	{
		GD.Print($"{this.Name} has no enemy scene assigned.");
	}
	#endregion
}
