# EmailSenderProgram


A modular and extensible .NET Console Application designed to automate email delivery via scheduled background jobs using [Hangfire](https://www.hangfire.io/). It currently supports sending **Welcome Emails** and **Comeback Emails** using customizable HTML templates.

## Getting Started

To run the program:

1. Clone the repository.
2. Configure SMTP and other values in `App.config`.
3. Build and run using Visual Studio or `dotnet`.

## Installation 

 **Requires:** .NET Framework 4.8 Language Version: C# 7.3  and Visual Studio 2019 or later.
1. Open the solution `EmailSenderProgram.sln` in Visual Studio.
2. Restore NuGet packages.
3. Build the solution.


## Features

- Plug-and-play notifier system via `[EmailNotifier]` attribute.
- SMTP support with customizable templates.
- Automated job scheduling using Hangfire.
- Serilog-based logging to both console and file.
- Clean modular codebase for easy maintenance.

## Architecture

- **Jobs**: Defined in `Jobs/`, executed by Hangfire.
- **Notifiers**: Implementations of `BaseNotifier`, registered via reflection.
- **Managers**: Service layer for SMTP, templates, and voucher logic.
- **Infrastructure**: Common utilities like logger and job activator.
- **Extensions**: Static classes to extend DI and host builder logic.

## Adding New Email Job

To add a new type of email:

1. Create a new class that inherits from `BaseNotifier`.
2. Decorate it with `[EmailNotifier("your-key")]`.
3. Create a corresponding `.html` template under the `Emails/` folder.
4. Add a new job class that calls the notifier via `EmailNotifierRegistry`.
5. Register your job class in `ServiceCollectionExtension.ConfigureJobs()`.
6. Schedule your job in `Program.ScheduleEmailJobs()` using `RecurringJob.AddOrUpdate`.

## Things to Improve / TODO

- **Secure Configuration**: Move sensitive info like the SMTP password to a secure location.
- **Real Storage**: Switch to using a persistent database for Hangfire storage instead of in-memory storage.
- **Retry System**: Add a service to try sending failed emails again.
- **Message Broker**: Can integrate a message broker to enqueue SMTP emails. 
- **Unit Test**: Add unit tests to check if each part of the program works properly and keeps working when changes are made. 
- **Job Time Execution**: I can also set the job execution times through App.config.
## Documents

- **Task 1 - Programming Task**: Fix bugs, improve performance, refactor code, integrate logging, and enhance configuration management.
- **Task 2 - Theoretical Task**: Proposed a microservices based scalable architecture for the Email Sender Program  following separation of concerns concept.

