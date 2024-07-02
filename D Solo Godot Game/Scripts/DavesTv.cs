using Godot;
using System;

public partial class DavesTv : Interactable
{
	[Export]
	private AudioStreamMP3[] davesTvNoises;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!isBroken)
		{
			if (!GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Playing)
			{
				StreamRandomSound();
				GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Play();
			}

			if (!GetNode<AnimationPlayer>("AnimationPlayer").IsPlaying())
			{
				GetNode<AnimationPlayer>("AnimationPlayer").Play("tv_img_wiggle", -1);
			}
		}

		else
		{
			GetNode<Decal>("Decal").Visible = false;
			GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Stop();
		}
	}

	private void StreamRandomSound()
    {
        AudioStreamMP3 soundToPlay;
        RandomNumberGenerator soundRandomizer = new RandomNumberGenerator();
        soundToPlay = davesTvNoises[soundRandomizer.RandiRange(0, davesTvNoises.Length - 1)];
        GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Stream = soundToPlay;
    }
}
