using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SecondsConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            ConvertCalendarToSeconds("BAD45 Dumbbells", "BAD45_Calendar_Dumbbell.json"  , "BAD45_DB_Timers.seconds");
            ConvertCalendarToSeconds("BAD45 Kettlebells", "BAD45_Calendar_Kettlebell.json", "BAD45_KB_Timers.seconds");
        }

        static void ConvertCalendarToSeconds(string pack_name, string input_filepath, string output_filepath)
        {
            string bad45_calendar_json = File.ReadAllText(input_filepath);
            BAD45.Calendar calendar = BAD45.Calendar.FromJson(bad45_calendar_json);

            Seconds.Template pack = new Seconds.Template();

            List<Seconds.Pack> packs = new List<Seconds.Pack>();
            Seconds.Pack p = new Seconds.Pack();
            packs.Add(p);

            p.Name = pack_name;
            p.PurpleType = "pack";

            List<Seconds.Timer> items = new List<Seconds.Timer>();

            List<int> colors = new List<int> { 0, 2, 3, 8, 7, 1, 9, 6, 5 };
            for (int week = 0; week < calendar.Weeks.Length; ++week)
            {
                BAD45.Week badweek = calendar.Weeks[week];
                for (int day = 0; day < badweek.Days.Length; day++)
                {
                    string name = badweek.Days[day];
                    BAD45.Workout workout = calendar.FindWorkout(name);

                    string wname = String.Format("{0:D2}.{1}:{2}", week + 1, GetNameOfDay(day), workout.Name);
                    Seconds.Timer timer = new Seconds.Timer(wname);
                    items.Add(timer);

                    timer.SetRest = new Seconds.Interval("Rest");
                    timer.Cooldown = new Seconds.Interval("Cooldown");
                    timer.Warmup = new Seconds.Interval("Warmup");
                    timer.IntervalRest = new Seconds.Interval("Rest");

                    timer.Music = new Seconds.Music();
                    timer.SoundScheme = 6;  // Boxing, MMA=7

                    // Put this workout in Tabata mode which means that
                    // exercise are done one by one.
                    // Normally all exercises are put behind each other.
                    List<Seconds.Interval> intervals = new List<Seconds.Interval>();
                    if (workout.SetMode == "every")
                    {
                        for (int exer = 0, c = 0; exer < workout.Exercises.Length; exer++)
                        {
                            string exercise_name = workout.Exercises[exer];
                            var interval = new Seconds.Interval(exercise_name);
                            interval.HalfwayAlert = calendar.HalfwayExercise(exercise_name);
                            interval.Duration = workout.ExerciseDuration[exer];
                            interval.Color = colors[c++];
                            intervals.Add(interval);

                            if (workout.HasRest[exer])
                            {
                                var rest = new Seconds.Interval("Rest");
                                rest.Duration = 30;
                                rest.Color = 4; // Green
                                rest.Rest = true; // Exclude from count
                                intervals.Add(rest);
                            }
                        }
                        timer.NumberOfSets = workout.NumSets;
                    }
                    else
                    {
                        for (int exer = 0, c = 0; exer < workout.Exercises.Length; exer++)
                        {
                            for (int rep = 0; rep < workout.NumSets; rep++)
                            {
                                string exercise_name = workout.Exercises[exer];
                                var interval = new Seconds.Interval(exercise_name);
                                interval.HalfwayAlert = calendar.HalfwayExercise(exercise_name);
                                interval.Duration = workout.ExerciseDuration[exer];
                                interval.Color = colors[c];
                                intervals.Add(interval);

                                if (workout.HasRest[exer])
                                {
                                    var rest = new Seconds.Interval("Rest");
                                    rest.Duration = 30;
                                    rest.Color = 4; // Green
                                    rest.Rest = true; // Exclude from count
                                    intervals.Add(rest);
                                }
                            }
                            c++;
                        }
                        timer.NumberOfSets = 1;
                    }

                    timer.Intervals = intervals.ToArray();
                }
            }

            p.Timers = items.ToArray();
            pack.Packs = packs.ToArray();

            string json = Seconds.Serialize.ToJson(pack);
            File.WriteAllText(output_filepath, json);

        }

        static string GetNameOfDay(int day)
        {
            switch (day)
            {
                case 0:
                    return "Monday";
                case 1:
                    return "Tuesday";
                case 2:
                    return "Wednesday";
                case 3:
                    return "Thursday";
                case 4:
                    return "Friday";
                case 5:
                    return "Saturday";
                case 6:
                    return "Sunday";
            }
            return "?";
        }
    }

}
