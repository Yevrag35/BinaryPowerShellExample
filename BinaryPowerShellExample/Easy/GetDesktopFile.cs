using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace BinaryPowerShellExample
{
    [Cmdlet(VerbsCommon.Get, "DesktopFile", ConfirmImpact = ConfirmImpact.None)]
    [Alias("getdeskfiles")]
    public class GetDesktopFile : PSCmdlet
    {
        private string _desktop;
        private SearchOption _recurse;

        [Parameter(Mandatory = false)]
        public SwitchParameter Recurse
        {
            get => Convert.ToBoolean((int)_recurse);
            set => _recurse = (SearchOption)Convert.ToInt32(value);
        }

        protected override void BeginProcessing()
        {
            _desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

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
    }
}
