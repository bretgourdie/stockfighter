namespace StockFighter.Solutions
{
    /// <summary>
    /// A class implementing this interface must (attempt to) solve a level and
    /// report its solvedness.
    /// </summary>
    public interface ISolvable
    {
        /// <summary>
        /// A method to solve the level.
        /// </summary>
        /// <returns>Returns if the level was solved or not.</returns>
        bool Solve();

        /// <summary>
        /// Provide the formatted name of the level in string form.
        /// </summary>
        /// <returns>Returns the name of the level.</returns>
        string ToString();

        /// <summary>
        /// Provides the name of the level for the REST call.
        /// </summary>
        /// <returns>Returns the name of the level for the REST call.</returns>
        string LevelName { get; }
    }
}
