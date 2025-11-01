using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility
{
    public enum AccountStatus
    {
        Unlocked,
        Locked
    }

    public enum Align
    {
        Top,
        Middle,
        Bottom
    }

    public enum ColourFG
    {
        None,
        Reset,
        Black,
        BlackBright,
        Blue,
        BlueBright,
        Cyan,
        CyanBright,
        Green,
        GreenBright,
        Magenta,
        MagentaBright,
        Red,
        RedBright,
        White,
        WhiteBright,
        Yellow,
        YellowBright
    }

    public enum ColourBG
    {
        None,
        Reset,
        Black,
        BlackBright,
        Blue,
        BlueBright,
        Cyan,
        CyanBright,
        Green,
        GreenBright,
        Magenta,
        MagentaBright,
        Red,
        RedBright,
        White,
        WhiteBright,
        Yellow,
        YellowBright,
    }

    public enum OrderBy
    {
        Row,
        Column
    }

    public enum PageType
    {
        Login,
        AdminDashboard,
        ClientDashboard,
        Transfer,
        Transaction,
        Account,
        Loan
    }

    public enum LayoutBorder
    {
        None,
        Ascii,
        Square,
        Rounded,
        Heavy,
        Double
    }

    public enum Justify
    {
        Start,
        Center,
        End
    }

    public enum TextAlign
    {
        Left,
        Center,
        Right
    }

    public enum TransferStatus
    {
        Pending,
        Completed,
        Failed
    }

    public enum UserType
    {
        Admin,
        Client
    }

    public enum UpOrDown
    {
        Up,
        Down
    }

}
