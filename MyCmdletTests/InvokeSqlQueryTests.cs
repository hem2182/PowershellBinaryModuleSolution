using MyCmdlets;
using NUnit.Framework;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace MyCmdletTests
{
    [TestFixture]
    public class InvokeSqlQueryTests : PSCmdlet
    {
        [Test]
        public void SelectStarTest()
        {
            var initialSessionState = InitialSessionState.CreateDefault();
            initialSessionState.Commands.Add(new SessionStateCmdletEntry("Invoke-SqlQuery", typeof(InvokeSqlQuery), null));

            using (var runspace = RunspaceFactory.CreateRunspace(initialSessionState))
            {
                runspace.Open();
                using (var powershell = PowerShell.Create())
                {
                    powershell.Runspace = runspace;
                    var sqlQueryCommand = new Command("Invoke-SqlQuery");
                    sqlQueryCommand.Parameters.Add("Server", "localhost");
                    sqlQueryCommand.Parameters.Add("Database", "NTest");
                    sqlQueryCommand.Parameters.Add("Query", "SELECT * FROM People");

                    powershell.Commands.AddCommand(sqlQueryCommand);

                    var results = powershell.Invoke();

                    Assert.AreEqual(results.Count, 4);
                    Assert.AreEqual(results[0].Properties.Count(), 3);
                    Assert.IsNotNull(results[0].Properties["FirstName"]);
                    Assert.IsNotNull(results[0].Properties["LastName"]);
                    Assert.IsNotNull(results[0].Properties["EmailAddress"]);
                }
            }
        }
    }
}
