
internal class Program
{
    private static void Main(string[] args)
    {
        //경로 지정
        var ini = new Test(new FileInfo("test.ini"));
        ini.LoadINI();
        //로드한 거 확인
        Console.WriteLine(ini.Test1);
        Console.WriteLine(ini.Test2);
        Console.WriteLine(ini.Test3);
        Console.WriteLine(ini.Test4);
        Console.WriteLine(ini.Test5);
        Console.WriteLine(ini.Test6);

        //저장할 거
        ini.Test1 = 10;
        ini.Test2 = "test";
        ini.Test3 = ETest.B;
        ini.Test4 = true;
        ini.Test5 = 0.15434f;
        ini.Test6 = 0.5223;
        
        //여기서 세팅 끝나면 classtoini class에서 프로퍼티값 바뀌고 saveini에선 바뀐 프로퍼티값 ini파일에 저장
        //저장
        ini.SaveINI();
    }
    public enum ETest
    {
        A,
        B,
        C
    }
    public class Test : ClassToIni
    {
        //세션 네임을 지정가능 기본은 클레스 이름
        [SectionName("Base")]
        public int Test1 { get; set; } = 0; //기본값
        public string Test2 { get; set; } = ""; //기본값
        public ETest Test3 { get; set; } = ETest.A; //기본값
        public bool Test4 { get; set; } = false; //기본값
        public float Test5 { get; set; } = 0.0f; //기본값
        [SectionName("Hi")]
        public double Test6 { get; set; } = 0.0; //기본값

        /// <summary>
        /// 사용하지 않음
        /// </summary>
        [UseINI(false)]
        public string NotUse { get; set; } = "";

        public Test(FileInfo fileInfo) : base(fileInfo)
        {

        }
    }
}
