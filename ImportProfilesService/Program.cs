using System;
using System.Xml.Schema;
using Topshelf;

namespace ImportProfilesService
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
           {
               x.Service<MainServiceClass>(s =>
              {
                  s.ConstructUsing(cls => new MainServiceClass());
                  s.WhenStarted(cls => cls.Start());
                  s.WhenStopped(cls => cls.Stop());
              });

               x.RunAsLocalSystem();

               x.SetServiceName("ImportProfilesService");
               x.SetDisplayName("Import Profiles Service");
               x.SetDescription("Import Profiles Service: Import all profiles from csv file");
           });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
