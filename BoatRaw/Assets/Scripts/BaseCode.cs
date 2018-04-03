using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSpace;

namespace GameSpace
{
	
	public class Director : System.Object
    {
		private static Director _instance;
		public SceneController currentSceneController { get; set; }

		public static Director getInstance()
        {
			if (_instance == null)
            {
				_instance = new Director ();
			}
			return _instance;
		}
	}

	public interface SceneController
    {
		void loadResources ();
	}

	public interface UserAction
    {
		void moveBoat();
		void characterIsClicked(GenGameObject characterCtrl);
		void restart();
        void LoadWithNum(int pri, int dev);

    }

	
}