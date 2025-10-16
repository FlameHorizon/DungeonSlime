using System;

using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Input;

public class InputManager {
  /// <summary>
  /// Gets the state information of keyboard input.
  /// </summary>
  public KeyboardInfo Keyboard { get; private set; }

  /// <summary>
  /// Gets the state information of automated keyboard input.
  /// Used only when automation is enabled.
  /// </summary>
  public AutomatedKeyboardInfo AutomatedKeyboard { get; private set; }

  /// <summary>
  /// Gets the state information of mouse input.
  /// </summary>
  public MouseInfo Mouse { get; private set; }

  /// <summary>
  /// Gets the state information of a gamepad.
  /// </summary>
  public GamePadInfo[] GamePads { get; private set; }

  /// <summary>
  /// Inidicates if automated keyboard is enabled or not.
  /// </summary>
  public bool AutomationEnabled = false;

  /// <summary>
  /// Creates a new InputManager.
  /// </summary>
  public InputManager() {
    Keyboard = new KeyboardInfo();

    // Provide automationed version fo the keyboard.
    // Useful for testing purposes.
    AutomatedKeyboard = new AutomatedKeyboardInfo();
    Mouse = new MouseInfo();

    GamePads = new GamePadInfo[4];
    for (int i = 0; i < 4; i++) {
      // @@QUEST: Why are we using PlayerIndex instead of just plain int?
      GamePads[i] = new GamePadInfo((PlayerIndex)i);
    }
  }

  /// <summary>
  /// Updates the state information for the keyboard, mouse, and gamepad inputs.
  /// </summary>
  /// <param name="gameTime">A snapshot of the timing values for the current frame.</param>
  public void Update(GameTime gameTime) {
    // @@QUEST: Do we really need to update all devices?
    // Cost here is non-zero to update all devices. Why not detect if one is 
    // connect and update only it?

    // Here we decide if game should be updated using automationed keyboard or not.
    if (AutomationEnabled) {
      AutomatedKeyboard.Update(gameTime);
    }
    else {
      Keyboard.Update();
    }
    Mouse.Update();

    for (int i = 0; i < 4; i++) {
      GamePads[i].Update(gameTime);
    }
  }
}