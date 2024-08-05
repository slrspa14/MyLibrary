using System.Runtime.InteropServices;
using System.Text;
using System;
using static Program;
using System.IO;
using java.util;

class ClassToIni
{
    private static string mFilePath = "d";
    private static string mFileName = "test.ini";
    private static string mFolderPath = "D:\\study_project\\Library\\MyLibrary\\ClassToIni";
    private DirectoryInfo mDirectory = new DirectoryInfo(mFolderPath);


    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(int Test1, string Test2, Enum Test3, bool Test4, float Test5, double Test6);
    [DllImport("kernel32")]
    private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder reVal, int size, string filePath);
    public sealed class SectionName : Attribute
    {
        public string Name { get; }
        public SectionName(string name)
        {
            Name = name;
        }
    }

    public sealed class UseINI : Attribute
    {
        public bool status;
        public UseINI(bool status)
        {
            this.status = status;
        }
    }
    [SectionName("Base")]
    public int Test1 { get; set; } = 0; //기본값
    public string Test2 { get; set; } = ""; //기본값
    public ETest Test3 { get; set; } = ETest.A; //기본값
    public bool Test4 { get; set; } = false; //기본값
    public float Test5 { get; set; } = 0.0f; //기본값
    [SectionName("Hi")]
    public double Test6 { get; set; } = 0.0; //기본값
    public ClassToIni(FileInfo fileName)
    {

    }
    public string LoadINI()
    {
        StringBuilder result = new StringBuilder();
        //그냥 경로 가서 ini 파일 읽어오고 보내서 출력확인하고
        return result.ToString();
    }
    public void SaveINI() 
    {
        //설정값 저장
    }
    public void CreateINI()
    {
        if(!mDirectory.Exists)
        {
            WritePrivateProfileString(Test1, Test2, Test3, Test4, Test5, Test6);
        }

    }
    

    //public static void Main(string[] args)
    //{
    //    WritePrivateProfileString("test123", "xValue", "6", "D:\\study_project\\Library\\MyLibrary\\ClassToIni/test.ini");
    //}
}
