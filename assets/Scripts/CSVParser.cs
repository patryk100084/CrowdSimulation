using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// script responsible for reading data from CSV file
/// </summary>
public class CSVParser : MonoBehaviour {
    /// <summary>
    /// path of file to be read
    /// </summary>
    public string filePath;
    /// <summary>
    ///  character separating data in file
    /// </summary>
    public char delimiter;
    /// <summary>
    /// list of genders read from file
    /// </summary>
    public List<string> genderList = new List<string>();
    /// <summary>
    /// list of model scale read from file 
    /// </summary>
    public List<float> scaleList = new List<float>();
    /// <summary>
    /// list of model parameters read from file
    /// </summary>
    public List<float[]> paramsList = new List<float[]>();
    /// <summary>
    ///  list of texture names or numbers read from file
    /// </summary>
    public List<string> textureNames = new List<string>();
    /// <summary>
    /// list of animation numbers read from file
    /// </summary>
    public HashSet<int> animationNumberList = new HashSet<int>();
    /// <summary>
    /// length of lists
    /// </summary>
    public int listsLength;
    /// <summary>
    /// checks whether gender is correct
    /// </summary>
    /// <param name="s">text to be checked</param>
    /// <returns>adequate gender if text is correct, male otherwise</returns>
    string CheckGender(string s)
	{
		if(s == "male" || s =="female")
			return s;
		else
			return "male";
	}
    /// <summary>
    /// checks if floating point number is withing certain limits
    /// </summary>
    /// <param name="value">value to be checked</param>
    /// <param name="min">minmal allowed value</param>
    /// <param name="max">maximal allowed value</param>
    /// <returns>min if number is too low, max if number if too high, value otherwise</returns>
    float CheckFloat(float value, float min, float max)
	{
		if(value <= min)
			return min;
		if(value >= max)
			return max;
		else
			return value;
	}
    /// <summary>
    /// checks if integer number is withing certain limits
    /// </summary>
    /// <param name="value">value to be checked</param>
    /// <param name="min">minmal allowed value</param>
    /// <param name="max">maximal allowed value</param>
    /// <returns>min if number is too low, max if number if too high, value otherwise</returns>
    int CheckInt(int value, int min, int max)
	{
		if(value <= min)
			return min;
		if(value >= max)
			return max;
		else
			return value;
	}
    /// <summary>
    /// parses file
    /// </summary>
    /// <exception cref="CSVParserException">
	/// thrown when file could not be found or has no usable data
	/// </exception>
    public void ParseCSVFile()
	{
		try
		{
			if(!Path.IsPathRooted(filePath))
			{
				string appDirectory = Path.GetDirectoryName(Application.dataPath);
				string temp = filePath;
				filePath = appDirectory + "/" + temp;
			}
			StreamReader reader = new StreamReader(filePath);
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine();
				string[] values = line.Split(delimiter);

				if(values.Length == 13)
				{
					try
					{
						float[] param = new float[10];
						
						for(int i=1; i<param.Length; i++)
						{
							param[i] = CheckFloat(Single.Parse(values[i+1]), -5f, 5f);
						}
						if(animationNumberList.Add(CheckInt(Int32.Parse(values[12]), 1, 8)))
						{
							genderList.Add(CheckGender(values[0]));
							scaleList.Add(CheckFloat(Single.Parse(values[1]), 0.95f, 1.05f));
							paramsList.Add(param);
							textureNames.Add(values[11]);
							listsLength++;
						}
					}
					catch
					{

					}
				}
			}
		}
		catch
		{
			throw new CSVParserException("Error! - CSV File doesn't exist");
		}
		if(listsLength == 0)
		{
			throw new CSVParserException("Error! - No data found in CSV file!");
		}
	}
}
