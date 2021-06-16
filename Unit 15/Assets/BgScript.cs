using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScript : MonoBehaviour
{
    public static BgScript BgInstance;

    // Method that will allow background music to play between scenes
    private void Awake()
    {

        if (BgInstance != null && BgInstance != this) {
            Destroy(this.gameObject);
            return;
        }

        BgInstance = this;
        DontDestroyOnLoad(this);
    }
}
