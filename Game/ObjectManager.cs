using BattleshipClient.Game.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipClient.Game
{
    class ObjectManager
    {
        private List<GameObject> gameObjectList;
        private List<GameObject> gameObjectAddList;
        private List<GameObject> gameObjectRemoveList;
        public ObjectManager()
        {
            gameObjectList = new List<GameObject>();
            gameObjectAddList = new List<GameObject>();
            gameObjectRemoveList = new List<GameObject>();
        }
        public void Add(GameObject gameObject)
        {
            gameObjectAddList.Add(gameObject);
        }
        public void Remove(GameObject gameObject)
        {
            gameObjectRemoveList.Add(gameObject);
        }
        public void Update()
        {
            foreach (GameObject gameObject in gameObjectList)
            {
                gameObject.Update();
            }

            foreach (GameObject addedObject in gameObjectAddList)
            {
                addedObject.OnAdded();
                gameObjectList.Add(addedObject);
            }
            gameObjectAddList.Clear();
            foreach (GameObject removedObject in gameObjectRemoveList)
            {
                removedObject.OnRemoved();
                gameObjectList.Remove(removedObject);
            }
            gameObjectRemoveList.Clear();
        }
        public void Render()
        {
            foreach (GameObject gameObject in gameObjectList)
            {
                gameObject.Render();
            }
        }
    }
}
