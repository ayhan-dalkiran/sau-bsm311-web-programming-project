namespace GymWebsite.Models;

public readonly struct WeekDayTime {
    public const int TicksPerMinute = 1;
    public const int TicksPerHour = 60 * TicksPerMinute;
    public const int TicksPerDay = 24 * TicksPerHour;
    public const int TicksPerWeek = 7 * TicksPerDay;

    public int MinuteTicks { get; } = 0;

    public DayOfWeek Day => (DayOfWeek)(MinuteTicks / TicksPerWeek);
    public int Hour => MinuteTicks / TicksPerHour % TicksPerDay;
    public int Minute => MinuteTicks / TicksPerMinute % TicksPerHour;

    public WeekDayTime(DayOfWeek day, int hour, int minute) {
        MinuteTicks = (int)day * TicksPerDay;
        MinuteTicks += hour * TicksPerHour;
        MinuteTicks += minute * TicksPerMinute;
    }
}
