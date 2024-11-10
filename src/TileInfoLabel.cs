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
					string bottomHex = HexRepresentation(tile.GetByteAtLayer(MapLayer.Bottom));
					string topHex = HexRepresentation(tile.GetByteAtLayer(MapLayer.Top));
					SetText("Pos: " + tile.Position.ToString() + 
					        "\nBottom: " + (tile.HasBottomLayer ? bottomHex : "None") + 
							" | Top: " + (tile.HasTopLayer ? topHex : "None"));
				};
			}
		}
	}
	
	public static string HexRepresentation(byte value)
	{
		byte[] arr = new byte[1];
		arr[0] = value;
		return Convert.ToHexString(arr);
	}
}
