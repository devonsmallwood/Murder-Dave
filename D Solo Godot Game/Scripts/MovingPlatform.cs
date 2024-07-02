using Godot;
using System;

public partial class MovingPlatform : Interactable
{
	[Export(PropertyHint.Enum, "upDown,fwdBck")]
	public string platformType;
	[Export]
	public bool startAtEnd;

	private bool playBackwards;

    public override void _Ready()
    {
        base._Ready();

		if (startAtEnd)
		{
			if (platformType == "upDown")
			{
				GetNode<AnimationPlayer>("AnimationPlayer").Play("move_up_down");
			}

			else if (platformType == "fwdBck")
			{
				GetNode<AnimationPlayer>("AnimationPlayer").Play("move_fwd_bck");
			}

			playBackwards = true;
		}
    }

    public override void Interact()
    {
        base.Interact();

		if (!playBackwards)
		{
			if (platformType == "upDown")
			{
				GetNode<AnimationPlayer>("AnimationPlayer").Play("move_up_down");

				playBackwards = true;
			}

			else if (platformType == "fwdBck")
			{
				GetNode<AnimationPlayer>("AnimationPlayer").Play("move_fwd_bck");

				playBackwards = true;
			}
		}

		else
		{
			if (platformType == "upDown")
			{
				GetNode<AnimationPlayer>("AnimationPlayer").PlayBackwards("move_up_down");

				playBackwards = false;
			}

			else if (platformType == "fwdBck")
			{
				GetNode<AnimationPlayer>("AnimationPlayer").PlayBackwards("move_fwd_bck");

				playBackwards = false;
			}
		}
    }
}
