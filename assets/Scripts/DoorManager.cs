using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// script opening doors when any character is near it and closing them otherwise
/// </summary>
public class DoorManager : MonoBehaviour {
    /// <summary>
    /// all doors found on scene
    /// </summary>
    private GameObject[] doors;
    /// <summary>
    /// rotation of door when closed
    /// </summary>
    private Quaternion[] doorsClosedRotations;
    /// <summary>
    /// rotation of door when open
    /// </summary>
    private Quaternion[] doorsOpenRotations;

    // Use this for initialization
    /// <summary>
    /// get all doors from scene, get rotations of closed doors and calculate rotations of open doors
    /// </summary>
    void Start () {
		doors = GameObject.FindGameObjectsWithTag("Door");
		doorsClosedRotations = new Quaternion[doors.Length];
		doorsOpenRotations = new Quaternion[doors.Length];
		for(int i=0; i<doors.Length; i++)
		{
			doorsClosedRotations[i] = doors[i].transform.localRotation;
			doorsOpenRotations[i] = doors[i].transform.localRotation * Quaternion.Euler(0f, 0f, -90f);
		}
	}
    /// <summary>
    /// checks if door should be opening
    /// </summary>
    /// <param name="door">door to be checked</param>
    /// <returns>true if any person is near door, false otherwise</returns>
    bool ShouldDoorOpen(GameObject door)
	{
		GameObject[] people = GameObject.FindGameObjectsWithTag("Human");
		foreach(var person in people)
		{
			if(person != null)
			{
				if(Vector3.Distance(door.transform.position,  person.transform.position) < 3f)
					return true;
			}
		}
		return false;
	}

    // Update is called once per frame
    /// <summary>
    /// slowly opens doors is any character is near it, closes them otherwise
    /// </summary>
    void Update () 
	{
		for(int i=0; i<doors.Length; i++)
		{
			if(ShouldDoorOpen(doors[i]))
				doors[i].transform.localRotation = Quaternion.Slerp(doors[i].transform.localRotation, doorsOpenRotations[i], 0.04f);
			else
				doors[i].transform.localRotation = Quaternion.Slerp(doors[i].transform.localRotation, doorsClosedRotations[i], 0.04f);
		}
	}
}
