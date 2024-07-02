using Godot;
using System;

public partial class MainMenu : Control
{
	public void Play()
    {
        GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
    }

    public void Options()
    {
        GetTree().ChangeSceneToFile("res://Scenes/OptionsMenu.tscn");
    }

    public void Quit()
    {
        GetTree().Quit();
    }
}
