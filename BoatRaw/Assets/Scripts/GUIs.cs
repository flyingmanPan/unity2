using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSpace;

public class UserGUI : MonoBehaviour
{
    private UserAction action;
	public int status = 0;
	GUIStyle style;
	GUIStyle buttonStyle;
    int Priest = 3;
    int Devil = 3;
	void Start()
    {
		action = Director.getInstance ().currentSceneController as UserAction;

        style = new GUIStyle
        {
            fontSize = 40,
            alignment = TextAnchor.MiddleCenter
        };

        buttonStyle = new GUIStyle("button")
        {
            fontSize = 30
        };
    }
	void OnGUI()
    {
		if (status == -1)
            GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-85, 100, 50), "Game Over", style);
        else if(status == 1)
            GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-85, 100, 50), "You Win", style);

        if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", buttonStyle))
        {
            status = 0;
            action.restart();
        }
        Priest = (int)GUI.HorizontalSlider(new Rect(25, 25, 100, 30), Priest, 1.0F, 6.0F);
        Devil = (int)GUI.HorizontalSlider(new Rect(25, 50, 100, 30), Devil, 1.0F, 6.0F);
        if(GUI.Button(new Rect(25,70,30,50),"Change"))
        {
            if (Priest < Devil)
                Debug.Log("Illegal");
            else if (Priest + Devil > 12)
            {
                Debug.Log("Too much");
            }
            else
            {
                action.LoadWithNum(Priest, Devil);
            }
        }

    }
}

public class ClickGUI : MonoBehaviour
{
    UserAction action;
    GenGameObject characterController;

    public void setController(GenGameObject characterCtrl)
    {
        characterController = characterCtrl;
    }

    void Start()
    {
        action = Director.getInstance().currentSceneController as UserAction;
    }

    void OnMouseDown()
    {
        if (gameObject.name == "boat")
        {
            action.moveBoat();
        }
        else
        {
            action.characterIsClicked(characterController);
        }
    }
}
