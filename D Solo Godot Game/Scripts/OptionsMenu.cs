using Godot;
using System;

public partial class OptionsMenu : Control
{
	public void Audio()
	{
		GetTree().ChangeSceneToFile("res://Scenes/AudioMenu.tscn");
	}

	public void Controls()
	{
		GetTree().ChangeSceneToFile("res://Scenes/ControlsMenu.tscn");	
	}

	public void Credits()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Credits.tscn");
	}

	public void Back()
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
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
