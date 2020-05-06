using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class Timer
    {
        public event System.Action onCancel = delegate { };
        public event System.Action onFinish = delegate { };
        public bool active { get; private set; } = false;
        private float timeLeft = 0f;
        private float originalTime = 0f;

        public Timer(float time, bool startActive = false)
        {
            active = startActive;
            this.originalTime = time;
            this.timeLeft = time;
        }

        public void Update(float timestep)
        {
            if (active)
            {
                timeLeft -= timestep;
                if (timeLeft <= 0f)
                {
                    active = false;
                    if (onFinish != null) onFinish.Invoke();
                }
            }
        }

        public float RemainingTime()
        {
            return timeLeft;
        }

        public void Reset(bool start = false)
        {
            timeLeft = originalTime;
            active = start;
        }

        public void Start()
        {
            active = true;
        }

        public void Stop()
        {
            Reset();
            if (onCancel != null) onCancel.Invoke();
        }

        public void Override(float duration)
        {
            originalTime = duration;
        }
    }
}