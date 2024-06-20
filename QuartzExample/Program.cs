// See https://aka.ms/new-console-template for more information
using Quartz.Impl;
using Quartz;
using QuartzExample;
using Quartz.Logging;

Console.WriteLine("Quartz Example!");

LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

// Grab the Scheduler instance from the Factory
StdSchedulerFactory factory = new StdSchedulerFactory();
IScheduler scheduler = await factory.GetScheduler();

// and start it off
await scheduler.Start();

// define the job and tie it to our HelloJob class
IJobDetail job = JobBuilder.Create<HelloJob>()
 .WithIdentity("job1", "group1")
 .Build();

// Trigger the job to run now, and then repeat every 10 seconds
ITrigger trigger = TriggerBuilder.Create()
 .WithIdentity("trigger1", "group1")
 .StartNow()
 .WithSimpleSchedule(x => x
  .WithIntervalInSeconds(5)
  .RepeatForever())
 .Build();

// Tell Quartz to schedule the job using our trigger
await scheduler.ScheduleJob(job, trigger);

// You could also schedule multiple triggers for the same job with
// await scheduler.ScheduleJob(job, new List<ITrigger>() { trigger1, trigger2 }, replace: true);

// some sleep to show what's happening
//await Task.Delay(TimeSpan.FromSeconds(10));

// and last shut down the scheduler when you are ready to close your program
//await scheduler.Shutdown();

// Programın sonlanmasını engelle
Console.WriteLine("Press 'Enter' to close the program...");
Console.ReadLine();

// simple log provider to get something to the console
public class ConsoleLogProvider : ILogProvider
{
    public Logger GetLogger(string name)
    {
        return (level, func, exception, parameters) =>
        {
            if (level >= LogLevel.Info && func != null)
            {
                Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
            }
            return true;
        };
    }

    public IDisposable OpenNestedContext(string message)
    {
        throw new NotImplementedException();
    }

    public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
    {
        throw new NotImplementedException();
    }
}