using PspPos.Models;

namespace PspPos.Commons
{
    /* Dummy implementation for a tax system */
    public class TaxSystem
    {
        // this should be used as a multiplier for calculating tax.
        public readonly static float TaxMultiplier = 0.1f;

        public static ServiceDiscount CreateDefaultServiceDiscount (Guid serviceId)
        {
            return new ServiceDiscount { ServiceId = serviceId, Conditions = "Simple discount", DiscountPercentage = 20, ValidUntil = DateTime.Now.AddMonths(1) };
        }
    }
}
