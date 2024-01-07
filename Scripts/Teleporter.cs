using Godot;
using System;

public partial class Teleporter : Area2D
{
	#region Public Vars
	[Export]
	public Area2D targetTeleporter { get; set; }
	public int cooldown;
	#endregion

	#region Private Vars
	private int baseCooldown = 10; // 30 seconds
	private Timer timer;
	private Label label;
	private CharacterBody2D player;

	private bool hasCollided = false;
	private bool hasTeleported = false;
	#endregion

	#region Methods
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (targetTeleporter == null)
		{
			ErrorNoTargetPortal();
			return;
		}

		timer = this.GetChild<Timer>(1);
		cooldown = baseCooldown;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (targetTeleporter == null)
		{
			ErrorNoTargetPortal();
			return;
		}

		if (hasCollided && !hasTeleported)
		{
			// Teleport Player
			player.Position = new Vector2(targetTeleporter.Position.X, targetTeleporter.Position.Y);
			hasTeleported = true;
		}

		if(timer.TimeLeft == 0)
		{
			hasCollided = false;
			hasTeleported = false;
		}
	}

	private void _on_body_entered(CharacterBody2D body)
	{
		if(targetTeleporter == null)
		{
			ErrorNoTargetPortal();
			return;
		}

		if(cooldown > 0 && hasCollided)
		{
			return;
		}

		// Assign player
		player = body;

		// Start cooldown
		timer.Start(cooldown);
		GD.Print($"Teleported {body.Name} from {this.Name} to {targetTeleporter.Name} @ Cooldown {timer.TimeLeft}s");

		targetScript.hasCollided = true;
		targetScript.hasTeleported = true;
		hasCollided = true;
	}

	private void ErrorNoTargetPortal()
	{
		GD.Print($"{this.Name} has no target teleporter.");
	}
	#endregion
}
