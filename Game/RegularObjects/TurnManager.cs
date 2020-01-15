using BattleshipClient.Engine;
using BattleshipClient.Engine.Net;
using BattleshipClient.Game.GameObjects;
using BattleshipClient.Game.Structure;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleshipClient.Game.RegularObjects
{
    class TurnManager : RegularObject
    {
        public TurnPhase Phase { get; private set; } = TurnPhase.Neutral;
        public DateTime PhaseDeadline { get; private set; } = DateTime.UtcNow;
        public bool CanPlaceShips { get; private set; } = false;
        public bool IsMenuEnabled { get; private set; } = false;

        public List<Missile> activeMissiles;
        private bool sentCSF = false;

        public TurnManager(GameContainer container) : base(container)
        {
            activeMissiles = new List<Missile>();
        }
        public override void Update(float delta)
        {
            switch (Phase)
            {
                case TurnPhase.Neutral:
                    NeutralLogic();
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

            SetCooldownText();
        }
        public void Advance(int timestamp)
        {
            PhaseDeadline = new DateTime(DateTime.UtcNow.Year, 1, 1) + TimeSpan.FromSeconds(timestamp);
            Console.WriteLine(PhaseDeadline);

            switch (Phase)
            {
                case TurnPhase.Neutral:
                    Phase = TurnPhase.LandClaiming;
                    OnLandClaimingEntered();
                    break;
                case TurnPhase.LandClaiming:
                    Phase = TurnPhase.ShipPlacement;
                    OnShipPlacementEntered();
                    break;
                case TurnPhase.ShipPlacement:
                    Phase = TurnPhase.Strategy;
                    OnStrategyEntered();
                    break;
                case TurnPhase.Strategy:
                    Phase = TurnPhase.Cinematics;
                    OnCinematicsEntered();
                    break;
                case TurnPhase.Cinematics:
                    Phase = TurnPhase.Strategy;
                    OnStrategyEntered();
                    break;
            }
        }
        public void EnterNeutral()
        {
            Phase = TurnPhase.Neutral;
            OnNeutralEntered();
        }
        public void EnableShipPlacement(byte initialLength)
        {
            Container.CursorCtrl.ShipLength = initialLength;
            CanPlaceShips = true;
        }
        public void DisableShipPlacement()
        {
            Container.CursorCtrl.ShipLength = 0;
            CanPlaceShips = false;
        }
        public void BuyStategicOption(ActionType option)
        {
            Packet packet = new Packet(PacketType.PurchaseRequest, new ByteChunk((byte)option));
            Container.NetCom.SendPacket(packet);
        }
        public void Ready()
        {
            Packet packet = new Packet(PacketType.ReadyRequest);
            Container.NetCom.SendPacket(packet);
        }
        public void ToggleMenu()
        {
            switch (IsMenuEnabled)
            {
                case true:
                    {
                        Task.Run(BeginClosingMenu);
                    }
                    break;
                case false:
                    {
                        Task.Run(BeginOpeningMenu);
                    }
                    break;
            }
        }
        private async Task BeginOpeningMenu()
        {
            IsMenuEnabled = true;
            Container.UI.MenuInteractionEnabled = false;
            Container.UI.UpperButton.Position = new Vector2(0, 1);
            Container.UI.RightButton.Position = new Vector2(1, 0);
            Container.UI.LowerButton.Position = new Vector2(0, -1);
            Container.UI.LeftButton.Position = new Vector2(-1, 0);
            await Task.Delay(100);
            Container.UI.MenuInteractionEnabled = true;
        }
        private async Task BeginClosingMenu()
        {
            Container.UI.MenuInteractionEnabled = false;
            Container.UI.desiredUpperPosition = new Vector2(0, 1);
            Container.UI.desiredRightPosition = new Vector2(1, 0);
            Container.UI.desiredLowerPosition = new Vector2(0, -1);
            Container.UI.desiredLeftPosition = new Vector2(-1, 0);
            await Task.Delay(100);
            IsMenuEnabled = false;
        }
        private void NeutralLogic()
        {
            if (Input.IsKeyPressed(Key.E))
            {
                ToggleMenu();
            }
        }
        private void LandClaimingLogic()
        {
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                Container.NetCom.SendPacket(new Packet(PacketType.LandRequest, new ByteChunk((byte)Container.CursorCtrl.ClaimPosition.X), new ByteChunk((byte)Container.CursorCtrl.ClaimPosition.Y)));
            }
        }
        private void ShipPlacementLogic()
        {
            if (CanPlaceShips)
            {
                if (Input.IsMouseButtonPressed(MouseButton.Left))
                {
                    Packet packet = new Packet(PacketType.ShipRequest, new ByteChunk((byte)Container.CursorCtrl.Position.X), new ByteChunk((byte)Container.CursorCtrl.Position.Y), new ByteChunk((byte)Container.CursorCtrl.ShipLength), new BoolChunk(Container.CursorCtrl.IsShipVertical));
                    Container.NetCom.SendPacket(packet);
                }
                if (Input.IsKeyPressed(Key.Space))
                {
                    Container.CursorCtrl.IsShipVertical = !Container.CursorCtrl.IsShipVertical;
                }
            }
        }
        private void StrategyLogic()
        {
            if (Input.IsMouseButtonPressed(MouseButton.Left) && !Container.TurnManager.IsMenuEnabled)
            {
                if (Container.Board.LocalPlayer.Actions.Count > 0)
                {
                    ActionType action = Container.Board.LocalPlayer.Actions.Peek();
                    Packet packet = new Packet(PacketType.ActionRequest, new ByteChunk((byte)Container.CursorCtrl.Position.X), new ByteChunk((byte)Container.CursorCtrl.Position.Y), new ByteChunk((byte)action));
                    Container.NetCom.SendPacket(packet);
                }
            }
            if (Input.IsKeyPressed(Key.E))
            {
                ToggleMenu();
            }
        }
        private void CinematicsLogic()
        {
            if (activeMissiles.Count == 0 && !sentCSF)
            {
                Container.NetCom.SendPacket(new Packet(PacketType.CutsceneFinished));
                Container.Board.Actions.Clear();

                sentCSF = true;
            }
        }

        private void OnNeutralEntered()
        {
            Container.CameraCtrl.TargetZoom = 32;
        }
        private void OnLandClaimingEntered()
        {
            Container.CameraCtrl.TargetZoom = 32;
        }
        private void OnShipPlacementEntered()
        {
            Container.CameraCtrl.TargetZoom = 8;
            Container.Board.Renderer.SetClaimMapTexture(Container.Board.CreateClaimBitmap(false));
        }
        private void OnStrategyEntered()
        {
            Container.CameraCtrl.TargetZoom = 10;
            Container.Board.LocalPlayer.Actions.Clear();
        }
        private void OnCinematicsEntered()
        {
            sentCSF = false;
            Container.CameraCtrl.TargetZoom = 18;
            foreach (Player player in Container.Board.Players)
            {
                player.ClearAttackIndicators();
            }
            Container.Board.CreateMissiles();

            IsMenuEnabled = false;
        }
        private void SetCooldownText()
        {
            if (PhaseDeadline > DateTime.UtcNow)
            {
                TimeSpan timeSpan = (PhaseDeadline - DateTime.UtcNow);
                Container.UI.CooldownText.Text = timeSpan.ToString("mm\\:ss");
                Container.UI.CooldownText.Color = timeSpan.TotalSeconds > 10 ? Color4.White : Color4.Red;
            }
            else
            {
                Container.UI.CooldownText.Text = "00:00";
                Container.UI.CooldownText.Color = Color4.Red;
            }
        }
    }
}
