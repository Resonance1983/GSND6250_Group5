using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Enviro
{
    [Serializable]
    public class EnviroEvents
    {
        [Serializable]
        public class EnviroActionEvent : UnityEngine.Events.UnityEvent
        {
        }

        public EnviroActionEvent onHourPassedActions = new();
        public EnviroActionEvent onDayPassedActions = new();
        public EnviroActionEvent onYearPassedActions = new();
        public EnviroActionEvent onWeatherChangedActions = new();
        public EnviroActionEvent onSeasonChangedActions = new();
        public EnviroActionEvent onNightActions = new();
        public EnviroActionEvent onDayActions = new();
        public EnviroActionEvent onZoneChangedActions = new();
    }
}