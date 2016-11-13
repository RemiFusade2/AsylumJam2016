using UnityEngine;
using System.Collections;

public class CreepyEyesBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetVisible(bool visible)
    {
        this.GetComponent<Animator>().SetBool("Visible", visible);
    }
}
