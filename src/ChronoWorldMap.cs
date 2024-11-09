using Godot;
using System;

public partial class DetailedTileData : GodotObject
{
	public Vector2I Position;
	public TileData BottomLayerData;
	public TileData TopLayerData;

	public Vector2I BottomLayerInAtlas;
	public Vector2I TopLayerInAtlas;

	public DetailedTileData()
	{
		Position = default;
		BottomLayerData = null;
		TopLayerData = null;
		BottomLayerInAtlas =  new Vector2I(-1, -1);
		TopLayerInAtlas = new Vector2I(-1, -1);
	}

	public bool HasBottomLayer => BottomLayerData != null;

	public bool HasTopLayer => TopLayerData != null;
	
	public bool Valid => HasBottomLayer || HasTopLayer;
}
public partial class ChronoWorldMap : Node2D
{
	public TileMapLayer TopLayer;
	public TileMapLayer BottomLayer;

	const int LayerWidth = 96;
	const int LayerHeight = 64;
	
	[Export] public Camera2D Camera;
	[Export] public Sprite2D TileCursorSprite;
	private Vector2 CameraDragStart;
	
	public DetailedTileData SelectedTile;
	
	[Signal]
	public delegate void TileClickedEventHandler(DetailedTileData tile);
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
					DetailedTileData hovered = GetHoveredTile();

					if (hovered.Valid)
					{
						SelectedTile = hovered;
						EmitSignal(SignalName.TileClicked, SelectedTile);
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

	public Vector2 GetMousePositionOnMap()
	{
		return GetViewport().GetMousePosition() + Camera.Offset;
	}
	public DetailedTileData GetHoveredTile()
	{
		DetailedTileData result = new DetailedTileData();
		result.Position = BottomLayer.LocalToMap(GetMousePositionOnMap());
		result.BottomLayerData = BottomLayer.GetCellTileData(result.Position);
		result.TopLayerData = TopLayer.GetCellTileData(result.Position);
		result.BottomLayerInAtlas = BottomLayer.GetCellAtlasCoords(result.Position);
		result.TopLayerInAtlas = TopLayer.GetCellAtlasCoords(result.Position);
		return result;
	}
}
