using EmailSenderProgram.Infrastructure.Managers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace EmailSenderProgram.Tests
{
    public class TemplateManagerTests
    {
        [Fact]
        public async Task RenderTemplateAsync_ShouldRenderTemplate_WithVariables()
        {
            
            var manager = new TemplateManager("DummyPath"); 
            var template = "Hello {{ name }}!";
            var variables = new Dictionary<string, object>
                            {
                            { "name", "Alice" }
                            };
           
            var result = await manager.RenderTemplateAsync(template, variables);
          
            Assert.Equal("Hello Alice!", result);
        }
    }
}
