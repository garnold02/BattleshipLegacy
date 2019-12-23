using OpenTK.Input;

namespace BattleshipClient.Engine
{
    class Input
    {
        private static KeyboardState currentKeyboardState;
        private static KeyboardState previousKeyboardState;
        private static MouseState currentMouseState;
        private static MouseState previousMouseState;

        public static void Initialize()
        {
            currentKeyboardState = Keyboard.GetState();
            previousKeyboardState = Keyboard.GetState();

            currentMouseState = Mouse.GetState();
            previousMouseState = Mouse.GetState();
        }
        public static void Begin()
        {
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
        }
        public static void End()
        {
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;
        }

        public static bool IsKeyDown(Key key)
        {
            return currentKeyboardState[key] && Root.GameContainer.IsFocused;
        }
        public static bool IsKeyPressed(Key key)
        {
            return currentKeyboardState[key] && (currentKeyboardState[key] != previousKeyboardState[key]) && Root.GameContainer.IsFocused;
        }
        public static bool IsMouseButtonDown(MouseButton button)
        {
            return currentMouseState[button] && Root.GameContainer.IsFocused;
        }
        public static bool IsMouseButtonPressed(MouseButton button)
        {
            return currentMouseState[button] && (currentMouseState[button] != previousMouseState[button]) && Root.GameContainer.IsFocused;
        }
    }
}
