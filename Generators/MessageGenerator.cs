using NureCistBot.DateManagment;
using NureCistBot.Classes;
using NureCistBot.Services;

namespace NureCistBot.Generators;

public class MessageGenerator
{
    public static string DonateHTML = "\n \n" +
                                      "<a href=\"https://t.me/kiuki_22_botinfo\">Info</a> | " +
                                      "<a href=\"https://donatello.to/ketronix\">Donate</a> | " +
                                      "<a href=\"https://t.me/ketronix_dev\">Support</a> | " +
                                      "<a href=\"https://github.com/ketronix-dev/nure-cist-bot\">Source code</a>" +
                                      "\n";
    public static string GenerateMessageForToday(List<CistEvent>? events, DbChat chat)
    {
        if (events is not null)
        {
            string message = "";

            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                message = "Пар сьогодні нема, дозволяю відпочити";
            }
            else
            {
                message = $"Розклад на: {$"{DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year}"} \n --------------------------- \n";
                foreach (var i in events)
                {
                    message += HtmlService.GetEventHtml(i, chat.CistId.ToString());
                }
            }
            return message + DonateHTML;
        }
        else
        {
            return "Пар нема.";
        }
    }

    public static string GenerateMessageForWeek(DbChat chat, DateOnly startDate, DateOnly endDate, string Key)
    {
        var result = NureCistParser.Parse(chat.CistId, Key);

        if (result is not null)
        {
            string startWeek = startDate.ToString("dd.MM.yyyy");
            string endWeek = endDate.ToString("dd.MM.yyyy");
            var message = $"Розклад на: {$"{startWeek} - {endWeek}"} \n -------------------------- \n";

            var monday = $"\n Понеділок | {DateService.GetWeekDays(startWeek, endWeek)[0]} \n";
            var tuesday = $"\n \n Вівторок | {DateService.GetWeekDays(startWeek, endWeek)[1]} \n";
            var wednesday = $"\n \n Середа | {DateService.GetWeekDays(startWeek, endWeek)[2]} \n";
            var thurdday = $"\n \n Четвер | {DateService.GetWeekDays(startWeek, endWeek)[3]} \n";
            var friday = $"\n \n П'ятниця | {DateService.GetWeekDays(startWeek, endWeek)[4]} \n";
            var saturday = $"\n \n Субота | {DateService.GetWeekDays(startWeek, endWeek)[5]} \n";

            foreach (var i in result)
            {
                if (i.date == DateOnly.ParseExact(DateService.GetWeekDays(startWeek, endWeek)[0], "d.M.yyyy"))
                {
                    monday += HtmlService.GetEventHtml(i, chat.CistId.ToString());
                }
                else if (i.date == DateOnly.ParseExact(DateService.GetWeekDays(startWeek, endWeek)[1], "d.M.yyyy"))
                {
                    tuesday += HtmlService.GetEventHtml(i, chat.CistId.ToString());
                }
                else if (i.date == DateOnly.ParseExact(DateService.GetWeekDays(startWeek, endWeek)[2], "d.M.yyyy"))
                {
                    wednesday += HtmlService.GetEventHtml(i, chat.CistId.ToString());
                }
                else if (i.date == DateOnly.ParseExact(DateService.GetWeekDays(startWeek, endWeek)[3], "d.M.yyyy"))
                {
                    thurdday += HtmlService.GetEventHtml(i, chat.CistId.ToString());
                }
                else if (i.date == DateOnly.ParseExact(DateService.GetWeekDays(startWeek, endWeek)[4], "d.M.yyyy"))
                {
                    friday += HtmlService.GetEventHtml(i, chat.CistId.ToString());
                }
                else if (i.date == DateOnly.ParseExact(DateService.GetWeekDays(startWeek, endWeek)[5], "d.M.yyyy"))
                {
                    saturday += HtmlService.GetEventHtml(i, chat.CistId.ToString());
                }
            }

            if (monday == $"\n Понеділок | {DateService.GetWeekDays(startWeek, endWeek)[0]} \n")
            {
                monday = $"\n \n Понеділок | {DateService.GetWeekDays(startWeek, endWeek)[0]} \n В цей день пар нема.";
            }
            if (tuesday == $"\n \n Вівторок | {DateService.GetWeekDays(startWeek, endWeek)[1]} \n")
            {
                tuesday = $"\n \n Вівторок | {DateService.GetWeekDays(startWeek, endWeek)[1]} \n В цей день пар нема.";
            }
            if (wednesday == $"\n \n Середа | {DateService.GetWeekDays(startWeek, endWeek)[2]} \n")
            {
                wednesday = $"\n \n Середа | {DateService.GetWeekDays(startWeek, endWeek)[2]} \n В цей день пар нема.";
            }
            if (thurdday == $"\n \n Четвер | {DateService.GetWeekDays(startWeek, endWeek)[3]} \n")
            {
                thurdday = $"\n \n Четвер | {DateService.GetWeekDays(startWeek, endWeek)[3]} \n В цей день пар нема.";
            }
            if (friday == $"\n \n П'ятниця | {DateService.GetWeekDays(startWeek, endWeek)[4]} \n")
            {
                friday = $"\n \n П'ятниця | {DateService.GetWeekDays(startWeek, endWeek)[4]} \n В цей день пар нема.";
            }
            if (saturday == $"\n \n Субота | {DateService.GetWeekDays(startWeek, endWeek)[5]} \n")
            {
                saturday = $"\n \n Субота | {DateService.GetWeekDays(startWeek, endWeek)[5]} \n В цей день пар нема.";
            }

            message += monday + tuesday + wednesday + thurdday + friday + saturday;

            return message + DonateHTML;
        }
        else
        {
            return "Для цієї групи нема пар на CIST.";
        }
    }
}
