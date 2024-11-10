using Godot;
using System;

public enum MapLayer
{
    Top,
    Bottom
}

public partial class DetailedTileInfo : GodotObject
{
    public Vector2I Position = default;

    public TileData BottomLayerData = null;
    public TileData TopLayerData = null;

    public Vector2I BottomLayerInAtlas = new(-1, -1);
    public Vector2I TopLayerInAtlas = new(-1, -1);

    public bool HasBottomLayer => BottomLayerData != null;

    public bool HasTopLayer => TopLayerData != null;
	
    public bool Valid => HasBottomLayer || HasTopLayer;

    public TileData GetDataByLayer(MapLayer layer)
    {
        return layer == MapLayer.Bottom ? BottomLayerData : TopLayerData;
    }

    public Vector2I GetAtlasPositionByLayer(MapLayer layer)
    {
        return layer == MapLayer.Bottom ? BottomLayerInAtlas : TopLayerInAtlas;
    }

    public void SetAtlasPositionByLayer(MapLayer layer, Vector2I atlasPosition)
    {
        if (layer == MapLayer.Bottom)
        {
            BottomLayerInAtlas = atlasPosition;
        }
        TopLayerInAtlas = atlasPosition;
    }

    public byte GetByteAtLayer(MapLayer layer)
    {
        return AtlasPositionToByte(GetAtlasPositionByLayer(layer));
    }

    public void SetByteAtLayer(MapLayer layer, byte value)
    {
        Vector2I atlasPosition = ByteToAtlasPosition(value);
        SetAtlasPositionByLayer(layer, atlasPosition);
    }

    public static byte AtlasPositionToByte(Vector2I atlasPosition)
    {
        byte x = (byte)atlasPosition.X;
        byte y = (byte)atlasPosition.Y;

        return (byte)(x + y * 16);
    }

    public static Vector2I ByteToAtlasPosition(byte value)
    {
        int atlasX = value % 16;
        int atlasY = value / 16;
        return new Vector2I(atlasX, atlasY);
    }
}