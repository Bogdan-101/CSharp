using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace FileOperations
{
    public class MyFile
    {
        public enum StatusCode
        {
            // if operation was successful
            Success,
            SuccessPrint,

            // if operation was failed
            Error,
            ErrorPrint,

            // codes for files
            FileNotExist,
            FileIsBeingUsed,
            MoveError,

            //codes for directories
            DirectoryNotExist,
            DirectoryDestinationNotExist,
            DirectorySourceNotExist,
        }
        public static void CompressAndMove(string fileName, string filePath, string TargetPath, string extension)
        {
            //moving part

            string year = fileName.Substring(6, 4),
                month = fileName.Substring(11, 2),
                day = fileName.Substring(14, 2),
                hour = fileName.Substring(17, 2),
                minute = fileName.Substring(20, 2),
                second = fileName.Substring(23, 2);

            string[] info = { year, month, day, hour, minute, second };

            foreach (string part in info)
            {
                TargetPath += part;

                DirectoryInfo dirInfo = new DirectoryInfo(TargetPath);

                if (!dirInfo.Exists)
                    dirInfo.Create();

                TargetPath += @"\";
            }

            TargetPath += fileName;

            TargetPath = TargetPath.Replace(".txt", extension);

            CompressFile(filePath, TargetPath);
        }

        public static string GetPathOfFileInTargetDir(string fileName)
        {
            string targetPathOfFile = @"C:\Users\Lenovo\Documents\GitHub\CSharp\ETL project\TargetDirectory\";

            string year = fileName.Substring(6, 4);
            string month = fileName.Substring(11, 2);
            string day = fileName.Substring(14, 2);
            string hour = fileName.Substring(17, 2);
            string minute = fileName.Substring(20, 2);
            string second = fileName.Substring(23, 2);

            string[] data = { year, month, day, hour, minute, second };

            foreach (string period in data)
            {
                targetPathOfFile += period;

                DirectoryInfo dirInfo = new DirectoryInfo(targetPathOfFile);

                if (!dirInfo.Exists)
                {
                    return "error";
                }

                targetPathOfFile += @"\";
            }

            string temp = targetPathOfFile;
            temp += fileName;

            if (File.Exists(temp))
                return targetPathOfFile;
            else
                return "error";
        }
        public static void DecompressFileToTargetDir(string fileName, string filePath, string extension)
        {
            /* fileName is file.txt We need to change it to file.gz, because it is comressed in archieve */
            string gzFileName = fileName.Replace(".txt", extension);

            /* now we need to decompress archieve. But before this, we need to get path to archieve, that has .gz format in TargetDirectory */
            string targetPathOfFileDir = GetPathOfFileInTargetDir(gzFileName);
            string compressedFile = targetPathOfFileDir;
            compressedFile += gzFileName;

            string decompressedFile = targetPathOfFileDir;
            decompressedFile += fileName;
            Console.WriteLine("starting decompressing\ncompressed file: " + compressedFile);
            DecompressFile(compressedFile, decompressedFile);
        }

        public static StatusCode CompressFile(string sourceFile, string compressedFile)
        {
            // поток для чтения исходного файла
            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                // поток для записи сжатого файла
                using (FileStream targetStream = File.Create(compressedFile))
                {
                    // поток архивации
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
                        Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                            sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());

                        return StatusCode.Success;
                    }
                }
            }
        }

        public static StatusCode DecompressFile(string compressedFile, string targetFile)
        {
            // поток для чтения из сжатого файла
            using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
            {
                // поток для записи восстановленного файла
                using (FileStream targetStream = File.Create(targetFile))
                {
                    // поток разархивации
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                        Console.WriteLine("Восстановлен файл: {0}", targetFile);

                        return StatusCode.Success;
                    }
                }
            }
        }

        public static void EncryptFile(string fileName, string filePath, string key)
        {
            string data;
            string encryptedData;

            using (StreamReader reader = new StreamReader(filePath))
            {
                data = reader.ReadToEnd();
                encryptedData = EncryptString(key, data);
            }

            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine(encryptedData);
            }
        }

        public static void DecryptFile(string fileName, string filePath, string key)
        {
            string data;
            string decryptedData;

            using (StreamReader reader = new StreamReader(filePath))
            {
                data = reader.ReadToEnd();
                decryptedData = DecryptString(key, data);
            }

            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine(decryptedData);
            }
        }

        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
