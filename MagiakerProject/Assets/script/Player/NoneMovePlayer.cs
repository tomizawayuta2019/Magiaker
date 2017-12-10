using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneMovePlayer : Character {

	public override bool isAligment (aligment value)
	{
		return (value == aligment.player);
	}
}
