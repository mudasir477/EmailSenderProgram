using EmailSenderProgram.Infrastructure.IManagers;

namespace EmailSenderProgram.Infrastructure.Managers
{
    public class VoucherManager : IVoucherManager
    {
        public string GetVoucherCode()
        {
            //TODO: in future we can get this from an API 
            return "EOComebackDiscount";
        }
    }
}
