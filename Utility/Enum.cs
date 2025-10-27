using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bank_app.Utility
{
    public enum Justify
    {
        Start,
        Center,
        End
    }

    public enum Align
    {
        Top,
        Middle,
        Bottom
    }

    public enum TextAlign
    {
        Left,
        Center,
        Right
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


    public enum UserType
    {
        Admin,
        Client
    }

    public enum TransferStatus
    {
        Pending,
        Completed,
        Failed
    }

    public enum AccountStatus
    {
        Unlocked,
        Locked
    }
}
