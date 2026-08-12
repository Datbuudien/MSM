using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

namespace VoxelArsenal
{

public class VoxelSceneSelect : MonoBehaviour
{
	public bool GUIHide = false;
	public bool GUIHide2 = false;
	public bool GUIHide3 = false;
	
    public void LoadSceneDemoMissiles()
    {
        SceneManager.LoadScene("VoxelMissileDemo");
    }
	
	public void LoadSceneDemoBeams()
    {
        SceneManager.LoadScene("VoxelBeamDemo");
    }
	
    public void LoadSceneDemo01()
    {
        SceneManager.LoadScene("VoxelDemo01");
    }
    public void LoadSceneDemo02()
    {
        SceneManager.LoadScene("VoxelDemo02");
    }
    public void LoadSceneDemo03()
    {
        SceneManager.LoadScene("VoxelDemo03");
    }
    public void LoadSceneDemo04()
    {
        SceneManager.LoadScene("VoxelDemo04");
    }
    public void LoadSceneDemo05()
    {
        SceneManager.LoadScene("VoxelDemo05");
    }
    public void LoadSceneDemo06()
    {
        SceneManager.LoadScene("VoxelDemo06");
    }
    public void LoadSceneDemo07()
    {
        SceneManager.LoadScene("VoxelDemo07");
    }
    public void LoadSceneDemo08()
    {
        SceneManager.LoadScene("VoxelDemo08");
    }
    public void LoadSceneDemo09()
    {
        SceneManager.LoadScene("VoxelDemo09");
    }
    public void LoadSceneDemo10()
    {
        SceneManager.LoadScene("VoxelDemo10");
    }
	public void LoadSceneDemo11()
    {
        SceneManager.LoadScene("VoxelDemo11");
    }
	public void LoadSceneDemo12()
    {
        SceneManager.LoadScene("VoxelDemo12");
    }
	public void LoadSceneDemo13()
    {
        SceneManager.LoadScene("VoxelDemo13");
    }
	public void LoadSceneDemo14()
    {
        SceneManager.LoadScene("VoxelDemo14");
    }
	public void LoadSceneDemo15()
    {
        SceneManager.LoadScene("VoxelDemo15");
    }
	public void LoadSceneDemo16()
    {
        SceneManager.LoadScene("VoxelDemo16");
    }
	public void LoadSceneDemo17()
    {
        SceneManager.LoadScene("VoxelDemo17");
    }
	public void LoadSceneDemo18()
    {
        SceneManager.LoadScene("VoxelDemo18");
    }
	public void LoadSceneDemo19()
    {
        SceneManager.LoadScene("VoxelDemo19");
    }
	public void LoadSceneDemo20()
    {
        SceneManager.LoadScene("VoxelDemo20");
    }
	public void LoadSceneDemo21()
    {
        SceneManager.LoadScene("VoxelDemo21");
    }
	public void LoadSceneDemo22()
    {
        SceneManager.LoadScene("VoxelDemo22");
    }
	public void LoadSceneDemo23()
    {
        SceneManager.LoadScene("VoxelDemo23");
    }
	
	 void Update ()
	 {
 
     if(Input.GetKeyDown(KeyCode.L))
	 {
         GUIHide = !GUIHide;
     
         if (GUIHide)
		 {
             GameObject.Find("CanvasSceneSelect").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("CanvasSceneSelect").GetComponent<Canvas> ().enabled = true;
         }
     }
	      if(Input.GetKeyDown(KeyCode.J))
	 {
         GUIHide2 = !GUIHide2;
     
         if (GUIHide2)
		 {
             GameObject.Find("CanvasMissiles").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("CanvasMissiles").GetComponent<Canvas> ().enabled = true;
         }
     }
		if(Input.GetKeyDown(KeyCode.H))
	 {
         GUIHide3 = !GUIHide3;
     
         if (GUIHide3)
		 {
             GameObject.Find("CanvasTips").GetComponent<Canvas> ().enabled = false;
         }
		 else
		 {
             GameObject.Find("CanvasTips").GetComponent<Canvas> ().enabled = true;
         }
     }
	}
}



}