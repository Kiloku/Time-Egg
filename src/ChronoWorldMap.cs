using Godot;
using System;

public partial class ChronoWorldMap : Node2D
{
	public TileMapLayer TopLayer;
	public TileMapLayer BottomLayer;

	const int LayerWidth = 96;
	const int LayerHeight = 64;
	
	[Export] public Camera2D Camera;
	private Vector2 CameraDragStart;
	
	public override void _Ready()
	{
		TopLayer = GetNode<TileMapLayer>("TopLayer");
		BottomLayer = GetNode<TileMapLayer>("BottomLayer");
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event is InputEventMouseButton mouseButtonEvent)
		{
			if (mouseButtonEvent.GetButtonIndex() == MouseButton.Middle)
			{
				if (mouseButtonEvent.IsPressed())
				{
					CameraDragStart = GetViewport().GetMousePosition() - Camera.Offset; 
					Input.SetDefaultCursorShape(Input.CursorShape.Drag);
				}
				else if (mouseButtonEvent.IsReleased())
				{
					Input.SetDefaultCursorShape(Input.CursorShape.Arrow);
				}
			}
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (Input.IsMouseButtonPressed(MouseButton.Middle))
		{
			Camera.Offset = GetViewport().GetMousePosition() - CameraDragStart;
		}
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
	
	public TileMapLayer GetLayer(int layerIndex)
	{
		if (layerIndex == 0)
		{
			return TopLayer;
		}

		return BottomLayer;
	}
}
