using System.Runtime.InteropServices;
using System.Text;
using System;
using static Program;
using System.Reflection;

class ClassToIni
{
    [SectionName("Base")]
    public int Test_1 { get; set; } = 0;
    public string Test_2 { get; set; } = "";
    public ETest Test_3 { get; set; } = ETest.A;
    public bool Test_4 { get; set; } = false;
    public float Test_5 { get; set; } = 0.0f;
    [SectionName("Hi")]
    public double Test_6 { get; set; } = 0.0;

    private static string mFilePath => $"../../../test.ini";

    public ClassToIni(FileInfo fileName)
    {
        
    }

    [DllImport("kernel32")]
    private static extern bool WritePrivateProfileString(string section, string key, string value, string filePath);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

    public sealed class SectionName : Attribute
    {
        public string Name { get; set; }
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

    public string LoadINI()
    {
        CreateINI();

        StringBuilder result = new StringBuilder(255);
        return result.ToString();
    }
    
    public void SaveINI()
    {
        //설정값 저장
        //저장해서 결과 확인하기
        //Test asd = new(new FileInfo("test.ini"));
        //PropertyInfo sss = asd.GetType().GetProperty("Test2");
        //if(sss != null)
        //{
        //    sss.SetValue(asd,"aaa");
        //}
        //Console.WriteLine(sss);
        var propertyInfo = typeof(Test).GetProperty(nameof(Test.Test1));
        var s = Attribute.GetCustomAttributes(propertyInfo, true);
        foreach (var a in s)
        {
            Console.WriteLine(a.GetType().ToString());
            if(a.GetType() == typeof(SectionName))
            {
                Console.WriteLine((a as SectionName).Name);
            }
        }

    }
    public void CreateINI()
    {
        try
        {
            if (!File.Exists(mFilePath))
            {
                //파일 만들고 초기값 설정하는데 파일 핸들 오류나서
                //WritePrivateProfileString 바로 실행하는데 파일 준비 안되서 초기값 안들어감
                using FileStream createFile = File.Create(mFilePath);
                //FileStream createFile = new FileStream(mFilePath, FileMode.Create, FileAccess.Write);
                //createFile.Close();
            }
            WritePrivateProfileString("Test", "Test3", "B", mFilePath);
            WritePrivateProfileString("Test", "Test2", "test", mFilePath);
            WritePrivateProfileString("Test", "Test4", "1", mFilePath);
            WritePrivateProfileString("Test", "Test5", "0.15434", mFilePath);
            WritePrivateProfileString("Base", "Test1", "10", mFilePath);
            WritePrivateProfileString("Hi", "Test6", "0.5223", mFilePath);

        }
        catch(Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
