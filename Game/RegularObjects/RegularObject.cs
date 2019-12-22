using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipClient.Game.RegularObjects
{
    abstract class RegularObject
    {
        public GameContainer Container { get; }
        public RegularObject(GameContainer container)
        {
            Container = container;
        }
        public abstract void Update(float delta);
    }
}
