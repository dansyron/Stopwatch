using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Constants
{
	#region Fields

    // constant variables for decelereation, number of jumps, first max speed, second max speed, and jump height
    //public const int NUM_JUMPS = 1;          //the number of times the plater can jump
    //public const float FIRST_MAX_SPEED = 7;  //initial max speed
    //public const float SECOND_MAX_SPEED = 10; //max speed after full acceleration
    //public const float JUMP_HEIGHT = 18;      //how high the player can jump

    // for creating varying pitch in repeating soundeffects
    public const float LOW_PITCH_RANGE = .95f;              //The lowest a sound effect will be randomly pitched.
    public const float HIGH_PITCH_RANGE = 1.05f;            //The highest a sound effect will be randomly pitched.

    // constant variables involved in the timer and slowdown effect
    public const float TIMER_INITIAL_AMOUNT = 10f;         // The amount of time in seconds a timer starts with in a level
    public const float TIMER_SLOWDOWN_DURATION = 3f;       // The amount of real time a slowdown effect lasts in seconds
    public const float TIME_REWARD_FOR_ENEMY_KILLED = 1.0f;  // The amount of time in seconds that is added to a clock for killing an enemy
    public const float SLOWDOWN_RELATIVE_TIMESCALE = 5f; // The multiplier value by which perceived time is slowed down during slowdown

    //The dictionary that gives the proper string for each SceneName enum
    public static readonly Dictionary<SceneName, string> SCENE_STRINGS = new Dictionary<SceneName, string>
    {
        { SceneName.Menu, "MainMenu" },
        { SceneName.Game, "Game" },
        { SceneName.End, "End" },
    };

    // Timer UI constants
    public const float STARTING_TIME = 10f;
    public const float STAMINA_AMOUNT = 100f;
    public const float STAMINA_FILL_AMOUNT = 50f;
    public const float STAMINA_USAGE_AMOUNT = 50f;
    public const float OVERFLOW_BLOCK_ADD_AMOUNT = 10f;
    public const float STAMINA_DODGE_DRAIN_AMOUNT = 200f;

    //Block Constants
    public const float PHASE_BLOCK_SAFETY_DISTANCE_X = 1f;
    public const float PHASE_BLOCK_SAFETY_DISTANCE_Y = 2f;
    // Module-related constants
    public const int MODULE_GRID_HEIGHT = 16;
    public const int MODULE_GRID_WIDTH = 16;

    // Constants that dictate number of blocks in the game scene
    public const int NUM_NORMAL_BLOCKS = 512;
    public const int NUM_STUN_BLOCKS = 128;
    public const int NUM_SPEED_BLOCKS = 128;
    public const int NUM_WALL_BLOCKS = 128;
    public const int NUM_OVERFLOW_BLOCKS = 128;
    public const int NUM_IN_PHASE_BLOCKS = 128;
    public const int NUM_OUT_PHASE_BLOCKS = 128;

    #endregion


    #region Player Constants

    public const float HOR_MAX_SPEED = 8;
    public const float HOR_BOOST_SPEED = 12;
    public const float HOR_ACCEL_TIMER = .5f;
    public const float HOR_ACCEL_SPEED = HOR_MAX_SPEED / HOR_ACCEL_TIMER;
    public const float HOR_BOOST_ACCEL_SPEED = HOR_BOOST_SPEED / HOR_ACCEL_TIMER;
    public const float INIT_JUMP_VELOCITY = 11;
    public const float VARIABLE_JUMP_TIMER = .25f;
    public const float HURT_HOR_MAX_SPEED = .5f;
    public static readonly int[] GROUND_LAYERS = new int[] { LayerMask.NameToLayer("Ground"), LayerMask.NameToLayer("Platform") };
    public static readonly int[] ENEMY_LAYERS = new int[] { LayerMask.NameToLayer("Enemy") };
    public const float WALL_SLIDE_VELOCITY = 3f;
    public const float WALL_CLIMB_VELOCITY = 15f;
    public const float DODGE_TIME = 1f;
    public const float HURT_TIME = .25f;
    public const float ATTACK_TIME = .5f;
    public const float TIME_REWARD = 0.29f;

    #endregion
}
