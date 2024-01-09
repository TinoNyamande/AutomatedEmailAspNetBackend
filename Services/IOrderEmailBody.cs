using EmailAPI.Models;
namespace EmailAPI.Services
{
    public interface IOrderEmailBody
    {
        public string GetHtmlString(Order order);
    }
}


