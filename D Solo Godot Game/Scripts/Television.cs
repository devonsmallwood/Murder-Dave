using Godot;
using System;

public partial class Television : Item
{
	[Export]
	private AudioStreamMP3[] tvNoises;

    public override void _Ready()
    {
        base._Ready();

		StreamRandomTvNoise(true);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

		if (!GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Playing)
        {
			StreamRandomTvNoise(true);
			GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Play();
        }
    }

    public override void Break()
    {
        isBroken = false;

		StreamRandomTvNoise(false);
    }

	private void StreamRandomTvNoise(bool canBeCurrentNoise)
    {
        AudioStreamMP3 soundToPlay;
        RandomNumberGenerator soundRandomizer = new RandomNumberGenerator();
        soundToPlay = tvNoises[soundRandomizer.RandiRange(0, tvNoises.Length - 1)];

		// To add: if !canBeCurrentNoise, prevent from playing current noise

        GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D").Stream = soundToPlay;
    }
}
