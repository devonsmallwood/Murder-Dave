using Godot;
using System;

public partial class Pistol : Node3D
{
	[Export]
	private AudioStreamMP3 pistolSound;
	public void Fire()
	{
		if (!GetNode<AnimationPlayer>("AnimationPlayer").IsPlaying())
		{
			GetNode<AnimationPlayer>("AnimationPlayer").Play("Fire", -1, 2);
			GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Stream = pistolSound;
        	GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Play();
		}
	}
}
