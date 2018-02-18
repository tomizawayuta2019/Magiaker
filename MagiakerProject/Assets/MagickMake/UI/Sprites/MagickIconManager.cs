using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Example/MagicIconManager")]
public class MagickIconManager : ScriptableObject {
    public List<Sprite> Sprites;
    public static MagickIconUI instance;
    public Vector2 position;

    public void Init() {
        InitMagickSelect();
        instance.transform.position = position;
    }

    public static GameObject InitMagickSelect() {
        if (instance == null) {
            instance = (Instantiate(Resources.Load("Prefabs/MagickSelectUI"))as GameObject).GetComponent<MagickIconUI>();
        }
        return instance.gameObject;
    }
}
