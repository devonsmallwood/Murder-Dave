using Godot;
using System;
using System.IO;

public partial class Dave : Node3D
{
    public bool isDead = false;
    private Player player;

    [Export]
    private AudioStreamMP3 deathSound;
    [Export]
    private AudioStreamMP3[] fuckingNoises;

    public override void _Ready()
    {
        player = GetNode<Player>("../Player");
    }

    public void Die()
	{
        GetNode<AnimationPlayer>("AnimationPlayer").Play("die", -1, 1.5f);
        GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Stream = deathSound;
        GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Play();
        isDead = true;
	}

    public override void _Process(double delta)
    {
        if (!isDead && !GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Playing)
        {
            StreamRandomSound();
            GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Play();
        }
        
        if (!player.GetNode<RayCast3D>("Head/HeadRay").IsColliding() || (player.GetNode<RayCast3D>("Head/HeadRay").GetCollider() as Node3D).GetNodeOrNull("../") == null || (player.GetNode<RayCast3D>("Head/HeadRay").GetCollider() as Node3D).GetNodeOrNull("../") != this || isDead)
        {
            player.isLookingAtDave = false;
        }
        else
        {
            player.isLookingAtDave = true;
        }
    }

    private void StreamRandomSound()
    {
        AudioStreamMP3 soundToPlay;
        RandomNumberGenerator soundRandomizer = new RandomNumberGenerator();
        soundToPlay = fuckingNoises[soundRandomizer.RandiRange(0, fuckingNoises.Length - 1)];
        GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Stream = soundToPlay;
    }
}
