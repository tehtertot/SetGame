using System;
using System.IO;
using Xamarin.Forms;
using Set.Droid;

[assembly: Dependency(typeof(FileHelper))]
namespace Set.Droid
{

    class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}