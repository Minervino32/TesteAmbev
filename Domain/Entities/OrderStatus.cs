using System.ComponentModel;

public enum OrderStatus
{
    [Description("Order Imported")]
    Imported,
    [Description("Order Processed")]
    Processed,
    [Description("Order Concluded")]
    Concluded
}