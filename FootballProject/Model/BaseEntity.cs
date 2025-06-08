using System;

namespace FootballProject.Model
{
    /// <summary>
    /// Represents the base entity class for all models in the system.
    /// Provides a common integer ID property for database or object identification.
    /// </summary>
    public class BaseEntity
    {
        private int id;

        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
