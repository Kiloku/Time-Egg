using Godot;
using System;


public partial class ChronoWorldMap : Node2D
{
	public TileMapLayer TopLayer;
	public TileMapLayer BottomLayer;

	const int LayerWidth = 96;
	const int LayerHeight = 64;
	
	[Export] public Camera2D Camera;
	[Export] public Sprite2D TileCursorSprite;
	[Export] public Sprite2D SelectedTileSprite;
	private Vector2 CameraDragStart;
	
	public DetailedTileInfo SelectedTile;
	
	[Signal]
	public delegate void TileClickedEventHandler(DetailedTileInfo tile);
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

			if (mouseButtonEvent.ButtonIndex == MouseButton.Left)
			{
				if (mouseButtonEvent.IsPressed())
				{
					DetailedTileInfo hovered = GetHoveredTile();

					if (hovered.Valid)
					{
						SelectedTile = hovered;
						EmitSignal(SignalName.TileClicked, SelectedTile);
						SelectedTileSprite.Position = BottomLayer.MapToLocal(SelectedTile.Position);
					}
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
		
		TileCursorSprite.SetPosition(BottomLayer.MapToLocal(GetHoveredTile().Position));
	}

	public void InitMap(byte[] mapData)
	{
		for (int i = 0; i < mapData.Length/2; i++)
		{
			int x = i%96;
			int y = i/96;
			Vector2I position = new Vector2I(x, y);
			
			TopLayer.SetCell(position, 0, DetailedTileInfo.ByteToAtlasPosition(mapData[i]));
			BottomLayer.SetCell(position, 1, DetailedTileInfo.ByteToAtlasPosition(mapData[i+mapData.Length/2]));
		}
	}
	
	public TileMapLayer GetLayerNode(MapLayer layer)
	{
		return layer == MapLayer.Bottom ? BottomLayer : TopLayer;
	}

	public Vector2 GetMousePositionOnMap()
	{
		return GetViewport().GetMousePosition() + Camera.Offset;
	}
	public DetailedTileInfo GetHoveredTile()
	{
		DetailedTileInfo result = new DetailedTileInfo();
		result.Position = BottomLayer.LocalToMap(GetMousePositionOnMap());
		result.BottomLayerData = BottomLayer.GetCellTileData(result.Position);
		result.TopLayerData = TopLayer.GetCellTileData(result.Position);
		result.BottomLayerInAtlas = BottomLayer.GetCellAtlasCoords(result.Position);
		result.TopLayerInAtlas = TopLayer.GetCellAtlasCoords(result.Position);
		return result;
	}
}
