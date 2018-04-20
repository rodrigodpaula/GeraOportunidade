using System.Collections;
using System.ComponentModel;

namespace GeraControleDeMalha
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
        protected override void OnBeforeInstall(IDictionary savedState)
        {
            string parameter = @"C:\Rodrigo-TMP\CONTROLEDEMALHA\LOGS\arq_TESTE.log";
            Context.Parameters["assemblypath"] = "\"" + Context.Parameters["assemblypath"] + "\" \"" + parameter + "\"";
            base.OnBeforeInstall(savedState);
        }
    }
}
