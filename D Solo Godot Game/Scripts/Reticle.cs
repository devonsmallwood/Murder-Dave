using Godot;
using System;

public partial class Reticle : Control
{
	[Export]
	float dotRadius = 1.0f;
	[Export]
	Color dotColor;

    public override void _Ready()
    {
        QueueRedraw();
    }

    public override void _Draw()
    {
        DrawCircle(Vector2.Zero, dotRadius, dotColor);
    }
}
