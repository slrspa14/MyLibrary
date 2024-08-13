using System;
using System.IO;
using System.Windows;
using System.Data.SQLite;
using System.Text;
using System.Globalization;

namespace LogManager
{
    public class LogManage
    {
        private string mLog;
        private static int mFileNumber = 0;
        private static string mFolderPath = @"/TestLog/bh.cho/" + DateTime.Today.ToString("yyyy/MM/dd") + "/UI";
        private static string mdbFilePath = "../../Logviewer.db";
        private string mQuery;
        private string mLogFilePath = $"{mFolderPath}/UI_{DateTime.Today:dd}_{mFileNumber}.log";

        private DirectoryInfo mDirectory = new DirectoryInfo(mFolderPath);
        private SQLiteConnection mConnectDB = new SQLiteConnection($"Data Source={mdbFilePath};Version=3;");
        
        
        public void ConnectDB()
        {
            mConnectDB.Open();
        }
        public void CreateLog(string Date, string Button)
        {
            try
            {
                if (!mDirectory.Exists)
                {
                    mDirectory.Create();
                }

                CreateTable();

                FileSizeCheck();

                using (StreamWriter WriteLog = new StreamWriter(mLogFilePath, true, Encoding.UTF8))
                {
                    mLog = string.Format($"{Date} {Button}_Click");
                    WriteLog.WriteLine(mLog);
                    InsertData(Date, $"{Button}_Click");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public string SelectAllData(DateTime Date, string Log)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                mQuery = "SELECT * FROM LOG WHERE date LIKE '%" + Date + "%' AND log LIKE '%" + Log + "%'";
                using (SQLiteCommand command = new SQLiteCommand(mQuery, mConnectDB))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Append(reader.GetString(0));
                        }
                        return result.ToString();
                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }

        }
        public string SelectLogData(string Log)
        {
            try
            {
                mQuery = "SELECT log FROM LOG WHERE log LIKE @Log";
                using (SQLiteCommand command = new SQLiteCommand(mQuery, mConnectDB))
                {
                    command.Parameters.AddWithValue("@Log", "%" + Log + "%");
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        StringBuilder result = new StringBuilder();
                        while (reader.Read())
                        {
                            string log = reader["log"].ToString();
                            result.AppendLine($"Log: {log}");
                        }
                        return result.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
        public string SelectDateData(DateTime Date)
        {
            try
            {
                string searchDate = Date.ToString("yyyy-MM-dd");
                mQuery = "SELECT date FROM LOG WHERE date LIKE '%"+ searchDate + "%'";
                using (SQLiteCommand command = new SQLiteCommand(mQuery, mConnectDB))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        StringBuilder result = new StringBuilder();

                        while (reader.Read())
                        {
                            string dateResult = reader["date"].ToString();
                            result.AppendLine(dateResult);
                        }
                        return result.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
        public void DeleteLog()
        {
            try
            {
                DirectoryInfo baseDirectory = new DirectoryInfo(@"/TestLog/bh.cho/");
                foreach (DirectoryInfo directory in baseDirectory.GetDirectories("*", SearchOption.AllDirectories))
                {
                    foreach (FileInfo fileInfo in directory.GetFiles())
                    {
                        if (fileInfo.CreationTime < DateTime.Today)
                        {
                            fileInfo.Delete();
                        }
                    }

                    if (directory.GetFiles().Length == 0 && directory.GetDirectories().Length == 0)
                    {
                        directory.Delete();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        
        private void FileSizeCheck()
        {
            while (true)
            {
                FileInfo fileInfo = new FileInfo(mLogFilePath);
                if (!fileInfo.Exists || fileInfo.Length <= 1000)
                {
                    break;
                }
                mFileNumber++;
            }
        }
        private void CreateTable()
        {

            if (!File.Exists(mdbFilePath))
            {
                SQLiteConnection.CreateFile(mdbFilePath);
            }

            mQuery = "CREATE TABLE IF NOT EXISTS LOG(date DATETIME, log TEXT)";
            using (SQLiteCommand command = new SQLiteCommand(mQuery, mConnectDB))
            {
                command.ExecuteNonQuery();
            }
        }
        private void InsertData(string Date, string Log)
        {
            mQuery = "INSERT INTO LOG(date, log) VALUES(@Date, @Log)";
            using (SQLiteCommand command = new SQLiteCommand(mQuery, mConnectDB))
            {
                command.Parameters.AddWithValue("@Date", Date);
                command.Parameters.AddWithValue("@Log", Log);
                command.ExecuteNonQuery();
            }
        }
    }
}
