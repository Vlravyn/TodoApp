namespace TodoApp.CustomControls
{
    /// <summary>
    /// Represents whether type of integer to be allowed in the <see cref="NumberBox"/>
    /// </summary>
    public enum NumberBoxRange
    {
        /// <summary>
        /// Represents that the number box allows both positive and negative numbers
        /// </summary>
        AllowBoth = 0,

        /// <summary>
        /// the number box allows only positive numbers
        /// </summary>
        OnlyPositive = 1,

        /// <summary>
        /// the number box allows only negative numbers
        /// </summary>
        OnlyNegative = 2,
    }
}