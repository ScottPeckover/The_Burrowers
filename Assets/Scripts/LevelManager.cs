using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	//Loads the specified scene
	public void LoadNextLevel(int x) {
		SceneManager.LoadScene (x);
	}

}
