namespace TodoApp.CustomControls
{
    /// <summary>
    /// The behaviour of <see cref="NumberBox"/> when the user tries to go above the max value or below the minimum value.
    /// </summary>
    public enum ChangeByOutOfBoundsBehaviours
    {
        /// <summary>
        /// Trying to go out of bounds by using the <see cref="NumberBox.IncreaseByCommand"/> and <see cref="NumberBox.DecreaseByCommand"/> will not do anything.
        /// </summary>
        EndAtBound = 0,

        /// <summary>
        /// Trying to go out of bounds will go to the other bounds(from min value to max value and vice versa)
        /// </summary>
        LoopToOtherBound = 1
    }
}
