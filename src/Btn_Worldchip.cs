using Godot;
using System;

public partial class Btn_Worldchip : Button
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public int Layer = 0;

	public override void _Ready()
	{
		Pressed += OnButtonPressed;
	}

	private void OnButtonPressed()
	{
		
		FileDialog fd_Worldchip = new FileDialog();
		fd_Worldchip.Title = "Open worldchip PNG file";
		fd_Worldchip.ModeOverridesTitle = false;
		fd_Worldchip.FileSelected += OnChipFileSelected;
		fd_Worldchip.FileMode = FileDialog.FileModeEnum.OpenFile;
		fd_Worldchip.Access = FileDialog.AccessEnum.Filesystem;
		fd_Worldchip.UseNativeDialog = true;
		
		fd_Worldchip.Show();
	}
	

	private void OnChipFileSelected(string path)
	{
		//TODO: A more reliable way to match the Map instead of NodePath.
		ChronoWorldMap map = GetNode<ChronoWorldMap>(new NodePath("../MapView/SubViewport/Camera2D/ChronoWorldMap"));
		
		int newLayer = map.GetLayerNode((MapLayer)Layer).TileSet.AddSource(TileSetGenerator.GenerateChronoAtlasSource(path));

		foreach (var cellPos in map.GetLayerNode((MapLayer)Layer).GetUsedCells())
		{
			Vector2I atlasCoords = map.GetLayerNode((MapLayer)Layer).GetCellAtlasCoords(cellPos);
			map.GetLayerNode((MapLayer)Layer).SetCell(cellPos, newLayer, atlasCoords);
		}
	}

}
