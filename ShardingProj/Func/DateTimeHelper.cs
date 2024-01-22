namespace ShardingProj.Func;

public static class DateTimeHelper
{
    public static long ConvertDateTimeToLong(DateTime dt)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        TimeSpan toNow = dt.Subtract(dtStart);
        long timeStamp = toNow.Ticks;
        timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 4));
        return timeStamp;

    }

    public static DateTime ConvertLongToDateTime(long d)
    { 
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(d + "0000");
        TimeSpan toNow = new TimeSpan(lTime);
        DateTime dtResult = dtStart.Add(toNow);
        return dtResult;

    }
}