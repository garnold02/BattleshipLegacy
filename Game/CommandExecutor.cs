using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipClient.Game
{
    class CommandExecutor
    {
        private GameContainer Container { get; }
        public CommandExecutor(GameContainer container)
        {
            Container = container;
        }
        public void HandleServerCommands()
        {
            Queue<string> commands = Container.NetCom.FetchCommands();

            while (commands.Count > 0)
            {
                string command = commands.Dequeue();
                string[] tokens = command.Split(' ');
                string[] parameters = new string[tokens.Length - 1];
                for (int i = 0; i < tokens.Length - 1; i++)
                {
                    parameters[i] = tokens[i + 1];
                }

                HandleCommand(tokens[0], parameters);
            }
        }
        private void HandleCommand(string command, string[] parameters)
        {

        }
    }
}
