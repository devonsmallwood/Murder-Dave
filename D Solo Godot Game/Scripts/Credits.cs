using Godot;
using System;

public partial class Credits : Control
{
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
