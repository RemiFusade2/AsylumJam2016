using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WatcherEngineScript : MonoBehaviour {

    public GameObject playerBlink;

    public List<WatcherBehaviour> watchers;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (playerBlink.GetComponent<Blink>().invisible >= 1)
        {
            foreach (WatcherBehaviour watcher in watchers)
            {
                watcher.MoveWatcher();
            }
        }
        else
        {
            foreach (WatcherBehaviour watcher in watchers)
            {
                watcher.StopWatcher();
            }
        }
	}
}
