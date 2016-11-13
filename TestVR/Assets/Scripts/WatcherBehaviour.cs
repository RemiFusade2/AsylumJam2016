using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WatcherBehaviour : MonoBehaviour
{
    public GameObject player;

    private Vector3 destination;

    // Use this for initialization
    void Start()
    {
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerIsSeen()
    {
        MonsterBehaviour.lastSeenPosition = player.transform.position;

        this.GetComponent<Animator>().SetBool("Visible", false);

        this.GetComponent<AudioSource>().Play();
    }

    public void MoveWatcher()
    {
        if (Vector3.Distance(destination, transform.position) < 20.0f)
        {
            Vector2 unitCircle = Random.insideUnitCircle;

            float distance = Random.Range(50.0f, 100.0f);
            Vector3 direction = new Vector3(unitCircle.x, 0, unitCircle.y);

            destination = this.transform.position + distance * direction;
            
            this.GetComponent<NavMeshAgent>().SetDestination(destination);
        }
        this.GetComponent<NavMeshAgent>().Resume();
    }

    public void StopWatcher()
    {
        this.GetComponent<NavMeshAgent>().Stop();
    }
}
