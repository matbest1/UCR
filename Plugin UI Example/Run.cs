using UCS.Core.Interfaces;

namespace UCS_Editor
{
    class Run : IGeneralPlugin
    {
        public string Title => "UCS Editor";
        public string AuthorName => "Ultrapowa";
        public string ImageURL => "";
        public string Information => "Edit textures\nSupports only 7.65 clients\nConverted in WPF for better performance";
        public string URL => "http://www.ultrapowa.com";
        public string Version => "0.0.0.1";

        public void RunUI()
        {
            MainWindow MW = new MainWindow();
            MW.ShowDialog();
        }
    }
}
