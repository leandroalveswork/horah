namespace HoraH.Domain.Models;
public class MongoId
{
    public static string NewMongoId
    {
        get
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 24);
        }
    }
}