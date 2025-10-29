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

    public enum Colour
    {
        fullReset,
        bgReset,
        bgBlack,
        bgBlackBright,
        bgBlue,
        bgBlueBright,
        bgCyan,
        bgCyanBright,
        bgGreen,
        bgGreenBright,
        bgMagenta,
        bgMagentaBright,
        bgRed, 
        bgRedBright,
        bgWhite,
        bgWhiteBright,
        bgYellow,
        bgYellowBright,
        fgReset,
        fgBlack,
        fgBlackBright,
        fgBlue,
        fgBlueBright,
        fgCyan,
        fgCyanBright,
        fgGreen,
        fgGreenBright,
        fgMagenta,
        fgMagentaBright,
        fgRed,
        fgRedBright,
        fgWhite,
        fgWhiteBright,
        fgYellow,
        fgYellowBright
    }

    public enum OrderBy
    {
        Row,
        Column
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
}
