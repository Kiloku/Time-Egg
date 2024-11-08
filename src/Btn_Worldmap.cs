using Godot;
using System;
using System.Security.AccessControl;

public partial class Btn_Worldmap : Button
{
	public override void _Ready()
	{
		Pressed += OnButtonPressed;
	}

	private void OnButtonPressed()
	{
		FileDialog fd_WorldMap = new FileDialog();
		fd_WorldMap.Title = "Open Map .dat file";
		fd_WorldMap.ModeOverridesTitle = false;
		fd_WorldMap.FileSelected += OnMapFileSelected;
		fd_WorldMap.FileMode = FileDialog.FileModeEnum.OpenFile;
		fd_WorldMap.Access = FileDialog.AccessEnum.Filesystem;
		fd_WorldMap.UseNativeDialog = true;
		
		fd_WorldMap.Show();
	}

	private void OnMapFileSelected(string file)
	{
		FileAccess access = FileAccess.Open(file, FileAccess.ModeFlags.Read);
		ulong size = access.GetLength();
		byte[] data = new byte[size];
		
		ulong index = 0;
		while ((index = access.GetPosition()) < access.GetLength())
		{
			data[index] = access.Get8();
		}
		//TODO: A more reliable way to match the Map instead of NodePath.
		ChronoWorldMap map = GetNode<ChronoWorldMap>(new NodePath("../MapView/SubViewport/Camera2D/ChronoWorldMap"));
		map.InitMap(data);
	}
}
