using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace MyCmdlets
{
    [Cmdlet(VerbsCommon.Get,"RandomName")]
    public class GetRandomName : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 1, ValueFromPipeline = true)]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(_names.Where(x => x.Length == Name.Length).OrderBy(x => Guid.NewGuid()).First());
        }

        protected override void BeginProcessing()
        {
        }

        protected override void EndProcessing()
        {
        }
        protected override void StopProcessing()
        {
        }

        private static readonly string[] _names = new[]
        {
            "John","William","James","Charles","George","Frank","Joseph","Thomas","Henry","Robert","Edward",
            "Harry","Walter","Arthur","Fred","Albert","Samuel","David","Louis","Joe","Charlie","Clarence","Richard",
            "Andrew","Daniel","Ernest","Will","Jesse","Oscar","Lewis","Peter","Benjamin","Frederick","Willie","Alfred",
            "Sam","Roy","Herbert","Jacob","Tom","Elmer","Carl","Lee","Howard","Martin","Michael","Bert","Herman","Jim",
            "Francis","Harvey","Earl","Eugene","Ralph","Ed","Claude","Edwin","Ben","Charley","Paul","Edgar","Isaac","Otto",
            "Luther","Lawrence","Ira","Patrick","Guy","Oliver","Theodore","Hugh","Clyde","Alexander","August","Floyd",
            "Homer","Jack","Leonard","Horace","Marion","Philip","Allen","Archie","Stephen","Chester","Willis",
            "Raymond","Rufus","Warren","Jessie","Milton","Alex","Leo","Julius","Ray","Sidney","Bernard","Dan","Jerry"
        };
    }
}
