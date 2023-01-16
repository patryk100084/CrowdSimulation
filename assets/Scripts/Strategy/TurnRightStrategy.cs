using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// class implementing movement behaviour strategy
/// </summary>
public class TurnRightStrategy : IMovementStrategy {
    /// <summary>
    /// method representing turning right to avoid obstacle
    /// </summary>
    /// <param name="speed">rotation speed of character</param>
    /// <param name="trs">position towards which character is heading</param>
    /// <param name="targrtRot">rotation towards which character is heading</param>
    public void MoveCharacter(ref float speed, Transform trs, Quaternion targrtRot)
	{
		trs.Rotate(speed * Vector3.up);
		if(speed < 2.5f)
			speed += 0.1f;
	}

}
