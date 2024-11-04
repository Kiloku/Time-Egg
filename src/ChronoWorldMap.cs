using Godot;
using System;

public partial class ChronoWorldMap : Node2D
{
	public TileMapLayer TopLayer;
	public TileMapLayer BottomLayer;

	const int LayerWidth = 96;
	const int LayerHeight = 64;
	
	public override void _Ready()
	{
		TopLayer = GetNode<TileMapLayer>("TopLayer");
		BottomLayer = GetNode<TileMapLayer>("BottomLayer");
	}

	public void InitMap(byte[] mapData)
	{
		for (int i = 0; i < mapData.Length/2; i++)
		{
			int y = i/96;
			int x = i%96;
			Vector2I position = new Vector2I(x, y);

			int atlasX = mapData[i] % 16;
			int atlasY = mapData[i] / 16;

			TopLayer.SetCell(position, 0, new Vector2I(atlasX, atlasY));
			
			atlasX = mapData[i+mapData.Length/2] % 16;
			atlasY = mapData[i+mapData.Length/2] / 16;
			
			BottomLayer.SetCell(position, 1, new Vector2I(atlasX, atlasY));
		}
	}
}
