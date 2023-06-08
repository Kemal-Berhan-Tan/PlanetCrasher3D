using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Core;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Models;
using Lean.Touch;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameFolders.Scripts.Managers.MidLevelManagers
{
    public class InputManager : BaseManager
    {
        #region Private Variables

        [SerializeField] private LeanMultiUpdate leanMultiUpdate;

        [SerializeField] private LeanFingerUpdate leanFingerUpdate;

        private List<float> deltaHistory = new List<float>();

        private bool isfingerUp, isFingerTap;

        private GameModel _gameModel;

        #endregion

        #region Begining

        protected override void Start()
        {
            base.Start();
            leanMultiUpdate.OnDelta.AddListener(LeanOnDelta);
            leanFingerUpdate.OnFinger.AddListener(LeanOnFinger);
        }

        #endregion

        #region Implemented Methods

        public override void Receive(BaseEventArgs baseEventArgs)
        {
            switch (baseEventArgs)
            {
            }
        }

        #endregion

        #region Public Methods

        public void InjectModel(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        #endregion

        #region Private Methods

        private void LeanOnDelta(Vector2 delta)
        {
            if (isfingerUp)
                return;

            deltaHistory.Add(delta.x);
            bool isSwerve = false;
            if (deltaHistory.Count > 5)
            {
                float tempf = deltaHistory[0];
                foreach (var d in deltaHistory)
                {
                    if (d != tempf)
                        isSwerve = true;
                    break;
                }

                deltaHistory.Clear();
            }
            else isSwerve = true;


            Broadcast(new LeanOnDeltaEventArgs(delta, isSwerve));
        }

        private void LeanOnFinger(LeanFinger finger)
        {
            BaseEventArgs args = new LeanOnFingerEventArgs(finger);
            Broadcast(args);
            // BroadcastUpward(args);


            if (finger.Up)
            {
                isfingerUp = true;
                deltaHistory.Clear();
            }


            if (finger.Down)
                isfingerUp = false;
        }

        #endregion

        #region Incoming Events

        #endregion

        #region Incoming Receive Events

        #endregion
    }
}