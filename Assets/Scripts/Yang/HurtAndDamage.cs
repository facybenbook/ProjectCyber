﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtAndDamage : MonoBehaviour {



	public TypeDamagePair[] DamageOtherList;
	public TypeDamagePair[] HurtSelfList;

	Dictionary<ObjectType, float> DamageOtherDict = new Dictionary<ObjectType, float>();
	Dictionary<ObjectType, float> HurtSelfDict = new Dictionary<ObjectType, float>();

	// if true, this object would kill anything collides into itself
	public bool instantKillOther = false;
	// if true, this object would kill itself when collides into anything
	public bool instantKillSelf = false;

	// if set true, this object would not be able to hurt others anymore,
	// even with instantKillOther enabled.
	[HideInInspector]public bool canHurtOther = true;
	// if set false, this object would not be able to hurt itself anymore,
	// even with instantKillSelf enabled.
	[HideInInspector]public bool canHurtSelf = true;

	HealthSystem selfHealthSystem;
	// Use this for initialization
	void Start () {
		// initialize fields
		selfHealthSystem = GetComponent<HealthSystem> ();

		// translate the two arrays into dictionary to boost speed
		foreach (TypeDamagePair pair in DamageOtherList){
			DamageOtherDict.Add (pair.type, pair.damage);
		}
		foreach (TypeDamagePair pair in HurtSelfList){
			HurtSelfDict.Add (pair.type, pair.damage);
		}
	}

	/* this object could only hurt itself when:
	 * 1. this.canHurtSelf && other.canHurtOther
	 * 2. this.canHurtSelf && other does not have HurtAndDamage
	 */
	bool VerifyHurtSelf(Transform other){
		if(!this.canHurtSelf){
			return false;
		}
		HurtAndDamage hd = other.GetComponent<HurtAndDamage> ();
		if(hd == null || hd.canHurtOther == true){
			return true;
		} else{
			return false;
		}
	}

	/* this object could only hurt other when:
	 * 1. this.canHurtOther && other.canHurtSelf
	 * 2. this.canHurtOther && other does not have HurtAndDamage
	 */
	bool VerifyHurtOther(Transform other){
		if(!this.canHurtOther){
			return false;
		}
		HurtAndDamage hd = other.GetComponent<HurtAndDamage> ();
		if(hd == null || hd.canHurtSelf == true){
			return true;
		} else{
			return false;
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if(instantKillOther && VerifyHurtOther(coll.transform)){
			HealthSystem hs = coll.transform.GetComponent<HealthSystem> ();
			if(hs){
				hs.InstantDead ();
			}
		}
		if(instantKillSelf && selfHealthSystem && VerifyHurtSelf(coll.transform)){
			selfHealthSystem.InstantDead ();
		}


		// get the type of the colliding object
		ObjectIdentity oi = coll.transform.GetComponent<ObjectIdentity> ();
		if(oi == null){
			return;
		}

		// if the colliding object has an identity and type...
		ObjectType otherType = oi.objType;
		float damage;
		if(!instantKillOther && 
			DamageOtherDict.TryGetValue(otherType, out damage) &&
			VerifyHurtOther(coll.transform) )
		{
			// do damage to the other type
			HealthSystem hs = coll.transform.GetComponent<HealthSystem> ();
			if(hs){
				hs.Damage (damage);
			}
		}

		if(!instantKillSelf &&
			HurtSelfDict.TryGetValue(otherType, out damage) &&
			VerifyHurtSelf(coll.transform) ) 
		{
			// do damage to this object itself
//			Debug.Log ("Damage");
			if(selfHealthSystem){
				selfHealthSystem.Damage (damage);
			}
		}

	}


}


// serializable class for visualing the table
[System.Serializable]
public class TypeDamagePair {
	public ObjectType type;
	public float damage;
}