using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WatcherEngineScript : MonoBehaviour {

    public GameObject playerBlink;

    public Transform watchersContainer;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (playerBlink.GetComponent<Blink>().invisible >= 1)
        {
            foreach (Transform watcher in watchersContainer)
            {
                watcher.GetComponent<WatcherBehaviour>().MoveWatcher();
            }
        }
        else
        {
            foreach (Transform watcher in watchersContainer)
            {
                watcher.GetComponent<WatcherBehaviour>().StopWatcher();
            }
        }
	}
}
