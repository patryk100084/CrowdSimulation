using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// script generating textures based on loaded images
/// </summary>
public class ImageLoader : MonoBehaviour {
    /// <summary>
    /// list of generated textures
    /// </summary>
    public List<Texture2D> textures = new List<Texture2D>();

    // Use this for initialization
    /// <summary>
    /// loads images from "Textures" folder and generates textures based on images
    /// </summary>
    /// <exception cref="ImageLoaderException">
	/// thrown when folder is missing or no pictures were found
	/// </exception>
    public void LoadImages() {
		string directoryPath = Path.GetDirectoryName(Application.dataPath) + "/Textures/";
		try
		{
			string[] files = Directory.GetFiles(directoryPath);
			Array.Sort(files);
			foreach(var file in files)
			{
				if(Path.GetExtension(file) == ".jpg" || Path.GetExtension(file) == ".png")
				{
					string fileName = Path.GetFileName(file);
					byte[] byteArray = File.ReadAllBytes(file);
					Texture2D texture = new Texture2D(2,2);
					texture.name = fileName;
					bool isLoaded = texture.LoadImage(byteArray);
					if(isLoaded)
					{
						textures.Add(texture);
					}
				}
			}
		}
		catch
		{	
			throw new ImageLoaderException("Error! - Textures folder not found");
		}
		if(textures.Count == 0)
		{
			throw new ImageLoaderException("Error! - no textures found");
		}
	}

}
