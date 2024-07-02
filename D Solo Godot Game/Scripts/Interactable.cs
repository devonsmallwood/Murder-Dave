using Godot;
using System;

public partial class Interactable : RigidBody3D
{
	public bool isPlayerInRange;
	public bool isLookedAt;
	public bool canPlayerInteract;
	public Vector3 spawnPos;
	public AudioStreamPlayer3D audioStreamPlayer;

	public Player player;
	
	[ExportGroup("Visual")]
	[Export]
	public Material inactiveMaterial;
	[Export]
	public Material activeMaterial;
	[Export]
	public MeshInstance3D mesh;
	[Export]
	public MeshInstance3D[] outlines;
	[ExportGroup("Breakable")]
	[Export]
	public bool isBreakable;
	[Export]
	public bool isBroken;
	[ExportGroup("Player Interactable")]
	[Export]
	public bool isPlayerInteractable;
	[Export]
	public bool canBlockPlayer;
	[Export]
	public string interactText;
	[Export]
	public string interactableName;
	[ExportGroup("")]
	[Export]
	public Interactable[] receivers;
	[Export]
	public Interactable interactableToSpawn;
	[Export]
	public bool canActivatePressurePlates;
	[Export]
	public AudioStreamMP3 interactSound;

    public override void _Ready()
    {
		if (inactiveMaterial != null)
		{
			if (outlines != null)
			{
				foreach (MeshInstance3D outline in outlines)
				{
					outline.Visible = true;
					outline.MaterialOverride = inactiveMaterial;
				}
			}
		}

		if (GetNodeOrNull<AudioStreamPlayer3D>("AudioStreamPlayer3D") != null)
		{
			audioStreamPlayer = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");

			if (interactSound != null)
			{
				audioStreamPlayer.Stream = interactSound;
			}
		}

		spawnPos = Position;
        player = GetNode<Player>("../../Player");
    }

    public override void _Process(double delta)
    {
        if (isPlayerInteractable && !isBroken && canPlayerInteract)
		{
			player.SetActionPanelVisible(true);
			player.GetNode<Label>("UI/Message/PanelContainer/MarginContainer/GridContainer/ActionLabel").Text = interactText + " " + interactableName;

			if (Input.IsActionJustPressed("interact"))
			{
				Interact();
			}
		}
    }

    public override void _PhysicsProcess(double delta)
    {
        isLookedAt = CheckIfItemIsLookedAt();

		if (isLookedAt && isBreakable)
		{
			player.isLookingAtBreakable = true;
		}

		if (isBroken)
		{
			Break();
		}
    }

	public virtual bool CheckIfItemIsLookedAt()
	{
		if (!player.GetNode<RayCast3D>("Head/HeadRay").IsColliding() || player.GetNode<RayCast3D>("Head/HeadRay").GetCollider() as Interactable != this)
		{
			player.SetActionPanelVisible(false);
			return false;
		}
		else
		{
			return true;
		}
	}

	// Moves Interactable back to its initial position
	public virtual void PlaceAtSpawn()
	{
		if (isBroken)
		{
			Repair();
		}

		Position = spawnPos;
	}

    public virtual void Interact()
	{
		if (receivers.Length > 0)
        {
            foreach (Interactable receiver in receivers)
			{
				if (!receiver.isBroken)
				{
					receiver.Interact();
				}
			}
        }

		if (interactableToSpawn != null)
		{
			interactableToSpawn.PlaceAtSpawn();
		}

		ToggleOutlines();
	}

	public virtual void SetCollisionsEnabled(bool areCollisionsEnabled)
	{
		if (areCollisionsEnabled)
		{
			if (canBlockPlayer)
			{
				SetCollisionLayerValue(1, true);
				SetCollisionMaskValue(1, true);
			}

			SetCollisionLayerValue(2, true);
			SetCollisionMaskValue(2, true);
		}
		else
		{
			if (canBlockPlayer)
			{
				SetCollisionLayerValue(1, false);
				SetCollisionMaskValue(1, false);
			}
			
			SetCollisionLayerValue(2, false);
			SetCollisionMaskValue(2, false);
		}
	}

	public virtual void SetVisibility(bool isVisible)
	{
		if (mesh != null)
		{
			if (isVisible)
			{
				mesh.Visible = true;
			}
			else
			{
				mesh.Visible = false;
			}
		}
	}

	public void ToggleOutlines()
	{
		if (activeMaterial != null && inactiveMaterial != null)
		{
			if (outlines != null)
			{
				foreach (MeshInstance3D outline in outlines)
				{
					outline.Visible = true;
					
					if (outline.MaterialOverride == inactiveMaterial)
					{
						outline.MaterialOverride = activeMaterial;
					}
					else
					{
						outline.MaterialOverride = inactiveMaterial;
					}
				}
			}
		}
	}

	public virtual void Break()
	{
		SetCollisionsEnabled(false);
		SetVisibility(false);
	}

	public virtual void Repair()
	{
		isBroken = false;

		SetCollisionsEnabled(true);
		SetVisibility(true);
	}

	public virtual void EnteredRange(Rid body_rid, Node3D body, long body_shape_index, long local_shape_index)
	{
		if (body == player)
		{
			isPlayerInRange = true;
		}
	}
	public virtual void ExitedRange(Rid body_rid, Node3D body, long body_shape_index, long local_shape_index)
	{
		if (body == player)
		{
			isPlayerInRange = false;
			player.SetActionPanelVisible(false);
		}
	}
}
