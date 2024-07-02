using Godot;
using System;

public partial class PauseMenu : Control
{
	[Export]
	private Control ui;

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

		if (Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			if (Input.IsActionJustPressed("pause"))
			{
				GetTree().Paused = true;
				Input.MouseMode = Input.MouseModeEnum.Visible;
				ui.Visible = false;
				Visible = true;
			}
		}		
		else
		{
			if (Input.IsActionJustPressed("pause"))
			{
				Continue();
			}
		}
    }

    public void Continue()
	{
		GetTree().Paused = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
		ui.Visible = true;
		Visible = false;
	}

	public void Restart()
	{
		GetTree().Paused = false;
		GetTree().ReloadCurrentScene();
	}

	public void Quit()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
	}
}
