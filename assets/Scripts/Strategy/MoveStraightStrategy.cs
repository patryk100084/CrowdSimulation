using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class implementing movement behaviour strategy
/// </summary>
public class MoveStraightStrategy : IMovementStrategy {

    /// <summary>
    /// method representing straight movement
    /// </summary>
    /// <param name="speed">rotation speed of character</param>
    /// <param name="trs">position towards which character is heading</param>
    /// <param name="targrtRot">rotation towards which character is heading</param>
    public void MoveCharacter(ref float speed, Transform trs, Quaternion targrtRot)
	{
		speed = 0f;
	}
}
