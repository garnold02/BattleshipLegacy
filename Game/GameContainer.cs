using BattleshipClient.Engine.Net;
using BattleshipClient.Engine.Rendering;
using BattleshipClient.Game.Structure;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleshipClient.Game
{
    class GameContainer
    {
        #region Properties
        public float AspectRatio => window.Width / (float)window.Height;
        public bool IsInGame { get; private set; }
        public Camera MainCamera { get; set; }
        #endregion
        #region Components
        public NetCommunicator NetCom { get; }
        public CommandExecutor CommandExe { get; }
        public ObjectManager ObjManager { get; }
        public Board GameBoard { get; }
        #endregion

        private GameWindow window;

        public GameContainer()
        {
            NetCom = new NetCommunicator("127.0.0.1", 5555);
            CommandExe = new CommandExecutor(this);
            ObjManager = new ObjectManager();
            GameBoard = new Board(6, 10);

            Task.Run(InitiateHandshake);
        }
        public void Start()
        {
            InitializeWindow();
            InitializeGL();
            InitializeGeneral();
            window.Run(30);
        }
        #region Initializators
        private void InitializeWindow()
        {
            window = new GameWindow(800, 800)
            {
                Title = "Torpedó"
            };
            window.UpdateFrame += Update;
            window.RenderFrame += Render;
            window.Resize += OnResized;
        }
        private void InitializeGL()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }
        private void InitializeGeneral()
        {
            Assets.LoadAll();

            ObjManager.Add(new GameObjects.BoardRenderer(GameBoard));
            MainCamera = new Camera(40, 0.1f, 100f);
            MainCamera.Transform.localPosition = new Vector3(-10, 20, 10);
            MainCamera.Transform.Rotate(55, 45, 0);
        }
        #endregion
        #region FrameEvents
        private void Update(object sender, FrameEventArgs e)
        {
            ObjManager.Update();
            CommandExe.HandleServerCommands();
        }
        private void Render(object sender, FrameEventArgs e)
        {
            ObjManager.Render();
            window.SwapBuffers();
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
        private void OnResized(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, window.Width, window.Height);
        }
        #endregion
        #region Other
        private async void InitiateHandshake()
        {
            Task clientConnectedWaitTask = NetCom.IsConnectedTCS.Task;
            if (await Task.WhenAny(clientConnectedWaitTask, Task.Delay(8000)) == clientConnectedWaitTask)
            {
                NetCom.SendRequest("JRQ {0}", "player");
            }
        }
        #endregion
    }
}
