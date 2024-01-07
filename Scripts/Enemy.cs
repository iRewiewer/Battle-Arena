using Godot;

public partial class Enemy : CharacterBody2D
{
	#region Public Vars
	[Export]
	public PackedScene targetEntity;
	[Export]
	public int baseHealth = 10;
	[Export]
	public int baseDamage = 1;
	[Export]
	public int baseSpeed = 100;
	#endregion

	#region Private Vars
	private int health;
	private int damage;
	private int speed;
	private Vector2 inputVector = Vector2.Zero;
	private AnimationTree animTree;
	private AnimationNodeStateMachinePlayback animStateMachine;

	private bool hahafunny = true;
	#endregion

	#region Public Methods
	public override void _Ready()
	{
		if(targetEntity == null)
		{
			ErrorNoTargetEntity();
		}

		SetupAttributes();

		animTree = (AnimationTree)this.FindChild("AnimationTree");
		animStateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
	}

	public override void _PhysicsProcess(double delta)
	{
		MovementHandler(delta);
	}
	#endregion

	#region Private Methods
	private void MovementHandler(double delta)
	{
		if(hahafunny)
		{
			inputVector = new Vector2(0, 1);
			hahafunny = false;
		}
		else
		{
			inputVector = new Vector2(0, 0);
		}

		if (inputVector == Vector2.Zero)
		{
			animStateMachine.Travel("Idle");
			return;
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

	private void SetupAttributes()
	{
		health = baseHealth;
		damage = baseDamage;
		speed = baseSpeed;
	}
	#endregion

	#region Error Handlers
	private void ErrorNoTargetEntity()
	{
		GD.Print($"{this.Name} has no target entity.");
		// change this to a random action such as dance in place
		this.QueueFree(); // destroy this
	}
	#endregion
}
