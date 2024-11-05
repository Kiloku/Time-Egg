using Godot;
using System;

public partial class TileSetGenerator : GodotObject
{
	public static TileSetAtlasSource GenerateChronoAtlasSource(string path)
	{
		Image img = Image.LoadFromFile(path);
		ImageTexture texture = ImageTexture.CreateFromImage(img);
		TileSetAtlasSource Source = new()
		{
			Texture = texture,
			TextureRegionSize = new Vector2I(32, 32)
		};
		
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				Source.CreateTile(new Vector2I(i,j));
			}
		}

		return Source;
	}
}
