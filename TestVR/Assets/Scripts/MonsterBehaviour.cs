using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class MonsterBehaviour : MonoBehaviour {

    private bool played;
	public static Vector3 lastSeenPosition;
	private NavMeshAgent _agent;
	public bool _moving;
	public float _sightRange;
	public GameObject _player;

	// Use this for initialization
	void Start () {
        played = false;
		_agent = this.GetComponent<NavMeshAgent> ();
    }
	
	// Update is called once per frame
	void Update () {

		if (lastSeenPosition == Vector3.zero )
        {
			if (_moving == false)
				_agent.destination = this.transform.position + new Vector3 (Random.insideUnitCircle.x, this.transform.position.y, Random.insideUnitCircle.y);
			else
            {
				if (Vector3.Distance (_agent.destination, lastSeenPosition) < 0.2f)
                {
					Invoke ("StopIdle", 2.5f);
				}
			}
		}
        else
        {
			_agent.destination = lastSeenPosition;
			if(Vector3.Distance( _agent.destination, lastSeenPosition) < 0.2f )
			{
				lastSeenPosition = Vector3.zero;
			}
		}

		if (Vector3.Distance (this.transform.position, _player.transform.position) < _sightRange && Vector3.Angle(this.transform.position, _player.transform.position) < 60f) 
		{
			RaycastHit hit;
			if (Physics.Linecast (this.transform.position, _player.transform.position, out hit)) 
			{
				if (hit.collider.tag != "Monster" && hit.collider.tag != "Player") 
				{
					_agent.destination = _player.transform.position;
					lastSeenPosition = _player.transform.position;
				}
			}
		}
	}

    void OnTriggerEnter(Collider objectCol)
    {
		if (objectCol.tag == "Player") 
		{
            //GameOver
            Application.LoadLevel("level1");
		}
	}

	void StopIdle()
	{
		_moving = false;
	}
}
