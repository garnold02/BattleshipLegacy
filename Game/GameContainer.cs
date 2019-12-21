using Battleship.Engine;
using BattleshipClient.Engine.Net;
using BattleshipClient.Engine.Rendering;

using BattleshipClient.Game.Structure;
using BattleshipClient.Game.GameObjects;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Threading.Tasks;
using System.Drawing;
using swf = System.Windows.Forms;

namespace BattleshipClient.Game
{
    class GameContainer
    {
        #region Properties
        public int Width => window.Width;
        public int Height => window.Height;
        public float AspectRatio => Width / (float)Height;
        public Vector2 MousePosition
        {
            get
            {
                Point curPos = window.PointToClient(swf.Cursor.Position);
                return new Vector2(curPos.X, curPos.Y);
            }
        }
        public bool IsInGame { get; set; }
        public Camera MainCamera { get; set; }
        #endregion
        #region Components
        public NetCommunicator NetCom { get; private set; }
        public CommandExecutor CommandExe { get; private set; }
        public ObjectManager ObjManager { get; private set; }
        public TurnManager TurnManager { get; private set; }
        public Cursor Cursor { get; private set; }
        public Board Board { get; private set; }
        #endregion
        private GameWindow window;

        public void Start(string playerName)
        {
            InitializeWindow();
            InitializeGL();
            InitializeGeneral(playerName);
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
        private void InitializeGeneral(string playerName)
        {

            Assets.LoadAll();
            Input.Initialize();

            NetCom = new NetCommunicator("127.0.0.1", 5555);
            CommandExe = new CommandExecutor(this);
            ObjManager = new ObjectManager();
            TurnManager = new TurnManager(this);
            Cursor = new Cursor(this);
            Board = new Board(this, 6, 10,playerName);
            AddGameObjects();
            Task.Run(() => InitiateHandshake(playerName));

            MainCamera = new Camera(40, 0.1f, 100f);
            MainCamera.Transform.localPosition = new Vector3(-10, 20, 10);
            MainCamera.Transform.Rotate(55, 45, 0);
        }
        private void AddGameObjects()
        {
            CursorRenderer cursorRenderer = new CursorRenderer(this);
            BoardRenderer boardRenderer = new BoardRenderer(this);
            ObjManager.Add(cursorRenderer);
            ObjManager.Add(boardRenderer);
        }
        #endregion
        #region FrameEvents
        private void Update(object sender, FrameEventArgs e)
        {
            Input.Begin();
            TurnManager.Update();
            Cursor.Update();
            ObjManager.Update();
            CommandExe.HandleServerCommands();
            Input.End();
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
        public async void InitiateHandshake(string playerName)
        {
            Log("Initiating handshake...");
            Task clientConnectedWaitTask = NetCom.IsConnectedTCS.Task;
            if (await Task.WhenAny(clientConnectedWaitTask, Task.Delay(8000)) == clientConnectedWaitTask)
            {
                NetCom.SendRequest("JRQ {0}", playerName);
                Log("Handshake successful.");
            }
            else
            {
                Log("Handshake timed out.");
            }
        }
        private void Log(string message, params object[] parameters)
        {
            Console.WriteLine("[GCNT] {0}", string.Format(message, parameters));
        }
        #endregion
    }
}
