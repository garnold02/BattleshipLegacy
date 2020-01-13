using BattleshipClient.Engine.Net;
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
            HandlePackets();
        }
        private void HandlePackets()
        {
            Queue<Packet> packets = Container.NetCom.FetchPackets();

            while (packets.Count > 0)
            {
                Packet packet = packets.Dequeue();
                HandlePacket(packet);
            }
        }
        private void HandlePacket(Packet packet)
        {
            Chunk[] chunks = packet.GetChunks();

            Log("Received: {0}", Enum.GetName(typeof(PacketType), packet.Type));
            switch (packet.Type)
            {
                case PacketType.JoinRequestAccepted:
                    {
                        Container.IsInGame = true;
                    }
                    break;
                case PacketType.JoinRequestDenied:
                    {
                        //TODO
                    }
                    break;
                case PacketType.Prices:
                    {
                        //TODO
                    }
                    break;
                case PacketType.LandRequestAccepted:
                    {

                    }
                    break;
                case PacketType.LandRequestDenied:
                    {

                    }
                    break;
                case PacketType.InitiateShipPlacement:
                    {
                        byte initialLength = (chunks[0] as ByteChunk).Data;
                        Container.TurnManager.EnableShipPlacement(initialLength);
                    }
                    break;
                case PacketType.ShipRequestAccepted:
                    {
                        int x = (chunks[0] as ByteChunk).Data;
                        int y = (chunks[1] as ByteChunk).Data;
                        int length = (chunks[2] as ByteChunk).Data;
                        bool isVertical = (chunks[3] as BoolChunk).Data;
                        Ship ship = new Ship(Container.Board.LocalPlayer, x, y, length, isVertical);
                        Container.Board.LocalPlayer.AddShip(ship);
                        Container.CursorCtrl.ShipLength = (chunks[4] as ByteChunk).Data;
                    }
                    break;
                case PacketType.ShipRequestDenied:
                    {

                    }
                    break;
                case PacketType.PurchaseRequestAccepted:
                    {

                    }
                    break;
                case PacketType.PurchaseRequestDenied:
                    {

                    }
                    break;
                case PacketType.AttackRequestAccepted:
                    {
                        int x = (chunks[0] as ByteChunk).Data;
                        int y = (chunks[1] as ByteChunk).Data;
                        Attack attack = new Attack(null, null, x, y, false);
                        Container.Board.LocalPlayer.AddAttackIndicator(attack);
                    }
                    break;
                case PacketType.AttackRequestDenied:
                    break;
                case PacketType.AdvanceTurn:
                    {
                        int timestamp = (chunks[0] as IntChunk).Data;
                        Container.TurnManager.Advance(timestamp);
                    }
                    break;
                case PacketType.PlayerList:
                    {
                        Container.Board.FillPlayerList(chunks);
                    }
                    break;
                case PacketType.LandBroadcast:
                    {
                        Player player = Container.Board.GetPlayerByID((chunks[0] as ByteChunk).Data);
                        byte x = (chunks[1] as ByteChunk).Data;
                        byte y = (chunks[2] as ByteChunk).Data;
                        player.BoardClaim = Container.Board.Pieces[x, y];
                        player.BoardClaim.Owner = player;

                        if (player == Container.Board.LocalPlayer)
                        {
                            Container.Board.Renderer.SetClaimPosition(x, y);
                        }
                    }
                    break;
                case PacketType.AttackBroadcast:
                    {
                        List<Player> players = new List<Player>();
                        List<StrategyOption> playerAttackTypes = new List<StrategyOption>();

                        byte x = (chunks[0] as ByteChunk).Data;
                        byte y = (chunks[1] as ByteChunk).Data;
                        bool hit = (chunks[2] as BoolChunk).Data;
                        byte playerCount = (chunks[3] as ByteChunk).Data;
                        for (int i = 0; i < playerCount; i++)
                        {
                            int pos = 4 + i * 2;
                            byte id = (chunks[pos + 0] as ByteChunk).Data;
                            StrategyOption type = (StrategyOption)(chunks[pos + 1] as ByteChunk).Data;

                            players.Add(Container.Board.GetPlayerByID(id));
                            playerAttackTypes.Add(type);
                        }
                        Attack attack = new Attack(players, playerAttackTypes, x, y, hit);
                        Container.Board.Attacks.Add(attack);
                    }
                    break;
                case PacketType.RepairBroadcast:
                    {

                    }
                    break;
                case PacketType.ShipSunk:
                    break;
                case PacketType.Score:
                    {
                        for (int i = 0; i < chunks.Length / 2; i++)
                        {
                            int pos = i * 2;
                            byte id = (chunks[pos] as ByteChunk).Data;
                            int score = (chunks[pos + 1] as IntChunk).Data;

                            Player player = Container.Board.GetPlayerByID(id);
                            player.Score = score;
                            if (player == Container.Board.LocalPlayer)
                            {
                                Container.UI.ScoreText.Text = string.Format("{0} pont", player.Score);
                            }
                        }
                    }
                    break;
                case PacketType.Oil:
                    {
                        Container.Board.LocalPlayer.Oil = (chunks[0] as IntChunk).Data;
                        Container.UI.OilText.Text = string.Format("{0} liter olaj", Container.Board.LocalPlayer.Oil);
                    }
                    break;
                case PacketType.OutOfGame:
                    {
                        Container.TurnManager.EnterNeutral();
                    }
                    break;
                case PacketType.EndOfGame:
                    break;
                case PacketType.Disconnect:
                    break;
            }
        }
        private void Log(string message, params object[] parameters)
        {
            Console.WriteLine("[CMDE] {0}", string.Format(message, parameters));
        }
    }
}
