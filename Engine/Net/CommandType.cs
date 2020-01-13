﻿namespace BattleshipClient.Engine.Net
{
    enum PacketType
    {
        JoinRequest,
        JoinRequestAccepted,
        JoinRequestDenied,
        Prices,
        LandRequest,
        LandRequestAccepted,
        LandRequestDenied,
        InitiateShipPlacement,
        ShipRequest,
        ShipRequestAccepted,
        ShipRequestDenied,
        PurchaseRequest,
        PurchaseRequestAccepted,
        PurchaseRequestDenied,
        AttackRequest,
        AttackRequestAccepted,
        AttackRequestDenied,
        AdvanceTurn,
        PlayerList,
        LandBroadcast,
        AttackBroadcast,
        RepairBroadcast,
        ShipSunk,
        CutsceneFinished,
        Score,
        Oil,
        OutOfGame,
        EndOfGame,
        Disconnect
    }
}
