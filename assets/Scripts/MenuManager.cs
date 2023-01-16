using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// script getting data from menu inputs
/// </summary>
public class MenuManager : MonoBehaviour {
    /// <summary>
    ///  button on which click data is sent to corresponding scripts
    /// </summary>
    public Button btn;

    /// <summary>
    /// adds listener to button
    /// </summary>
    void Start () {
		btn.onClick.AddListener(GetInputs);
	}
    /// <summary>
    /// gets integer value from text input 
    /// </summary>
    /// <param name="str">string to be parsed</param>
    /// <returns>parsed value uopn success, 10 otherwise</returns>
    int GetIntFromInput(string str)
	{
		try
		{
			return int.Parse(str);
		}
		catch
		{
			return 10;
		}
	}
    /// <summary>
    /// prints error message on menu
    /// </summary>
    /// <param name="error">message to be printed</param>
    void ShowErrorMessage(string error)
	{
		Transform trs = gameObject.transform.Find("ErrorMessage");
		if(trs)
		{
			Text txt = trs.GetComponent<Text>();
			txt.text = error;
		}
	}
    /// <summary>
    /// gets data from inputs and send them to adeque scripts, prints error message if any of scripts has thrown exception
    /// </summary>
    void GetInputs()
	{
		GameObject sceneManager = GameObject.Find("SceneManager");
		GameObject camera = GameObject.Find("Main Camera");
		GameObject canvas = GameObject.Find("Canvas");
		if(sceneManager && canvas)
		{
			CrowdManager cm = sceneManager.GetComponent<CrowdManager>();
			SceneMenagerScript sms = sceneManager.GetComponent<SceneMenagerScript>();
			ScreenshotAndDataHandler dataHandler = camera.GetComponent<ScreenshotAndDataHandler>();
			CSVParser parser = sceneManager.GetComponent<CSVParser>();
			ImageLoader il = sceneManager.GetComponent<ImageLoader>();
			if(cm && sms && parser && dataHandler)
			{
				for(int i = 0; i < gameObject.transform.childCount; i++)
				{
					Transform trs = gameObject.transform.GetChild(i);
					string name = trs.name;
					switch(name)
					{
						case "MaxPeople":
							InputField input = trs.gameObject.GetComponent<InputField>();
							cm.maxPeople = GetIntFromInput(input.text);
							break;
						case "CSVPath":
							input = trs.gameObject.GetComponent<InputField>();
							parser.filePath = input.text;
							break;
						case "CSVSeparator":
							input = trs.gameObject.GetComponent<InputField>();
							if(input.text == "")
								parser.delimiter = ';';
							else
								parser.delimiter = char.Parse(input.text);
							break;
						case "DataPath":
							input = trs.gameObject.GetComponent<InputField>();
							dataHandler.dataPath = input.text;
							break;
						case "FrameRate":
							Slider slider = trs.gameObject.GetComponent<Slider>();
							sms.animationFrameRate = (int)slider.value;
							break;
						case "GetScreenshots":
							Toggle toggle = trs.gameObject.GetComponent<Toggle>();
							dataHandler.takeScreenshots = toggle.isOn;
							break;
						case "GetData":
							toggle = trs.gameObject.GetComponent<Toggle>();
							dataHandler.getData = toggle.isOn;
							break;
					}
				}
			}
			try
			{
				il.LoadImages();
				parser.ParseCSVFile();
				dataHandler.CreateDataDictionaries();
				sms.enabled = true;
				cm.enabled = true;
				canvas.SetActive(false);
			}
			catch(CSVParserException ex)
			{
				il.textures.Clear();
				ShowErrorMessage(ex.Message);
			}
			catch(DataHandlerException ex)
			{
				il.textures.Clear();
				parser.genderList.Clear();
				parser.scaleList.Clear();
				parser.paramsList.Clear();
				parser.textureNames.Clear();
				parser.animationNumberList.Clear();
				parser.listsLength = 0;
				ShowErrorMessage(ex.Message);
			}
			catch(ImageLoaderException ex)
			{
				ShowErrorMessage(ex.Message);
			}	
		}
	}
}
