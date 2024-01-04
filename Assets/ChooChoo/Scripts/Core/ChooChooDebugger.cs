using Timberborn.SingletonSystem;

namespace ChooChoo
{
    public class ChooChooDebugger : IPostLoadableSingleton
    {
        private const string Directory = @"C:\Users\jordy\SynologyDrive\Unity Projecten\TimberbornModsUnity Update 4\ThunderKit\BepInExPack\BepInEx\plugins\";
        
        private const string OldFolderName = @"ChooChoo";
        
        private const string NewFolderName = @"ChooChoo_testing";
        
        public void PostLoad()
        {
            System.IO.Directory.Move(Directory + OldFolderName, Directory + NewFolderName);
            
            System.IO.Directory.Move(Directory + NewFolderName, Directory + OldFolderName);
        }
    }
}
