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
                    //TODO
                    break;
                case PacketType.LandRequestAccepted:
                    break;
                case PacketType.LandRequestDenied:
                    break;
                case PacketType.ShipRequestAccepted:
                    {
                        int x = (chunks[0] as ByteChunk).Data;
                        int y = (chunks[1] as ByteChunk).Data;
                        int length = (chunks[2] as ByteChunk).Data;
                        bool isVertical = (chunks[3] as BoolChunk).Data;
                        Ship ship = new Ship(Container.Board.LocalPlayer, x, y, length, isVertical);
                        Container.Board.LocalPlayer.AddShip(ship);
                        //Container.CursorCtrl.ShipLength = (chunks[4] as IntChunk).Data;
                        Container.CursorCtrl.ShipLength = 1;
                    }
                    break;
                case PacketType.ShipRequestDenied:
                    break;
                case PacketType.AttackRequestAccepted:
                    {
                        int x = (chunks[0] as ByteChunk).Data;
                        int y = (chunks[1] as ByteChunk).Data;
                        Attack attack = new Attack(null, x, y, false);
                        Container.Board.LocalPlayer.AddAttackIndicator(attack);
                    }
                    break;
                case PacketType.AttackRequestDenied:
                    break;
                case PacketType.AdvanceTurn:
                    {
                        int timestamp = (chunks[0] as IntChunk).Data;
                        Container.TurnManager.Advance(timestamp);
                        //Console.WriteLine("ellbé: {0}", lbcount);
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
                        List<Player> playerList = new List<Player>();

                        int state = 0;  //0 = fetch xyh; 1 = id chain length; 2 = fetch ids; 3 = create attack
                        byte x = 0, y = 0, chainLength = 0;
                        bool hit = false;
                        for (int i = 0; i < chunks.Length; i++)
                        {
                            switch (state)
                            {
                                case 0:
                                    {
                                        x = (chunks[i] as ByteChunk).Data;
                                        y = (chunks[i + 1] as ByteChunk).Data;
                                        hit = (chunks[i + 2] as BoolChunk).Data;
                                        i += 2;
                                        state = 1;
                                    }
                                    continue;
                                case 1:
                                    {
                                        chainLength = (chunks[i] as ByteChunk).Data;
                                        state = 2;
                                    }
                                    continue;
                                case 2:
                                    {
                                        if (playerList.Count < chainLength)
                                        {
                                            byte id = (chunks[i] as ByteChunk).Data;
                                            Player player = Container.Board.GetPlayerByID(id);
                                            playerList.Add(player);
                                            if (playerList.Count == chainLength)
                                            {
                                                Attack attack = new Attack(playerList, x, y, hit);
                                                Container.Board.Attacks.Add(attack);
                                                playerList = new List<Player>();
                                                state = 0;
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
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
                case PacketType.OutOfGame:
                    {
                        Container.TurnManager.EnterNeutral();
                    }
                    break;
                case PacketType.EndOfGame:
                    break;
                case PacketType.Disconnect:
                    break;

                case PacketType.JoinRequest:
                    //Irrelevant
                    break;
                case PacketType.ShipRequest:
                    //Irrelevant
                    break;
                case PacketType.LandRequest:
                    //Irrelevant
                    break;
                case PacketType.AttackRequest:
                    //Irrelevant
                    break;
                case PacketType.CutsceneFinished:
                    //Irrelevant
                    break;
            }
        }
        private void Log(string message, params object[] parameters)
        {
            Console.WriteLine("[CMDE] {0}", string.Format(message, parameters));
        }
    }
}
