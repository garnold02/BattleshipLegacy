using BattleshipClient.Engine.Net;
using BattleshipClient.Game.Structure;
using OpenTK;
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
                        ActionType action = (ActionType)(chunks[0] as ByteChunk).Data;
                        Container.Board.LocalPlayer.Actions.Enqueue(action);
                    }
                    break;
                case PacketType.PurchaseRequestDenied:
                    {

                    }
                    break;
                case PacketType.ActionRequestAccepted:
                    {
                        byte x = (chunks[0] as ByteChunk).Data;
                        byte y = (chunks[1] as ByteChunk).Data;
                        ActionType actionType = (ActionType)(chunks[2] as ByteChunk).Data;

                        StrategyAction action = new StrategyAction(x, y, new bool[3, 3], null, actionType);
                        Container.Board.LocalPlayer.AddActionIndicator(action);
                        Container.Board.LocalPlayer.Actions.Dequeue();
                    }
                    break;
                case PacketType.ActionRequestDenied:
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
                        Container.Board.Renderer.SetClaimMapTexture(Container.Board.CreateClaimBitmap(true));
                    }
                    break;
                case PacketType.ActionBroadcast:
                    {
                        byte x = (chunks[0] as ByteChunk).Data;
                        byte y = (chunks[1] as ByteChunk).Data;
                        byte hitm1 = (chunks[2] as ByteChunk).Data;
                        byte hitm2 = (chunks[3] as ByteChunk).Data;
                        ActionType actionType = (ActionType)(chunks[4] as ByteChunk).Data;
                        byte id = (chunks[5] as ByteChunk).Data;

                        bool[,] matrix = new bool[3, 3]
                        {
                            {
                                ((hitm1>>0)&1)==1,
                                ((hitm1>>1)&1)==1,
                                ((hitm1>>2)&1)==1
                            },
                            {
                                ((hitm1>>3)&1)==1,
                                ((hitm1>>4)&1)==1,
                                ((hitm1>>5)&1)==1
                            },
                            {
                                ((hitm1>>6)&1)==1,
                                ((hitm1>>7)&1)==1,
                                ((hitm2>>0)&1)==1,
                            }
                        };

                        Player player = Container.Board.GetPlayerByID(id);

                        StrategyAction action = new StrategyAction(x, y, matrix, player, actionType);
                        Container.Board.Actions.Add(action);
                    }
                    break;
                case PacketType.ShipSunk:
                    {
                        byte x = (chunks[0] as ByteChunk).Data;
                        byte y = (chunks[1] as ByteChunk).Data;
                        byte length = (chunks[2] as ByteChunk).Data;
                        bool isVertical = (chunks[3] as BoolChunk).Data;

                        BoardPiece piece = Container.Board.Pieces[x / Container.Board.PieceLength, y / Container.Board.PieceLength];
                        if (piece.Owner == Container.Board.LocalPlayer)
                        {
                            int rx = x % Container.Board.PieceLength;
                            int ry = y % Container.Board.PieceLength;
                            Ship ship = piece.Cells[rx, ry].Ship;
                            piece.Owner.RemoveShip(ship);
                            Container.Board.Renderer.AddSunkShip(ship);
                        }
                        else
                        {
                            Container.Board.Renderer.AddSunkShip(new Ship(piece.Owner, x, y, length, isVertical));
                        }
                    }
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
