using System.Runtime.InteropServices;
using System.Text;
using static Program;
using System.Reflection;

class ClassToIni
{
    private string mFilePath;
    public ClassToIni(FileInfo fileName)
    {
        mFilePath = fileName.FullName;
        if(!File.Exists(mFilePath))
        {
            SaveINI();
        }
    }
    [DllImport("kernel32")]
    private static extern bool WritePrivateProfileString(string section, string key, string value, string filePath);
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

    public class SectionName : Attribute
    {
        public string sectionName { get; set; }
        public SectionName(string name)
        {
            sectionName = name;
        }
    }
    public class UseINI : Attribute
    {
        public bool bStatus { get; set; }
        public UseINI(bool status)
        {
            this.bStatus = status;
        }
    }
    public void LoadINI()
    {
        //파일 열어서 값 가져오기
        //프로퍼티에 값 넣기
        foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
        {
            var sectionName = propertyInfo.GetCustomAttribute<SectionName>();
            var section = sectionName != null ? sectionName.sectionName : this.GetType().Name;
            var key = propertyInfo.Name;
            var value = GetIniValue(section, key, propertyInfo.GetValue(this)?.ToString() ?? "", mFilePath);
            SetProperty(propertyInfo, value);
        }
    }
    public void SaveINI()
    {
        //설정값 저장
        foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
        {
            //NOTUSE는 안 들어가게
            var sectionName = propertyInfo.GetCustomAttribute<SectionName>();
            //var section = sectionName.Name;// null
            var section = sectionName != null ? sectionName.sectionName : this.GetType().Name;
            var key = propertyInfo.Name;
            var value = propertyInfo.GetValue(this)?.ToString() ?? "";
            if (key != "NotUse")
            {
                WritePrivateProfileString(section, key, value, mFilePath);
            }
        }
    }
    
    private string GetIniValue(string section, string key, string Default, string filePath)
    {
        //ini 파일 읽어오기
        StringBuilder IniValue = new StringBuilder(255);
        GetPrivateProfileString(section, key, Default, IniValue, 255, filePath);
        return IniValue.ToString();
    }
    private void SetProperty(PropertyInfo propertyInfo, string value)
    {
        //프로퍼티값 설정하기
        if(propertyInfo.PropertyType == typeof(int))
        {
            propertyInfo.SetValue(this, int.Parse(value));
        }
        else if(propertyInfo.PropertyType == typeof(string))
        {
            propertyInfo.SetValue(this, value);
        }
        else if (propertyInfo.PropertyType == typeof(ETest))
        {
            propertyInfo.SetValue(this, Enum.Parse(propertyInfo.PropertyType,value));
        }
        else if(propertyInfo.PropertyType == typeof(bool))
        {
            propertyInfo.SetValue(this, bool.Parse(value));
        }
        else if( propertyInfo.PropertyType == typeof(float))
        {
            propertyInfo.SetValue(this, float.Parse(value));
        }
        else if(propertyInfo.PropertyType == typeof(double))
        {
            propertyInfo.SetValue(this, double.Parse(value));
        }
    }
}