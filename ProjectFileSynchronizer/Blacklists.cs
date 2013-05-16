namespace Ischyrus.ProjectFileSynchronizer
{
    public static class Blacklists
    {
        // These are file projects that are specific to Windows 8
        // and should not be referenced by other platforms.
        public static string[] Windows8Blacklist = 
        { 
            "Program.cs", 
            @"Properties\AssemblyInfo.cs",
            @"Assets\Logo.png", 
            @"Assets\SmallLogo.png", 
            @"Assets\SplashScreen.png", 
            @"Assets\StoreLogo.png"
        };
    }
}
