using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BAD45
{

    // To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
    //
    //    using QuickType;
    //
    //    var data = Calendar.FromJson(jsonString);



    public partial class Calendar
    {
        [JsonProperty("weeks")]
        public Week[] Weeks { get; set; }

        [JsonProperty("halfway")]
        public string[] HalfWay { get; set; }

        [JsonProperty("workouts")]
        public Workout[] Workouts { get; set; }

        public Workout FindWorkout(string name)
        {
            foreach(Workout w in Workouts)
            {
                if (String.CompareOrdinal(w.Name, name)==0)
                {
                    return w;
                }
            }
            return null;
        }

        public bool HalfwayExercise(string name)
        {
            foreach (string w in HalfWay)
            {
                if (String.CompareOrdinal(w, name) == 0)
                {
                    return true;
                }
            }
            return false;
        }

    }

    public partial class Week
    {
        [JsonProperty("days")]
        public string[] Days { get; set; }
    }

    public partial class Workout
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cooldown")]
        public long CooldownDuration { get; set; }

        [JsonProperty("duration")]
        public long[] ExerciseDuration { get; set; }

        [JsonProperty("rest")]
        public bool[] HasRest { get; set; }

        [JsonProperty("set_mode")]
        public string SetMode { get; set; }

        [JsonProperty("num_sets")]
        public long NumSets { get; set; }

        [JsonProperty("exercises")]
        public string[] Exercises { get; set; }
    }

    public partial class Calendar
    {
        public static Calendar FromJson(string json) => JsonConvert.DeserializeObject<Calendar>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Calendar self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}