using System.Runtime.InteropServices;
using System.Text;
using System;
using static Program;
using System.Reflection;


class ClassToIni
{
    public ClassToIni(FileInfo fileName)
    {
        CreateINI();
    }

    [DllImport("kernel32")]
    private static extern bool WritePrivateProfileString(string section, string key, string value, string filePath);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

    public class SectionName : Attribute
    {
        public string Name { get; set; }
        public SectionName(string name)
        {
            Name = name;
        }
    }

    public class UseINI : Attribute
    {
        public bool status;
        public UseINI(bool status)
        {
            this.status = status;
        }
    }

    public void LoadINI()
    {
        //파일 열어서 값 가져오기
        //프로퍼티에 값 넣기
        foreach (var propertyInfo in this.GetType().GetProperties())
        {
            var sectionName = propertyInfo.GetCustomAttribute<SectionName>();
            var section = sectionName != null ? sectionName.Name : this.GetType().Name;
            var key = propertyInfo.Name;
            var value = GetIniValue(section, key, propertyInfo.GetValue(this)?.ToString() ?? "", mFilePath);
            SetProperty(propertyInfo, value);
        }
    }
    
    public void SaveINI()
    {
        //설정값 저장
        foreach (var propertyInfo in this.GetType().GetProperties())
        {
            //NOTUSE는 안 들어가게
            var sectionName = propertyInfo.GetCustomAttribute<SectionName>();
            //var section = sectionName.Name// null 아닐 때 뭐 넣지
            var section = sectionName != null ? sectionName.Name : this.GetType().Name;
            var key = propertyInfo.Name;
            var value = propertyInfo.GetValue(this)?.ToString() ?? "";
            if (key != "NotUse")
            {
                WritePrivateProfileString(section, key, value, mFilePath);
            }
        }
    }
    private static string mFilePath => $"../../../test.ini";

    private string GetIniValue(string section, string key, string Default, string filePath)
    {
        StringBuilder loadData = new StringBuilder(255);
        GetPrivateProfileString(section, key, Default, loadData, 255, filePath);
        return loadData.ToString();
    }
    private void CreateINI()
    {
        try
        {
            if (!File.Exists(mFilePath))
            {
                //파일 만들고 초기값 설정하는데 파일 핸들 오류나서
                //WritePrivateProfileString 바로 실행하는데 파일 준비 안되서 초기값 안들어감
                //FileStream createFile = new FileStream(mFilePath, FileMode.Create, FileAccess.Write);
                //createFile.Close();
                using FileStream createFile = File.Create(mFilePath);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
    private void SetProperty(PropertyInfo property, string value)
    {
        //프로퍼티값에 맞는 형식에 넣게?
        if (property.PropertyType == typeof(int))
        {
            property.SetValue(this, int.Parse(value));
        }
        else if (property.PropertyType == typeof(string))
        {
            property.SetValue(this, value);
        }
        else if (property.PropertyType == typeof(ETest))
        {
            property.SetValue(this, Enum.Parse(property.PropertyType, value));
        }
        else if (property.PropertyType == typeof(bool))
        {
            property.SetValue(this, bool.Parse(value));
        }
        else if (property.PropertyType == typeof(float))
        {
            property.SetValue(this, float.Parse(value));
        }
        else if (property.PropertyType == typeof(double))
        {
            property.SetValue(this, double.Parse(value));
        }

    }


}