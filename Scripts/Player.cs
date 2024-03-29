using Godot;

public partial class Player : CharacterBody2D
{
	#region Public Vars
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
	#endregion

	#region Public Methods
	public override void _Ready()
	{
		SetupAttributes();

		animTree = (AnimationTree)this.FindChild("AnimationTree");
		animStateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
	}

	public override void _PhysicsProcess(double delta)
	{
		MovementHandler(delta);
		SprintHandler();
		Settings.UpdateSettingsUI(this);

		if (Input.IsKeyPressed(Key.Escape))
		{
			Settings.QuitGame(this);
		}
	}
	#endregion

	#region Private Methods
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
			speed = baseSpeed * 2;
		}
		else
		{
			speed = baseSpeed;
		}
	}

	private void SetupAttributes()
	{
		health = baseHealth;
		damage = baseDamage;
		speed = baseSpeed;
	}
	#endregion

	#region Error Handling
	#endregion
}
