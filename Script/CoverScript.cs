using UnityEngine;
using System.Collections;

public class CoverScript : MonoBehaviour {

	public GameObject player;
	public float maxLength = 100f;
	public bool safe;
	public bool full;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;
		if (Physics.Raycast (transform.position, (player.transform.position - transform.position), out hit, maxLength)) {
			if (hit.transform.gameObject.tag == "Player"){
				safe = false;
			}
			else{
				safe = true;
			}
		}
	}

	void OnTriggerEnter(Collider other){
		full = true;
		if (other.gameObject.tag == "Enemy") {
			AITakeCover aiTakeCover = other.gameObject.GetComponent<AITakeCover>();
			aiTakeCover.inCover = true;
		}
	}

	void OnTriggerExit(Collider other){
		full = false;
		if (other.gameObject.tag == "Enemy") {
			AITakeCover aiTakeCover = other.gameObject.GetComponent<AITakeCover>();
			aiTakeCover.inCover = false;
		}
	}
}
