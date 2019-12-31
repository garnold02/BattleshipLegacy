using BattleshipClient.Game.Structure;
using System;
using System.Collections.Generic;

namespace BattleshipClient.Game.RegularObjects
{
    class CommandExecutor : RegularObject
    {
        public CommandExecutor(GameContainer container) : base(container)
        {
        }
        public override void Update(float delta)
        {
            HandleServerCommands();
        }
        private void HandleServerCommands()
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
            Log("Received: [{0} {1}]", command, string.Join(" ", parameters));
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
                    Container.Board.FillPlayerList(parameters);
                    break;
                case "LB":
                    {
                        Player player = Container.Board.GetPlayerByID(byte.Parse(parameters[0]));
                        int x = int.Parse(parameters[1]);
                        int y = int.Parse(parameters[2]);
                        player.BoardClaim = Container.Board.Pieces[x, y];

                        if (player == Container.Board.LocalPlayer)
                        {
                            Container.Board.Renderer.SetClaimPosition(x, y);
                        }
                    }
                    break;
                case "ADV":
                    Container.TurnManager.Advance(int.Parse(parameters[0]));
                    break;
                case "SRQA":
                    {
                        Ship ship = new Ship(Container.Board.LocalPlayer, int.Parse(parameters[0]), int.Parse(parameters[1]), int.Parse(parameters[2]), bool.Parse(parameters[3]));
                        Container.Board.LocalPlayer.AddShip(ship);

                        Container.CursorCtrl.ShipLength++;
                    }
                    break;
                case "ARQA":
                    {
                        Attack attack = new Attack(null, int.Parse(parameters[0]), int.Parse(parameters[1]), false);
                        Container.Board.LocalPlayer.AddAttackIndicator(attack);
                    }
                    break;
                case "AB":
                    {
                        foreach (string chunk in parameters)
                        {
                            int x = chunk[0] - 33;
                            int y = chunk[1] - 33;
                            bool hit = (chunk[2] - 33) == 1;
                            List<Player> owners = new List<Player>();
                            for (int i = 3; i < chunk.Length; i++)
                            {
                                Player player = Container.Board.GetPlayerByID((byte)(chunk[i] - 33));
                                owners.Add(player);
                            }
                            Attack attack = new Attack(owners, x, y, hit);
                            Container.Board.Attacks.Add(attack);
                        }
                    }
                    break;
                case "SHS":
                    {

                    }
                    break;
                case "OUT":
                    Container.TurnManager.EnterNeutral();
                    break;
            }
        }
        private void Log(string message, params object[] parameters)
        {
            Console.WriteLine("[CMDE] {0}", string.Format(message, parameters));
        }
    }
}
