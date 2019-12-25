using BattleshipClient.Engine.Net;
using BattleshipClient.Game.Structure;
using BattleshipClient.Game.GameObjects;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Threading.Tasks;
using System.Drawing;
using swf = System.Windows.Forms;
using BattleshipClient.Game.RegularObjects;
using BattleshipClient.Engine;

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
        public bool IsFocused => window.Focused;
        public bool IsInGame { get; set; }
        #endregion
        #region Components
        public NetCommunicator NetCom { get; private set; }
        public CommandExecutor CommandExe { get; private set; }
        public ObjectManager ObjManager { get; private set; }
        public TurnManager TurnManager { get; private set; }
        public CursorController CursorCtrl { get; private set; }
        public CameraController CameraCtrl { get; private set; }
        public ParticlePool ParticlePl { get; private set; }
        public Board Board { get; private set; }
        #endregion
        private GameWindow window;

        public void Start(string playerName, string hostname, int port)
        {
            InitializeWindow();
            InitializeGL();
            InitializeGeneral(playerName, hostname, port);
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
        private void InitializeGeneral(string playerName, string hostname, int port)
        {

            Assets.LoadAll();
            Input.Initialize();

            NetCom = new NetCommunicator(hostname, port);
            CommandExe = new CommandExecutor(this);
            ObjManager = new ObjectManager(this);
            TurnManager = new TurnManager(this);
            CursorCtrl = new CursorController(this);
            CameraCtrl = new CameraController(this);
            Board = new Board(this, 6, 10, playerName);
            ParticlePl = new ParticlePool(this);

            AddGameObjects();
            Task.Run(() => InitiateHandshake(playerName));
        }
        private void AddGameObjects()
        {
            CursorRenderer cursorRenderer = new CursorRenderer(this);
            BoardRenderer boardRenderer = new BoardRenderer(this);
            ObjManager.Add(cursorRenderer);
            ObjManager.Add(boardRenderer);

            Board.Renderer = boardRenderer;

            ParticleSystem sys = new ParticleSystem(this)
            {
                Frequency = 5,
                ParticleProperties = new ParticleProperties()
                {
                    TextureName = "smoke",
                    ConstantForce = new Vector3(0, 1, 0),
                    ForceProbability = new Vector3(0.25f, 0, 0.25f),
                    StartColor = OpenTK.Graphics.Color4.Transparent,
                    MiddleColor = OpenTK.Graphics.Color4.White,
                    EndColor = OpenTK.Graphics.Color4.Transparent,
                    ColorBlendSeparator = 0.1f,
                    StartScale = 0.5f,
                    EndScale = 0,
                    Lifetime = 6
                }
            };
            sys.Transform.localPosition = new Vector3(0.5f, 0, 0.5f);
            ObjManager.Add(sys);
        }
        #endregion
        #region FrameEvents
        private void Update(object sender, FrameEventArgs e)
        {
            float delta = (float)e.Time;

            Input.Begin();
            TurnManager.Update(delta);
            CursorCtrl.Update(delta);
            ObjManager.Update(delta);
            CommandExe.Update(delta);
            CameraCtrl.Update(delta);
            ParticlePl.Update(delta);
            Input.End();
        }
        private void Render(object sender, FrameEventArgs e)
        {
            ObjManager.Render();
            ParticlePl.Render();
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
