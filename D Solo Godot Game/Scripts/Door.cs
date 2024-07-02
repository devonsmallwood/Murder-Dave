using Godot;
using System;

public partial class Door : Interactable
{
	private bool isDoorOpen;
	private bool isPlayerAtFront;
	private bool isPlayerAtBack;
	private string prevAnim;
	private AnimationPlayer animationPlayer;
	private AudioStreamPlayer3D audioStreamPlayer;

	[Export]
	private bool doesDoorStartOpen;
	[Export]
	private bool requiresKey;
	[Export(PropertyHint.Enum, "blue,red,green")]
	private string doorLabel;
	[Export]
	private AudioStreamMP3 doorOpenSound;
	[Export]
	private AudioStreamMP3 doorCloseSound;

    public override void _Ready()
    {
        base._Ready();
		
		animationPlayer = GetNode<AnimationPlayer>("Hinge/DoorMesh/AnimationPlayer");
		audioStreamPlayer = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");

		if (doesDoorStartOpen)
		{
			animationPlayer.Play("open_from_back");
			prevAnim = "open_from_back";
			isDoorOpen = true;
		}
    }

    public override void _Process(double delta)
    {
		base._Process(delta);

        if (isPlayerInRange && !player.isLookingAtInRangeItem && (isDoorOpen || !requiresKey || player.keyLabel == doorLabel))
		{
			canPlayerInteract = true;
		}
		else
		{
			canPlayerInteract = false;
		}
    }

    public override void Interact()
    {
		if (!isDoorOpen)
		{
			interactText = "close";
			player.GetNode<Label>("UI/Message/PanelContainer/MarginContainer/GridContainer/ActionLabel").Text = interactText + " " + interactableName;
			if (!isPlayerInteractable || isPlayerAtBack)
			{
				animationPlayer.Play("open_from_back");
				prevAnim = "open_from_back";
			}
			else if (isPlayerAtFront)
			{
				animationPlayer.Play("open_from_front");
				prevAnim = "open_from_front";
			}
			
			isDoorOpen = true;
			audioStreamPlayer.Stream = doorOpenSound;
			audioStreamPlayer.Play();
		}
		else
		{
			interactText = "open";
			player.GetNode<Label>("UI/Message/PanelContainer/MarginContainer/GridContainer/ActionLabel").Text = interactText + " " + interactableName;
			animationPlayer.PlayBackwards(prevAnim);
			isDoorOpen = false;
			audioStreamPlayer.Stream = doorCloseSound;
			audioStreamPlayer.Play();
		}

		ToggleOutlines();
    }

    private void EnteredDoorArea(Rid body_rid, Node3D body, long body_shape_index, long local_shape_index)
	{
		if (body == player)
		{
			isPlayerInRange = true;
		}
	}
	
	private void ExitedDoorArea(Rid body_rid, Node3D body, long body_shape_index, long local_shape_index)
	{
		if (body == player)
		{
			isPlayerInRange = false;
		}
	}

    private void EnteredRangeFromFront(Rid body_rid, Node3D body, long body_shape_index, long local_shape_index)
	{
		if (body == player)
		{
			isPlayerAtFront = true;
		}
	}

	private void ExitedRangeFromFront(Rid body_rid, Node3D body, long body_shape_index, long local_shape_index)
	{
		if (body == player)
		{
			isPlayerAtFront = false;
			player.SetActionPanelVisible(false);
		}
	}

	private void EnteredRangeFromBack(Rid body_rid, Node3D body, long body_shape_index, long local_shape_index)
	{
		if (body == player)
		{
			isPlayerAtBack = true;
		}
	}

	private void ExitedRangeFromBack(Rid body_rid, Node3D body, long body_shape_index, long local_shape_index)
	{
		if (body == player)
		{
			isPlayerAtBack = false;
			player.SetActionPanelVisible(false);
		}
	}
}
