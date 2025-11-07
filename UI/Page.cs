using bank_app.Models.Users;
using bank_app.Utility.UI;

namespace bank_app.UI
{
    public abstract class Page
    {
        internal User? CurrentUser { get; set; }
        internal bool ContinueRunning { get; set; }

        /// <summary>
        /// Method that should be used for loading of the page
        /// </summary>
        /// <param name="user">user goes into page</param>
        internal abstract UIComponent LoadPage();

        // Displays page. Should only be called from PageManager
        public void DisplayPage(User user)
        {
            Console.CursorVisible = false;
            CurrentUser = user;
            var rootElement = LoadPage();
            rootElement.Render();
            while (ContinueRunning)
            {
                rootElement.Pressed();
            }
        }
    }
}
