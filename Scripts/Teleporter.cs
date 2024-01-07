using Godot;

public partial class Teleporter : Area2D
{
	#region Public Vars
	[Export]
	public Area2D targetTeleporter { get; set; }
	public int cooldown = 15; // seconds
	#endregion

	#region Private Vars
	private string baseName;
	private Timer timer;
	private Label label;
	private CharacterBody2D player;
	private Teleporter targetScript;

	private bool hasCollided = false;
	private bool hasTeleported = false;
	#endregion

	#region Public Methods
	public override void _Ready()
	{
		if (targetTeleporter == null)
		{
			ErrorNoTargetPortal();
			return;
		}

		timer = this.GetChild<Timer>(1);
		baseName = this.Name;
		targetScript = GetNodeOrNull<Teleporter>(targetTeleporter.GetPath());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (targetTeleporter == null)
		{
			ErrorNoTargetPortal();
			return;
		}

		if (timer.TimeLeft == 0)
		{
			hasTeleported = false;
			hasCollided = false;
			return;
		}

		if(timer.TimeLeft != cooldown)
		{
			return;
		}

		if (targetScript.hasCollided)
		{
			hasCollided = true;
			hasTeleported = true;
			timer.Start(cooldown);
			return;
		}

		if (hasCollided && !hasTeleported)
		{
			// Teleport Player
			player.Position = new Vector2(targetTeleporter.Position.X, targetTeleporter.Position.Y);
			hasTeleported = true;
		}
	}
	#endregion

	#region Private Methods
	// Called on collision
	private void _on_body_entered(CharacterBody2D body)
	{
		if(targetTeleporter == null)
		{
			ErrorNoTargetPortal();
			return;
		}

		if((hasCollided && hasTeleported) || (targetScript.hasCollided && targetScript.hasTeleported))
		{
			return;
		}

		player = body;

		if(player.Name != "Player")
		{
			return;
		}

		// Start cooldown
		timer.Start(cooldown);
		hasCollided = true;
		GD.Print($"Teleported {body.Name} from {this.Name} to {targetTeleporter.Name}");
	}
	#endregion

	#region Error Handlers
	private void ErrorNoTargetPortal()
	{
		GD.Print($"{this.Name} has no target teleporter.");
	}
	#endregion
}
