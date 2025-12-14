using System.ComponentModel.DataAnnotations;

namespace GymWebsite.Models;

public enum WeekDay {
    [Display(Name = "Pazartesi")] Monday = 0,
    [Display(Name = "Salı")] Tuesday = 1,
    [Display(Name = "Çarşamba")] Wednesday = 2,
    [Display(Name = "Perşembe")] Thursday = 3,
    [Display(Name = "Cuma")] Friday = 4,
    [Display(Name = "Cumartesi")] Saturday = 5,
    [Display(Name = "Pazar")] Sunday = 6,
}

public readonly struct WeekDayTime {
    public const int TicksPerMinute = 1;
    public const int TicksPerHour = 60 * TicksPerMinute;
    public const int TicksPerDay = 24 * TicksPerHour;
    public const int TicksPerWeek = 7 * TicksPerDay;

    public int MinuteTicks { get; } = 0;

    public WeekDay Day => (WeekDay)(MinuteTicks / TicksPerWeek);
    public int Hour => MinuteTicks / TicksPerHour % TicksPerDay;
    public int Minute => MinuteTicks / TicksPerMinute % TicksPerHour;

    public WeekDayTime(int minuteTicks) => MinuteTicks = minuteTicks;

    public WeekDayTime(WeekDay day, int hour, int minute) {
        MinuteTicks = (int)day * TicksPerDay;
        MinuteTicks += hour * TicksPerHour;
        MinuteTicks += minute * TicksPerMinute;
    }
}
