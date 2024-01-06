using PspPos.Models;

namespace PspPos.Commons
{
    /* Dummy implementation for a tax system */
    public class TaxSystem
    {
        public readonly static float Tax = 0.1f;

        public static ServiceDiscount CreateDefaultServiceDiscount (Guid serviceId)
        {
            return new ServiceDiscount { ServiceId = serviceId, Conditions = "Simple discount", DiscountPercentage = 20, ValidUntil = DateTime.Now.AddMonths(1) };
        }
    }
}
