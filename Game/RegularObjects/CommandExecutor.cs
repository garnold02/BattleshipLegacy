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
            byte[][] chunks = packet.GetChunks();

            Log("Received: {0}", Enum.GetName(typeof(CommandType), packet.Type));
            switch (packet.Type)
            {
                case CommandType.JoinRequest:
                    break;
                case CommandType.JoinRequestAccepted:
                    break;
                case CommandType.JoinRequestDenied:
                    break;
                case CommandType.LandRequest:
                    break;
                case CommandType.LandRequestAccepted:
                    break;
                case CommandType.LandRequestDenied:
                    break;
                case CommandType.ShipRequest:
                    break;
                case CommandType.ShipRequestAccepted:
                    break;
                case CommandType.ShipRequestDenied:
                    break;
                case CommandType.AttackRequest:
                    break;
                case CommandType.AttackRequestAccepted:
                    break;
                case CommandType.AttackRequestDenied:
                    break;
                case CommandType.AdvanceTurn:
                    break;
                case CommandType.PlayerList:
                    break;
                case CommandType.LandBroadcast:
                    break;
                case CommandType.AttackBroadcast:
                    break;
                case CommandType.ShipSunk:
                    break;
                case CommandType.CutsceneFinished:
                    break;
                case CommandType.Score:
                    break;
                case CommandType.OutOfGame:
                    break;
                case CommandType.EndOfGame:
                    break;
                case CommandType.Disconnect:
                    break;
            }
        }
        private void Log(string message, params object[] parameters)
        {
            Console.WriteLine("[CMDE] {0}", string.Format(message, parameters));
        }
    }
}
