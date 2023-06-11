using NureCistBot.Classes;

namespace NureCistBot.CistServices;

public class EventComposer
{
    public static Teacher? FindTeacherById(Teacher[] teachers, int id)
    {
        foreach (Teacher teacher in teachers)
        {
            if (teacher.id == id)
            {
                return teacher;
            }
        }

        return null; // якщо клас з таким ідентифікатором не знайдено
    }

    public static Subject? FindSubjectById(Subject[] subjects, int id)
    {
        foreach (Subject subject in subjects)
        {
            if (subject.id == id)
            {
                return subject;
            }
        }

        return null; // якщо клас з таким ідентифікатором не знайдено
    }

    public static string GetEventType(int id)
    {
        if (id == 10 || id == 12)
        {
            return "Пз";
        }
        else if (id == 20 | id == 21 || id == 22 || id == 23 || id == 24)
        {
            return "Лб";
        }
        else if (id == 30)
        {
            return "Конс";
        }
        else if (id == 40 || id == 41)
        {
            return "Зал";
        }
        else if (id == 50 || id == 51 || id == 52 || id == 53 || id == 54 || id == 55)
        {
            return "Екз";
        }
        else if (id == 60)
        {
            return "КП/КР";
        }

        return "Лк";
    }

    public static CistEvent[]? GetEvents(Schedule? schedule)
    {
        List<CistEvent> events = new List<CistEvent>();

        if (schedule is not null)
        {
            if (schedule.events is not null)
            {
                foreach (var item in schedule.events)
                {
                    var timeAndDateStart = DateTimeOffset.FromUnixTimeSeconds((long)item.start_time);
                    var timeAndDateEnd = DateTimeOffset.FromUnixTimeSeconds((long)item.end_time);
                    var cistEvent = new CistEvent()
                    {
                        number_pair = (int)item.number_pair,
                        subject = FindSubjectById(schedule.subjects, (int)item.subject_id),
                        date = DateOnly.FromDateTime(timeAndDateStart.LocalDateTime),
                        start_time = TimeOnly.FromDateTime(timeAndDateStart.LocalDateTime),
                        end_time = TimeOnly.FromDateTime(timeAndDateEnd.LocalDateTime),
                        type = GetEventType((int)item.type),
                        teachers = new List<Teacher>()
                    };

                    if (cistEvent.subject.id == 8051836)
                    {
                        cistEvent.teachers.Add(new Teacher()
                        {
                            full_name = "Не зазначено",
                            short_name = "Не зазначено",
                            id = 12345
                        });
                    }
                    else
                    {
                        foreach (var teacher in item.teachers)
                        {
                            var teach = FindTeacherById(schedule.teachers, teacher);

                            cistEvent.teachers.Add(teach);
                        }
                    }

                    events.Add(cistEvent);
                }
                return events.ToArray();
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}