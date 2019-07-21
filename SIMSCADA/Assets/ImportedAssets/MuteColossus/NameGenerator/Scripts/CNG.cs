using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuteColossus;

public class CNG : MonoBehaviour {
	/// <summary> 
  /// 	Singletone instance of this class
  /// </summary> 	
	public static CNG instance = null;

	/// <summary> 
  /// 	NameGenerator class
  /// </summary> 	
	public NameGenerator gen = new NameGenerator();

	/// <summary> 
  /// 	Tells the instance whether or not to destroy itself on load
  /// </summary> 	
	public bool dontDestroy = false;

	void Awake() {
		if (instance == null)
		{
			instance = this;
		} else if (instance != this)
		{
			Destroy(gameObject);
		}

		if (this.dontDestroy) {
			DontDestroyOnLoad(gameObject);
		}
	}	
}
