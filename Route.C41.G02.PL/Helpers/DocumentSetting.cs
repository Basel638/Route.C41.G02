using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Route.C41.G02.PL.Helpers
{
	public class DocumentSetting
	{
        public static async Task<string>  UploadFile(IFormFile file,string folderName)
		{
			// 1. Get Located Folder Path
			//string folderPath = $"D:\\Route Course\\MVC\\Ahmed Nasr\\Session 03\\Demos\\Route.C41.G02.PL\\Route.C41.G02.PL\\wwwroot\\Files\\{folderName}";

			//string folderPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Files\\{folderName}";

			string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);

			if(!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);
			
			// 2. Get File Name and Make it UNIQUE

			string fileName=$"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

			// 3. Get File Path
			string filePath= Path.Combine(folderPath, fileName);

			// 4. save file as streams[Data Per Time]
			var fileStream = new FileStream(filePath, FileMode.Create);
			await file.CopyToAsync(fileStream);

			return fileName;



		}

		public static void DeleteFile(string fileName,string folderName)
		{
			string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//files", folderName);
			string filePath = Path.Combine(folderPath, fileName);
			if(File.Exists(filePath))
				File.Delete(filePath);
		}


    }
}
