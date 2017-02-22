namespace SongConstructionService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.service1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceInstaller1.Password = null;
            this.serviceInstaller1.Username = null;
            // 
            // service1
            // 
            this.service1.DisplayName = "SongConstructionService";
            this.service1.ServiceName = "SongConstructionService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceInstaller1,
            this.service1});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceInstaller1;
        private System.ServiceProcess.ServiceInstaller service1;
    }
}