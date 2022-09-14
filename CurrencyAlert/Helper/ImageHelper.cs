using ImGuiScene;
using System;
using System.IO;

namespace CurrencyAlert.Helper
{
    internal class ImageHelper
    {
        public static TextureWrap? LoadImage(string imageName)
        {
            var assemblyLocation = PluginHelper.PluginInterface.AssemblyLocation.DirectoryName!;
            var imagePath = Path.Combine(assemblyLocation, $@"images\{imageName}.png");

            try
            {
                return PluginHelper.PluginInterface.UiBuilder.LoadImage(imagePath);
            }
            catch (SystemException e)
            {
                return null;
            }
        }
    }
}
