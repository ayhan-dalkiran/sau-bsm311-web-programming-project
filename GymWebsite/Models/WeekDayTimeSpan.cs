namespace GymWebsite.Models;

public readonly struct WeekDayTimeSpan(WeekDayTime startWeekDayTime, WeekDayTime endWeekDayTime) {
    public WeekDayTime StartWeekDayTime { get; } = startWeekDayTime;
    public WeekDayTime EndWeekDayTime { get; } = endWeekDayTime;
}
