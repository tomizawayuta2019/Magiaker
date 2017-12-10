using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BOSS : EnemyController {
	public override void Death ()
	{
		GotoResult.InitGotoResult ();
		base.Death ();
	}
}
