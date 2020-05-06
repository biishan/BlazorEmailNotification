using EmailTemplates.Shared.Models;
using FluentEmail.Core;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DemoFluentEmail.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        protected IFluentEmailFactory EmailFactory { get; set; }

        protected List<string> _logs = new List<string>();

        public async Task SendEmailAsync()
        {
            // TemplateDir: C:\Users\<Profile>\...\SolutionName\...\bin\Debug\netcoreapp3.1\EmailTemplates.
            var templateDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"EmailTemplates");

            // TemplateFilePath: C:\Users\<Profile>\...\SolutionName\...\bin\Debug\netcoreapp3.1\EmailTemplates\NewAccount.cshtml.
            var templateFilePath = Path.Combine(templateDir, "NewAccount.cshtml");

            try
            {
                var vm = new NewAccountVm() { 
                    Username = "John Doe",
                    Message = "Thank you for signing up with us!"
                };

                await EmailFactory.Create()
                    .To("johndoe@domain.com")
                    .Subject("New Account")
                    .UsingTemplateFromFile(templateFilePath, vm)
                    .SendAsync();

                _logs.Add($"{DateTime.Now}: Email notification was sent successfully 📧✔!");
            }
            catch (Exception ex)
            {
                _logs.Add($"{DateTime.Now}: Failed to send email notification ❌! ({ex.Message})");
            }
        }
    }
}