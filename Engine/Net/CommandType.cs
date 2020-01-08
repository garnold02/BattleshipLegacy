using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipClient.Engine.Net
{
    enum CommandType
    {
        JoinRequest,
        JoinRequestAccepted,
        JoinRequestDenied,
        LandRequest,
        LandRequestAccepted,
        LandRequestDenied,
        ShipRequest,
        ShipRequestAccepted,
        ShipRequestDenied,
        AttackRequest,
        AttackRequestAccepted,
        AttackRequestDenied,
        AdvanceTurn,
        PlayerList,
        LandBroadcast,
        AttackBroadcast,
        ShipSunk,
        CutsceneFinished,
        Score,
        OutOfGame,
        EndOfGame,
        Disconnect
    }
}
