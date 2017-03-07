﻿/* Written by Yang Liu
 * This script:
 * 1. create a new GameObject called "ControlLine" on Start(),
 * 2. update the LineRenderer and EdgeCollider on "ControlLine" each frame
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ControlStatus))]
public class LineUpdate : MonoBehaviour {
	
	GameObject ControlLine;

	public Material EnemyLineMaterial;
	public Material PlayerLineMateial;

	public float lineWidth;


	// private field for LineRenderer and EdgeCollider
	LineRenderer lr;
	EdgeCollider2D LineEC;

	// a read-only Boss property that reads Boss from ControlStatus
	GameObject Boss{
		get{
			return GetComponent<ControlStatus> ().Boss;
		}
	}

	// a read-only Boss property that reads Hacker from ControlStatus
	GameObject Hacker{
		get{
			return GetComponent<ControlStatus> ().Hacker;
		}
	}

	// a read-only controller property that reads from ControlStatus
	Controller controller{
		get{
			return GetComponent<ControlStatus> ().controller;
		}
	}

	// Use this for initialization
	void Start () {
		ControlLine = new GameObject();
		ControlLine.transform.position = gameObject.transform.position;

		// create and initialize LineRenderer
		ControlLine.AddComponent<LineRenderer>();
		lr = ControlLine.GetComponent<LineRenderer>();
		lr.material = EnemyLineMaterial;
		Color color = Color.white;
		lr.startWidth = lineWidth;
		lr.endWidth = lineWidth;
		lr.startColor = color;
		lr.endColor = color;
		lr.SetPosition(0, gameObject.transform.position);
		lr.SetPosition(1, Boss.transform.position);

		// create and initialize EdgeCollider2D
		ControlLine.AddComponent<EdgeCollider2D>();
		LineEC = ControlLine.GetComponent<EdgeCollider2D>();
		LineEC.isTrigger = true;
		Vector2[] temparray = new Vector2[2];
		temparray[0] = new Vector2(0, 0);
		temparray[1] = new Vector2(Boss.transform.position.x- gameObject.transform.position.x, Boss.transform.position.y - gameObject.transform.position.y);
		LineEC.points = temparray;

		// initialize the tag as "EnemyLine"
		ControlLine.tag = "EnemyLine";

		// rename this new game object
		ControlLine.name = "ControlLine";

		ControlLine.transform.SetParent(gameObject.transform);
		// Debug.Log(gameObject.name + ControlLine.transform.position);
	}

	void Draw(GameObject start, GameObject end, Material Mat)
	{
		lr.material = Mat;
		Color color = Color.white;
		lr.startColor = color;
		lr.endColor = color;
		lr.SetPosition(0, start.transform.position);
		lr.SetPosition(1, end.transform.position);
		lr.enabled = true;
	}
		

	// Update is called once per frame
	void Update () {

		// Update LineRenderer
		if(controller == Controller.Boss)
		{
			Draw(gameObject, Boss, EnemyLineMaterial);
			LineEC.enabled = true;
			ControlLine.tag = "EnemyLine";
		}
		if (controller == Controller.None)
		{
			lr.enabled = false;
			LineEC.enabled = false;
			ControlLine.tag = "Untagged";

		}
		if (controller == Controller.Hacker)
		{
			Draw(gameObject, Hacker, PlayerLineMateial);
			LineEC.enabled = true;
			ControlLine.tag = "PlayerLine";
		}



		// Update edge collider
		LineEC.isTrigger = true;
		Vector2[] temparray = new Vector2[2];
		Vector3 Boss2Self = Boss.transform.position - transform.position;
		temparray[0] = new Vector2(0, 0);
		temparray[1] = ControlLine.transform.InverseTransformVector (Boss2Self);
		LineEC.points = temparray;

	}
		
}