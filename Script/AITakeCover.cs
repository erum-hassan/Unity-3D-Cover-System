using UnityEngine;
using System.Collections;

public class AITakeCover : MonoBehaviour {

	public GameObject player;
	public GameObject[] cover;

	public float detectAngle = 120f;
	public float detectDistance = 20f;
	public float sideDetectDistance = 5f;
	public bool notice;

	public int coverNum = -1;
	
	public bool inCover;

	public NavMeshAgent nav;

	// Use this for initialization
	void Start () {
		nav = this.GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 playerVector = player.transform.position - transform.position;
		float distance = Vector3.Distance (this.transform.position, player.transform.position);
		if ((((Mathf.Abs ((int)Vector3.Angle (this.transform.forward, playerVector)) < detectAngle / 2f) && (distance < detectDistance))||(distance < sideDetectDistance))
		    && Facing()) {
			notice = true;
		}
		else{
			notice = false;
		}

		if (notice) {
			TakeCover ();
			detectDistance = 1000f;
			sideDetectDistance = 1000f;
		}

		if (coverNum >= 0) {
			nav.SetDestination (cover [coverNum].transform.position);
		}

		if(inCover){
			FacePlayer();
		}
	}

	//Find the closest, empty and safe cover
	void TakeCover(){
		float[] coverDistance = new float[cover.Length];
		for(int i=0; i<cover.Length; i++){
			coverDistance[i] = Vector3.Distance(this.transform.position, cover[i].transform.position);
		}
		float[] sortedDistance = (float[])coverDistance.Clone ();
		System.Array.Sort (sortedDistance);
		for (int i=0; i<cover.Length; i++) {
			int j = System.Array.IndexOf(coverDistance, sortedDistance[i]);
			CoverScript coverscript = cover[j].GetComponent<CoverScript>();
			if(coverscript.safe && !coverscript.full){
				coverNum = j;
				break;
			}
		}
	}

	bool Facing(){
		RaycastHit hit;
		Vector3 playerVector = player.transform.position - transform.position;
		return (Physics.Raycast (transform.position, playerVector, out hit)) && (hit.transform.gameObject.tag == "Player");
	}

	void FacePlayer(){
		Vector3 lookPosition = player.transform.position - transform.position;
		lookPosition.y = 0;
		Quaternion rotation = Quaternion.LookRotation (lookPosition);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
	}
}
