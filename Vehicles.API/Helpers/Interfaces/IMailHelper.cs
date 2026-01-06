using Vehicles.Common.Models;

namespace Vehicles.API.Helpers.Interfaces
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body);
    }
}
