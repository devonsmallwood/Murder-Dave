using Godot;
using System;

public partial class EndCredits : Control
{
    public override void _Ready()
    {
        GetNode<AnimationPlayer>("AnimationPlayer").Play("Scroll", -1);
    }

    public void AnimationFinished(string animName)
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
	}
}
