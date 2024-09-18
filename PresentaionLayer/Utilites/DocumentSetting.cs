namespace PresentaionLayer.Utilites
{
    public static class DocumentSetting
    {
        public static string UploadFile(IFormFile file, string foldername)
        {
            string folderpath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Files",foldername);

            string fileName = $"{Guid.NewGuid()}-{file.FileName}";

            string filePath = Path.Combine(folderpath, fileName);

            using var Stream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(Stream);

            return fileName;
        }


        public static void DeleteFile(string FolderName , string FileName)
        {
            string filepath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/Files", FolderName, FileName);

            if (File.Exists(filepath)) 
                File.Delete(filepath);
        }
    }
}
