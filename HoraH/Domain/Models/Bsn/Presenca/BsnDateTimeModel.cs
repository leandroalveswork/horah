namespace HoraH.Domain.Models.Bsn.Presenca;
public class BsnDateTimeModel
{
    public TimeSpan OffSet { get; private set; }
    public BsnDateTimeModel()
    {
    }
    public BsnDateTimeModel(TimeZoneInfo timeZone)
    {
        OffSet = timeZone.BaseUtcOffset;
    }
    public void UsarTimeZone(TimeZoneInfo timeZone)
    {
        OffSet = timeZone.BaseUtcOffset;
    }
    private DateTime _dbDateTime;
    public DateTime Value
    {
        get
        {
            return _dbDateTime + OffSet;
        }
        set
        {
            _dbDateTime = value - OffSet;
        }
    }
    public static BsnDateTimeModel FromDb(DateTime dbDateTime, TimeZoneInfo timeZone)
    {
        var o = new BsnDateTimeModel(timeZone);
        o._dbDateTime = dbDateTime;
        return o;
    }
}