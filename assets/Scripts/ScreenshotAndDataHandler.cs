using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/// <summary>
/// script responsible for generating data and screenshots of application
/// </summary>
public class ScreenshotAndDataHandler : MonoBehaviour 
{
    /// <summary>
    /// array of characters
    /// </summary>
	private GameObject[] people;
    /// <summary>
    /// camera
    /// </summary>
    private Camera thisCamera;
    /// <summary>
    /// folder in which data is saved
    /// </summary>
    private string folderName = "";
    /// <summary>
    /// screen height
    /// </summary>
    private int height = 0;
    /// <summary>
    /// screen width
    /// </summary>
    private int width = 0;
    /// <summary>
    /// should application take screenshot
    /// </summary>
    public bool takeScreenshots;
    /// <summary>
    /// should application generate data
    /// </summary>
    public bool getData;
    /// <summary>
    /// path at which are generated folders for data
    /// </summary>
    public string dataPath;
    /// <summary>
    /// creates folders for data
    /// </summary>
    /// <exception cref="DataHandlerException">
    /// throws exception when folder could not be created or found
    /// </exception>
    public void CreateDataDictionaries()
    {
        folderName = System.DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
        try
        {
            if (!Directory.Exists(dataPath + "/Screenshots/" + folderName) && takeScreenshots)
                Directory.CreateDirectory(dataPath + "/Screenshots/" + folderName);
            if (!Directory.Exists(dataPath + "/Data/" + folderName) && getData)
                Directory.CreateDirectory(dataPath + "/Data/" + folderName);
            this.enabled = true;
        }
        catch
        {
            throw new DataHandlerException("Error! - Data Directory Path could not found nor created");
        }
    }
    /// <summary>
    /// finds camera and people on scene
    /// </summary>
    void Start()
    {
        thisCamera = GetComponent<Camera>();
        people = GameObject.FindGameObjectsWithTag("Human");
    }
    /// <summary>
    /// checks how many people are on scene, generates data if any person are on scene at given moment
    /// </summary>
    void Update() 
    {
        people = GameObject.FindGameObjectsWithTag("Human");
        if(people.Length > 0)
        {
            if(takeScreenshots)
                StartCoroutine(MakeScreenshot());
            if(getData)
                GetData();    
        }
    }
    /// <summary>
    /// coroutine that saves data in text file
    /// </summary>
    /// <param name="data"> 
    /// data to be saved
    /// </param>
    IEnumerator SaveData(string data)
	{
		string fileName = System.DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss.fff");

        try
        {
            System.IO.File.WriteAllText(dataPath + "/Data/" + folderName + "/" + fileName + ".txt", data);
        }
        catch
        {

        }
		yield return null;
	}

    /// <summary>
    /// coroutine saving screenshot
    /// </summary>
    /// <param name="renderResult">
    /// screenshot to be saved
    /// </param>
    IEnumerator SaveFile(Texture2D renderResult)
    {
        string fileName = System.DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss.fff");

        byte[] byteArray = renderResult.EncodeToJPG(75);
        try
        {
            System.IO.File.WriteAllBytes(dataPath + "/Screenshots/" + folderName + "/" + fileName + ".jpg", byteArray);
        }
        catch
        {
            
        }

        yield return null;
    }

    /// <summary>
    /// gets data from each character, discards any non-usable data (when most joints have 0,0,0 rotations)
    /// </summary>
    void GetData()
    {
        string data = "";
        foreach (var person in people)
        {
            Transform root = FindRigRoot(person);
            int notRotatedJoints = 0;
            string personData = "";
            if(root)
            {
                foreach(Transform joint in root.GetComponentsInChildren<Transform>())
                    if(joint.localRotation == Quaternion.identity)
                        notRotatedJoints++;
                 if(notRotatedJoints < 6)
                 {
                    IDGenerator IDgen = person.GetComponent<IDGenerator>();
                    SkinnedMeshRenderer smr = person.GetComponentInChildren<SkinnedMeshRenderer>();
                    personData += "person ID: " + IDgen.GetID() + "\n";
                    personData += "Bounding Box: (min[X,Y,Z]) - (max[X,Y,Z])\n"; 
                    personData += "[" + smr.bounds.min.x + ", " + smr.bounds.min.y + ", " + smr.bounds.min.z + "] - [" + smr.bounds.max.x + ", " + smr.bounds.max.y + ", "  + smr.bounds.max.z + "]" + "\n";
                    personData += "Bounding Box Projection: (min[X,Y,Z]) - (max[X,Y,Z])\n";
                    Vector3 projectedBBoxMin = thisCamera.projectionMatrix.MultiplyPoint(smr.bounds.min);
                    Vector3 projectedBBoxMax = thisCamera.projectionMatrix.MultiplyPoint(smr.bounds.max);
                    personData += "[" + projectedBBoxMin.x + ", " + projectedBBoxMin.y + ", " + projectedBBoxMin.z + "] - [" + projectedBBoxMax.x + ", " + projectedBBoxMax.y + ", "  + projectedBBoxMax.z + "]" + "\n";
                    personData += "Joint Rotations (X,Y,Z): \n";
                    foreach(Transform joint in root.GetComponentsInChildren<Transform>())
                    {
                        Vector3 rot = joint.localRotation.eulerAngles;
                        personData += joint.name + ":" + " " + rot.x + " " + rot.y + " " + rot.z + "\n";
                    }
                    personData += "\n";
                    data += personData;
                 }
            }
        }
        if(data != "")
            StartCoroutine(SaveData(data));
    }
    /// <summary>
    /// coroutine generating screenshot from camera view
    /// </summary>
    IEnumerator MakeScreenshot()
    {
        yield return new WaitForEndOfFrame();

        height = Mathf.Min(Screen.height, 1080);
        width = Mathf.Min(Screen.width, 1920);

        thisCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        RenderTexture renderTexture = thisCamera.targetTexture;

        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        renderResult.ReadPixels(rect, 0, 0);

        StartCoroutine(SaveFile(renderResult));

        RenderTexture.ReleaseTemporary(renderTexture);
        thisCamera.targetTexture = null;
    }
    /// <summary>
    /// finds root of character rig
    /// </summary>
    /// <param name="go">
    /// object representing character
    /// </param>
    /// <returns>
    /// rig root if it was found 
    /// </returns>
    Transform FindRigRoot(GameObject go)
	{
		if(go.name == "MaleAvatar(Clone)")
			return go.transform.Find("m_avg_root/m_avg_Pelvis");
		else if(go.name == "FemaleAvatar(Clone)")
			return go.transform.Find("f_avg_root/f_avg_Pelvis");
        else
            return null;
	}
}
