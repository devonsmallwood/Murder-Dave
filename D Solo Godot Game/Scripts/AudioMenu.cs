using Godot;
using System;

public partial class AudioMenu : Control
{
	int SfxBusId = AudioServer.GetBusIndex("SFX");
	int BgmBusId = AudioServer.GetBusIndex("BGM");

    public override void _Ready()
    {
        GetNode<HSlider>("MarginContainer/VBoxContainer/GridContainer/SFXSlider").Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(SfxBusId));
        GetNode<HSlider>("MarginContainer/VBoxContainer/GridContainer/BGMSlider").Value = Mathf.DbToLinear(AudioServer.GetBusVolumeDb(BgmBusId));
    }

    public void SfxVolumeChanged(float volume)
	{
		AudioServer.SetBusVolumeDb(SfxBusId, Mathf.LinearToDb(volume));
		AudioServer.SetBusMute(SfxBusId, volume < .05);
	}

	public void BgmVolumeChanged(float volume)
	{
		AudioServer.SetBusVolumeDb(BgmBusId, Mathf.LinearToDb(volume));
		AudioServer.SetBusMute(BgmBusId, volume < .05);
	}

	public void SfxTestButtonPressed()
	{
		GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
	}

	public void Back()
	{
		GetTree().ChangeSceneToFile("res://Scenes/OptionsMenu.tscn");
	}

	public override void _Input(InputEvent @event)
    {
        base._Input(@event);

		if (Input.IsActionJustPressed("pause"))
		{
			Back();
		}
    }
}
