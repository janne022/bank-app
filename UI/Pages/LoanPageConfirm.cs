using bank_app.Managers;
using bank_app.Utility;
using bank_app.Utility.Components;
using bank_app.Utility.UI;
using bank_app.Utility.UI.Components;
using bank_app.Utility.UI.Invokables;
using Figgle.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.UI.Pages
{
    internal class LoanPageConfirm:Page
    {

        internal override UIComponent LoadPage()
        {
            Console.CursorVisible = false;
    
            var createAccountInvoke = new Invokable(ContinueHandler);     
            // Create new 3x3 grid
            Grid grid = new(3, 3);

            grid.AddGridComponent(0, 1, new AsciiArt(AsciiType.String, FiggleFonts.Small.Render("Loan Account"))).SetAlign(Align.Middle).SetJustify(Justify.Center);
            grid.AddGridComponent(1, 1, new Text("Loan account created")).SetJustify(Justify.Center);
            grid.AddGridComponent(2, 1, new Button(createAccountInvoke, "Continue")).SetAlign(Align.Middle);

            return new Panel(grid, LayoutBorder.Heavy);
        }

        private void ContinueHandler()
        {

            PageManager.SwitchPage(PageType.ClientDashboard);
        }
    }
}
