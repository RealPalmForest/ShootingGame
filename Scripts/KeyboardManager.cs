using Microsoft.Xna.Framework.Input;


namespace ShootingGame.Scripts
{
    public class KeyboardManager
    {
        KeyboardState currentKeyState;
        KeyboardState previousKeyState;

        public KeyboardManager() { }

        public KeyboardState UpdateState()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
            return currentKeyState;
        }

        public bool IsHeld(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        public bool HasBeenPressed(Keys key)
        {
            return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        }
    }
}