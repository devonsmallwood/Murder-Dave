using Godot;
using System;

public partial class PressurePlate : Interactable
{
	private bool isPressurePlateDown;
	private int numBodies;

    public override void EnteredRange(Rid body_rid, Node3D body, long body_shape_index, long local_shape_index)
    {
        if (body == player || body.GetNodeOrNull<Interactable>(".") != null && body.GetNode<Interactable>(".").canActivatePressurePlates)
		{
			numBodies++;

			if (numBodies == 1)
			{
				isPressurePlateDown = true;
				
				Interact();
			}
		}
    }

    public override void ExitedRange(Rid body_rid, Node3D body, long body_shape_index, long local_shape_index)
    {
        if (body == player || body.GetNodeOrNull<Interactable>(".") != null && body.GetNode<Interactable>(".").canActivatePressurePlates)
		{
			numBodies--;

			if (numBodies == 0)
			{
				isPressurePlateDown = false;

				Interact();
			}
		}
    }
}
