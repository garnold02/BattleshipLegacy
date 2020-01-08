﻿using BattleshipClient.Engine;
using BattleshipClient.Engine.Net;
using BattleshipClient.Game.Structure;
using OpenTK.Graphics;
using OpenTK.Input;
using System;

namespace BattleshipClient.Game.RegularObjects
{
    class TurnManager : RegularObject
    {
        public TurnPhase Phase { get; private set; } = TurnPhase.Neutral;
        public DateTime PhaseDeadline { get; private set; } = DateTime.Now;
        public TurnManager(GameContainer container) : base(container)
        {

        }
        public override void Update(float delta)
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

            SetCooldownText();
        }
        public void Advance(int timestamp)
        {
            PhaseDeadline = new DateTime(DateTime.Now.Year, 1, 1) + TimeSpan.FromSeconds(timestamp);

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

        private void LandClaimingLogic()
        {
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {

                Container.NetCom.SendPacket(new Packet(CommandType.LandRequest, (int)Container.CursorCtrl.ClaimPosition.X, (int)Container.CursorCtrl.ClaimPosition.Y));
            }
        }
        private void ShipPlacementLogic()
        {
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                Packet packet = new Packet(CommandType.ShipRequest, (int)Container.CursorCtrl.Position.X, (int)Container.CursorCtrl.Position.Y, Container.CursorCtrl.ShipLength, Container.CursorCtrl.IsShipVertical.ToString().ToLower());
                Container.NetCom.SendPacket(packet);
            }
            if (Input.IsKeyPressed(Key.Space))
            {
                Container.CursorCtrl.IsShipVertical = !Container.CursorCtrl.IsShipVertical;
            }
        }
        private void StrategyLogic()
        {
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                Packet packet = new Packet(CommandType.AttackRequest, (int)Container.CursorCtrl.Position.X, (int)Container.CursorCtrl.Position.Y);
                Container.NetCom.SendPacket(packet);
            }
        }
        private void CinematicsLogic()
        {
            if (Input.IsKeyPressed(Key.Space))
            {
                Container.NetCom.SendPacket(new Packet(CommandType.CutsceneFinished));
                Container.Board.Attacks.Clear();
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
        }
        private void OnStrategyEntered()
        {
            Container.CameraCtrl.TargetZoom = 10;
        }
        private void OnCinematicsEntered()
        {
            Container.CameraCtrl.TargetZoom = 12;
            foreach (Player player in Container.Board.Players)
            {
                player.ClearAttackIndicators();
            }
            Container.Board.CreateMissiles();
        }
        private void SetCooldownText()
        {
            if (PhaseDeadline > DateTime.Now)
            {
                TimeSpan timeSpan = (PhaseDeadline - DateTime.Now);
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
