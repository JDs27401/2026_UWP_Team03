using System;
using UnityEngine;

namespace C__Classes.Systems
{
    public class Universe : MonoBehaviour
    {
        public static Universe Instance;
        
        private static int Day = 1;
        [Range(0, 24)] private static int Hour = 0;
        [Range(0,60)] private static float Minute = 0;
        [Range(0,24)] private static float RealTime = 0; //should be used for light movement, angle, etc.
        
        private static Phase TimeOfDay;

        [Header("Time Speed")]
        [SerializeField] private float Ratio = 1;
        [Header("Starting settings")]
        [SerializeField] private int StartingHour = 8;
        [SerializeField] private float StartingMinute = 0;
        [Header("Day Phases Thresholds")]
        [SerializeField] private int DayThreshold = 8;
        [SerializeField] private int SundownThreshold = 18;
        [SerializeField] private int NightThreshold = 21;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            Hour = StartingHour;
            Minute = StartingMinute;
        }
        
        private void Update()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            var increment = Time.deltaTime * Ratio;
            Minute += increment;

            if (Minute >= 60)
            {
                Hour += 1;
                Minute = 0;
            }

            if (Hour >= 24)
            {
                Day += 1;
                Hour = 0;
            }
            
            RealTime += increment;
            RealTime %= 24f;
            
            SetDayPhase();
            // PrintTime();
        }

        private void SetDayPhase()
        {
            if (Hour >= NightThreshold || Hour < DayThreshold) 
                TimeOfDay = Phase.Night;
            else if (Hour >= SundownThreshold) 
                TimeOfDay = Phase.Sundown;
            else if (Hour >= DayThreshold) 
                TimeOfDay = Phase.Day;
        }

        private static void PrintTime() //just a debug method
        {
            print($"Day: {Day} Hour: {Hour} Minute: {(int) Minute} Phase: {TimeOfDay} Realtime: {RealTime}");
        }

        public static int GetDay()
        {
            return Day;
        }

        public static int GetHour()
        {
            return Hour;
        }

        public static float GetMinute()
        {
            return Minute;
        }

        public static float GetRealTime()
        {
            return RealTime;
        }

        public static Phase GetTimeOfDay()
        {
            return TimeOfDay;            
        }
        
        public enum Phase
        {
            Day,
            Sundown,
            Night,
        }
    }
}