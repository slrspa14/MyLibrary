using System.Runtime.InteropServices;
using System.Text;

class ClassToIni
{
    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);
    [DllImport("kernel32")]
    private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder reVal, int size, string filePath);
    public ClassToIni(FileInfo fileName)
    {

    }
    public void LoadINI()
    {

    }
    public void SaveINI() 
    {

    }
    public void CreateINI()
    {

    }
    

    public static void Main(string[] args)
    {
        WritePrivateProfileString("test123", "xValue", "6", "D:\\study_project\\Library\\MyLibrary\\ClassToIni/test.ini");
    }
}
