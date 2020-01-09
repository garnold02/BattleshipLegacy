namespace BattleshipClient.Engine.Net
{
    enum PacketType
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
