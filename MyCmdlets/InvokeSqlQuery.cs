using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace MyCmdlets
{
    [Cmdlet(VerbsLifecycle.Invoke, "SqlQuery", DefaultParameterSetName = IntegratedAuth)]
    public class InvokeSqlQuery : PSCmdlet
    {
        private const string IntegratedAuth = "IntegratedAuth";
        private const string SqlAuth = "SqlAuth";

        [Parameter(Position = 1, ParameterSetName = IntegratedAuth)]
        [Parameter(Position = 1, ParameterSetName = SqlAuth)]
        public string Server { get; set; }

        [Parameter(Position = 2, ParameterSetName = IntegratedAuth)]
        [Parameter(Position = 2, ParameterSetName = SqlAuth)]
        public string Database { get; set; }

        [Parameter(Position = 3, Mandatory = true, ParameterSetName = IntegratedAuth)]
        [Parameter(Position = 3, Mandatory = true, ParameterSetName = SqlAuth)]
        public string Query { get; set; }

        [Parameter(Position = 4, Mandatory = true, ParameterSetName = SqlAuth)]
        public string  Username { get; set; }

        [Parameter(Position = 5, Mandatory = true, ParameterSetName = SqlAuth)]
        public string Password { get; set; }


        private SqlConnection _connection;

        protected override void BeginProcessing()
        {
            ValidateParameters();

            WriteVerbose(this.ParameterSetName);

            string connectionString;
            if (this.ParameterSetName == IntegratedAuth)
            {
                connectionString = String.Format(@"DataSource={0};InitialCatalog={1};IntegratedSecurity=SSPI;Persist Security Info = true", Server, Database);
            }
            else
            {
                connectionString = String.Format(@"DataSource={0};InitialCatalog={1};User ID={2};Password={3}", Server, Database,Username,Password);
            }


            
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        protected override void EndProcessing()
        {
            if (_connection != null)
                _connection.Dispose();
        }

        protected override void StopProcessing()
        {
            if (_connection != null)
                _connection.Dispose();
        }

        protected override void ProcessRecord()
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = Query;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var record = new PSObject();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            record.Properties.Add(new PSVariableProperty(new PSVariable( reader.GetName(i),reader[i])));
                        }

                        WriteObject(record);
                    }
                }
            }

        }

        private void ValidateParameters()
        {
            ////If not using session state
            //if (string.IsNullOrEmpty(Server))
            //    ThrowParameterError("Server");

            //if (string.IsNullOrEmpty(Database))
            //    ThrowParameterError("Database");

            const string serverVariable = "InvokeSqlQueryServer";
            const string databaseVariable = "InvokeSqlQueryDatabase";
            //make these names longer to make sure these aren't going to be used by any other cmdlet. 
            //we could have used Server and Database here but then there will be a high chance of these getting preseent as a parameter
            //in some other cmdlet. 
            if (!String.IsNullOrEmpty(Server))
            {
                SessionState.PSVariable.Set(serverVariable, Server);
            }
            else
            {
                //if the server and database variables are not supplied then
                //we will look for these variables/Parameters in the currently saved session variable. 
                SessionState.PSVariable.GetValue(serverVariable, String.Empty).ToString();
                if (string.IsNullOrEmpty(Server))
                {
                    ThrowParameterError("Server");
                }

            }

            if (!String.IsNullOrEmpty(Database))
            {
                SessionState.PSVariable.Set(databaseVariable, Database);
            }
            else
            {
                //if the server and database variables are not supplied then
                //we will look for these variables/Parameters in the currently saved session variable. 
                SessionState.PSVariable.GetValue(databaseVariable, String.Empty).ToString();
                if (string.IsNullOrEmpty(Database))
                {
                    ThrowParameterError("Database");
                }

            }
        }

        private void ThrowParameterError(string parameterName)
        {
            ThrowTerminatingError(
                new ErrorRecord(
                    new ArgumentException(String.Format("Must Specify '{0}'", parameterName)), Guid.NewGuid().ToString(), ErrorCategory.InvalidArgument, null
                    ));
        }
    }
}
