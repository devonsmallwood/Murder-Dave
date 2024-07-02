using Godot;
using System;

public partial class Item : Interactable
{
	private bool hasFoundGround;
	private Node3D groundObj;

	public bool isHeld;
	public Vector3 itemSavePoint;

	[ExportGroup("Key")]
	[Export]
	public bool isKey;
	[Export(PropertyHint.Enum, "blue,red,green")]
	public string ThisKeyLabel;

    public override void _Ready()
    {
		base._Ready();
		
		itemSavePoint = spawnPos;
    }

    public override void _Process(double delta)
    {
		base._Process(delta);

		if (!isHeld && isPlayerInRange)
		{
			player.isInRangeOfItem = true;
		}
		else
		{
			player.isInRangeOfItem = false;
		}

        if (player.item == null && isPlayerInRange && isLookedAt)
		{
			canPlayerInteract = true;
		}
		else
		{
			canPlayerInteract = false;
		}
    }

	public override void _PhysicsProcess(double delta)
    {
		base._PhysicsProcess(delta);

		if (Position.Y < -3)
		{
			FixItemPos();
		}

		// If the Player is holding the Item, move the Item with the Player
		if (isHeld)
		{
			ResetVelocity();
			MoveItemWithPlayer();
		}
		// Otherwise, reset the Item's orientation
		else
		{
			ResetRotation();
		}

		if (isLookedAt && isPlayerInRange)
		{
			player.isLookingAtInRangeItem = true;
		}
    }

    public override void Interact()
    {
		if (!isBroken)
		{
			base.Interact();

			OnPickup();

			if (isKey)
			{
				player.keyLabel = ThisKeyLabel;
			}
		}
    }

	public override void SetCollisionsEnabled(bool areCollisionsEnabled)
	{
		base.SetCollisionsEnabled(areCollisionsEnabled);

		if (areCollisionsEnabled)
		{
			SetCollisionLayerValue(3, false);
			SetCollisionMaskValue(3, false);
		}
		else
		{
			SetCollisionLayerValue(3, true);
			SetCollisionMaskValue(3, true);
		}
	}

    private void OnPickup()
    {
		player.item = this;
		SetCollisionsEnabled(false);
		isHeld = true;
    }

	public void FindGround()
	{
		if (!GetNode<RayCast3D>("CheckForGround").IsColliding() || !(GetNode<RayCast3D>("CheckForGround").GetCollider() as Node3D).IsInGroup("Ground"))
		{
			hasFoundGround = false;
		}
		else
		{
			groundObj = GetNode<RayCast3D>("CheckForGround").GetCollider() as Node3D;
			hasFoundGround = true;
			itemSavePoint = Position;
		}
	}

	// Tries to place Item higher to prevent it from falling through the ground
	// If the ground cannot be found, moves Item to a previous safe position
	private void FixItemPos()
	{
		ResetVelocity();

		Vector3 fixPos = Position;

		if (hasFoundGround)
		{
			fixPos.Y = groundObj.Position.Y + 1;
			Position = fixPos;
		}
		else
		{
			Position = itemSavePoint;
		}
	}

    public override void PlaceAtSpawn()
    {
        if (isHeld)
		{
			player.DropItem(this);
		}
		
		base.PlaceAtSpawn();
    }

    // Tracks item to Player's "hand"
    private void MoveItemWithPlayer()
	{
		Position = player.GetNode<Node3D>("Head/LArmSpring/HeldItemLocation").GlobalPosition;
		Rotation = player.GetNode<Node3D>("Head/LArmSpring/HeldItemLocation").GlobalRotation;
	}

	private void ResetVelocity()
	{
		LinearVelocity = Vector3.Zero;
		AngularVelocity = Vector3.Zero;
	}

	private void ResetRotation()
	{
		Rotation = Vector3.Zero;
	}
}