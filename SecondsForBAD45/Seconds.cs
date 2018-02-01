
namespace Seconds
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class Template
    {
        [JsonProperty("packs")]
        public Pack[] Packs { get; set; }
    }

    public partial class Pack
    {
        [JsonProperty("_type")]
        public string PurpleType { get; set; } = "pack";

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("items")]
        public Timer[] Timers { get; set; }
    }

    public partial class Timer
    {
        public Timer(string name)
        {
            Name = name;
        }

        [JsonProperty("setRest")]
        public Interval SetRest { get; set; }

        [JsonProperty("cooldown")]
        public Interval Cooldown { get; set; }

        [JsonProperty("hasBeenUsed")]
        public bool HasBeenUsed { get; set; }

        [JsonProperty("random")]
        public bool Random { get; set; }

        [JsonProperty("music")]
        public Music Music { get; set; } = new Music();

        [JsonProperty("warmup")]
        public Interval Warmup { get; set; }

        [JsonProperty("overrun")]
        public bool Overrun { get; set; }

        [JsonProperty("type")]
        public long FluffyType { get; set; } = 3;

        [JsonProperty("numberOfSets")]
        public long NumberOfSets { get; set; } = 4;

        [JsonProperty("intervals")]
        public Interval[] Intervals { get; set; }

        [JsonProperty("intervalRest")]
        public Interval IntervalRest { get; set; }

        [JsonProperty("group")]
        public bool Group { get; set; }

        [JsonProperty("soundScheme")]
        public long SoundScheme { get; set; } = 5;

        [JsonProperty("activity")]
        public long Activity { get; set; } = 0;

        [JsonProperty("_type")]
        public string PurpleType { get; set; } = "circ";

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Interval
    {
        public Interval(string name)
        {
            Name = name;
        }

        [JsonProperty("color")]
        public long Color { get; set; }

        [JsonProperty("split")]
        public bool Split { get; set; }

        [JsonProperty("vibration")]
        public bool Vibration { get; set; } = false;

        [JsonProperty("_type")]
        public string PurpleType { get; set; } = "int";

        [JsonProperty("halfwayAlert")]
        public bool HalfwayAlert { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; } = 0;

        [JsonProperty("music")]
        public Music Music { get; set; } = new Music();

        [JsonProperty("rest")]
        public bool Rest { get; set; } = false;

        [JsonProperty("name")]
        public string Name { get; set; } = "Rest";
    }

    public partial class Music
    {
        [JsonProperty("_type")]
        public string PurpleType { get; set; } = "music";

        [JsonProperty("shuffle")]
        public bool Shuffle { get; set; }

        [JsonProperty("volume")]
        public long Volume { get; set; } = 1;

        [JsonProperty("resume")]
        public bool Resume { get; set; }

        [JsonProperty("persist")]
        public bool Persist { get; set; }
    }

    public partial class Template
    {
        public static Template FromJson(string json) => JsonConvert.DeserializeObject<Template>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Template self) => JsonConvert.SerializeObject(self, Converter.Settings);
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
