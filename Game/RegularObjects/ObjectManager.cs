using BattleshipClient.Game.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipClient.Game.RegularObjects
{
    class ObjectManager : RegularObject
    {
        private readonly List<GameObject> gameObjectList;
        private readonly List<GameObject> gameObjectAddList;
        private readonly List<GameObject> gameObjectRemoveList;
        public ObjectManager(GameContainer container) : base(container)
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
        public override void Update(float delta)
        {
            foreach (GameObject gameObject in gameObjectList)
            {
                gameObject.Update(delta);
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
            gameObjectList.Sort((g1, g2) => { return g1.Depth - g2.Depth; });
            foreach (GameObject gameObject in gameObjectList)
            {
                gameObject.Render();
            }
        }
    }
}
