using Services.Account.Surface.Common;

namespace Services.Account.Surface
{

    [BsonCollection("CustomerDetails")]
    public class CustomerDetails : Document
    {
       /// <summary>
       /// Users first name
       /// </summary>
       public string FirstName { get; set; }

       /// <summary>
       /// Users last name
       /// </summary>
       public string LastName { get; set; }
    }
}
