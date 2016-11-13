using UnityEngine;
using System.Collections;

public class DoorAndKey : MonoBehaviour {

	public GameObject _key;
	public bool _keyGrabed;
	public GameObject _door;
	public bool etBill;

    public Transform player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("joystick button 5")) {
			Debug.Log ("grabed");
		}

		if (Vector3.Distance (player.transform.position, _key.transform.position) < 2f) 
		{
			if (Input.GetKeyDown ("joystick button 5")) 
			{
				Debug.Log ("key grabed");
				_keyGrabed = true;
                _key.SetActive(false);
            }
		}

		if (Vector3.Distance (player.transform.position, _door.transform.position) < 2f && _keyGrabed) 
		{
			if (Input.GetKeyDown ("joystick button 5")) 
			{
				Debug.Log ("DoorOpen");
                // YOU WIN
                _door.GetComponent<AudioSource>().Play();
            }
		}
	}
}
