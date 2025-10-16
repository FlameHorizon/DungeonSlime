using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class AutomatedKeyboardInfo {

  /// <summary>
  /// Gets the state of keyboard input during the previous update cycle.
  /// </summary>
  public KeyboardState PreviousState { get; private set; }

  /// <summary>
  /// Gets the state of keyboard input during the current input cycle.
  /// </summary>
  public KeyboardState CurrentState { get; private set; }

  private (KeyboardState State, TimeSpan Duration) _state;
  private TimeSpan _elapsedSinceLastUpdate = TimeSpan.Zero;
  private readonly Queue<(KeyboardState State, TimeSpan Duration)> _states = [];

  public AutomatedKeyboardInfo() {
    // Right now I'm going to simulate only one key press for 1s.
    EnqueueInput(new KeyboardState(Keys.D), TimeSpan.FromSeconds(1));
    EnqueueInput(new KeyboardState(Keys.S), TimeSpan.FromSeconds(1));

    // Load first state available.
    PreviousState = new KeyboardState();

    _state = _states.Dequeue();
    CurrentState = _state.State;
  }

  public void EnqueueInput(KeyboardState state, TimeSpan duration) {
    _states.Enqueue((state, duration));
  }

  public void Update(GameTime gameTime) {
    // On first update, grab first available state and keep it as CurrentState
    // until one second passes.
    _elapsedSinceLastUpdate += gameTime.ElapsedGameTime;

    if (_state.Duration >= _elapsedSinceLastUpdate) {
      // Keep current state.
      return;
    }

    if (_states.Count == 0) {
      PreviousState = CurrentState;
      CurrentState = new KeyboardState();
    }
    else {
      // Change state
      PreviousState = CurrentState;
      _state = _states.Dequeue();
      CurrentState = _state.State;

      _elapsedSinceLastUpdate = TimeSpan.Zero;
    }
  }

  /// <summary>
  /// Returns a value that indicates if the specified key is currently down.
  /// </summary>
  /// <param name="key">The key to check.</param>
  /// <returns>true if the specified key is currently down; otherwise, false.</returns>
  public bool IsKeyDown(Keys key) {
    return CurrentState.IsKeyDown(key);
  }

  /// <summary>
  /// Returns a value that indicates whether the specified key is currently up.
  /// </summary>
  /// <param name="key">The key to check.</param>
  /// <returns>true if the specified key is currently up; otherwise, false.</returns>
  public bool IsKeyUp(Keys key) {
    return CurrentState.IsKeyUp(key);
  }

  /// <summary>
  /// Returns a value that indicates if the specified key was just pressed on the current frame.
  /// </summary>
  /// <param name="key">The key to check.</param>
  /// <returns>true if the specified key was just pressed on the current frame; otherwise, false.</returns>
  public bool WasKeyJustPressed(Keys key) {
    return CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
  }

  /// <summary>
  /// Returns a value that indicates if the specified key was just released on the current frame.
  /// </summary>
  /// <param name="key">The key to check.</param>
  /// <returns>true if the specified key was just released on the current frame; otherwise, false.</returns>
  public bool WasKeyJustReleased(Keys key) {
    return CurrentState.IsKeyUp(key) && PreviousState.IsKeyDown(key);
  }
}