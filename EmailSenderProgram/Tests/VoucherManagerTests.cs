using EmailSenderProgram.Infrastructure.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EmailSenderProgram.Tests
{
    public class VoucherManagerTests
    {
        [Fact]
        public void GetVoucherCode_ShouldReturnExpectedCode()
        {
            
            var voucherManager = new VoucherManager();
            var result = voucherManager.GetVoucherCode();
            Assert.Equal("EOComebackDiscount", result);
        }
    }
}
