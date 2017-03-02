using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class TorchLightBehaviour : MonoBehaviour {

    public GameObject player;

    public Animator TorchAnimator;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (player.GetComponent<FirstPersonController>().GetCharacterController().velocity.magnitude > 2.0f)
        {
            TorchAnimator.SetBool("Moving", true);
        }
        else
        {
            TorchAnimator.SetBool("Moving", false);
        }
	}
}
