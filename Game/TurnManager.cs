using Battleship.Engine;
using BattleshipClient.Game.GameObjects;
using OpenTK.Input;

namespace BattleshipClient.Game
{
    class TurnManager
    {
        public GameContainer Container { get; }
        public TurnPhase Phase { get; private set; } = TurnPhase.Neutral;
        public TurnManager(GameContainer gameContainer)
        {
            Container = gameContainer;
        }
        public void Update()
        {
            switch (Phase)
            {
                case TurnPhase.Neutral:
                    break;
                case TurnPhase.LandClaiming:
                    LandClaimingLogic();
                    break;
                case TurnPhase.ShipPlacement:
                    ShipPlacementLogic();
                    break;
                case TurnPhase.Strategy:
                    StrategyLogic();
                    break;
                case TurnPhase.Cinematics:
                    CinematicsLogic();
                    break;
            }
        }
        public void Advance()
        {
            switch (Phase)
            {
                case TurnPhase.Neutral:
                    Phase = TurnPhase.LandClaiming;
                    break;
                case TurnPhase.LandClaiming:
                    Phase = TurnPhase.ShipPlacement;
                    break;
                case TurnPhase.ShipPlacement:
                    Phase = TurnPhase.Strategy;
                    break;
                case TurnPhase.Strategy:
                    Phase = TurnPhase.Cinematics;
                    break;
                case TurnPhase.Cinematics:
                    Phase = TurnPhase.Strategy;
                    break;
            }
        }

        private void LandClaimingLogic()
        {
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                Container.NetCom.SendRequest("LRQ {0} {1}", (int)Container.Cursor.ClaimPosition.X, (int)Container.Cursor.ClaimPosition.Y);
            }
        }
        private void ShipPlacementLogic()
        {
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                Container.NetCom.SendRequest("SRQ {0} {1} {2} {3}", (int)Container.Cursor.Position.X, (int)Container.Cursor.Position.Y, Container.Cursor.ShipLength, Container.Cursor.IsShipVertical.ToString().ToLower());
            }
            if (Input.IsKeyPressed(Key.Space))
            {
                Container.Cursor.IsShipVertical = !Container.Cursor.IsShipVertical;
            }
        }
        private void StrategyLogic()
        {

        }
        private void CinematicsLogic()
        {

        }
    }
}
