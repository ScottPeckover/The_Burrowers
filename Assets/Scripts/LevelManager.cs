using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
	/// <summary>
	/// 
	/// This script is used to load a specific scene
	/// 
	/// </summary>


	//Loads the specified scene
	public void LoadNextLevel(int x) {
		SceneManager.LoadScene (x);
	}

	//Reloads the current level
    public static void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
