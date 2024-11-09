using Godot;
using System;

public partial class TileInfoLabel : Label
{
	[Export] private Node MapView;

	//private ChronoWorldMap Map;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (MapView == null)
		{
			return;
		}
		var Nodes = MapView.FindChildren("*");
		foreach (var node in Nodes)
		{
			if (node is ChronoWorldMap)
			{
				((ChronoWorldMap)node).TileClicked += (tile) =>
				{
					string bottomHex = HexRepresentation(tile.BottomLayerInAtlas.X + (tile.BottomLayerInAtlas.Y * 16));
					string topHex = HexRepresentation(tile.TopLayerInAtlas.X + (tile.TopLayerInAtlas.Y * 16));
					SetText("Pos: " + tile.Position.ToString() + 
					        "\nBottom: " + (tile.HasBottomLayer ? bottomHex : "None") + 
							" | Top: " + (tile.HasTopLayer ? topHex : "None"));
				};
			}
		}
	}
	
	public string HexRepresentation(int value)
	{
		if (value < 0 || value > 255)
		{
			return "NaN";
		}

		byte[] arr = new byte[1];
		arr[0] = (byte)value;
		return Convert.ToHexString(arr);
	}
}
