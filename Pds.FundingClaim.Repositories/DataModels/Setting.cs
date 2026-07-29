using System;

namespace Pds.FundingClaim.Repositories.DataModels
{
    /// <summary>
    /// Represents a setting in the system that is not expected to change regularly.
    /// </summary>
    public partial class Setting
    {
        /// <summary>
        /// Gets or sets identifier for the setting.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of setting that this instance represents.
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the value of the setting.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets when the setting was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets when the setting's value was last updated.
        /// </summary>
        /// <remarks>Will be set to the same value as the CreatedAt upon creation.</remarks>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the setting description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the setting edit type.
        /// </summary>
        public int EditType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the setting is read only.
        /// </summary>
        public bool ReadOnly { get; set; }
    }
}
