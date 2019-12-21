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
            switch (command)
            {
                default:
                    Log("Can't interpret command \"{0}\"", command);
                    break;
                case "JRQA":
                    Container.IsInGame = true;
                    break;
                case "JRQD":
                    //TODO: Join denied
                    break;
                case "PLY":
                    Container.GameBoard.FillPlayerList(parameters);
                    break;
                case "LB":
                    Container.GameBoard.GetPlayer(parameters[0]).BoardClaim = Container.GameBoard.Pieces[int.Parse(parameters[1]), int.Parse(parameters[2])];
                    break;
                case "ADV":
                    //TODO: Advance turn
                    break;
                case "SRQA":
                    //TODO: Ship request accepted
                    break;
                case "ARQA":
                    //TODO: Attack request accepted
                    break;
                case "AB":
                    //TODO: Attack broadcast
                    break;
            }
        }
        private void Log(string message, params object[] parameters)
        {
            Console.WriteLine("[CMDE] {0}", string.Format(message, parameters));
        }
    }
}
