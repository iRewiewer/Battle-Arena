using Godot;
using System;

public partial class Player : CharacterBody2D
{
	#region Public Vars
	#endregion

	#region Private Vars
	private int speed;
	private static int baseSpeed = 100;
	private Vector2 inputVector = Vector2.Zero;
	private AnimationTree animTree;
	private AnimationNodeStateMachinePlayback animStateMachine;
	#endregion

	#region Methods
	public override void _Ready()
	{
		speed = baseSpeed;
		animTree = (AnimationTree)this.FindChild("AnimationTree");
		animStateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
	}

	public override void _Process(double delta)
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
		MovementHandler(delta);
		SprintHandler();
		Settings.UpdateSettingsUI(this);
	}

	private void MovementHandler(double delta)
	{
		inputVector = Input.GetVector("left", "right", "up", "down");

		if (inputVector == Vector2.Zero)
		{
			animStateMachine.Travel("Idle");
		}
		else
		{
			animTree.Set("parameters/Idle/blend_position", inputVector);
			animTree.Set("parameters/Walk/blend_position", inputVector);
			animStateMachine.Travel("Walk");
		}

		Velocity = inputVector.Normalized() * speed;
		MoveAndSlide();
	}

	private void SprintHandler()
	{
		if (Input.IsActionPressed("sprint"))
		{
			SetSprintingSpeed();
		}
		else
		{
			SetWalkingSpeed();
		}
	}

	private void SetSprintingSpeed()
	{
		speed = baseSpeed * 2;
	}
	
	private void SetWalkingSpeed()
	{
		speed = baseSpeed;
	}
	#endregion
}
