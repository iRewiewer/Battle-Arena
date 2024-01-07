using Godot;

public partial class WorldManager : Node2D
{
	#region Public Vars
	[Export]
	public PackedScene enemyScene;
	#endregion

	#region Private Vars
	#endregion

	#region Public Methods
	public override void _Ready()
	{
		if (enemyScene == null)
		{
			ErrorNoEnemyScene();
			return;
		}
		Enemy enemy = enemyScene.Instantiate<Enemy>();
		enemy.Position = new Vector2(550, 50);
		this.AddChild(enemy);
	}
	#endregion

	#region Private Methods
	#endregion

	#region Error Handling
	private void ErrorNoEnemyScene()
	{
		GD.Print($"{this.Name} has no enemy scene assigned.");
	}
	#endregion
}
