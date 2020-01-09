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
using BattleshipClient.Engine.UI;
using OpenTK.Graphics;

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
        public ImageFont DefaultFont { get; private set; }
        public NetCommunicator NetCom { get; private set; }
        public CommandExecutor CommandExe { get; private set; }
        public ObjectManager ObjManager { get; private set; }
        public TurnManager TurnManager { get; private set; }
        public CursorController CursorCtrl { get; private set; }
        public CameraController CameraCtrl { get; private set; }
        public ParticlePool ParticlePl { get; private set; }
        public UIManager UIManager { get; private set; }
        public UI UI { get; private set; }
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
            window = new GameWindow(800, 800, new GraphicsMode(new ColorFormat(32), 8, 0, 4))
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
            GL.Enable(EnableCap.Multisample);
            GL.CullFace(CullFaceMode.Back);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        }
        private void InitializeGeneral(string playerName, string hostname, int port)
        {

            Assets.LoadAll();
            Input.Initialize();

            DefaultFont = new ImageFont("font", 109, 119);
            NetCom = new NetCommunicator(hostname, port);
            CommandExe = new CommandExecutor(this);
            ObjManager = new ObjectManager(this);
            TurnManager = new TurnManager(this);
            CursorCtrl = new CursorController(this);
            CameraCtrl = new CameraController(this);
            Board = new Board(this, 6, 10, playerName);
            ParticlePl = new ParticlePool(this);
            UIManager = new UIManager(this);
            UI = new UI(this);

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
        }
        #endregion
        #region FrameEvents
        private void Update(object sender, FrameEventArgs e)
        {
            float delta = (float)e.Time;

            Input.Begin();
            UIManager.Update(delta);
            UI.Update(delta);
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
            UIManager.Render();
            window.SwapBuffers();
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
        private void OnResized(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, window.Width, window.Height);
            UIManager.Arrange();
        }
        #endregion
        #region Other
        public async void InitiateHandshake(string playerName)
        {
            Log("Initiating handshake...");
            Task clientConnectedWaitTask = NetCom.IsConnectedTCS.Task;
            if (await Task.WhenAny(clientConnectedWaitTask, Task.Delay(8000)) == clientConnectedWaitTask)
            {
                NetCom.SendPacket(new Packet(PacketType.JoinRequest, new StringChunk(playerName)));
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
