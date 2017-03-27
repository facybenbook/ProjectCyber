﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusManager : MonoBehaviour {

	public GameObject virusPrefab;
	// the amount of virus that would be respawned at one time
	public int respawnCount = 4;

	public float respawnRadius = 3f;
	// the amount of virus that is alive for now
	public int currentCount{
		get{
			int count = 0;
			foreach(Transform child in transform){
				// does if have a objectIdentity and the identity is virus?
				ObjectIdentity oi = child.GetComponent<ObjectIdentity> ();
				if(oi && oi.objType == ObjectType.Virus){
					// append it to the virus list
					ControlStatus cs = child.GetComponent<ControlStatus> ();
					if (cs.controller == Controller.Boss) {
						count++;
					}
				}
			}
			return count;
		}
	}

	public Transform[] targets;

	FieldOfView fov;
	// Use this for initialization
	void Start () {
		fov = GetComponent<FieldOfView> ();
	}
	
	// Update is called once per frame
	void Update () {
		// set the facing
		float dist = Mathf.Infinity;
		Transform target = null;
		foreach(Transform trans in targets){
			float newDist = Vector3.Distance (trans.position, transform.position);
			if(newDist < dist){
				//Debug.Log ("iterating target");
				dist = newDist;
				target = trans;
			}
		}
		if(target != null && fov != null){
			//Debug.Log ("set facing");
			Vector3 dir = target.position - transform.position;
			dir.Normalize ();
			fov.facing = dir;
		}

		if(currentCount == 0){
			Respawn ();
		}
	}


	// Respawn the virus around a circle with the radius of respawnRadius
	void Respawn(){
		for(int i = 0; i < respawnCount; ++i){
			GameObject newVirus = 
				Instantiate (virusPrefab, transform.position, transform.rotation);
			if(fov){
				newVirus.transform.up = fov.facing;
			}
			newVirus.transform.parent = transform;

			// stop the virus from changing its virusState until it is
			// reaching the spreadRadius
			VirusStateControl vsc = newVirus.GetComponent<VirusStateControl> ();
			vsc.enabled = false;
			StartCoroutine (EnableStateChange (newVirus.transform));

		}
//		if(virusPrefab == null || respawnCount == 0){
//			return;
//		}
//		float deltaAngle = 360f / respawnCount;
//		float curAngle = 0f;
//		for(int virusIndex = 0; virusIndex < respawnCount; virusIndex ++){
//			float dirx = Mathf.Cos (curAngle * Mathf.Deg2Rad);
//			float diry = Mathf.Sin (curAngle * Mathf.Deg2Rad);
//			Vector3 virusDir = new Vector3 (dirx, diry, 0f);
//			virusDir.Normalize ();
//
//			Vector3 virusPosOffset = virusDir * respawnRadius;
//			Vector3 virusNewPos = transform.position + virusPosOffset;
//
//			Quaternion newRot = new Quaternion();
//			newRot.eulerAngles = new Vector3 (0f, 0f, curAngle - 90f);
//			// instantiate the virus
//			GameObject newVirus = Instantiate (virusPrefab, virusNewPos, newRot);
//			newVirus.transform.SetParent (this.transform);
//		
//			curAngle += deltaAngle;
//		}
//		currentCount += respawnCount;
	}

	IEnumerator EnableStateChange(Transform virusTrans){

		yield return new WaitUntil (() => {return ReachingSpreadRadius(virusTrans);});
		if (virusTrans) {
			VirusStateControl vsc = virusTrans.GetComponent<VirusStateControl> ();
			if (vsc) {
				vsc.enabled = true;
			}
		}
	}

	bool ReachingSpreadRadius(Transform virusTrans){
		VirusPosManager vpm = GetComponent<VirusPosManager>();
		float radius = 0f;
		if(vpm){
			radius = vpm.spreadRadius;
		}
		if(virusTrans == null){
			return true;
		}
		float dist = Vector3.Distance (transform.position, virusTrans.position);

		// 0.75 is a magic number
		return (dist >= 0.75 * radius);
	}
//	public void LoseOneVirus(){
//		currentCount--;
//	}
}
