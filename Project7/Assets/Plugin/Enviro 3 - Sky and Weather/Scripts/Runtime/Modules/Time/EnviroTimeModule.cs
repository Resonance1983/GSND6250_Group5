using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Enviro
{
    [Serializable]
    public class EnviroTime
    {
        public bool simulate;
        public DateTime date = new(1, 1, 1, 0, 0, 0);

        [SerializeField] public int secSerial, minSerial, hourSerial, daySerial, monthSerial, yearSerial;
        public float timeOfDay;

        [Range(-90, 90)] [Tooltip("-90,  90   Horizontal earth lines")]
        public float latitude;

        [Range(-180, 180)] [Tooltip("-180, 180  Vertical earth line")]
        public float longitude;

        [Range(-13, 13)] [Tooltip("Time offset for timezones")]
        public int utcOffset;

        [Tooltip("Realtime minutes for a 24h game time cycle.")]
        public float cycleLengthInMinutes = 10f;

        [Tooltip("Day length modifier will increase/decrease time progression speed at daytime.")] [Range(0.1f, 10f)]
        public float dayLengthModifier = 1f;

        [Tooltip("Night length modifier will increase/decrease time progression speed at nighttime.")]
        [Range(0.1f, 10f)]
        public float nightLengthModifier = 1f;
    }

    [Serializable]
    [ExecuteInEditMode]
    public class EnviroTimeModule : EnviroModule
    {
        public EnviroTime Settings;
        public EnviroTimeModule preset;
        public bool showTimeControls, showLocationControls;
        public float LST; // changed to make accessible outside the module
        private float internalTimeOverflow;

        ///////// Time
        public void SetDateTime(int sec, int min, int hours, int day, int month, int year)
        {
            if (year == 0)
                year = 1;
            if (month == 0)
                month = 1;
            if (day == 0)
                day = 1;

            Settings.secSerial = sec;
            Settings.minSerial = min;
            Settings.hourSerial = hours;
            Settings.daySerial = day;
            Settings.monthSerial = month;
            Settings.yearSerial = year;

            var curTime = new DateTime(1, 1, 1, 0, 0, 0);

            curTime = curTime.AddYears(Settings.yearSerial - 1);
            curTime = curTime.AddMonths(Settings.monthSerial - 1);
            curTime = curTime.AddDays(Settings.daySerial - 1);
            curTime = curTime.AddHours(Settings.hourSerial);
            curTime = curTime.AddMinutes(Settings.minSerial);
            curTime = curTime.AddSeconds(Settings.secSerial);

            //Events
            if (EnviroManager.instance != null && EnviroManager.instance.Events != null &&
                EnviroManager.instance.notFirstFrame && Application.isPlaying)
            {
                if (Settings.date.Hour != curTime.Hour)
                    EnviroManager.instance.NotifyHourPassed();

                if (Settings.date.Day != curTime.Day)
                    EnviroManager.instance.NotifyDayPassed();

                if (Settings.date.Year != curTime.Year)
                    EnviroManager.instance.NotifyYearPassed();
            }

            Settings.date = curTime;

            Settings.secSerial = Settings.date.Second;
            Settings.minSerial = Settings.date.Minute;
            Settings.hourSerial = Settings.date.Hour;
            Settings.daySerial = Settings.date.Day;
            Settings.monthSerial = Settings.date.Month;
            Settings.yearSerial = Settings.date.Year;

            Settings.timeOfDay = Settings.date.Hour + Settings.date.Minute * 0.0166667f +
                                 Settings.date.Second * 0.000277778f;
        }

        //Time
        public int seconds
        {
            get => Settings.date.Second;
            set =>
                //Settings.secSerial = value;
                SetDateTime(value, Settings.minSerial, Settings.hourSerial, Settings.daySerial, Settings.monthSerial,
                    Settings.yearSerial);
        }

        public int minutes
        {
            get => Settings.date.Minute;
            set =>
                //Settings.minSerial = value;
                SetDateTime(Settings.secSerial, value, Settings.hourSerial, Settings.daySerial, Settings.monthSerial,
                    Settings.yearSerial);
        }

        public int hours
        {
            get => Settings.date.Hour;
            set =>
                //Settings.hourSerial = value;
                SetDateTime(Settings.secSerial, Settings.minSerial, value, Settings.daySerial, Settings.monthSerial,
                    Settings.yearSerial);
        }

        public int days
        {
            get => Settings.date.Day;
            set =>
                //Settings.daySerial = value;
                SetDateTime(Settings.secSerial, Settings.minSerial, Settings.hourSerial, value, Settings.monthSerial,
                    Settings.yearSerial);
        }

        public int months
        {
            get => Settings.date.Month;
            set =>
                //Settings.monthSerial = value;
                SetDateTime(Settings.secSerial, Settings.minSerial, Settings.hourSerial, Settings.daySerial, value,
                    Settings.yearSerial);
        }

        public int years
        {
            get => Settings.date.Year;
            set =>
                //Settings.yearSerial = value;
                SetDateTime(Settings.secSerial, Settings.minSerial, Settings.hourSerial, Settings.daySerial,
                    Settings.monthSerial, value);
        }


        // Update Method 
        public override void UpdateModule()
        {
            if (!active)
                return;

            if (Settings.simulate && Application.isPlaying)
            {
                var t = 0f;

                var timeProgressionModifier = 1f;

                if (!EnviroManager.instance.isNight)
                    timeProgressionModifier = Settings.dayLengthModifier;
                else
                    timeProgressionModifier = Settings.nightLengthModifier;

                t = 24.0f / 60.0f / (Settings.cycleLengthInMinutes * timeProgressionModifier);
                t = t * 3600f * Time.deltaTime;

                if (t < 1f)
                    internalTimeOverflow += t;
                else
                    internalTimeOverflow = t;

                seconds += (int)internalTimeOverflow;

                if (internalTimeOverflow >= 1f)
                    internalTimeOverflow = 0f;
            }

            SetDateTime(Settings.secSerial, Settings.minSerial, Settings.hourSerial, Settings.daySerial,
                Settings.monthSerial, Settings.yearSerial);
            UpdateSunAndMoonPosition();
        }


        public void UpdateSunAndMoonPosition()
        {
            if (EnviroManager.instance == null)
                return;

            float d = 367 * years - 7 * (years + (months + 9) / 12) / 4 + 275 * months / 9 + days -
                      730530; // corrected a bracket typo

            d += GetUniversalTimeOfDay() / 24f; //Universal ToD

            var ecl = 23.4393f - 3.563E-7f * d;

            if (EnviroManager.instance.Sky != null)
            {
                if (EnviroManager.instance.Sky.Settings.moonMode == EnviroSky.MoonMode.Simple)
                {
                    CalculateSunPosition(d, ecl, true);
                }
                else
                {
                    CalculateSunPosition(d, ecl, false);
                    CalculateMoonPosition(d, ecl);
                }
            }
            else
            {
                CalculateSunPosition(d, ecl, false);
                CalculateMoonPosition(d, ecl);
            }

            CalculateStarsPosition(LST);
        }

        /// <summary>
        /// Get current time in hours. UTC0 (12.5 = 12:30)
        /// </summary>
        /// <returns>The the current time of day in hours.</returns>
        public float GetUniversalTimeOfDay()
        {
            return Settings.timeOfDay - Settings.utcOffset;
        }

        /// <summary>
        /// Get current time in hours with UTC time offset.
        /// </summary>
        /// <returns>The the current time of day in hours.</returns>
        public float GetTimeOfDay()
        {
            return Settings.timeOfDay;
        }

        /// <summary>
        /// Get current date in hours.
        /// </summary>
        /// <returns>The date in hour format</returns>
        public double GetDateInHours()
        {
            double dateInHours = Settings.timeOfDay + days * 24f + years * 365 * 24f;
            return dateInHours;
        }

        /// Get current time in a nicely formatted string with seconds!
        /// </summary>
        /// <returns>The time string.</returns>
        public string GetTimeStringWithSeconds()
        {
            return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }

        /// <summary>
        /// Get current time in a nicely formatted string!
        /// </summary>
        /// <returns>The time string.</returns>
        public string GetTimeString()
        {
            return string.Format("{0:00}:{1:00}", hours, minutes);
        }

        /// <summary>
        /// Set the time of day in hours. (12.5 = 12:30)
        /// </summary>
        public void SetTimeOfDay(float tod)
        {
            Settings.timeOfDay = tod;
            hours = (int)tod;
            tod -= hours;
            minutes = (int)(tod * 60f);
            tod -= minutes * 0.0166667f;
            seconds = (int)(tod * 3600f);
        }

        public Vector3 OrbitalToLocal(float theta, float phi)
        {
            Vector3 pos;

            var sinTheta = Mathf.Sin(theta);
            var cosTheta = Mathf.Cos(theta);
            var sinPhi = Mathf.Sin(phi);
            var cosPhi = Mathf.Cos(phi);

            pos.z = sinTheta * cosPhi;
            pos.y = cosTheta;
            pos.x = sinTheta * sinPhi;

            return pos;
        }

        public float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public void CalculateSunPosition(float d, float ecl, bool simpleMoon)
        {
            /////http://www.stjarnhimlen.se/comp/ppcomp.html#5////
            ///////////////////////// SUN ////////////////////////
            var w = 282.9404f + 4.70935E-5f * d;
            var e = 0.016709f - 1.151E-9f * d;
            var M = 356.0470f + 0.9856002585f * d;
            // minor correction for turns
            while (M > 360.0f) M -= 360.0f;
            while (M < 0.0f) M += 360.0f;

            var E = M + e * Mathf.Rad2Deg * Mathf.Sin(Mathf.Deg2Rad * M) * (1.0f + e * Mathf.Cos(Mathf.Deg2Rad * M));

            var xv = Mathf.Cos(Mathf.Deg2Rad * E) - e;
            var yv = Mathf.Sin(Mathf.Deg2Rad * E) * Mathf.Sqrt(1 - e * e);

            var v = Mathf.Rad2Deg * Mathf.Atan2(yv, xv);
            var r = Mathf.Sqrt(xv * xv + yv * yv);

            var l = v + w;

            var xs = r * Mathf.Cos(Mathf.Deg2Rad * l);
            var ys = r * Mathf.Sin(Mathf.Deg2Rad * l);

            var xe = xs;
            var ye = ys * Mathf.Cos(Mathf.Deg2Rad * ecl);
            var ze = ys * Mathf.Sin(Mathf.Deg2Rad * ecl);

            var decl_rad = Mathf.Atan2(ze, Mathf.Sqrt(xe * xe + ye * ye));
            var decl_sin = Mathf.Sin(decl_rad);
            var decl_cos = Mathf.Cos(decl_rad);

            var Ls = M + w; // Sun's mean longitude correction

            var GMST0 = Ls + 180; // same as above
            var GMST = GMST0 + GetUniversalTimeOfDay() * 15;
            LST = GMST + Settings.longitude;
            // LST turn correction (fit to the right ascension of Zenith)
            while (LST > 360.0f) LST -= 360.0f;
            while (LST < 0.0f) LST += 360.0f;

            var HA_deg = LST - Mathf.Rad2Deg * Mathf.Atan2(ye, xe);
            var HA_rad = Mathf.Deg2Rad * HA_deg;
            var HA_sin = Mathf.Sin(HA_rad);
            var HA_cos = Mathf.Cos(HA_rad);

            var x = HA_cos * decl_cos;
            var y = HA_sin * decl_cos;
            var z = decl_sin;

            var sin_Lat = Mathf.Sin(Mathf.Deg2Rad * Settings.latitude);
            var cos_Lat = Mathf.Cos(Mathf.Deg2Rad * Settings.latitude);

            var xhor = x * sin_Lat - z * cos_Lat;
            var yhor = y;
            var zhor = x * cos_Lat + z * sin_Lat;

            var azimuth = Mathf.Atan2(yhor, xhor) + Mathf.Deg2Rad * 180;
            var altitude = Mathf.Atan2(zhor, Mathf.Sqrt(xhor * xhor + yhor * yhor));

            var sunTheta = 90 * Mathf.Deg2Rad - altitude;
            var sunPhi = azimuth;

            //Set SolarTime: 1 = mid-day (sun directly above you), 0.5 = sunset/dawn, 0 = midnight;
            EnviroManager.instance.solarTime = Mathf.Clamp01(Remap(sunTheta, -1.5f, 0f, 1.5f, 1f));

            EnviroManager.instance.Objects.sun.transform.localPosition = OrbitalToLocal(sunTheta, sunPhi);
            EnviroManager.instance.Objects.sun.transform.LookAt(EnviroManager.instance.transform);

            if (simpleMoon)
            {
                EnviroManager.instance.Objects.moon.transform.localPosition =
                    OrbitalToLocal(sunTheta - Mathf.PI, sunPhi);
                EnviroManager.instance.lunarTime = Mathf.Clamp01(Remap(sunTheta - Mathf.PI, -3.0f, 0f, 0f, 1f));
                EnviroManager.instance.Objects.moon.transform.LookAt(EnviroManager.instance.transform);
            }
        }

        public void CalculateMoonPosition(float d, float ecl)
        {
            var N = 125.1228f - 0.0529538083f * d;
            var i = 5.1454f;
            var w = 318.0634f + 0.1643573223f * d;
            var a = 60.2666f;
            var e = 0.054900f;
            var M = 115.3654f + 13.0649929509f * d;

            var rad_M = Mathf.Deg2Rad * M;
            var E = rad_M + e * Mathf.Sin(rad_M) * (1f + e * Mathf.Cos(rad_M));

            var xv = a * (Mathf.Cos(E) - e);
            var yv = a * (Mathf.Sqrt(1f - e * e) * Mathf.Sin(E));

            var v = Mathf.Rad2Deg * Mathf.Atan2(yv, xv);
            var r = Mathf.Sqrt(xv * xv + yv * yv);

            var rad_N = Mathf.Deg2Rad * N;
            var sin_N = Mathf.Sin(rad_N);
            var cos_N = Mathf.Cos(rad_N);

            var l = Mathf.Deg2Rad * (v + w);
            var sin_l = Mathf.Sin(l);
            var cos_l = Mathf.Cos(l);

            var rad_i = Mathf.Deg2Rad * i;
            var cos_i = Mathf.Cos(rad_i);

            var xh = r * (cos_N * cos_l - sin_N * sin_l * cos_i);
            var yh = r * (sin_N * cos_l + cos_N * sin_l * cos_i);
            var zh = r * (sin_l * Mathf.Sin(rad_i));

            var cos_ecl = Mathf.Cos(Mathf.Deg2Rad * ecl);
            var sin_ecl = Mathf.Sin(Mathf.Deg2Rad * ecl);

            var xe = xh;
            var ye = yh * cos_ecl - zh * sin_ecl;
            var ze = yh * sin_ecl + zh * cos_ecl;

            var ra = Mathf.Atan2(ye, xe);
            var decl = Mathf.Atan2(ze, Mathf.Sqrt(xe * xe + ye * ye));

            var HA = Mathf.Deg2Rad * LST - ra;

            var x = Mathf.Cos(HA) * Mathf.Cos(decl);
            var y = Mathf.Sin(HA) * Mathf.Cos(decl);
            var z = Mathf.Sin(decl);

            var latitude = Mathf.Deg2Rad * Settings.latitude;
            var sin_latitude = Mathf.Sin(latitude);
            var cos_latitude = Mathf.Cos(latitude);

            var xhor = x * sin_latitude - z * cos_latitude;
            var yhor = y;
            var zhor = x * cos_latitude + z * sin_latitude;

            var azimuth = Mathf.Atan2(yhor, xhor) + Mathf.Deg2Rad * 180f;
            var altitude = Mathf.Atan2(zhor, Mathf.Sqrt(xhor * xhor + yhor * yhor));

            var MoonTheta = 90f * Mathf.Deg2Rad - altitude;
            var MoonPhi = azimuth;

            EnviroManager.instance.Objects.moon.transform.localPosition = OrbitalToLocal(MoonTheta, MoonPhi);
            EnviroManager.instance.lunarTime = Mathf.Clamp01(Remap(MoonTheta, -1.5f, 0f, 1.5f, 1f));

            EnviroManager.instance.Objects.moon.transform.LookAt(EnviroManager.instance.transform.position);
        }

        public void CalculateStarsPosition(float siderealTime)
        {
            // LST was corrected in degrees in Sun position update

            // The transform behaved incorrectly regarding longitude. The 180 degrees is to orientate the stars correctly
            // with the texture center with Zenith having 0 degrees right ascension towards the vernal equinox around 21 March
            var starsRotation = Quaternion.AngleAxis(90.0f - Settings.latitude, Vector3.right) *
                                Quaternion.AngleAxis(180.0f + siderealTime, Vector3.up);
            EnviroManager.instance.Objects.stars.transform.localRotation = starsRotation;

            Shader.SetGlobalMatrix("_StarsMatrix", EnviroManager.instance.Objects.stars.transform.worldToLocalMatrix);
        }


        //Save and Load
        public void LoadModuleValues()
        {
            if (preset != null)
                Settings = JsonUtility.FromJson<EnviroTime>(JsonUtility.ToJson(preset.Settings));
            else
                Debug.Log("Please assign a saved module to load from!");
        }

        public void SaveModuleValues()
        {
#if UNITY_EDITOR
            var t = CreateInstance<EnviroTimeModule>();
            t.name = "Time Preset";
            t.Settings = JsonUtility.FromJson<EnviroTime>(JsonUtility.ToJson(Settings));

            var assetPathAndName =
                UnityEditor.AssetDatabase.GenerateUniqueAssetPath(EnviroHelper.assetPath + "/New " + t.name + ".asset");
            UnityEditor.AssetDatabase.CreateAsset(t, assetPathAndName);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        public void SaveModuleValues(EnviroTimeModule module)
        {
            module.Settings = JsonUtility.FromJson<EnviroTime>(JsonUtility.ToJson(Settings));

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(module);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}