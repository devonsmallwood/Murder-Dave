using Godot;
using System;

public partial class Button : Interactable
{
    public override void _Process(double delta)
    {
        base._Process(delta);

        if (isPlayerInRange && isLookedAt)
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
        base.Interact();

        audioStreamPlayer.Play();
    }

    public void AudioStreamPlayerFinished()
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
    }
}
