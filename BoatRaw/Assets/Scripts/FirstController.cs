using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSpace;

public class FirstController : MonoBehaviour, SceneController, UserAction {

	Vector3 water_pos = new Vector3(0,0.5F,0);
	public UserGUI userGUI;

	public CoastController fromCoast;
	public CoastController toCoast;
	public BoatController boat;
	public GenGameObject[] characters;
    int NumDevil = 3;
    int NumPriest = 3;
	void Awake() {
		Director director = Director.getInstance ();
		director.currentSceneController = this;
		userGUI = gameObject.AddComponent <UserGUI>() as UserGUI;
		characters = new GenGameObject[NumPriest+NumDevil];
		loadResources ();
	}

	public void loadResources() {
		GameObject water = Instantiate (Resources.Load ("Perfabs/Water", typeof(GameObject)), water_pos, Quaternion.identity, null) as GameObject;
		water.name = "water";

		fromCoast = new CoastController ("from",NumDevil+NumPriest);
		toCoast = new CoastController ("to", NumDevil + NumPriest);
		boat = new BoatController ();

        for (int i = 0; i < NumPriest; i++)
        {
            GenGameObject cha = new GenGameObject("priest");
            cha.setName("priest" + i);
            cha.setPosition(fromCoast.getEmptyPosition());
            cha.getOnCoast(fromCoast);
            fromCoast.getOnCoast(cha);

            characters[i] = cha;
        }

        for (int i = 0; i < NumDevil; i++)
        {
            GenGameObject cha = new GenGameObject("devil");
            cha.setName("devil" + i);
            cha.setPosition(fromCoast.getEmptyPosition());
            cha.getOnCoast(fromCoast);
            fromCoast.getOnCoast(cha);

            characters[i + NumPriest] = cha;
        }
    }

	public void LoadWithNum(int pri,int dev)
    {
        restart();
        if(pri!=NumPriest||dev!=NumDevil)
        {
            Destroy(fromCoast.coast);
            Destroy(toCoast.coast);
            fromCoast = new CoastController("from", pri + dev);
            toCoast = new CoastController("to", pri + dev);

            for (int i = 0; i < NumPriest+NumDevil; i++)
                Destroy(characters[i].character);

            for (int i = 0; i < pri; i++)
            {
                GenGameObject cha = new GenGameObject("priest");
                cha.setName("priest" + i);
                cha.setPosition(fromCoast.getEmptyPosition());
                cha.getOnCoast(fromCoast);
                fromCoast.getOnCoast(cha);

                characters[i] = cha;
            }

            for (int i = 0; i < dev; i++)
            {
                GenGameObject cha = new GenGameObject("devil");
                cha.setName("devil" + i);
                cha.setPosition(fromCoast.getEmptyPosition());
                cha.getOnCoast(fromCoast);
                fromCoast.getOnCoast(cha);

                characters[i + pri] = cha;
            }
            NumPriest = pri;
            NumDevil = dev;
        }
	}


	public void moveBoat() {
		if (boat.isEmpty ()||GameStatus()!=0)
			return;
		boat.Move ();
		userGUI.status = GameStatus ();
	}

	public void characterIsClicked(GenGameObject characterCtrl) {
        if (GameStatus() == 0)
        {
            if (characterCtrl.isOnBoat())
            {
                CoastController whichCoast;
                if (boat.get_to_or_from() == -1)
                { // to->-1; from->1
                    whichCoast = toCoast;
                }
                else
                {
                    whichCoast = fromCoast;
                }

                boat.GetOffBoat(characterCtrl.getName());
                characterCtrl.moveToPosition(whichCoast.getEmptyPosition());
                characterCtrl.getOnCoast(whichCoast);
                whichCoast.getOnCoast(characterCtrl);

            }
            else
            {                                   
                CoastController whichCoast = characterCtrl.getCoastController();

                if (boat.getEmptyIndex() == -1) 
                    return;

                if (whichCoast.get_to_or_from() != boat.get_to_or_from())
                    return;

                whichCoast.getOffCoast(characterCtrl.getName());
                characterCtrl.moveToPosition(boat.getEmptyPosition());
                characterCtrl.getOnBoat(boat);
                boat.GetOnBoat(characterCtrl);
            }
            userGUI.status = GameStatus();
        }
	}

	int GameStatus()
    {	
        // 0->not finish, -1->lose, 1->win
		int from_priest = 0;
		int from_devil = 0;
		int to_priest = 0;
		int to_devil = 0;

		int[] fromCount = fromCoast.getCharacterNum ();
		from_priest += fromCount[0];
		from_devil += fromCount[1];

		int[] toCount = toCoast.getCharacterNum ();
		to_priest += toCount[0];
		to_devil += toCount[1];

		if (to_priest + to_devil == NumDevil+NumPriest)		// win
			return 1;

		int[] boatCount = boat.getCharacterNum ();
		if (boat.get_to_or_from () == -1) {	// boat at toCoast
			to_priest += boatCount[0];
			to_devil += boatCount[1];
		} else {	// boat at fromCoast
			from_priest += boatCount[0];
			from_devil += boatCount[1];
		}
		if (from_priest < from_devil && from_priest > 0) {		// lose
			return -1;
		}
		if (to_priest < to_devil && to_priest > 0) {
			return -1;
		}
		return 0;			
        // not finish
	}

	public void restart()
    {
		boat.reset ();
		fromCoast.reset ();
		toCoast.reset ();
		for (int i = 0; i < characters.Length; i++) {
			characters [i].reset ();
		}
	}
}
