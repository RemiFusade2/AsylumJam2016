using UnityEngine;
using System.Collections;

public class OutOfBoundariesBehaviour : MonoBehaviour {

    public GameObject Monster;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    void OnTriggerEnter(Collider objectCol)
    {
        if (objectCol.tag == "Player")
        {
            // spawn monster behind player
            float monsterY = Monster.transform.position.y;
            Vector3 newMonsterPos = objectCol.transform.position - objectCol.transform.forward * 15.0f;
            newMonsterPos += (-newMonsterPos.y + monsterY) * objectCol.transform.up;
            GameObject newMonster = Instantiate(Monster, newMonsterPos, Quaternion.identity) as GameObject;
            newMonster.GetComponent<MonsterBehaviour>()._safecallvisibilitythreshold = 2.0f;
        }
    }
}
