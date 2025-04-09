using EmailSenderProgram.Infrastructure.IManagers;
using Scriban;
using Scriban.Runtime;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EmailSenderProgram.Infrastructure.Managers
{

    public class TemplateManager : ITemplateManager
    {
        private readonly string _basePath;
        protected static readonly ILogger _logger = Log.ForContext<TemplateManager>();

        public TemplateManager(string folderName)
        {
            if (string.IsNullOrWhiteSpace(folderName))
                throw new ArgumentNullException(nameof(folderName));

            _basePath = Path.Combine(GetRootPath(), folderName);
        }

        public async Task<string> RenderTemplate(string templateName, IDictionary<string, object> variables)
        {
            var templatePath = Path.Combine(_basePath, templateName);
            try
            {
                var template = File.ReadAllText(templatePath);

                var updatedTemplate = await RenderTemplateAsync(template, variables);
                return updatedTemplate;
            }
            catch (FileNotFoundException)
            {
                _logger.Information($"Template file not found: {templatePath}");
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error while loading template: {templateName}. Details: {ex.Message}");
                return string.Empty;
            }
        }

        private string GetRootPath()
        {

            return Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
        }

        public async Task<string> RenderTemplateAsync(string templateContent, IDictionary<string, object> variables)
        {
            if (string.IsNullOrWhiteSpace(templateContent))
                throw new ArgumentException("Template content cannot be null or empty.", nameof(templateContent));

            if (variables == null)
                throw new ArgumentNullException(nameof(variables));

            var template = Template.Parse(templateContent);

            if (template.HasErrors)
                throw new InvalidOperationException($"Template parsing failed: {string.Join(", ", template.Messages)}");


            var scriptObject = new ScriptObject(StringComparer.OrdinalIgnoreCase);
            scriptObject.Import(variables);

            var context = new TemplateContext
            {
                MemberRenamer = member => member.Name
            };
            context.PushGlobal(scriptObject);

            return template.Render(context);
        }
    }
}
