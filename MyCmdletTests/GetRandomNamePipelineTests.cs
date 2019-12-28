using NUnit.Framework;
using System.Management.Automation;
using MyCmdlets;
using System.Linq;
using System.Management.Automation.Runspaces;

namespace MyCmdletTests
{
    [TestFixture]
    public class GetRandomNamePipelineTests
    {
        [Test]
        public void TestThreeNames()
        {
            var initialSessionState = InitialSessionState.CreateDefault();
            initialSessionState.Commands.Add(new SessionStateCmdletEntry("Get-RandomName", typeof(GetRandomName), null));

            using (var runspace = RunspaceFactory.CreateRunspace(initialSessionState))
            {
                runspace.Open();
                using (var powershell = PowerShell.Create())
                {
                    powershell.Runspace = runspace;
                    var sqlQueryCommand = new Command("Get-RandomName");

                    var results = powershell.Invoke<string>(new[] { "Nathan","John,","Ted" });

                    Assert.AreEqual(results.Count, 3);
                    Assert.AreEqual(results[0].Length, 6);
                    Assert.AreEqual(results[1].Length, 4);
                    Assert.AreEqual(results[2].Length, 3);

                }
            }
        }
    }
}
