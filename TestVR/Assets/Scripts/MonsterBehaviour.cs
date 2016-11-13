using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class MonsterBehaviour : MonoBehaviour {

    private bool played;

	// Use this for initialization
	void Start () {
        played = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!played && CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            played = true;
            this.GetComponent<AudioSource>().Play();
        }
	}
}
