using System;
using System.IO;
using System.Management.Automation;

namespace BinaryPowerShellExample
{
    // All binary PowerShell commands must include a 'Cmdlet' attribute on the class.
    // In addition, this must inherit from either "PSCmdlet" or "Cmdlet".
    [Cmdlet(VerbsCommon.Get, "DesktopFile", ConfirmImpact = ConfirmImpact.None)]
    [Alias("getdeskfiles")]
    public class GetDesktopFile : PSCmdlet
    {
        // Private (Backend) Fields
        //      Any member of the Cmdlet that is not decorated with a 'Parameter'
        //      attribute is invisible to exported PowerShell cmdlet.
        private string _desktop;
        private SearchOption _recurse;

        // Public Properties (PowerShell parameters)
        //      There is no 'param' declaration.
        //      Any public property with a getter and setter { get; set; } and
        //      is decorated with a "Parameter" attribute will added to the available
        //      parameters.
        [Parameter(Mandatory = false)]
        public SwitchParameter Recurse
        {
            get => Convert.ToBoolean((int)_recurse);
            set => _recurse = (SearchOption)Convert.ToInt32(value);
        }

        // The Begin block in a typical function: Begin { }
        protected override void BeginProcessing()
        {
            _desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        // The Process block in a typical function: Process { }
        protected override void ProcessRecord()
        {
            this.WriteVerbose("Checking Desktop files...");

            if (MyInvocation.BoundParameters.ContainsKey("Recurse"))
            {
                this.WriteVerbose("Including all files recursively.");
            }

            foreach (string filePath in Directory.GetFiles(_desktop, "*", _recurse))
            {
                FileInfo fileInfo = new FileInfo(filePath);

                this.WriteObject(fileInfo);
            }
        }

        // The End block in a typical function: End { }
        //      Binary cmdlets do not have to have Begin/End defined or even
        //      added to the code.
        protected override void EndProcessing()
        {
        }
    }
}
