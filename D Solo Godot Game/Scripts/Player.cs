using Godot;
using System;

public partial class Player : CharacterBody3D
{
	private bool canMove;
	private Node3D head;
	private Camera3D camera;
	private Vector3 startingPos;
	private Vector3 startingRot;

	[Export]
	private bool mouseBeginCaptured;
	[Export]
	private NodePath headNodePath;
	[Export]
	private NodePath cameraNodePath;
	[Export]
	private float mouseSensitivity;
	[Export]
	Node3D dave;
	[Export]
	private AudioStreamMP3[] footstepSounds;

	public bool isInRangeOfItem;
	public bool isLookingAtInRangeItem;
	public bool isLookingAtBreakable;
	public bool isLookingAtDave;
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle(); // Get the gravity from the project settings to be synced with RigidBody nodes.
	public Item item;

	[Export]
	public string keyLabel;

	public const float SPEED = 3.5f;
	public const float JUMP_VELOCITY = 4.5f;

	public override void _Ready()
	{
		head = GetNode<Node3D>(headNodePath);
		camera = GetNode<Camera3D>(cameraNodePath);
		startingPos = Position;
		startingRot = Rotation;
		
		if (mouseBeginCaptured)
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}

		canMove = false;

		GetNode<AnimationPlayer>("AnimationPlayer").Play("CinematicWakeUp", -1, 0.25f);
	}

	public void AnimationFinished(string animName)
	{
		if (animName == "CinematicWakeUp")
		{
			canMove = true;
		}

		if (animName == "FadeToBlack")
		{
			if (item != null)
			{
				DropItem(item);
			}

			Position = startingPos;
			Rotation = startingRot;
			GetNode<Node3D>("Head").Rotation = Vector3.Zero;

			canMove = false;

			GetNode<AnimationPlayer>("AnimationPlayer").Play("EndGameWakeUp", -1, 0.25f);
		}

		if (animName == "EndGameWakeUp")
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
			
			GetTree().ChangeSceneToFile("res://Scenes/EndCredits.tscn");
		}
	}

    public override void _Process(double delta)
    {
		
    }

	public override void _PhysicsProcess(double delta)
	{
		if (canMove)
		{
			Godot.Vector3 velocity = Velocity;

			// Add the gravity.
			if (!IsOnFloor())
				velocity.Y -= gravity * (float)delta;

			// Handle Jump.
			if (Input.IsActionJustPressed("jump") && IsOnFloor())
				velocity.Y = JUMP_VELOCITY;

			// Get the input direction and handle the movement/deceleration.
			Godot.Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
			Godot.Vector3 direction = (Transform.Basis * new Godot.Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
			if (direction != Godot.Vector3.Zero)
			{
				velocity.X = direction.X * SPEED;
				velocity.Z = direction.Z * SPEED;

				if (IsOnFloor())
				{
					Walk();
				}
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, SPEED);
				velocity.Z = Mathf.MoveToward(Velocity.Z, 0, SPEED);
			}

			isLookingAtInRangeItem = false;
			isLookingAtBreakable = false;

			Velocity = velocity;
			MoveAndSlide();
		}
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		
		if (canMove)
		{
			if (Input.MouseMode == Input.MouseModeEnum.Captured)
			{
				InputEventMouseMotion mouseMotion = @event as InputEventMouseMotion;
				
				if (mouseMotion != null)
				{
					HandleCameraRotation(mouseMotion);
				}
			}

			if (Input.IsActionJustPressed("drop_item") && item != null)
			{
				DropItem(item);
			}

			if (Input.IsActionJustPressed("fire"))
			{
				GetNode<Pistol>("Head/RArmSpring/GunLocation/Pistol").Fire();
				Fire();
			}
		}
	}

	private void HandleCameraRotation(InputEventMouseMotion mouseMotion)
	{
		head.RotateX(Mathf.DegToRad(-mouseMotion.Relative.Y * mouseSensitivity));
		head.RotationDegrees = new Godot.Vector3(Mathf.Clamp(head.RotationDegrees.X, -70, 70), head.RotationDegrees.Y, head.RotationDegrees.Z);
		
		RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * mouseSensitivity));
	}

	// Drops the Item currently being held by the Player
	public void DropItem(Item itemToDrop)
	{
		string itemPath = itemToDrop.GetPath();
		GetNode<Item>(itemPath).isHeld = false;
		GetNode<Item>(itemPath).FindGround();
		GetNode<Item>(itemPath).SetCollisionsEnabled(true);
		GetNode<Item>(itemPath).ToggleOutlines();
		keyLabel = "";
		item = null;
	}

	public void SetActionPanelVisible(bool value)
	{
		if (value)
		{
			GetNode<CenterContainer>("UI/Message").Visible = true;
		}
		else
		{
			GetNode<CenterContainer>("UI/Message").Visible = false;
		}
	}

	private void Fire()
	{
		if (isLookingAtDave)
		{
			dave.GetNode<Dave>(".").Die();
			GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeToBlack", -1);
		}

		if (isLookingAtBreakable && !(GetNode<RayCast3D>("Head/HeadRay").GetCollider() as Interactable).isBroken)
		{
			(GetNode<RayCast3D>("Head/HeadRay").GetCollider() as Interactable).isBroken = true;
		}
	}

	private void Walk()
	{
		if (!GetNode<AnimationPlayer>("AnimationPlayer").IsPlaying())
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Play("Walk", -1);
			StreamRandomFootstepSound();
        	GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Play();
		}
	}

	private void StreamRandomFootstepSound()
    {
        AudioStreamMP3 soundToPlay;
        RandomNumberGenerator soundRandomizer = new RandomNumberGenerator();
        soundToPlay = footstepSounds[soundRandomizer.RandiRange(0, footstepSounds.Length - 1)];
        GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Stream = soundToPlay;
    }
}
