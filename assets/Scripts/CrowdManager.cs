using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// script responsible for adding people onto scene within fixed time frame
/// </summary>
public class CrowdManager : MonoBehaviour {
    /// <summary>
    /// amount of people on scene at given moment
    /// </summary>
    public static int peopleOnScene = 0;
    /// <summary>
    /// maximum amount of people that can be on scene at given moment
    /// </summary>
    public int maxPeople;
    /// <summary>
    /// prototype of male character
    /// </summary>
    public GameObject maleAvatarPrefab;
    /// <summary>
    /// prototype of female character
    /// </summary>
    public GameObject femaleAvatarPrefab;
    /// <summary>
    /// array of points on which characters are spawn
    /// </summary>
    public GameObject[] spawnPoints;
    /// <summary>
    /// list of textures applied onto character model
    /// </summary>
    private List<Texture2D> textures;
    /// <summary>
    /// list contaning gender of spawned character
    /// </summary>
    private List<string> genderList;
    /// <summary>
    /// list contaning model scale of spawned character
    /// </summary>
    private List<float> scaleList;
    /// <summary>
    /// list contaning model parameters of spawned character
    /// </summary>
    private List<float[]> paramsList;
    /// <summary>
    /// list contaning name or number of texture applied to spawned character
    /// </summary>
    private List<string> textureNames;
    /// <summary>
    /// list contaning number of animation applied to spawned character
    /// </summary>
    private List<int> animationNumberList;
    /// <summary>
    /// index at which data is read from all lists 
    /// </summary>
    private int index;
    /// <summary>
    /// amount of frames left to spawn next character
    /// </summary>
    private int countdown;

    // Use this for initialization
    /// <summary>
    /// get data read from CSV file and loaded textures, set countdown between 45 to 90 frames
    /// </summary>
    void Start () 
	{
		textures = gameObject.GetComponent<ImageLoader>().textures;
		genderList = gameObject.GetComponent<CSVParser>().genderList;
		scaleList = gameObject.GetComponent<CSVParser>().scaleList;
		paramsList = gameObject.GetComponent<CSVParser>().paramsList;
		textureNames = gameObject.GetComponent<CSVParser>().textureNames;
		animationNumberList = gameObject.GetComponent<CSVParser>().animationNumberList.ToList();

		countdown = UnityEngine.Random.Range(45, 91);
	}
    /// <summary>
    /// count off time left to spawn character, once counting is finished spawn character and start counting process all over
    /// </summary>
    void Update()
	{
		countdown--;
		if(countdown == 0)
		{
			AddPerson();
			countdown = UnityEngine.Random.Range(45, 91);
		}
	}
    /// <summary>
    /// sets textures of character
    /// </summary>
    /// <param name="name">name or number of the texture</param>
    /// <param name="go">character</param>
    void SetTexture(string name, GameObject go)
	{
		Material mat = go.GetComponentInChildren<Renderer>().material;
		if(mat)
		{
			try
			{
				int i = int.Parse(name);
				if(i < 1)
					i = 1;
				if(i > textures.Count)
					i = textures.Count;
				mat.mainTexture = textures[i-1];
				return;
			}
			catch
			{
				string textureName = name.Replace("\"","");
				if(textureName == "*")
				{
					mat.mainTexture = textures[UnityEngine.Random.Range(0,textures.Count)];
					return;
				}
				foreach(var texture in textures)
				{
					Debug.Log("texture name: " + texture.name);
					if(texture.name == textureName)
					{
						mat.mainTexture = texture;
						return;
					}
				}
			}
		}
	}
	/// <summary>
	/// adds person onto scene if there are fewer than limit
	/// </summary>
	void AddPerson()
	{
		if(peopleOnScene < maxPeople)
		{
			if(index == genderList.Count)
			{
				index = 0;
			}
			GameObject go = null;

			GameObject spawnPoint = spawnPoints[UnityEngine.Random.Range(0,spawnPoints.Length)];
			GameObject targetPoint = spawnPoint.GetComponent<ConnectionList>().pointsArray[0];

			Vector3 spawnPosition = new Vector3(spawnPoint.transform.position.x, 0.0f, spawnPoint.transform.position.z);
			Vector3 targtPosition = targetPoint.transform.position;

			Quaternion spawnRotation = Quaternion.LookRotation(new Vector3(targtPosition.x - spawnPosition.x, 0.0f, targtPosition.z - spawnPosition.z));

			if(genderList[index] == "female")
			{
				go = Instantiate(femaleAvatarPrefab, spawnPosition, spawnRotation) as GameObject;
			}
			else if(genderList[index] == "male")
			{
				go = Instantiate(maleAvatarPrefab, spawnPosition, spawnRotation) as GameObject;
			}
			if(go)
			{
				float scale = scaleList[index];
				go.transform.localScale = new Vector3(scale, scale, scale);

				PersonWalk pw = go.GetComponent<PersonWalk>();
				pw.targetPoint = targetPoint;
				pw.animationNumber = animationNumberList[index];

				SMPLBlendshapes blendshapes = go.GetComponentInChildren<SMPLBlendshapes>();
				if(blendshapes)
				{
					float[] shapeParms = new float[10];
					for(int i = 0; i<10; i++)
						shapeParms[i] = paramsList[index][i];
					blendshapes._shapeParms = shapeParms;
					blendshapes.enabled = true;
				}
				
				SetTexture(textureNames[index], go);

				index++;
				peopleOnScene++;
			}
		}
	}
}
