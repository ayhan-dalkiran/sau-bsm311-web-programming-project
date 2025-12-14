using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymWebsite.Models.Staff;

public class TrainerOpenDayTimeSpan {
    [Key]
    public int Id { get; set; }

    public required Trainer Trainer { get; set; }

    [NotMapped]
    public WeekDayTimeSpan DayTimeSpan { get; set; }

    public int StartMinuteTicks {
        get => DayTimeSpan.StartWeekDayTime.MinuteTicks;
        set => DayTimeSpan = new WeekDayTimeSpan(new WeekDayTime(value), DayTimeSpan.EndWeekDayTime);
    }
    public int EndMinuteTicks {
        get => DayTimeSpan.EndWeekDayTime.MinuteTicks;
        set => DayTimeSpan = new WeekDayTimeSpan(DayTimeSpan.StartWeekDayTime, new WeekDayTime(value));
    }
}
