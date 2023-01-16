using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// script responsible for changing camera position when 'Q' is pressed
/// </summary>
public class ChangeCameraPosition : MonoBehaviour {
    /// <summary>
    /// game object representing camera
    /// </summary>
    public GameObject camera;
    /// <summary>
    /// index from which rotation and position for camera is read
    /// </summary>
    private int index = 0;
    /// <summary>
    /// list of camera positions
    /// </summary>
    private List<Vector3> cameraCoords = new  List<Vector3>();
    /// <summary>
    /// list of camera rotations
    /// </summary>
    private List<Quaternion> cameraRotations = new List<Quaternion>();

    // Use this for initialization
    /// <summary>
    /// fill lists with available positions and rotatins for camera
    /// </summary>
    void Start () {
		cameraCoords.Add(new Vector3(-20.75f, 5f, -2.3f));
		cameraCoords.Add(new Vector3(20.75f, 5f, -2.3f));
		cameraCoords.Add(new Vector3(-20.75f, 5f, 2.3f));
		cameraCoords.Add(new Vector3(20.75f, 5f, 2.3f));
		cameraCoords.Add(new Vector3(29.75f, 5f, 24f));
		cameraCoords.Add(new Vector3(25.5f, 5f, 24f));
		cameraCoords.Add(new Vector3(25.5f, 5f, -24f));
		cameraCoords.Add(new Vector3(29.75f, 5f, -24f));

		for(int i = 0; i<cameraCoords.Count; i++)
			cameraRotations.Add(new Quaternion(0f,0f,0f,0f));

		cameraRotations[0] = Quaternion.Euler(33f, 63f, 0f);
		cameraRotations[1] = Quaternion.Euler(33f, 63f, 0f);
		cameraRotations[2] = Quaternion.Euler(33f, 117f, 0f);
		cameraRotations[3] = Quaternion.Euler(33f, 117f, 0f);
		cameraRotations[4] = Quaternion.Euler(33f, -150f, 0f);
		cameraRotations[5] = Quaternion.Euler(33f, 150f, 0f);
		cameraRotations[6] = Quaternion.Euler(33f, 24f, 0f);
		cameraRotations[7] = Quaternion.Euler(33f, -24f, 0f);

		camera.transform.position = cameraCoords[0];
		camera.transform.rotation = cameraRotations[0];
		}

    // Update is called once per frame
    /// <summary>
    /// checks if 'Q' is pressed and if it is changes camera position
    /// </summary>
    void Update () {
		if(Input.GetKeyDown(KeyCode.Q))
		{
			if(index == cameraCoords.Count - 1)
				index = 0;
			else
				index++;
				
			camera.transform.position = cameraCoords[index];
			camera.transform.rotation = cameraRotations[index];
		}
	}
}
