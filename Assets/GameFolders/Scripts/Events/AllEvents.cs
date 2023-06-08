using System;
using Framework.Core;
using FluffyUnderware.Curvy;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Objects;
using Lean.Touch;
using UnityEngine;

namespace GameFolders.Scripts.Events
{
    #region Common Events

    public class CameraTransisitonEventArgs : BaseEventArgs
    {
        public CameraTransitionTypesEnum CameraType { get; private set; }
        public float SmoothDuration { get; set; }
        public bool IsSmooth { get; set; }
        public Vector3 Pos { get; set; }
        public Quaternion Rot { get; set; }

        public CameraTransisitonEventArgs(CameraTransitionTypesEnum cameraType, bool isSmooth = true,
            float smoothDuration = -1)
        {
            CameraType = cameraType;
            SmoothDuration = smoothDuration;
            IsSmooth = isSmooth;
        }

        public CameraTransisitonEventArgs(CameraTransitionTypesEnum cameraType, Vector3 pos = default,
            Quaternion rot = default)
        {
            CameraType = cameraType;
            Pos = pos;
            Rot = rot;
        }
    }

    public class CameraShakeEventArgs : BaseEventArgs
    {
    }

    public class OnLevelComplatedEventArgs : BaseEventArgs
    {
        public bool IsSuccessed { get; set; }

        public OnLevelComplatedEventArgs(bool IsSuccessed)
        {
            this.IsSuccessed = IsSuccessed;
        }
    }

    #endregion

    #region High Level Events

    #region Game Manager Events

    public class ResetTheManagersEventArgs : BaseEventArgs
    {
    }

    #endregion

    #endregion

    #region Mid Level Events

    #region Level Manger Events

    public class OnLevelCreatedEventArgs : BaseEventArgs
    {
        public CurvySpline Spline { get; set; }

        public OnLevelCreatedEventArgs(CurvySpline spline)
        {
            Spline = spline;
        }
    }

    #endregion

    #region Input Manager Events

    public class LeanOnFingerEventArgs : BaseEventArgs
    {
        public LeanFinger Finger { get; private set; }

        public LeanOnFingerEventArgs(LeanFinger finger)
        {
            Finger = finger;
        }
    }

    public class LeanOnDeltaEventArgs : BaseEventArgs
    {
        public Vector2 Delta { get; private set; }
        public bool IsSwerve { get; private set; }

        public LeanOnDeltaEventArgs(Vector2 delta, bool isSwerve)
        {
            Delta = delta;
            IsSwerve = isSwerve;
        }
    }

    #endregion

    #region Player Manager Events

    public class OnPlayerCreatedEventArgs : BaseEventArgs
    {
        public int Id { get; set; }

        public OnPlayerCreatedEventArgs(int ıd)
        {
            Id = ıd;
        }
    }

    public class OnDamageTakenPlayerEventArgs : BaseEventArgs
    {
        public int Damage { get; private set; }

        public OnDamageTakenPlayerEventArgs(int damage)
        {
            Damage = damage;
        }
    }

    public class GetCharacterBodyPartsTotalPowerScoreEventArgs : BaseEventArgs
    {
        public Action<int> Callback { get; private set; }

        public GetCharacterBodyPartsTotalPowerScoreEventArgs(Action<int> callback)
        {
            Callback = callback;
        }
    }

    public class GetCharacterBodyPartItemLevelAverageEventArgs : BaseEventArgs
    {
        public Action<int> Callback { get; private set; }
        public CharacterBodyPartTypes BodyPartType { get; set; }

        public GetCharacterBodyPartItemLevelAverageEventArgs(Action<int> callback,
            CharacterBodyPartTypes type = CharacterBodyPartTypes.None)
        {
            Callback = callback;
            BodyPartType = type;
        }
    }

    public class OnPlayerCollectedFuelEventArgs : BaseEventArgs
    {
        public int FuelAmount { get; private set; }
        public bool IsBoostActivated { get; set; }

        public OnPlayerCollectedFuelEventArgs(int fuelAmount)
        {
            FuelAmount = fuelAmount;
        }

        public OnPlayerCollectedFuelEventArgs(bool isBoostActivated = false)
        {
            IsBoostActivated = isBoostActivated;
        }
    }

    public class OnPlayerCollidedPlanetEventArgs : BaseEventArgs
    {
    }

    #endregion

    #region Character Upgrade Region Manager Events

    public class OnCharacterBodyPartUpdatedEventArgs : BaseEventArgs
    {
        public CharacterBodyPartDirectionTypes DirectionType { get; set; }
        public int BodyPartLevel { get; set; }
        public int BodyPartLevelsAverage { get; set; }
        public int BodyPartTotalPowerScore { get; set; }

        public OnCharacterBodyPartUpdatedEventArgs(CharacterBodyPartDirectionTypes directionType, int bodyPartLevel,
            int bodyPartLevelsAverage, int bodyPartTotalPowerScore)
        {
            DirectionType = directionType;
            BodyPartLevel = bodyPartLevel;
            BodyPartLevelsAverage = bodyPartLevelsAverage;
            BodyPartTotalPowerScore = bodyPartTotalPowerScore;
        }
    }

    public class OnStartTheTutorialLevelEventArgs : BaseEventArgs
    {
    }

    #endregion

    #region Game Play Manager Events

    public class StartThePlanetFightEventArgs : BaseEventArgs
    {
    }

    public class OnPlanetFightActionEventArgs : BaseEventArgs
    {
        public InputTypes PlayerReaction { get; set; }
        public bool IsBoss { get; set; }

        public OnPlanetFightActionEventArgs(InputTypes playerReaction)
        {
            PlayerReaction = playerReaction;
        }
    }

    #endregion

    #endregion

    #region Presenter Events

    public class PlayButtonClickedEventArgs : BaseEventArgs
    {
    }

    public class StartTheGamePlayEventArgs : BaseEventArgs
    {
    }

    public class RestartLevelButtonClickedEventArgs : BaseEventArgs
    {
    }

    public class SettingsButtonClickedEventArgs : BaseEventArgs
    {
    }

    public class SoundToggleClickedEventArgs : BaseEventArgs
    {
        public bool IsOn { get; }

        public SoundToggleClickedEventArgs(bool isOn)
        {
            IsOn = isOn;
        }
    }

    public class HapticToggleClickedEventArgs : BaseEventArgs
    {
        public bool IsOn { get; }

        public HapticToggleClickedEventArgs(bool isOn)
        {
            IsOn = isOn;
        }
    }

    public class JoystickToggleClickedEventArgs : BaseEventArgs
    {
        public bool IsOn { get; }

        public JoystickToggleClickedEventArgs(bool isOn)
        {
            IsOn = isOn;
        }
    }

    public class MusicToggleClickedEventArgs : BaseEventArgs
    {
        public bool IsOn { get; }

        public MusicToggleClickedEventArgs(bool isOn)
        {
            IsOn = isOn;
        }
    }

    public class SettingsButtonCloseClickedEventArgs : BaseEventArgs
    {
    }

    public class NextLevelButtonClickedEventArgs : BaseEventArgs
    {
    }

    #endregion
}