using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateSlider : MonoBehaviour {
    [SerializeField]
    GameObject targetImage, parentImage;
    private List<float> sizeX = new List<float>();

    private void Start()
    {
        StartCoroutine(LateScaleChange(1.5f));
    }

    // Update is called once per frame
    void Update () {
        sizeX.Add(targetImage.transform.localScale.x);
	}

    IEnumerator LateScaleChange(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        Vector3 size = targetImage.transform.localScale;
        while (true) {
            size.x = sizeX[0];
            targetImage.transform.localScale = size;
            sizeX.RemoveAt(0);
            yield return null;
        }
    }
}
