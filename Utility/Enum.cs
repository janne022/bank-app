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

    public enum AsciiType
    {
        File,
        String
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

    public enum InputFieldType
    {
        Normal,
        Number,
        Password
    }

    public enum PageType
    {
        LoginPage,
        TwoFactorPage,
        AdminDashboard,
        ClientDashboard,
        TransferPage,
        TransactionPage,
        LoanPage,
        CreateUserPage,
        TransactionLogPage,
        UpdateRatePage,
        CreateAccountPage,
        CheckingAccountPage,
        LoanPageConfirm,
        SavingsAccountConfirm

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

    public enum Currency
    {
        SEK,  //Swedish krona (BASE CURRENCY)
        USD,  //Dollar
        EUR,  //Euro
        GBP,  //Pound Sterling
        JPY,  //Japanese Yen
        AUD,  //Australian Dollar
        CAD,  //Canadian Dollar
        CHF,  //Swiss Franc
        CNY,  //Chinese Yuan
        NZD,  //New Zealand Dollar
        SLC   // Slava Coin
    }

    public enum Status
    {
        Active,
        Closed,
    }

    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        LoanInterest,
        LoanDisbursement,
        LoanRepayment
    }
    public enum AccountType
    {
        CheckingAcc,
        SavingsAcc,
        LoanAcc,
    }
}
