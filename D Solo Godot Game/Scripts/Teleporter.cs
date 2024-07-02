using Godot;
using System;

public partial class Teleporter : Interactable
{
	[Export(PropertyHint.Enum, "sender,receiver")]
	private string teleporterType;
	[Export]
	private Teleporter teleporterReceiver;

    public override void _Ready()
    {
        player = GetNode<Player>("../Player");
    }

    public override void EnteredRange(Rid body_rid, Node3D body, long body_shape_index, long local_shape_index)
    {
        if (teleporterType == "sender" && body == player)
		{
			if (player.item != null)
			{
				player.DropItem(player.item);
			}

			Interact();
		}
    }

    public override void Interact()
    {
		float playerRelativeToSenderX = Position.X - player.Position.X;
		float playerRelativeToSenderZ = Position.Z - player.Position.Z;

        if (teleporterReceiver != null)
		{
			player.Position = new Vector3(teleporterReceiver.Position.X - playerRelativeToSenderX, teleporterReceiver.Position.Y, teleporterReceiver.Position.Z - playerRelativeToSenderZ);
			player.Rotation += teleporterReceiver.Rotation;
		}
    }
}
