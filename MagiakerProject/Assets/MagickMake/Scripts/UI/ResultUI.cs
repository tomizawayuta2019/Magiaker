using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultUI : MonoBehaviour {
    public GameObject clear, lose;

	// Use this for initialization
	void Start () {
        clear.SetActive(GotoResult.isClear);
        lose.SetActive(!GotoResult.isClear);
    }
}
